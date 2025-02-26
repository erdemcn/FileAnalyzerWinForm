namespace FileAnalyzer
{
    partial class FileAnalyzer
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
            comboBoxFileType = new ComboBox();
            lblFileType = new Label();
            btnUpload = new Button();
            progressBar = new ProgressBar();
            tabControlResults = new TabControl();
            SuspendLayout();
            // 
            // comboBoxFileType
            // 
            comboBoxFileType.FormattingEnabled = true;
            comboBoxFileType.Location = new Point(596, 126);
            comboBoxFileType.Name = "comboBoxFileType";
            comboBoxFileType.Size = new Size(151, 28);
            comboBoxFileType.TabIndex = 1;
            comboBoxFileType.SelectedIndexChanged += comboBoxFileType_SelectedIndexChanged;
            // 
            // lblFileType
            // 
            lblFileType.AutoSize = true;
            lblFileType.Location = new Point(596, 78);
            lblFileType.Name = "lblFileType";
            lblFileType.Size = new Size(83, 20);
            lblFileType.TabIndex = 2;
            lblFileType.Text = "Dosya Türü";
            // 
            // btnUpload
            // 
            btnUpload.Location = new Point(596, 187);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(141, 51);
            btnUpload.TabIndex = 3;
            btnUpload.Text = "YÜKLE";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += btnUpload_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(596, 284);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(183, 49);
            progressBar.TabIndex = 4;
            // 
            // tabControlResults
            // 
            tabControlResults.Location = new Point(12, 12);
            tabControlResults.Name = "tabControlResults";
            tabControlResults.SelectedIndex = 0;
            tabControlResults.Size = new Size(560, 563);
            tabControlResults.TabIndex = 5;
            // 
            // FileAnalyzer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 587);
            Controls.Add(tabControlResults);
            Controls.Add(progressBar);
            Controls.Add(btnUpload);
            Controls.Add(lblFileType);
            Controls.Add(comboBoxFileType);
            Name = "FileAnalyzer";
            Text = "FileAnalyzer";
            Load += FileAnalyzer_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox comboBoxFileType;
        private Label lblFileType;
        private Button btnUpload;
        private ProgressBar progressBar;
        private TabControl tabControlResults;
    }
}