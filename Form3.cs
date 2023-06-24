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
    public partial class WarehouseMapForm : Form
    {
        public WarehouseMapForm()
        {
            InitializeComponent();
            float dpiScaleFactor = GetDpiScaleFactor();
            ScaleFont(this, dpiScaleFactor);
            int fullWidthHD = 1920;
            int fullHeightHD = 1080;
            this.Size = new Size(fullWidthHD, fullHeightHD);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            InitializeDataGridView1();
            Load_Warehouse();
            InitializeComboBoxPlaces();
            InitializeWarehouseMap();
            UpdateTableLayoutPanel();
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
        private Dictionary<string, List<string>> warehousePlaces;
        private void LoadWarehouseData()
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");
            warehousePlaces = new Dictionary<string, List<string>>();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(warehouseExcelPath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rows = worksheet.Dimension.Rows;
                int cols = worksheet.Dimension.Columns;

                for (int row = 2; row <= rows; row++)
                {
                    string place = worksheet.Cells[row, 1].GetValue<string>();
                    string item = worksheet.Cells[row, 6].GetValue<string>();

                    if (!warehousePlaces.ContainsKey(place))
                    {
                        warehousePlaces[place] = new List<string>();
                    }

                    warehousePlaces[place].Add(item);
                }
            }
        }
        private void InitializeWarehouseMap()
        {
            LoadWarehouseData();

            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    string place = $"{(char)('A' + row)}{col + 1}";

                    // Get the first item name for the current place
                    string itemName = warehousePlaces.TryGetValue(place, out var items) && items.Count > 0 ? items[0] : "No item";

                    Label placeLabel = new Label
                    {
                        Text = $"{(char)('A' + row)}{col + 1}",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    placeLabel.MouseEnter += PlaceLabel_MouseEnter;
                    placeLabel.MouseLeave += PlaceLabel_MouseLeave;

                    tableLayoutPanel1.Controls.Add(placeLabel, col, row);
                }
            }
        }
        private void PlaceLabel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label placeLabel)
            {
                string place = placeLabel.Text;
                List<string> itemsAtPlace = new List<string>();

                foreach (var entry in warehousePlaces)
                {
                    if (entry.Value.Contains(place))
                    {
                        itemsAtPlace.Add(entry.Key);
                    }
                }

                string placeInfo = itemsAtPlace.Count > 0 ? string.Join(", ", itemsAtPlace) : "Trống";
                toolTip1.SetToolTip(placeLabel, placeInfo);
            }
        }

        private void PlaceLabel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label placeLabel)
            {
                toolTip1.SetToolTip(placeLabel, "");
            }
        }
        private void InitializeDataGridView1()
        {           
            dataGridView1.Columns.Add("IngredientName", "Tên vật tư");
            dataGridView1.Columns.Add("CodeName", "Mã vật tư");
            dataGridView1.Columns.Add("Quantity", "Số lượng");
            dataGridView1.Columns.Add("Weight", "Khối lượng");
            dataGridView1.Columns.Add("Unit", "Đơn vị");
            dataGridView1.Columns.Add("Date", "Thời gian");
            dataGridView1.Columns.Add("Place", "Vị trí");
        }

        private void InitializeComboBoxPlaces()
        {
            List<string> places = new List<string>();

            // Loop through the characters 'A', 'B', and 'C'
            for (char letter = 'A'; letter <= 'C'; letter++)
            {
                // Loop from 1 to 40
                for (int number = 1; number <= 40; number++)
                {
                    string place = $"{letter}{number}";
                    places.Add(place);
                }
            }

            comboBoxPlaces.Items.AddRange(places.ToArray());
            comboBoxPlaces.SelectedIndex = 0;
        }
        private void Load_Warehouse()
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");

            string selectedFilePath = null;

            if (File.Exists(warehouseDataExcelPath))
            {
                selectedFilePath = warehouseDataExcelPath;
            }

            if (selectedFilePath != null)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(selectedFilePath)))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                    dataGridView1.Rows.Clear();

                    for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        int rowIndex = dataGridView1.Rows.Add();
                        DataGridViewRow row = dataGridView1.Rows[rowIndex];

                        for (int j = 1; j <= worksheet.Dimension.End.Column; j++)
                        {
                            row.Cells[j - 1].Value = worksheet.Cells[i, j].Value;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("WarehouseData.xlsx not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");

            string selectedFilePath = null;

            if (File.Exists(warehouseDataExcelPath))
            {
                selectedFilePath = warehouseDataExcelPath;
            }

            if (selectedFilePath != null)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(selectedFilePath)))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                    dataGridView1.Rows.Clear();

                    for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        int rowIndex = dataGridView1.Rows.Add();
                        DataGridViewRow row = dataGridView1.Rows[rowIndex];

                        for (int j = 1; j <= worksheet.Dimension.End.Column; j++)
                        {
                            row.Cells[j - 1].Value = worksheet.Cells[i, j].Value;
                        }
                    }
                }

                MessageBox.Show("Đã nạp dữ liệu từ tệp tin", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("WarehouseData.xlsx không tồn tại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                selectedRow.Cells["Vị trí"].Value = comboBoxPlaces.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Hãy chọn một vị trí đặt vật tư", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Create the WarehouseManager folder if it doesn't exist
            string warehouseManagerFolderPath = Path.Combine(downloadsFolderPath, "WarehouseManager");
            if (!Directory.Exists(warehouseManagerFolderPath))
            {
                Directory.CreateDirectory(warehouseManagerFolderPath);
            }

            string warehouseDataExcelPath = Path.Combine(warehouseManagerFolderPath, "WarehouseData.xlsx");
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                    }
                }

                File.WriteAllBytes(warehouseDataExcelPath, excelPackage.GetAsByteArray());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Get the user's Downloads folder path
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Create the WarehouseManager folder if it doesn't exist
            string warehouseManagerFolderPath = Path.Combine(downloadsFolderPath, "WarehouseManager");
            if (!Directory.Exists(warehouseManagerFolderPath))
            {
                Directory.CreateDirectory(warehouseManagerFolderPath);
            }

            // Set the Output.xlsx file path
            string outputExcelPath = Path.Combine(warehouseManagerFolderPath, "WarehouseData.xlsx");
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i].Cells[j].Value;
                    }
                }

                File.WriteAllBytes(outputExcelPath, excelPackage.GetAsByteArray());
            }
         
            UpdateTableLayoutPanel();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbWarehouseSection.SelectedIndexChanged += CmbWarehouseSection_SelectedIndexChanged;
        }
        private void CmbWarehouseSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWarehouseData();
            UpdateTableLayoutPanel();
        }
        private void UpdateTableLayoutPanel()
        {
            tableLayoutPanel1.Controls.Clear();
            Dictionary<string, (string Item, string Weight, string WeightUnit)> placeItems = new Dictionary<string, (string, string, string)>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCell placeCell = dataGridView1.Rows[i].Cells[6];
                DataGridViewCell itemCell = dataGridView1.Rows[i].Cells[0];
                DataGridViewCell weightCell = dataGridView1.Rows[i].Cells[3];
                DataGridViewCell weightUnitCell = dataGridView1.Rows[i].Cells[4];

                if (placeCell.Value != null && itemCell.Value != null && weightCell.Value != null && weightUnitCell.Value != null)
                {
                    string place = placeCell.Value.ToString();
                    string item = itemCell.Value.ToString();
                    string weight = weightCell.Value.ToString();
                    string weightUnit = weightUnitCell.Value.ToString();
                    placeItems[place] = (item, weight, weightUnit);
                }
            }

            int sectionOffset = (cmbWarehouseSection.SelectedIndex);

            for (int row = 0; row < tableLayoutPanel1.RowCount; row++)
            {
                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    // Skip the second row (hallway)
                    if (row == 1)
                    {
                        continue;
                    }

                    // Calculate the place number based on the row and column
                    int placeNumber = col + 1 + (row / 2) * 20;
                
                    string place = $"{(char)('A' + sectionOffset)}{placeNumber}";
                    
                    string itemName = placeItems.TryGetValue(place, out (string Item, string Weight, string WeightUnit) itemInfo)
                        ? $"{itemInfo.Item} ({itemInfo.Weight} {itemInfo.WeightUnit})"
                        : "Trống";
                    Label placeLabel = new Label
                    {
                        Text = $"{place}: \n {itemName}",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    placeLabel.MouseEnter += PlaceLabel_MouseEnter;
                    placeLabel.MouseLeave += PlaceLabel_MouseLeave;

                    tableLayoutPanel1.Controls.Add(placeLabel, col, row);
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WarehouseMapForm_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Sort DataGridView by the desired column (replace "ColumnName" with your column's name)
                dataGridView1.Sort(dataGridView1.Columns["IngredientName"], ListSortDirection.Ascending);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            string dateColumnName = "Thời gian"; 

            if (dataGridView1.Columns.Contains(dateColumnName))
            {
                if (checkBox2.Checked)
                {
                    // Sort DataGridView by the date column
                    dataGridView1.Sort(dataGridView1.Columns[dateColumnName], ListSortDirection.Ascending);
                }
            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                // Sort DataGridView by the desired column (replace "ColumnName" with your column's name)
                dataGridView1.Sort(dataGridView1.Columns["Place"], ListSortDirection.Ascending);
            }
        }
    }
}
