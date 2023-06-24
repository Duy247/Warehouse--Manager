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
    public partial class WarehouseOutputForm : Form
    {
        public WarehouseOutputForm()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            InitializeComponent();
            float dpiScaleFactor = GetDpiScaleFactor();
            ScaleFont(this, dpiScaleFactor);
            int fullWidthHD = 1920;
            int fullHeightHD = 1080;
            this.Size = new Size(fullWidthHD, fullHeightHD);
            InitializeDepartments();
            LoadProductList();
            Load_Warehouse();
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
        private void InitializeDepartments()
        {
            cmbDepartments.Items.Add("Department1");
            cmbDepartments.Items.Add("Department2");
            cmbDepartments.Items.Add("Department3");
        }
        private void Load_Warehouse()
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");

            if (File.Exists(warehouseDataExcelPath))
            {
                using (FileStream stream = new FileStream(warehouseDataExcelPath, FileMode.Open, FileAccess.Read))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
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
            else
            {
                MessageBox.Show("WarehouseData.xlsx not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");

            if (File.Exists(warehouseDataExcelPath))
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                using (FileStream stream = new FileStream(warehouseDataExcelPath, FileMode.Open, FileAccess.Read))
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
            else
            {
                MessageBox.Show("WarehouseData.xlsx not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDepartment = cmbDepartments.SelectedItem.ToString();
            LoadDepartmentData(selectedDepartment);
            if (cmbDepartments.SelectedItem == null)
            {
                // Load the department data when a new department is selected
                MessageBox.Show("Hãy chọn một tổ", "Chưa chọn tổ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        private void LoadDepartmentData(string departmentName)
        {
            
            string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string warehouseManagerFolder = Path.Combine(downloadsFolder, "Downloads", "WarehouseManager");
            string excelPath = Path.Combine(warehouseManagerFolder, departmentName + ".xlsx");

            if (!File.Exists(excelPath))
            {
                CreateDepartmentExcelFile(excelPath);
            }
            if (File.Exists(excelPath))
            {
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
                    }
                }
            }
            else
            {
                MessageBox.Show($"Tệp tin excel cho {departmentName} không tồn tại. Hãy mở lại trình duyệt.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateDepartmentExcelFile(string excelPath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add header row
                worksheet.Cells[1, 1].Value = "Tên vật tư";
                worksheet.Cells[1, 2].Value = "Mã vật tư";
                worksheet.Cells[1, 3].Value = "Số lượng";
                worksheet.Cells[1, 4].Value = "Khối lượng";
                worksheet.Cells[1, 5].Value = "Đơn vị";
                worksheet.Cells[1, 6].Value = "Thời gian";
                worksheet.Cells[1, 7].Value = "Từ vị trí";

                // Save the new Excel file
                package.SaveAs(new FileInfo(excelPath));
            }
        }
        private DataTable GetDataTableFromDataGridView(DataGridView dataGridView)
        {
            return (DataTable)dataGridView.DataSource;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("Hãy chọn một vật tư để chuyển", "Chưa chọn vật tư", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected item
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedItem = dataGridView1.Rows[selectedRowIndex];

            // Get the move amount from the TextBox
            if (!float.TryParse(txtMoveAmount.Text, out float moveAmount) || moveAmount <= 0)
            {
                MessageBox.Show("Hãy điền một giá trị để chuyển", "Chưa kê khai giá trị", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure that only one of the CheckBoxes is checked
            if (chkMoveByQuantity.Checked == chkMoveByWeight.Checked)
            {
                MessageBox.Show("Hãy chọn chuyển theo số lượng hoặc khối lượng.\n Mặc định số lượng và khối lượng là giống nhau, có thể chọn bất kỳ", "Chưa chọn phương thức chuyển", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Determine the move type and the corresponding column index
            int columnToCompare = chkMoveByQuantity.Checked ? 2 : 3;

            // Get the item to move and the current amount
            string itemToMove = selectedItem.Cells[0].Value.ToString();
            float currentAmount = Convert.ToSingle(selectedItem.Cells[columnToCompare].Value);

            if (moveAmount > currentAmount)
            {
                MessageBox.Show($"Không thể chuyển vượt quá số lượng có sẵn. Số lượng có sẵn: {currentAmount}", "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate the proportion of the moved amount to the total amount
            double moveProportion = (double)moveAmount / currentAmount;

            // Update the item's quantity and weight in the warehouse
            float currentQuantity = Convert.ToSingle(selectedItem.Cells[2].Value);
            float currentWeight = Convert.ToSingle(selectedItem.Cells[3].Value);


            float movedQuantity = chkMoveByQuantity.Checked ? moveAmount : (float)(currentQuantity * moveProportion);
            float movedWeight = chkMoveByQuantity.Checked ? (float)(currentWeight * moveProportion) : moveAmount;

            selectedItem.Cells[2].Value = currentQuantity - movedQuantity;
            selectedItem.Cells[3].Value = currentWeight - movedWeight;

            // Update the item's quantity and weight in the department
            DataTable departmentDataTable = GetDataTableFromDataGridView(dataGridView2);
            DataGridViewRow foundRow = null;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == itemToMove)
                {
                    foundRow = row;
                    break;
                }
            }

            if (foundRow != null)
            {
                foundRow.Cells[2].Value = Convert.ToSingle(foundRow.Cells[2].Value) + movedQuantity;
                foundRow.Cells[3].Value = Convert.ToSingle(foundRow.Cells[3].Value) + movedWeight;
            }
            else
            {
                // If the item is not found in the department, add a new row with the moved amount
                DataRow newRow = departmentDataTable.NewRow();
                newRow.ItemArray = selectedItem.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value).ToArray();
                newRow[2] = movedQuantity;
                newRow[3] = movedWeight;
                departmentDataTable.Rows.Add(newRow);
            }
            // Remove rows with 0 quantity from the warehouse DataGridView
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (!row.IsNewRow)
                {
                    float quantityInWarehouse = Convert.ToSingle(row.Cells[2].Value);
                    if (quantityInWarehouse == 0)
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }
            }

            // Remove rows with 0 quantity from the department DataGridView
            for (int i = dataGridView2.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dataGridView2.Rows[i];
                if (!row.IsNewRow)
                {
                    float quantityInDepartment = Convert.ToSingle(row.Cells[2].Value);
                    if (quantityInDepartment == 0)
                    {
                        dataGridView2.Rows.RemoveAt(i);
                    }
                }
            }
            // Update the warehouse and department Excel files
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");
            string departmentExcelPath = Path.Combine(downloadsFolderPath, $"{cmbDepartments.SelectedItem}.xlsx");

            UpdateExcelFile(warehouseExcelPath, dataGridView1);
            UpdateExcelFile(departmentExcelPath, dataGridView2);
            string downloadsFolderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseData.xlsx");
            string warehouseShortExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseShort.xlsx");
            SaveWarehouseShortData(warehouseDataExcelPath1, warehouseShortExcelPath1);
        }
        private void UpdateExcelFile(string excelPath, DataGridView dataGridView)
        {
            using (FileStream stream = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Clear the data rows if there are any
                    if (worksheet.Dimension != null && worksheet.Dimension.End.Row > 1)
                    {
                        worksheet.DeleteRow(2, worksheet.Dimension.End.Row - 1);
                    }

                    // Write the new data rows
                    for (int row = 0; row < dataGridView.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataGridView.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataGridView.Rows[row].Cells[col].Value;
                        }
                    }

                    // Save the changes
                    package.Save();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count == 0)
            {
                MessageBox.Show("Hãy chọn một vật tư để chuyển lại", "Chưa chọn vật tư", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected item
            int selectedRowIndex = dataGridView2.SelectedCells[0].RowIndex;
            DataGridViewRow selectedItem = dataGridView2.Rows[selectedRowIndex];

            // Get the move amount from the TextBox
            if (!float.TryParse(txtMoveAmount.Text, out float moveAmount) || moveAmount <= 0)
            {
                MessageBox.Show("Hãy điền một giá trị hợp lệ", "Giá trị không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure that only one of the CheckBoxes is checked
            if (chkMoveByQuantity.Checked == chkMoveByWeight.Checked)
            {
                MessageBox.Show("Hãy chọn chuyển theo số lượng hoặc khối lượng.\n Mặc định số lượng và khối lượng là giống nhau, có thể chọn bất kỳ", "Chưa chọn phương thức chuyển", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Determine the move type and the corresponding column index
            int columnToCompare = chkMoveByQuantity.Checked ? 2 : 3;

            // Get the item to move and the current amount
            string itemToMove = selectedItem.Cells[0].Value.ToString();
            float currentAmount = Convert.ToSingle(selectedItem.Cells[columnToCompare].Value);

            if (moveAmount > currentAmount)
            {
                MessageBox.Show($"Không thể chuyển vượt quá số lượng có sẵn. Số lượng có sẵn: {currentAmount}", "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate the proportion of the moved amount to the total amount
            double moveProportion = (double)moveAmount / currentAmount;

            // Update the item's quantity and weight in the department
            float currentQuantity = Convert.ToSingle(selectedItem.Cells[2].Value);
            float currentWeight = Convert.ToSingle(selectedItem.Cells[3].Value);

            float movedQuantity = chkMoveByQuantity.Checked ? moveAmount : (float)(currentQuantity * moveProportion);
            float movedWeight = chkMoveByQuantity.Checked ? (float)(currentWeight * moveProportion) : moveAmount;

            selectedItem.Cells[2].Value = currentQuantity - movedQuantity;
            selectedItem.Cells[3].Value = currentWeight - movedWeight;

            // Update the item's quantity and weight in the warehouse
            DataTable warehouseDataTable = GetDataTableFromDataGridView(dataGridView1);
            DataGridViewRow foundRow = null;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == itemToMove)
                {
                    foundRow = row;
                    break;
                }
            }

            if (foundRow != null)
            {
                foundRow.Cells[2].Value = Convert.ToSingle(foundRow.Cells[2].Value) + movedQuantity;
                foundRow.Cells[3].Value = Convert.ToSingle(foundRow.Cells[3].Value) + movedWeight;
            }
            else
            {
                // If the item is not found in the warehouse, add a new row with the moved amount
                DataRow newRow = warehouseDataTable.NewRow();
                newRow.ItemArray = selectedItem.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value).ToArray();
                newRow[2] = movedQuantity;
                newRow[3] = movedWeight;
                warehouseDataTable.Rows.Add(newRow);
            }
            // Remove rows with 0 quantity from the warehouse DataGridView
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (!row.IsNewRow)
                {
                    float quantityInWarehouse = Convert.ToSingle(row.Cells[2].Value);
                    if (quantityInWarehouse == 0)
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }
            }

            // Remove rows with 0 quantity from the department DataGridView
            for (int i = dataGridView2.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dataGridView2.Rows[i];
                if (!row.IsNewRow)
                {
                    float quantityInDepartment = Convert.ToSingle(row.Cells[2].Value);
                    if (quantityInDepartment == 0)
                    {
                        dataGridView2.Rows.RemoveAt(i);
                    }
                }
            }
            // Update the warehouse and department Excel files
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");
            string departmentExcelPath = Path.Combine(downloadsFolderPath, $"{cmbDepartments.SelectedItem}.xlsx");
            string downloadsFolderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseData.xlsx");
            string warehouseShortExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseShort.xlsx");
            SaveWarehouseShortData(warehouseDataExcelPath1, warehouseShortExcelPath1);
            UpdateExcelFile(warehouseExcelPath, dataGridView1);
            UpdateExcelFile(departmentExcelPath, dataGridView2);
        }


        private void button4_Click(object sender, EventArgs e)
        {
            // Update the warehouse and department Excel files
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");
            string departmentExcelPath = Path.Combine(downloadsFolderPath, $"{cmbDepartments.SelectedItem}.xlsx");

            CreateNewExcelFileAndReplaceOld(warehouseExcelPath, dataGridView1);
            CreateNewExcelFileAndReplaceOld(departmentExcelPath, dataGridView2);
            string downloadsFolderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseData.xlsx");
            string warehouseShortExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseShort.xlsx");
            SaveWarehouseShortData(warehouseDataExcelPath1, warehouseShortExcelPath1);

            MessageBox.Show("Changes saved successfully.", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void CreateNewExcelFileAndReplaceOld(string oldExcelPath, DataGridView dataGridView)
        {
            // Create a new Excel file with the updated data
            string newExcelPath = $"{oldExcelPath}.tmp";
            using (ExcelPackage package = new ExcelPackage(new FileInfo(newExcelPath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Write the column headers
                for (int col = 0; col < dataGridView.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dataGridView.Columns[col].HeaderText;
                }

                // Write the data rows
                for (int row = 0; row < dataGridView.Rows.Count; row++)
                {
                    for (int col = 0; col < dataGridView.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dataGridView.Rows[row].Cells[col].Value;
                    }
                }

                // Save the new Excel file
                package.Save();
            }

            // Replace the old Excel file with the new one
            File.Delete(oldExcelPath);
            File.Move(newExcelPath, oldExcelPath);
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
        private void LoadProductList()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            List<string> productFiles = Directory.EnumerateFiles(folderPath, "Product-*.xlsx")
                                                .Select(Path.GetFileNameWithoutExtension)
                                                .Select(file => file.Replace("Product-", ""))
                                                .ToList();

            cmbProducts.DataSource = productFiles;
        }
        private void LoadProductData1(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Tệp tin sản phẩm không tồn tại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            dataGridViewProducts.DataSource = dataTable;
        }


        private List<ProductData> LoadProductData(string filePath)
        {
            List<ProductData> productDataList = new List<ProductData>();

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Tệp tin sản phẩm không tồn tại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return productDataList;
            }

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    ProductData productData = new ProductData
                    {
                        Item = worksheet.Cells[row, 1].GetValue<string>(),
                        Quantity = worksheet.Cells[row, 2].GetValue<float>(),
                        Weight = worksheet.Cells[row, 3].GetValue<float>(),
                        Unit = worksheet.Cells[row,4].GetValue<string>()
                    };
                    productDataList.Add(productData);
                }
            }

            return productDataList;
        }
        private List<ProductData> _productData;
        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string productName = cmbProducts.SelectedItem.ToString();
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager", $"Product-{productName}.xlsx");
            LoadProductData1(filePath);
            _productData = LoadProductData(filePath);
        }
        public class ProductData
        {
            public string Item { get; set; }
            public float Quantity { get; set; }
            public float Weight { get; set; }

            public string Unit { get; set; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_productData == null)
            {
                MessageBox.Show("Hãy chọn một sản phẩm để chuyển", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable warehouseTable = (DataTable)dataGridView1.DataSource;
            DataTable departmentTable = (DataTable)dataGridView2.DataSource;

            foreach (ProductData product in _productData)
            {
                string itemToMove = product.Item;
                float moveAmount = product.Quantity;
                DateTime selectedDate = datePicker.Value;
                if (!float.TryParse(txtAmount.Text, out float multiplier))
                {
                    MessageBox.Show("Hãy điền một giá trị hợp lệ vào mục số lượng. Néu không cần định số lượng, điền 1.", "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                moveAmount *= multiplier;
                float remainingAmountToMove = moveAmount;
                // Find the item in warehouseTable
                DataRow foundRow = warehouseTable.AsEnumerable()
                    .FirstOrDefault(row => row.Field<string>("Tên vật tư") == itemToMove);

                if (foundRow != null)
                {
                    // Get the current quantity and weight from warehouseTable
                    float currentQuantity = Convert.ToSingle(foundRow["Số lượng"]);
                    float currentWeight = Convert.ToSingle(foundRow["Khối lượng"]);
                    string unit = foundRow.Field<string>("Đơn vị");
                    string dateString = foundRow.Field<string>("Thời gian");
                    DateTime.TryParse(dateString, out DateTime date);
                    string customColumnValue = foundRow.Field<string>(5);
                    string takenFromPlace = foundRow.Field<string>(5); 

                    if (moveAmount > currentQuantity)
                    {
                        MessageBox.Show($"Số lượng vật tư tại vị trí này không đủ, hãy chuyển {currentQuantity} từ vị trí này và {moveAmount- currentQuantity} từ vị trí khác", "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Calculate the proportion of the moved amount to the total amount
                    double moveProportion = (double)moveAmount / currentQuantity;

                    // Update the item's quantity and weight in warehouseTable
                    float movedQuantity = Math.Min(remainingAmountToMove, currentQuantity);
                    float movedWeight = (float)(currentWeight * moveProportion);
                    foundRow.SetField("Số lượng", Convert.ToString(currentQuantity - movedQuantity));
                    foundRow.SetField("Khối lượng", Convert.ToString(currentWeight - movedWeight));

                    // Find the item in departmentTable
                    DataRow foundRow2 = departmentTable.AsEnumerable()
                        .FirstOrDefault(row => row.Field<string>("Tên vật tư") == itemToMove && row.Field<string>(5) == customColumnValue);
                    

                    if (foundRow2 != null)
                    {
                        //Update the item's quantity, weight, unit, date, and taken from place in departmentTable
                        foundRow2.SetField("Số lượng", Convert.ToString(Convert.ToSingle(foundRow2["Số lượng"]) + movedQuantity));
                        foundRow2.SetField("Khối lượng", Convert.ToString(Convert.ToSingle(foundRow2["Khối lượng"]) + movedWeight));
                        foundRow2.SetField("Đơn vị", unit);
                        foundRow2.SetField("Thời gian", selectedDate);
                        foundRow2.SetField("Từ vị trí", takenFromPlace);
                    }
                    else
                    {
                        // If the item is not found in departmentTable, add a new row with the moved amount
                        DataRow newRow = departmentTable.NewRow();
                        newRow.SetField("Tên vật tư", itemToMove);
                        newRow.SetField("Số lượng", movedQuantity);
                        newRow.SetField("Khối lượng", movedWeight);
                        newRow.SetField("Đơn vị", unit);
                        newRow.SetField("Ngày", selectedDate);
                        newRow.SetField("Từ vị trí", takenFromPlace);
                        newRow.SetField(5, customColumnValue); // Set the "CustomColumn" value
                        departmentTable.Rows.Add(newRow);
                        remainingAmountToMove -= movedQuantity;
                    }
                }
            }

            // Remove rows with 0 quantity from warehouseTable
            for (int i = warehouseTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = warehouseTable.Rows[i];
                float quantityInWarehouse = Convert.ToSingle(row["Số lượng"]);
                if (quantityInWarehouse == 0)
                {
                    warehouseTable.Rows.RemoveAt(i);
                }
            }

            // Remove rows with 0 quantity from departmentTable
            for (int i = departmentTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = departmentTable.Rows[i];
                float quantityInDepartment = Convert.ToSingle(row["Số lượng"]);
                if (quantityInDepartment == 0)
                {
                    departmentTable.Rows.RemoveAt(i);
                }
            }

            // Update the warehouse and department Excel files
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseExcelPath = Path.Combine(downloadsFolderPath, "WarehouseData.xlsx");
            string departmentExcelPath = Path.Combine(downloadsFolderPath, $"{cmbDepartments.SelectedItem}.xlsx");

            UpdateExcelFile(warehouseExcelPath, dataGridView1);
            UpdateExcelFile(departmentExcelPath, dataGridView2);
            string downloadsFolderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string warehouseDataExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseData.xlsx");
            string warehouseShortExcelPath1 = Path.Combine(downloadsFolderPath1, "WarehouseShort.xlsx");
            SaveWarehouseShortData(warehouseDataExcelPath1, warehouseShortExcelPath1);
        }

        private void WarehouseOutputForm_Load(object sender, EventArgs e)
        {

        }
    }
}
