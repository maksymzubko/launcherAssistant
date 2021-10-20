namespace LauncherSchool
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.customPanel1 = new LauncherSchool.CustomElements.CustomPanel();
            this.customButton1 = new LauncherSchool.CustomElements.CustomButton(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.customComboBox1 = new School.Data.CustomElements.CustomComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.customPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.Alpha1 = 150;
            this.customPanel1.Alpha2 = 200;
            this.customPanel1.Angle = 45F;
            this.customPanel1.ColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(175)))), ((int)(((byte)(186)))));
            this.customPanel1.ColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(22)))), ((int)(((byte)(132)))));
            this.customPanel1.Controls.Add(this.customButton1);
            this.customPanel1.Controls.Add(this.label2);
            this.customPanel1.Controls.Add(this.customComboBox1);
            this.customPanel1.Controls.Add(this.label1);
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(437, 318);
            this.customPanel1.TabIndex = 12;
            // 
            // customButton1
            // 
            this.customButton1.BackColor = System.Drawing.Color.SlateBlue;
            this.customButton1.BackgroundColor = System.Drawing.Color.SlateBlue;
            this.customButton1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.customButton1.BorderRadius = 20;
            this.customButton1.BorderSize = 2;
            this.customButton1.FlatAppearance.BorderSize = 0;
            this.customButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.customButton1.ForeColor = System.Drawing.Color.White;
            this.customButton1.Location = new System.Drawing.Point(135, 204);
            this.customButton1.Name = "customButton1";
            this.customButton1.RoundMode = false;
            this.customButton1.Size = new System.Drawing.Size(155, 39);
            this.customButton1.TabIndex = 10;
            this.customButton1.Text = "Сохранить";
            this.customButton1.TextColor = System.Drawing.Color.White;
            this.customButton1.UseVisualStyleBackColor = false;
            this.customButton1.Click += new System.EventHandler(this.customButton1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(94, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Выберите язык приложения";
            // 
            // customComboBox1
            // 
            this.customComboBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.customComboBox1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.customComboBox1.BorderSize = 1;
            this.customComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.customComboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customComboBox1.ForeColor = System.Drawing.Color.DimGray;
            this.customComboBox1.IconColor = System.Drawing.Color.MediumSlateBlue;
            this.customComboBox1.Items.AddRange(new object[] {
            "Русский",
            "Українська"});
            this.customComboBox1.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.customComboBox1.ListTextColor = System.Drawing.Color.DimGray;
            this.customComboBox1.Location = new System.Drawing.Point(111, 144);
            this.customComboBox1.MinimumSize = new System.Drawing.Size(200, 30);
            this.customComboBox1.Name = "customComboBox1";
            this.customComboBox1.Padding = new System.Windows.Forms.Padding(1);
            this.customComboBox1.Size = new System.Drawing.Size(200, 30);
            this.customComboBox1.TabIndex = 6;
            this.customComboBox1.Texts = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(11, 380);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Готово к запуску\r\n";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 317);
            this.Controls.Add(this.customPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomElements.CustomPanel customPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private School.Data.CustomElements.CustomComboBox customComboBox1;
        private CustomElements.CustomButton customButton1;
    }
}