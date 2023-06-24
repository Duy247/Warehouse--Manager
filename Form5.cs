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

namespace Warehouse_Manager
{
    public partial class ProductCatalogForm : Form
    {
        public ProductCatalogForm()
        {
            InitializeComponent();
            float dpiScaleFactor = GetDpiScaleFactor();
            ScaleFont(this, dpiScaleFactor);
            int fullWidthHD = 1920;
            int fullHeightHD = 1080;
            this.Size = new Size(fullWidthHD, fullHeightHD);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            // Initialize the DataGridView with headers
            dataGridView.ColumnCount = 4;
            dataGridView.Columns[0].Name = "Tên vật tư";
            dataGridView.Columns[1].Name = "Mã vật tư";
            dataGridView.Columns[2].Name = "Số lượng";
            dataGridView.Columns[3].Name = "Khối lượng";
            DataGridViewComboBoxColumn unitColumn = new DataGridViewComboBoxColumn
            {
                Name = "Unit",
                HeaderText = "Đơn vị",
                DataSource = new List<string> { "Hộp", "KG", "Cuộn", "Tờ","Mét","Tấm","Chiếc","Bình","Cái","Bộ"}
            };
            dataGridView.Columns.Add(unitColumn);
            LoadIngredients();
            LoadExistingProducts();
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
        private void LoadExistingProducts()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            List<string> productFiles = directoryInfo.GetFiles("Product-*.xlsx")
                                                     .Select(file => Path.GetFileNameWithoutExtension(file.Name))
                                                     .ToList();

            cmbExistingProducts.DataSource = productFiles;
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
        private void button2_Click(object sender, EventArgs e)
        {
            string itemName = txtItemName.Text;
            if (string.IsNullOrWhiteSpace(itemName))
            {
                MessageBox.Show("Nhập tên sản phẩm", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Show the SaveFileDialog to let the user choose where to save the Excel file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = $"Product-{itemName}.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Save the DataGridView data to the selected Excel file
                    SaveDataGridViewToExcel(dataGridView, saveFileDialog.FileName);

                    MessageBox.Show("Đã lưu thư mục", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void SaveDataGridViewToExcel(DataGridView dataGridView, string fileName)
        {
            DataTable dataTable = new DataTable();

            // Add columns to the DataTable
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.HeaderText);
            }

            // Add rows to the DataTable
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    dataTable.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dataTable.Rows[dataTable.Rows.Count - 1][i] = row.Cells[i].Value?.ToString() ?? "";
                    }
                }
            }

            // Save the DataTable to an Excel file
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                // Format Quantity and Weight columns as integers
                worksheet.Column(3).Style.Numberformat.Format = "0";
                worksheet.Column(4).Style.Numberformat.Format = "0";

                File.WriteAllBytes(fileName, package.GetAsByteArray());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Ingredient selectedIngredient = cmbIngredient.SelectedItem as Ingredient;
            if (selectedIngredient == null)
            {
                MessageBox.Show("Hãy chọn một vật tư", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ingredient = selectedIngredient.Name;
            string codeName = selectedIngredient.CodeName;
            float quantity;
            float weight;

            if (!float.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Điền giá trị số lượng hợp lệ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!float.TryParse(txtWeight.Text, out weight))
            {
                MessageBox.Show("Điền giá trị khối lượng hợp lệ, mặc định điền giống số lượng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string unit = cmbUnit.SelectedItem.ToString();
            // Add the ingredient to the DataGridView
            dataGridView.Rows.Add(ingredient, codeName, quantity, weight, unit);

            // Clear the input fields
            txtQuantity.Clear();
            txtWeight.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Remove the first selected row
                dataGridView.Rows.Remove(dataGridView.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Hãy chọn 1 dòng để xoá", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbExistingProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProduct = cmbExistingProducts.SelectedItem.ToString();
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager", $"{selectedProduct}.xlsx");
            LoadDataGridViewFromFile(filePath);
            string itemName = selectedProduct.Replace("Product-", "");
            txtItemName.Text = itemName;
        }
        private void LoadDataGridViewFromFile(string filePath)
        {
            DataTable dataTable = LoadDataTableFromWorksheet(filePath);

            dataGridView.Rows.Clear();

            foreach (DataRow row in dataTable.Rows)
            {
                dataGridView.Rows.Add(row.ItemArray);
            }
        }
        private DataTable LoadDataTableFromWorksheet(string filePath)
        {
            DataTable dataTable = new DataTable();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.End.Row;
                int totalCols = worksheet.Dimension.End.Column;

                // Adding columns to DataTable
                for (int col = 1; col <= totalCols; col++)
                {
                    string columnName = worksheet.Cells[1, col].Value.ToString();
                    dataTable.Columns.Add(columnName);
                }

                // Adding rows to DataTable
                for (int row = 2; row <= totalRows; row++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int col = 1; col <= totalCols; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
    }
}
