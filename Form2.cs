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

namespace Warehouse_Manager
{
    public partial class WarehouseInputForm : Form
    {
        public WarehouseInputForm()
        {
            InitializeComponent();
            cmbUnit.SelectedIndexChanged += CmbUnit_SelectedIndexChanged;
            cmbWeightUnit.SelectedIndexChanged += CmbWeightUnit_SelectedIndexChanged;
            float dpiScaleFactor = GetDpiScaleFactor();
            ScaleFont(this, dpiScaleFactor);
            int fullWidthHD = 1920;
            int fullHeightHD = 1080;
            this.Size = new Size(fullWidthHD, fullHeightHD);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            InitializeDataGridView();
            LoadIngredients();
            label2.Visible = false;
        }
        private bool _userChangedWeightUnit = false;

        private void CmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_userChangedWeightUnit)
            {
                cmbWeightUnit.SelectedIndex = cmbUnit.SelectedIndex;
            }
        }

        private void CmbWeightUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            _userChangedWeightUnit = true;
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
                float dpiScaleFactor = 100 / dpiX;

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
        private void WarehouseInputForm_Load(object sender, EventArgs e)
        {

        }
        private void InitializeDataGridView()
        {
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
        }
        private void LoadIngredients()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager", "Ingredient.xlsx");
            List<Ingredient> ingredients = ReadIngredientsFromExcel(filePath);

            // Load the ingredients into the ComboBox
            cmbIngredient.DataSource = ingredients;
            cmbIngredient.DisplayMember = "DisplayName";
        }
        public class Ingredient
        {
            public string Name { get; set; }
            public string CodeName { get; set; }
            public string DisplayName
            {
                get
                {
                    return $"{Name} ({CodeName})";
                }
            }
        }
        private List<Ingredient> ReadIngredientsFromExcel(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Chưa nhập thông tin trong Thư Mục Vật Tư", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Ingredient>();
            }

            List<Ingredient> ingredients = new List<Ingredient>();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string ingredientName = worksheet.Cells[row, 1].GetValue<string>();
                    string ingredientCodeName = worksheet.Cells[row, 2].GetValue<string>();
                    if (!string.IsNullOrWhiteSpace(ingredientName) && !string.IsNullOrWhiteSpace(ingredientCodeName))
                    {
                        ingredients.Add(new Ingredient { Name = ingredientName, CodeName = ingredientCodeName });
                    }
                }
            }

            return ingredients;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Ingredient selectedIngredient = cmbIngredient.SelectedItem as Ingredient;
            if (selectedIngredient == null)
            {
                MessageBox.Show("Chưa chọn vật tư", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ingredient = selectedIngredient.Name;
            string codeName = selectedIngredient.CodeName;
            float quantity = 1;
            float weightPerBox = 1;
            string unit = cmbUnit.SelectedItem.ToString();
            DateTime date = dateTimePicker.Value.Date;
            string place = "Chưa có";
            string weightUnit = cmbWeightUnit.SelectedItem.ToString();
            // Set quantity to 1 if the textbox is empty, else parse the value
            if (!float.TryParse(txtQuantity.Text.Trim(), out quantity))
            {
                quantity = 1;
            }

            // Set weightPerBox to 1 if the textbox is empty, else parse the value
            if (!float.TryParse(txtWeight.Text.Trim(), out weightPerBox))
            {
                weightPerBox = 1;
            }

            if (unit == "Thùng")
            {
                float boxesPerCrate;

                // Set boxesPerCrate to 1 if the textbox is empty, else parse the value
                if (!float.TryParse(txtBoxesPerCrate.Text.Trim(), out boxesPerCrate))
                {
                    boxesPerCrate = 1;
                }

                quantity *= boxesPerCrate;
            }

            float totalWeight = quantity * weightPerBox;
            dataGridView.Rows.Add(ingredient, codeName, quantity, totalWeight, weightUnit, date.ToString("d"), place);
            // Clear the input fields
            txtQuantity.Text = "Số lượng";
            txtWeight.Text = "Hộp/Thùng nếu có";
            txtBoxesPerCrate.Text = "Khối lượng / Đơn vị (Mặc định 1)";                   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Remove the first selected row
                dataGridView.Rows.Remove(dataGridView.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Chưa chọn dòng để xoá", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Create the WarehouseManager folder if it doesn't exist
            string warehouseManagerFolderPath = Path.Combine(downloadsFolderPath, "WarehouseManager");
            if (!Directory.Exists(warehouseManagerFolderPath))
            {
                Directory.CreateDirectory(warehouseManagerFolderPath);
            }

            // Set the WarehouseData.xlsx file path
            string warehouseDataExcelPath = Path.Combine(warehouseManagerFolderPath, "WarehouseData.xlsx");
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView.Columns[i].HeaderText;
                }

                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataGridView.Rows[i].Cells[j].Value;
                    }
                }

                File.WriteAllBytes(warehouseDataExcelPath, excelPackage.GetAsByteArray());
            }

            MessageBox.Show("Data saved to Excel file successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string downloadsFolderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseData.xlsx");
            string warehouseShortExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseShort.xlsx");
            SaveWarehouseShortData(warehouseDataExcelPath1, warehouseShortExcelPath1);
        }
        private void SaveWarehouseShortData(string inputFilePath, string outputFilePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (ExcelPackage inputExcelPackage = new ExcelPackage(new FileInfo(inputFilePath)))
            {
                ExcelWorksheet inputWorksheet = inputExcelPackage.Workbook.Worksheets[0];

                using (ExcelPackage outputExcelPackage = new ExcelPackage())
                {
                    ExcelWorksheet outputWorksheet = outputExcelPackage.Workbook.Worksheets.Add("Sheet1");

                    // Add headers for first 4 columns, skipping the added column (Column 2)
                    outputWorksheet.Cells[1, 1].Value = inputWorksheet.Cells[1, 1].Value;
                    for (int i = 2; i < 4; i++)
                    {
                        outputWorksheet.Cells[1, i].Value = inputWorksheet.Cells[1, i + 1].Value;
                    }

                    // Dictionary to store unique items and corresponding sums
                    Dictionary<string, Tuple<double, double>> groupedData = new Dictionary<string, Tuple<double, double>>();

                    // Iterate through rows and update the dictionary
                    for (int i = 2; i <= inputWorksheet.Dimension.End.Row; i++)
                    {
                        string item = inputWorksheet.Cells[i, 1].Value.ToString();
                        double column3Value = Convert.ToDouble(inputWorksheet.Cells[i, 3].Value);
                        double column4Value = Convert.ToDouble(inputWorksheet.Cells[i, 4].Value);

                        if (groupedData.ContainsKey(item))
                        {
                            // Update the sums of columns 3 and 4
                            groupedData[item] = Tuple.Create(groupedData[item].Item1 + column3Value, groupedData[item].Item2 + column4Value);
                        }
                        else
                        {
                            groupedData[item] = Tuple.Create(column3Value, column4Value);
                        }
                    }

                    // Load Ingredient.xlsx
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager", "Ingredient.xlsx");
                    string ingredientFilePath = filePath;
                    using (ExcelPackage ingredientExcelPackage = new ExcelPackage(new FileInfo(ingredientFilePath)))
                    {
                        ExcelWorksheet ingredientWorksheet = ingredientExcelPackage.Workbook.Worksheets[0];
                        // Iterate through the Ingredient.xlsx rows and add missing items to the groupedData dictionary
                        for (int i = 2; i <= ingredientWorksheet.Dimension.End.Row; i++)
                        {
                            string ingredientName = ingredientWorksheet.Cells[i, 1].Value.ToString();
                            if (!groupedData.ContainsKey(ingredientName))
                            {
                                groupedData[ingredientName] = Tuple.Create(-1.0, -1.0); // Indicate that the item doesn't exist in WarehouseData
                            }
                        }
                    }

                    // Write the grouped data to the new Excel file
                    int rowIndex = 2;
                    foreach (var keyValue in groupedData)
                    {
                        outputWorksheet.Cells[rowIndex, 1].Value = keyValue.Key;
                        outputWorksheet.Cells[rowIndex, 2].Value = keyValue.Value.Item1;
                        outputWorksheet.Cells[rowIndex, 3].Value = keyValue.Value.Item2;
                        rowIndex++;
                    }

                    File.WriteAllBytes(outputFilePath, outputExcelPackage.GetAsByteArray());
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                    dataGridView.Rows.Clear();

                    for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        int rowIndex = dataGridView.Rows.Add();
                        DataGridViewRow row = dataGridView.Rows[rowIndex];

                        for (int j = 1; j <= worksheet.Dimension.End.Column; j++)
                        {
                            row.Cells[j - 1].Value = worksheet.Cells[i, j].Value;
                        }
                    }
                }

                MessageBox.Show("Đã nhập dữ liệu từ tệp tin", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBoxesPerCrate.Enabled = cmbUnit.SelectedItem.ToString() != "Hộp";
            if(cmbUnit.SelectedItem.ToString() == "Thùng")
            {
                label2.Visible = true;
            }
            else
            {
                label2.Visible = false;
            }
            _userChangedWeightUnit = false;
            cmbWeightUnit.SelectedIndex = cmbUnit.SelectedIndex;
        }

        private void cmbIngredient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    }
