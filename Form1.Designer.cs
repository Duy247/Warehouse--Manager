
namespace Warehouse_Manager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.WarehouseInputForm = new System.Windows.Forms.Button();
            this.WarehouseMapForm = new System.Windows.Forms.Button();
            this.WarehouseOutputForm = new System.Windows.Forms.Button();
            this.PrintoutCheckForm = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ProductCatalogForm = new System.Windows.Forms.Button();
            this.ResetTimer = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.NoteForm = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.labelWeather = new System.Windows.Forms.Label();
            this.labelClock = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.IngredientCatalogForm = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // WarehouseInputForm
            // 
            this.WarehouseInputForm.AutoSize = true;
            this.WarehouseInputForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.WarehouseInputForm.Image = ((System.Drawing.Image)(resources.GetObject("WarehouseInputForm.Image")));
            this.WarehouseInputForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.WarehouseInputForm.Location = new System.Drawing.Point(-1, 0);
            this.WarehouseInputForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WarehouseInputForm.Name = "WarehouseInputForm";
            this.WarehouseInputForm.Size = new System.Drawing.Size(228, 141);
            this.WarehouseInputForm.TabIndex = 0;
            this.WarehouseInputForm.Text = "NHẬP KHO";
            this.WarehouseInputForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WarehouseInputForm.UseVisualStyleBackColor = true;
            this.WarehouseInputForm.Click += new System.EventHandler(this.WarehouseInputForm_Click);
            // 
            // WarehouseMapForm
            // 
            this.WarehouseMapForm.AutoSize = true;
            this.WarehouseMapForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.WarehouseMapForm.Image = ((System.Drawing.Image)(resources.GetObject("WarehouseMapForm.Image")));
            this.WarehouseMapForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.WarehouseMapForm.Location = new System.Drawing.Point(-1, 140);
            this.WarehouseMapForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WarehouseMapForm.Name = "WarehouseMapForm";
            this.WarehouseMapForm.Size = new System.Drawing.Size(228, 134);
            this.WarehouseMapForm.TabIndex = 1;
            this.WarehouseMapForm.Text = "BẢN ĐỒ KHO";
            this.WarehouseMapForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WarehouseMapForm.UseVisualStyleBackColor = true;
            this.WarehouseMapForm.Click += new System.EventHandler(this.WarehouseMapForm_Click);
            // 
            // WarehouseOutputForm
            // 
            this.WarehouseOutputForm.AutoSize = true;
            this.WarehouseOutputForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.WarehouseOutputForm.Image = ((System.Drawing.Image)(resources.GetObject("WarehouseOutputForm.Image")));
            this.WarehouseOutputForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.WarehouseOutputForm.Location = new System.Drawing.Point(-1, 271);
            this.WarehouseOutputForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WarehouseOutputForm.Name = "WarehouseOutputForm";
            this.WarehouseOutputForm.Size = new System.Drawing.Size(228, 134);
            this.WarehouseOutputForm.TabIndex = 2;
            this.WarehouseOutputForm.Text = "XUẤT KHO";
            this.WarehouseOutputForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WarehouseOutputForm.UseVisualStyleBackColor = true;
            this.WarehouseOutputForm.Click += new System.EventHandler(this.WarehouseOutputForm_Click);
            // 
            // PrintoutCheckForm
            // 
            this.PrintoutCheckForm.AutoSize = true;
            this.PrintoutCheckForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PrintoutCheckForm.Image = ((System.Drawing.Image)(resources.GetObject("PrintoutCheckForm.Image")));
            this.PrintoutCheckForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintoutCheckForm.Location = new System.Drawing.Point(-1, 401);
            this.PrintoutCheckForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PrintoutCheckForm.Name = "PrintoutCheckForm";
            this.PrintoutCheckForm.Size = new System.Drawing.Size(228, 134);
            this.PrintoutCheckForm.TabIndex = 3;
            this.PrintoutCheckForm.Text = "XUẤT PHIẾU";
            this.PrintoutCheckForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PrintoutCheckForm.UseVisualStyleBackColor = true;
            this.PrintoutCheckForm.Click += new System.EventHandler(this.PrintoutCheckForm_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.RowTemplate.Height = 41;
            this.dataGridView1.Size = new System.Drawing.Size(667, 547);
            this.dataGridView1.TabIndex = 4;
            // 
            // ProductCatalogForm
            // 
            this.ProductCatalogForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ProductCatalogForm.Image = ((System.Drawing.Image)(resources.GetObject("ProductCatalogForm.Image")));
            this.ProductCatalogForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ProductCatalogForm.Location = new System.Drawing.Point(208, 0);
            this.ProductCatalogForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ProductCatalogForm.Name = "ProductCatalogForm";
            this.ProductCatalogForm.Size = new System.Drawing.Size(266, 117);
            this.ProductCatalogForm.TabIndex = 5;
            this.ProductCatalogForm.Text = "THƯ MỤC SẢN PHẨM";
            this.ProductCatalogForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ProductCatalogForm.UseVisualStyleBackColor = true;
            this.ProductCatalogForm.Click += new System.EventHandler(this.ProductCatalogForm_Click);
            // 
            // ResetTimer
            // 
            this.ResetTimer.Enabled = true;
            this.ResetTimer.Interval = 8000;
            this.ResetTimer.Tick += new System.EventHandler(this.ResetTimer_Tick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(473, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 117);
            this.button1.TabIndex = 8;
            this.button1.Text = "LÀM MỚI";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Moccasin;
            this.panel1.Controls.Add(this.NoteForm);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.PrintoutCheckForm);
            this.panel1.Controls.Add(this.WarehouseOutputForm);
            this.panel1.Controls.Add(this.WarehouseMapForm);
            this.panel1.Controls.Add(this.WarehouseInputForm);
            this.panel1.Location = new System.Drawing.Point(4, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 670);
            this.panel1.TabIndex = 9;
            // 
            // NoteForm
            // 
            this.NoteForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NoteForm.Image = ((System.Drawing.Image)(resources.GetObject("NoteForm.Image")));
            this.NoteForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NoteForm.Location = new System.Drawing.Point(-1, 534);
            this.NoteForm.Name = "NoteForm";
            this.NoteForm.Size = new System.Drawing.Size(228, 136);
            this.NoteForm.TabIndex = 4;
            this.NoteForm.Text = "GHI CHÚ";
            this.NoteForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.NoteForm.UseVisualStyleBackColor = true;
            this.NoteForm.Click += new System.EventHandler(this.NoteForm_Click);
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(679, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2084, 758);
            this.panel3.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.pictureBox5);
            this.panel6.Controls.Add(this.labelWeather);
            this.panel6.Controls.Add(this.labelClock);
            this.panel6.Controls.Add(this.pictureBox4);
            this.panel6.Location = new System.Drawing.Point(679, 423);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(475, 236);
            this.panel6.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(-1, -40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Thống kê vật tư";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(17, 123);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(106, 100);
            this.pictureBox5.TabIndex = 3;
            this.pictureBox5.TabStop = false;
            // 
            // labelWeather
            // 
            this.labelWeather.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelWeather.Location = new System.Drawing.Point(142, 123);
            this.labelWeather.Name = "labelWeather";
            this.labelWeather.Size = new System.Drawing.Size(333, 100);
            this.labelWeather.TabIndex = 2;
            this.labelWeather.Text = "Đang nạp thông tin thời tiết";
            this.labelWeather.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelClock
            // 
            this.labelClock.BackColor = System.Drawing.Color.Transparent;
            this.labelClock.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelClock.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelClock.Location = new System.Drawing.Point(127, 7);
            this.labelClock.Name = "labelClock";
            this.labelClock.Size = new System.Drawing.Size(348, 105);
            this.labelClock.TabIndex = 1;
            this.labelClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelClock.Click += new System.EventHandler(this.labelClock_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(17, 11);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(104, 101);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightBlue;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.ProductCatalogForm);
            this.panel2.Controls.Add(this.IngredientCatalogForm);
            this.panel2.Location = new System.Drawing.Point(4, 553);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(667, 117);
            this.panel2.TabIndex = 10;
            // 
            // IngredientCatalogForm
            // 
            this.IngredientCatalogForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IngredientCatalogForm.Image = ((System.Drawing.Image)(resources.GetObject("IngredientCatalogForm.Image")));
            this.IngredientCatalogForm.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IngredientCatalogForm.Location = new System.Drawing.Point(0, 0);
            this.IngredientCatalogForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.IngredientCatalogForm.Name = "IngredientCatalogForm";
            this.IngredientCatalogForm.Size = new System.Drawing.Size(209, 117);
            this.IngredientCatalogForm.TabIndex = 6;
            this.IngredientCatalogForm.Text = "THƯ MỤC VẬT TƯ";
            this.IngredientCatalogForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.IngredientCatalogForm.UseVisualStyleBackColor = true;
            this.IngredientCatalogForm.Click += new System.EventHandler(this.IngredientCatalogForm_Click);
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.BackColor = System.Drawing.Color.Gainsboro;
            this.panel4.Controls.Add(this.dataGridView2);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Location = new System.Drawing.Point(234, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1161, 673);
            this.panel4.TabIndex = 11;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(679, 0);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 82;
            this.dataGridView2.RowTemplate.Height = 41;
            this.dataGridView2.Size = new System.Drawing.Size(475, 416);
            this.dataGridView2.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1395, 673);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trình quản lý kho";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button WarehouseInputForm;
        private System.Windows.Forms.Button WarehouseMapForm;
        private System.Windows.Forms.Button WarehouseOutputForm;
        private System.Windows.Forms.Button PrintoutCheckForm;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button ProductCatalogForm;
        private System.Windows.Forms.Timer ResetTimer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button IngredientCatalogForm;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label labelClock;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label labelWeather;
        private System.Windows.Forms.Button NoteForm;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}

