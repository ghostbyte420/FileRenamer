namespace FileRenamer
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            main_button_translate = new Button();
            main_textBox_directoryLocation = new TextBox();
            main_button_searchDirectory = new Button();
            main_comboBox_selectLang_from = new ComboBox();
            main_comboBox_selectLang_to = new ComboBox();
            main_progressBar = new ProgressBar();
            main_label_status_header = new Label();
            main_label_status = new Label();
            label1 = new Label();
            label2 = new Label();
            tabControl1 = new TabControl();
            tabPage2 = new TabPage();
            tabPage1 = new TabPage();
            statusStrip1 = new StatusStrip();
            menuStrip1 = new MenuStrip();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // main_button_translate
            // 
            main_button_translate.Font = new Font("Segoe UI", 11F);
            main_button_translate.Location = new Point(429, 159);
            main_button_translate.Name = "main_button_translate";
            main_button_translate.Size = new Size(353, 64);
            main_button_translate.TabIndex = 0;
            main_button_translate.Text = "Translate";
            main_button_translate.UseVisualStyleBackColor = true;
            // 
            // main_textBox_directoryLocation
            // 
            main_textBox_directoryLocation.Location = new Point(116, 110);
            main_textBox_directoryLocation.Name = "main_textBox_directoryLocation";
            main_textBox_directoryLocation.Size = new Size(666, 23);
            main_textBox_directoryLocation.TabIndex = 1;
            // 
            // main_button_searchDirectory
            // 
            main_button_searchDirectory.Font = new Font("Segoe UI", 10F);
            main_button_searchDirectory.Location = new Point(9, 110);
            main_button_searchDirectory.Name = "main_button_searchDirectory";
            main_button_searchDirectory.Size = new Size(98, 27);
            main_button_searchDirectory.TabIndex = 2;
            main_button_searchDirectory.Text = "Select Directory";
            main_button_searchDirectory.UseVisualStyleBackColor = true;
            // 
            // main_comboBox_selectLang_from
            // 
            main_comboBox_selectLang_from.FormattingEnabled = true;
            main_comboBox_selectLang_from.Location = new Point(116, 197);
            main_comboBox_selectLang_from.Name = "main_comboBox_selectLang_from";
            main_comboBox_selectLang_from.Size = new Size(284, 23);
            main_comboBox_selectLang_from.TabIndex = 3;
            // 
            // main_comboBox_selectLang_to
            // 
            main_comboBox_selectLang_to.FormattingEnabled = true;
            main_comboBox_selectLang_to.Location = new Point(116, 161);
            main_comboBox_selectLang_to.Name = "main_comboBox_selectLang_to";
            main_comboBox_selectLang_to.Size = new Size(284, 23);
            main_comboBox_selectLang_to.TabIndex = 4;
            // 
            // main_progressBar
            // 
            main_progressBar.Location = new Point(0, 270);
            main_progressBar.Name = "main_progressBar";
            main_progressBar.Size = new Size(792, 23);
            main_progressBar.TabIndex = 5;
            // 
            // main_label_status_header
            // 
            main_label_status_header.AutoSize = true;
            main_label_status_header.Location = new Point(3, 298);
            main_label_status_header.Name = "main_label_status_header";
            main_label_status_header.Size = new Size(50, 15);
            main_label_status_header.TabIndex = 6;
            main_label_status_header.Text = "STATUS:";
            // 
            // main_label_status
            // 
            main_label_status.AutoSize = true;
            main_label_status.Location = new Point(64, 297);
            main_label_status.Name = "main_label_status";
            main_label_status.Size = new Size(103, 15);
            main_label_status.TabIndex = 7;
            main_label_status.Text = "<File Being Read>";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 164);
            label1.Name = "label1";
            label1.Size = new Size(99, 15);
            label1.TabIndex = 8;
            label1.Text = "Native Language:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 201);
            label2.Name = "label2";
            label2.Size = new Size(99, 15);
            label2.TabIndex = 9;
            label2.Text = "Translation From:";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 24);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 345);
            tabControl1.TabIndex = 10;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 317);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "File Renamer";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(main_button_searchDirectory);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(main_button_translate);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(main_textBox_directoryLocation);
            tabPage1.Controls.Add(main_label_status);
            tabPage1.Controls.Add(main_comboBox_selectLang_from);
            tabPage1.Controls.Add(main_label_status_header);
            tabPage1.Controls.Add(main_comboBox_selectLang_to);
            tabPage1.Controls.Add(statusStrip1);
            tabPage1.Controls.Add(main_progressBar);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 317);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "File Name Translator";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(3, 292);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(786, 22);
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new Font("Segoe UI", 11F);
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 11;
            menuStrip1.Text = "menuStrip1";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 369);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Main";
            Text = "File Renamer";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button main_button_translate;
        private TextBox main_textBox_directoryLocation;
        private Button main_button_searchDirectory;
        private ComboBox main_comboBox_selectLang_from;
        private ComboBox main_comboBox_selectLang_to;
        private ProgressBar main_progressBar;
        private Label main_label_status_header;
        private Label main_label_status;
        private Label label1;
        private Label label2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private StatusStrip statusStrip1;
        private TabPage tabPage2;
        private MenuStrip menuStrip1;
    }
}
