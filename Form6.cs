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
    public partial class IngredientCatalogForm : Form
    {
        private string _ingredientFilePath;
        public IngredientCatalogForm()
        {
            InitializeComponent();
            float dpiScaleFactor = GetDpiScaleFactor();
            ScaleFont(this, dpiScaleFactor);
            int fullWidthHD = 1920;
            int fullHeightHD = 1080;
            this.Size = new Size(fullWidthHD, fullHeightHD);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            // Initialize the DataGridView with headers
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Tên vật tư";
            dataGridView1.Columns[1].Name = "Mã vật tư";

            _ingredientFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager", "Ingredient.xlsx");

            LoadIngredients();
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
        private void LoadIngredients()
        {
            if (!File.Exists(_ingredientFilePath))
            {
                CreateNewIngredientExcelFile(_ingredientFilePath);
            }
            else
            {
                DataTable dataTable = ReadIngredientsFromExcel(_ingredientFilePath);
                foreach (DataRow row in dataTable.Rows)
                {
                    dataGridView1.Rows.Add(row.ItemArray);
                }
            }
        }
        private void CreateNewIngredientExcelFile(string filePath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add column header
                worksheet.Cells[1, 1].Value = "Tên vật tư";
                worksheet.Cells[1, 2].Value = "Mã vật tư";

                // Save the Excel file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
            }
        }
        private DataTable ReadIngredientsFromExcel(string filePath)
        {
            DataTable dataTable = new DataTable();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int col = 1; col <= colCount; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, col].GetValue<string>());
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].GetValue<string>();
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string ingredient = txtIngredient.Text;
            string codeName = txtCodeName.Text;

            if (string.IsNullOrWhiteSpace(ingredient))
            {
                MessageBox.Show("Please enter an ingredient.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Add the ingredient to the DataGridView
            dataGridView1.Rows.Add(ingredient, codeName);

            // Clear the input field
            txtIngredient.Clear();
            txtCodeName.Clear();

            // Save the DataGridView to the Excel file
            SaveIngredientsToExcel(dataGridView1, _ingredientFilePath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                // Save the DataGridView to the Excel file
                SaveIngredientsToExcel(dataGridView1, _ingredientFilePath);
            }
        }
        private void SaveIngredientsToExcel(DataGridView dataGridView, string filePath)
        {
            DataTable dataTable = new DataTable();

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.Name);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue; // Skip the new row placeholder
                DataRow dataRow = dataTable.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dataRow[cell.ColumnIndex] = cell.Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                File.WriteAllBytes(filePath, package.GetAsByteArray());
            }
        }

        private void IngredientCatalogForm_Load(object sender, EventArgs e)
        {

        }
    }
}
