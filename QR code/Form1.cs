namespace QR_code
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MaskComboBox.SelectedIndex = 0;
            MaskComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            CorrectionLevel.SelectedIndex = 0;
            CorrectionLevel.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            int m = pictureBox.Width;
            if (m > pictureBox.Height)
                m = pictureBox.Height;
            pictureBox.Image = new Bitmap(m, m);
        }

        Brush white = new SolidBrush(Color.White);
        Brush black = new SolidBrush(Color.Black);
        void UpdateQR()
        {
            string text = InputTextBox.Text.Substring(0);
            ECL ecl = 3 - (ECL)CorrectionLevel.SelectedIndex;
            Mask mask = (Mask)MaskComboBox.SelectedIndex - 1;

            // Clear the screen
            if (text.Length == 0)
            {
                labelVersion.Text = "";
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.Clear(Color.Transparent);
                graphics.Dispose();
                pictureBox.Refresh();
                return;
            }

            int version;
            bool[,] matrix = QRcode.GetMatrix(text, ecl, mask, out version);

            // There was error creating the QR-code
            if (matrix == null)
            {
                Graphics Graph = Graphics.FromImage(pictureBox.Image);
                unchecked { Graph.Clear(Color.FromArgb((int)0xffffe0e0)); }
                Graph.DrawString("Error - QR-codes can't contain the wanted amount of data", new Font("Aptos", 35, FontStyle.Regular), new SolidBrush(Color.Red), pictureBox.Bounds);
                Graph.Dispose();
                pictureBox.Refresh();
                labelVersion.Text = "ERROR";
                return;
            }

            // Draw the QR into the screen
            int size = matrix.GetLength(0);
            Graphics G = Graphics.FromImage(pictureBox.Image);
            G.Clear(Color.Transparent);
            int ratio = pictureBox.Image.Width / size;
            G.FillRectangle(white, 0, 0, ratio * size, ratio * size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j])
                        G.FillRectangle(black, i * ratio, j * ratio, ratio, ratio);
                }
            G.Dispose();
            labelVersion.Text = $"Version {version}";
            pictureBox.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateQR();
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateQR();
        }
 
        private void Expand_TextBox(object sender, EventArgs e)
        {
            if ((sender as Label).Text == "\x21f2") // Expand
            {
                InputTextBox.Size += new Size(0, 500);
                InputTextBox.BorderStyle = BorderStyle.FixedSingle;
                (sender as Label).Text = "\x21f1";
            }
            else // Contract
            {
                InputTextBox.Size -= new Size(0, 500);
                InputTextBox.BorderStyle = BorderStyle.None;
                (sender as Label).Text = "\x21f2";
            }
        }
    }
}
