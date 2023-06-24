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
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

namespace Warehouse_Manager
{
    public partial class PrintoutCheckForm : Form
    {
        public PrintoutCheckForm()
        {
            InitializeComponent();
            InitializeDepartments();
        }
        private void InitializeDepartments()
        {
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cmbDepartments.Items.Add("Department1");
            cmbDepartments.Items.Add("Department2");
            cmbDepartments.Items.Add("Department3");
          
            cmbRDepartment.Items.Add("Department1");
            cmbRDepartment.Items.Add("Department2");
            cmbRDepartment.Items.Add("Department3");
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

                        dataGridView1.DataSource = dt;
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
        private void PrintoutCheckForm_Load(object sender, EventArgs e)
        {

        }

        private void cmbDepartments_SelectedIndexChanged_1(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string resourcesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string wordDocumentPath = Path.Combine(resourcesFolderPath, "Template.docx");

            if (File.Exists(wordDocumentPath))
            {
                Type wordType = Type.GetTypeFromProgID("Word.Application");
                dynamic wordApp = Activator.CreateInstance(wordType);
                wordApp.Visible = false;

                // Open the Word document
                dynamic wordDoc = wordApp.Documents.Open(wordDocumentPath);
                // Replace the placeholders with the desired text
                ReplacePlaceholder(wordDoc, "<txtReceive>", txtReceive.Text);
                ReplacePlaceholder(wordDoc, "<txtRDepartment>", cmbRDepartment.SelectedItem.ToString());
                ReplacePlaceholder(wordDoc, "<txtReason>", txtReason.Text);
                ReplacePlaceholder(wordDoc, "<txtWarehouse>", txtWarehouse.Text);
                ReplacePlaceholder(wordDoc, "<txtCt>", txtCt.Text);
                ReplacePlaceholder(wordDoc, "<txtL>", txtL.Text);
                ReplacePlaceholder(wordDoc, "<txtTKN>", txtTKN.Text);
                ReplacePlaceholder(wordDoc, "<txtTKC>", txtTKC.Text);
                ReplacePlaceholder(wordDoc, "<dd>", dateTimePicker1.Value.Day.ToString("00"));
                ReplacePlaceholder(wordDoc, "<MM>", dateTimePicker1.Value.Month.ToString("00"));
                ReplacePlaceholder(wordDoc, "<yyyy>", dateTimePicker1.Value.Year.ToString());

                dynamic tableBookmark = wordDoc.Bookmarks["myTable"];
                dynamic myTable = tableBookmark.Range.Tables[1];

                int insertRowIndex = 2; // Start inserting new rows at the second row of the table
                dynamic sourceRow = myTable.Rows[2];
                bool firstInsert = true;
                double sum = 0;

                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dynamic newRow;

                    if (firstInsert)
                    {
                        newRow = myTable.Rows[insertRowIndex];
                        firstInsert = false;
                    }
                    else
                    {
                        newRow = myTable.Rows[insertRowIndex-1];
                        sourceRow.Select();
                        wordApp.Selection.Copy();
                        newRow.Select();
                        wordApp.Selection.Paste();
                        newRow.Select();
                    }
                    newRow.Cells[1].Range.Text = insertRowIndex-1 ;
                    newRow.Cells[2].Range.Text = row.Cells[0].Value.ToString();
                    newRow.Cells[3].Range.Text = row.Cells[1].Value.ToString();
                    newRow.Cells[4].Range.Text = txtTKN.Text;               
                    newRow.Cells[6].Range.Text = row.Cells[2].Value.ToString();
                    newRow.Cells[5].Range.Text = row.Cells[4].Value.ToString();
                    bool validPriceInput;
                    double price;
                    do
                    {
                        validPriceInput = true;
                        string priceInput = Microsoft.VisualBasic.Interaction.InputBox($"Đơn giá của {row.Cells[0].Value.ToString()}", "Đơn giá");
                        if (double.TryParse(priceInput, out price))
                        {
                            newRow.Cells[7].Range.Text = AddSpaceEveryThreeCharacters(price.ToString());
                            if (double.TryParse(row.Cells[2].Value.ToString(), out double cellValue))
                            {
                                double result = price * cellValue;
                                sum += result;
                                newRow.Cells[8].Range.Text = AddSpaceEveryThreeCharacters(result.ToString());
                            }
                        }
                        else
                        {
                            validPriceInput = false;
                            MessageBox.Show("Đơn giá không hợp lệ. Hãy điền một giá trị hợp lệ, ví dụ 1 triệu 500 ngàn đồng sẽ được viết là 1500000.", "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } while (!validPriceInput);
                    // Increment the insertRowIndex
                    insertRowIndex++;
                }
                myTable.Rows[myTable.Rows.Count-1].Delete();
                myTable.Rows[myTable.Rows.Count].Cells[2].Range.Text = AddSpaceEveryThreeCharacters(sum.ToString());
                // Save the new document as test.docx
                string newDocumentPath = Path.Combine(downloadsFolderPath, $"Check{dateTimePicker1.Value.Day.ToString("00")}{dateTimePicker1.Value.Month.ToString("00")}.docx");
                wordDoc.SaveAs2(newDocumentPath);
                wordApp.Visible = true;


                // Release the COM objects
                Marshal.ReleaseComObject(wordDoc);
                Marshal.ReleaseComObject(wordApp);
            }
            else
            {
                MessageBox.Show("Template.docx not found.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string AddSpaceEveryThreeCharacters(string input)
        {
            int count = 0;
            StringBuilder sb = new StringBuilder();

            for (int i = input.Length - 1; i >= 0; i--)
            {
                sb.Insert(0, input[i]);
                count++;

                if (count % 3 == 0 && i != 0)
                {
                    sb.Insert(0, ' ');
                }
            }

            return sb.ToString();
        }
        private void ReplacePlaceholder(dynamic wordDoc, string placeholder, string replacementText)
        {
            Word.Range range = wordDoc.Content;
            range.Find.ClearFormatting();
            range.Find.Text = placeholder;
            range.Find.Replacement.ClearFormatting();
            range.Find.Replacement.Text = replacementText;

            object replaceAll = Word.WdReplace.wdReplaceAll;
            range.Find.Execute(Replace: replaceAll);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
