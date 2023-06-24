using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Warehouse_Manager
{
    public partial class Form1 : Form
    {
        private Timer resetTimer;
        public Form1()
        {
            string downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            string warehouseManagerFolderPath = Path.Combine(downloadsFolderPath, "WarehouseManager");

            // Create the WarehouseManager folder if it doesn't exist
            if (!Directory.Exists(warehouseManagerFolderPath))
            {
                Directory.CreateDirectory(warehouseManagerFolderPath);
            }
            InitializeComponent();
            float dpiScaleFactor = GetDpiScaleFactor();
            ScaleFont(this, dpiScaleFactor);
            int screenWidth = 1920;
            int screenHeight = 1080;
            this.Size = new Size(screenWidth, screenHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadWarehouseShortData();
            UpdateClockLabel();
            resetTimer = new Timer
            {
                Interval = 5000 // 5 seconds
            };
            resetTimer.Tick += ResetTimer_Tick;
            resetTimer.Start();
        }
        private float GetDpiScaleFactor()
        {
            // Get the Graphics object for the primary screen
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                // Get the DPI of the primary screen
                float dpiX = g.DpiX;
                float dpiY = g.DpiY;

                // Calculate the DPI scale factor
                float dpiScaleFactor = 100/ dpiX; 

                return dpiScaleFactor;
            }
        }

        private void ScaleFont(Control control, float scaleFactor)
        {
            // Scale the font size of the current control
            control.Font = new Font(control.Font.FontFamily, control.Font.Size * scaleFactor, control.Font.Style);

            // Recursively scale the font size of child controls
            foreach (Control child in control.Controls)
            {
                ScaleFont(child, scaleFactor);
            }
        }
        private void LoadWarehouseShortData()
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseShortExcelPath = Path.Combine(downloadsFolderPath, "WarehouseShort.xlsx");
            string excelPath = warehouseShortExcelPath;
            if (!File.Exists(excelPath))
            {
                using (ExcelPackage newPackage = new ExcelPackage())
                {
                    // Create a new worksheet
                    ExcelWorksheet newWorksheet = newPackage.Workbook.Worksheets.Add("Worksheet1");
                    // Add header row with column names
                    newWorksheet.Cells[1, 1].Value = "Tên vật tư";
                    newWorksheet.Cells[1, 2].Value = "Số lượng";
                    newWorksheet.Cells[1, 3].Value = "Khối lượng";
                    // Add more column names as needed
                    // Save the new Excel file
                    newPackage.SaveAs(new FileInfo(excelPath));
                }
            }
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (FileStream stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    DataTable dt = new DataTable();

                    // Read the header row
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dt.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                    }

                    // Read the data rows
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        DataRow newRow = dt.NewRow();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            newRow[col - 1] = worksheet.Cells[row, col].Value;
                        }
                        dt.Rows.Add(newRow);
                    }

                    dataGridView2.DataSource = dt;
                    // Add Threshold column
                    dt.Columns.Add("Mức tối thiểu", typeof(double));

                    // Get threshold data from Product-*.xlsx files
                    var thresholdData = GetThresholdData();

                    // Add threshold data to the DataTable
                    foreach (DataRow row in dt.Rows)
                    {
                        string ingredient = row[0].ToString();
                        if (thresholdData.ContainsKey(ingredient))
                        {
                            row["Mức tối thiểu"] = thresholdData[ingredient];
                        }
                        else
                        {
                            row["Mức tối thiểu"] = 0;
                        }
                    }

                    dataGridView2.DataSource = dt;
                }
            }
        }
        private Dictionary<string, double> GetThresholdData()
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            var productFiles = Directory.GetFiles(downloadsFolderPath, "Product-*.xlsx");

            Dictionary<string, double> thresholdData = new Dictionary<string, double>();

            foreach (string productFile in productFiles)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                using (FileStream stream = new FileStream(productFile, FileMode.Open, FileAccess.Read))
                {
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        // Iterate through the data rows
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string ingredient = worksheet.Cells[row, 1].Value.ToString();
                            double quantity = Convert.ToDouble(worksheet.Cells[row, 3].Value);

                            if (thresholdData.ContainsKey(ingredient))
                            {
                                thresholdData[ingredient] += quantity;
                            }
                            else
                            {
                                thresholdData[ingredient] = quantity;
                            }
                        }
                    }
                }
            }

            return thresholdData;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void LoadInventory()
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");
            string excelPath = warehouseExcelPath;
            if (!File.Exists(excelPath))
            {
                using (ExcelPackage newPackage = new ExcelPackage())
                {
                    // Create a new worksheet
                    ExcelWorksheet newWorksheet = newPackage.Workbook.Worksheets.Add("Worksheet1");
                    // Add header row with column names
                    newWorksheet.Cells[1, 1].Value = "Tên vật tư";
                    newWorksheet.Cells[1, 2].Value = "Mã vật tư";
                    newWorksheet.Cells[1, 3].Value = "Số lượng";
                    newWorksheet.Cells[1, 4].Value = "Khối lượng";
                    newWorksheet.Cells[1, 5].Value = "Đơn vị";
                    newWorksheet.Cells[1, 6].Value = "Thời gian";
                    newWorksheet.Cells[1, 7].Value = "Vị trí";
                    // Add more column names as needed
                    // Save the new Excel file
                    newPackage.SaveAs(new FileInfo(excelPath));
                }
            }
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (FileStream stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read))
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    DataTable dt = new DataTable();

                    // Read the header row
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dt.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                    }

                    // Read the data rows
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        DataRow newRow = dt.NewRow();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            newRow[col - 1] = worksheet.Cells[row, col].Value;
                        }
                        dt.Rows.Add(newRow);
                    }

                    dataGridView1.DataSource = dt;
                }
            }
        }
        private void WarehouseInputForm_Click(object sender, EventArgs e)
        {
            WarehouseInputForm warehouseInputForm = new WarehouseInputForm();
            warehouseInputForm.Show();
        }

        private void WarehouseMapForm_Click(object sender, EventArgs e)
        {
            WarehouseMapForm warehouseMapForm = new WarehouseMapForm();
            warehouseMapForm.Show();
        }

        private void WarehouseOutputForm_Click(object sender, EventArgs e)
        {
            WarehouseOutputForm warehouseOutputForm = new WarehouseOutputForm();
            warehouseOutputForm.Show();
        }

        private void PrintoutCheckForm_Click(object sender, EventArgs e)
        {
            PrintoutCheckForm PrintoutCheckForm = new PrintoutCheckForm();
            PrintoutCheckForm.Show();
        }

        private void ProductCatalogForm_Click(object sender, EventArgs e)
        {
            ProductCatalogForm ProductCatalogForm = new ProductCatalogForm();
            ProductCatalogForm.Show();
        }

        private void IngredientCatalogForm_Click(object sender, EventArgs e)
        {
            IngredientCatalogForm IngredientCatalogForm = new IngredientCatalogForm();
            IngredientCatalogForm.Show();
        }
        private void NoteForm_Click(object sender, EventArgs e)
        {
            NoteForm NoteForm = new NoteForm();
            NoteForm.Show();
        }
        private void ResetTimer_Tick(object sender, EventArgs e)
        {
            LoadWarehouseShortData();
            CompareQuantityAndThreshold();
            UpdateClockLabel();
            UpdateWeatherLabel();
        }
        private void CompareQuantityAndThreshold()
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                double totalQuantity = Convert.ToDouble(row.Cells["Số lượng"].Value);
                double threshold = Convert.ToDouble(row.Cells["Mức tối thiểu"].Value);

                if (totalQuantity < threshold)
                {
                    row.Cells["Tên vật tư"].Style.ForeColor = Color.Red;
                }
                else
                {
                    row.Cells["Tên vật tư"].Style.ForeColor = Color.Black;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private void UpdateClockLabel()
        {
            labelClock.Text = DateTime.Now.ToString("dd/MM - HH:mm tt");
        }
        public string TranslateWeatherDescription(string description)
        {
            var translations = new Dictionary<string, string>
        {
        { "clear sky", "bầu trời trong xanh" },
        { "few clouds", "ít mây" },
        { "scattered clouds", "mây rải rác" },
        { "broken clouds", "mây vỡ" },
        { "shower rain", "mưa rào" },
        { "rain", "có mưa" },
        { "mist", "sương mù" },
        { "thunderstorm with light rain", "giông với mưa nhẹ" },
        { "thunderstorm with rain", "giông với mưa" },
        { "thunderstorm with heavy rain", "giông với mưa lớn" },
        { "light thunderstorm", "giông nhẹ" },
        { "thunderstorm", "có giông" },
        { "heavy thunderstorm", "giông mạnh" },
        { "ragged thunderstorm", "giông bão" },
        { "thunderstorm with light drizzle", "giông và mưa phùn rải rác" },
        { "thunderstorm with drizzle", "giông và mưa phùn" },
        { "thunderstorm with heavy drizzle", "giông và mưa phùn mạnh" },
        { "light intensity drizzle", " mưa phùn nhẹ" },
        { "drizzle", "mưa phùn" },
        { "heavy intensity drizzle", " mưa phùn cường độ mạnh" },
        { "light intensity drizzle rain", "mưa phùn cường độ nhẹ" },
        { "drizzle rain", "mưa phùng nặng hạt" },
        { "heavy intensity drizzle rain", "mưa phùng nặng hạt cường độ mạnh" },
        { "shower rain and drizzle", "mưa rào và mưa phùn rải rác" },
        { "heavy shower rain and drizzle", "mưa rào lớn và mưa phùn" },
        { "shower drizzle", "mưa rào và mưa phùn" },
        { "light rain", "mưa nhẹ" },
        { "moderate rain", "có mưa" },
        { "heavy intensity rain", "mưa nặng hạt" },
        { "very heavy rain", "mưa lớn" },
        { "extreme rain", "mưua rất lớn" },
        { "freezing rain", "mưa lạnh" },
        { "light intensity shower rain", "mưa rào nhẹ" },
        { "heavy intensity shower rain", "mưu rào mạnh" },
        { "ragged shower rain", "mưa rào kèm gió dật" },
        { "overcast clouds", "trời âm u" },
        };
            if (translations.TryGetValue(description, out string translatedDescription))
            {
                return translatedDescription;
            }
            else
            {
                
                return description;
            }
        }
            private async Task<string> GetWeatherAsync()
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q=hanoi&appid=5ebd76820119e7eae93dfcf2cb14cf6f&units=metric";

            using (HttpClient client = new HttpClient())
            {
                string jsonContent = await client.GetStringAsync(url);
                JObject weatherData = JObject.Parse(jsonContent);

                string description = weatherData["weather"][0]["description"].ToString();
                string temperature = weatherData["main"]["temp"].ToString();
                string temperatureLike = weatherData["main"]["feels_like"].ToString();
                string humid = weatherData["main"]["humidity"].ToString();
                string VNdescription = TranslateWeatherDescription(description);
                return $"Thành phố Hà Nội \nThời tiết: {VNdescription} , độ ẩm {humid} \nNhiệt độ: {temperature}°C , cảm giác: {temperatureLike}°C";
            }
        }
        private async void UpdateWeatherLabel()
        {
            try
            {
                labelWeather.Text = await GetWeatherAsync();
            }
            catch (Exception ex)
            {
                labelWeather.Text = "Không có thông tin thời tiết " + ex.Message;
            }
        }

        private void labelClock_Click(object sender, EventArgs e)
        {

        }
    }
}

