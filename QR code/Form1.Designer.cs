namespace QR_code
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
            InputTextBox = new TextBox();
            label1 = new Label();
            pictureBox = new PictureBox();
            CorrectionLevel = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            MaskComboBox = new ComboBox();
            labelVersion = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // InputTextBox
            // 
            InputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            InputTextBox.Font = new Font("Segoe UI", 10F);
            InputTextBox.Location = new Point(60, 9);
            InputTextBox.Multiline = true;
            InputTextBox.Name = "InputTextBox";
            InputTextBox.ScrollBars = ScrollBars.Vertical;
            InputTextBox.Size = new Size(459, 23);
            InputTextBox.TabIndex = 0;
            InputTextBox.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(42, 21);
            label1.TabIndex = 1;
            label1.Text = "Data";
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(12, 139);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(531, 531);
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            // 
            // CorrectionLevel
            // 
            CorrectionLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            CorrectionLevel.Font = new Font("Segoe UI", 12F);
            CorrectionLevel.FormattingEnabled = true;
            CorrectionLevel.Items.AddRange(new object[] { "7%      (L) Low", "15%    (M) Medium", "25%    (Q) Quartile", "30%    (H) High" });
            CorrectionLevel.Location = new Point(140, 48);
            CorrectionLevel.Name = "CorrectionLevel";
            CorrectionLevel.Size = new Size(172, 29);
            CorrectionLevel.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(125, 21);
            label2.TabIndex = 4;
            label2.Text = "Error Correction:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(12, 96);
            label3.Name = "label3";
            label3.Size = new Size(50, 21);
            label3.TabIndex = 4;
            label3.Text = "Mask:";
            // 
            // MaskComboBox
            // 
            MaskComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MaskComboBox.Font = new Font("Segoe UI", 12F);
            MaskComboBox.FormattingEnabled = true;
            MaskComboBox.Items.AddRange(new object[] { "Automat", "flower  [ij%2+ij%3]", "rectangles  [(i/2+j/3)%2]", "Pazzle  [(ij%3+i+j)%2]", "XoredRectangles  [(ij%3+ij)%2]", "row2  [i%2]", "checkers  [(i+j)%2]", "diagonal3  [(i+j)%3]", "column3  [j%3]" });
            MaskComboBox.Location = new Point(68, 93);
            MaskComboBox.Name = "MaskComboBox";
            MaskComboBox.Size = new Size(244, 29);
            MaskComboBox.TabIndex = 5;
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Font = new Font("Segoe UI", 30F);
            labelVersion.ForeColor = SystemColors.Highlight;
            labelVersion.Location = new Point(332, 58);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(0, 54);
            labelVersion.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 15F);
            label4.Location = new Point(523, 5);
            label4.Name = "label4";
            label4.Size = new Size(29, 28);
            label4.TabIndex = 7;
            label4.Text = "⇲";
            label4.Click += Expand_TextBox;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(554, 682);
            Controls.Add(InputTextBox);
            Controls.Add(label4);
            Controls.Add(labelVersion);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(MaskComboBox);
            Controls.Add(CorrectionLevel);
            Controls.Add(pictureBox);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InputTextBox;
        private Label label1;
        private PictureBox pictureBox;
        private ComboBox CorrectionLevel;
        private Label label2;
        private Label label3;
        private ComboBox MaskComboBox;
        private Label labelVersion;
        private Label label4;
    }
}
