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

namespace Warehouse_Manager
{
    public partial class NoteForm : Form
    {
        public NoteForm()
        {
            InitializeComponent();
            float dpiScaleFactor = GetDpiScaleFactor();
            ScaleFont(this, dpiScaleFactor);
            int fullWidthHD = 1920;
            int fullHeightHD = 1080;
            this.Size = new Size(fullWidthHD, fullHeightHD);
            PopulateNoteFilesComboBox();
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
        private void PopulateNoteFilesComboBox()
        {
            string WarehouseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string NoteFolderPath = Path.Combine(WarehouseFolderPath, "Note");

            // Clear the existing items in the ComboBox.
            comboBox1.Items.Clear();

            if (Directory.Exists(NoteFolderPath))
            {
                // Get the text files that start with "Note".
                var noteFiles = Directory.GetFiles(NoteFolderPath, "Note_*.txt");

                // Add the file names without the "Note" part to the ComboBox.
                foreach (var noteFile in noteFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(noteFile).Substring(5);
                    comboBox1.Items.Add(fileName);
                }
            }
        }
        private void NoteForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string WarehouseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");

            // Create the Warehouse Manager folder if it doesn't exist.
            string NoteFolderPath = Path.Combine(WarehouseFolderPath, "Note");
            if (!Directory.Exists(NoteFolderPath))
            {
                Directory.CreateDirectory(NoteFolderPath);
            }

            // Create a new text file with a unique name in the Warehouse Manager folder.
            string formattedDate = DateTime.Now.ToString("dd_MM_yy-HH_mm");
            string textFilePath = Path.Combine(NoteFolderPath, $"Note_{formattedDate}.txt");
            File.WriteAllText(textFilePath, $"Ghi chú phía dưới được nhập ngày '{formattedDate}'");

            PopulateNoteFilesComboBox();
            string fileName = $"Note_{formattedDate}";
            comboBox1.SelectedItem = fileName.Substring(5);
            string fileContent = $"Ghi chú phía dưới được nhập ngày '{formattedDate}'";
            // Load the new file content into the RichTextBox.
            richTextBox1.Text = fileContent;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedNoteFile();
        }
        private void LoadSelectedNoteFile()
        {
            if (comboBox1.SelectedItem != null)
            {
                string WarehouseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
                string NoteFolderPath = Path.Combine(WarehouseFolderPath, "Note");
                string fileName = "Note_" + comboBox1.SelectedItem.ToString() + ".txt";
                string filePath = Path.Combine(NoteFolderPath, fileName);

                if (File.Exists(filePath))
                {
                    richTextBox1.Text = File.ReadAllText(filePath);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string WarehouseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
                string NoteFolderPath = Path.Combine(WarehouseFolderPath, "Note");
                string fileName = "Note_" + comboBox1.SelectedItem.ToString() + ".txt";
                string filePath = Path.Combine(NoteFolderPath, fileName);
                string formattedDate = DateTime.Now.ToString("dd_MM_yy-HH_mm");

                if (File.Exists(filePath))
                {
                    string content = richTextBox1.Text + Environment.NewLine + $"Ghi chú của ngày '{formattedDate}' kết thúc ";
                    File.WriteAllText(filePath, content);
                    MessageBox.Show("Ghi chú được lưu");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string WarehouseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
            string NoteFolderPath = Path.Combine(WarehouseFolderPath, "Note");

            string formattedDate = DateTime.Now.ToString("dd_MM_yy-HH_mm");
            string newFileName = $"Note_{formattedDate}.txt";
            string newFilePath = Path.Combine(NoteFolderPath, newFileName);

            string content = richTextBox1.Text + Environment.NewLine + $"Ghi chú của ngày '{formattedDate}' kết thúc ";
            File.WriteAllText(newFilePath, content);
            MessageBox.Show("Ghi chú được đã cập nhật");

            // Update the ComboBox with the new list of text files.
            PopulateNoteFilesComboBox();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DeleteCurrentNoteFile();
        }
        private void DeleteCurrentNoteFile()
        {
            if (comboBox1.SelectedItem != null)
            {
                string WarehouseFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\WarehouseManager");
                string NoteFolderPath = Path.Combine(WarehouseFolderPath, "Note");
                string fileName = "Note_" + comboBox1.SelectedItem.ToString() + ".txt";
                string filePath = Path.Combine(NoteFolderPath, fileName);

                if (File.Exists(filePath))
                {
                    DialogResult result = MessageBox.Show("Xoá ghi chú?", "Xác nhận xoá ghi chú", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(filePath);
                        MessageBox.Show("Ghi chú đã được xoá");

                        // Clear the RichTextBox content.
                        richTextBox1.Text = string.Empty;

                        // Update the ComboBox with the new list of text files.
                        PopulateNoteFilesComboBox();
                    }
                }
            }
        }
    }
}
