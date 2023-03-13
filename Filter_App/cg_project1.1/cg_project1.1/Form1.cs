using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace cg_project1._1
{
    public partial class Form : System.Windows.Forms.Form
    {
        // initializing global variables

        System.Drawing.Bitmap original;
        System.Drawing.Bitmap output;
        System.Drawing.Bitmap output_edit;
        TextBox[,] matrix = new TextBox[9,9];
        int custom_filter_index = 1;
        int old_kernel_matrixrow = 4;
        int old_kernel_matrixcol = 4;
        //bool filter_created = false;
        int custom_offset_red, custom_offset_green, custom_offset_blue;
        double custom_divisor;
        int custom_anchor_row, custom_anchor_col;
        double[,] custom_matrix;
        List<ConvolutionFilterBase> filter_list = new List<ConvolutionFilterBase>();
        double[,] newmatrix = new double[5, 5];
        int commonoffset = 0;
        int M = 250;

        int maxWidth;
        bool sent = false;
        private object lockObject = new object();
        enum mode
        {
            None,
            Brightness,
            Contrast,
            GammaCorr,
            Blur,
            GaussBlur,
            Sharpen,
            EdgeDet,
            Emboss,
            Custom,
            Distance
        }
        mode filter = mode.None;
        public Form()
        {
            InitializeComponent();
        }

        // Meta functions

        private void Form_Load(object sender, EventArgs e)
        {
            CustomMatrixTable.Visible = false;
            CustomMenuTable.Visible = false;
            CustomSettingsTable.Visible = false;
            NumericSuwak.Controls[0].Visible = false;

            matrix[0, 0] = textBox1;
            matrix[0, 1] = textBox2;
            matrix[0, 2] = textBox3;
            matrix[0, 3] = textBox4;
            matrix[0, 4] = textBox5;
            matrix[0, 5] = textBox6;
            matrix[0, 6] = textBox7;
            matrix[0, 7] = textBox8;
            matrix[0, 8] = textBox9;

            matrix[1, 0] = textBox10;
            matrix[1, 1] = textBox11;
            matrix[1, 2] = textBox12;
            matrix[1, 3] = textBox13;
            matrix[1, 4] = textBox14;
            matrix[1, 5] = textBox15;
            matrix[1, 6] = textBox16;
            matrix[1, 7] = textBox17;
            matrix[1, 8] = textBox18;

            matrix[2, 0] = textBox19;
            matrix[2, 1] = textBox20;
            matrix[2, 2] = textBox21;
            matrix[2, 3] = textBox22;
            matrix[2, 4] = textBox23;
            matrix[2, 5] = textBox24;
            matrix[2, 6] = textBox25;
            matrix[2, 7] = textBox26;
            matrix[2, 8] = textBox27;

            matrix[3, 0] = textBox28;
            matrix[3, 1] = textBox29;
            matrix[3, 2] = textBox30;
            matrix[3, 3] = textBox31;
            matrix[3, 4] = textBox32;
            matrix[3, 5] = textBox33;
            matrix[3, 6] = textBox34;
            matrix[3, 7] = textBox35;
            matrix[3, 8] = textBox36;

            matrix[4, 0] = textBox37;
            matrix[4, 1] = textBox38;
            matrix[4, 2] = textBox39;
            matrix[4, 3] = textBox40;
            matrix[4, 4] = textBox41;
            matrix[4, 5] = textBox42;
            matrix[4, 6] = textBox43;
            matrix[4, 7] = textBox44;
            matrix[4, 8] = textBox45;

            matrix[5, 0] = textBox46;
            matrix[5, 1] = textBox47;
            matrix[5, 2] = textBox48;
            matrix[5, 3] = textBox49;
            matrix[5, 4] = textBox50;
            matrix[5, 5] = textBox51;
            matrix[5, 6] = textBox52;
            matrix[5, 7] = textBox53;
            matrix[5, 8] = textBox54;

            matrix[6, 0] = textBox55;
            matrix[6, 1] = textBox56;
            matrix[6, 2] = textBox57;
            matrix[6, 3] = textBox58;
            matrix[6, 4] = textBox59;
            matrix[6, 5] = textBox60;
            matrix[6, 6] = textBox61;
            matrix[6, 7] = textBox62;
            matrix[6, 8] = textBox63;

            matrix[7, 0] = textBox64;
            matrix[7, 1] = textBox65;
            matrix[7, 2] = textBox66;
            matrix[7, 3] = textBox67;
            matrix[7, 4] = textBox68;
            matrix[7, 5] = textBox69;
            matrix[7, 6] = textBox70;
            matrix[7, 7] = textBox71;
            matrix[7, 8] = textBox72;

            matrix[8, 0] = textBox73;
            matrix[8, 1] = textBox74;
            matrix[8, 2] = textBox75;
            matrix[8, 3] = textBox76;
            matrix[8, 4] = textBox77;
            matrix[8, 5] = textBox78;
            matrix[8, 6] = textBox79;
            matrix[8, 7] = textBox80;
            matrix[8, 8] = textBox81;

            RowComboBox.SelectedIndex = 1;
            ColumnComboBox.SelectedIndex = 1;
            AnchorColTextbox.Text = "2";
            AnchorRowTextbox.Text = "2";
            filter_list.Add(new DefaultFilter());
            filter_list.Add(new Blur());
            filter_list.Add(new GaussianBlur());
            filter_list.Add(new Sharpening());
            filter_list.Add(new EdgeDetection());
            filter_list.Add(new Emboss());
            LoadFilterCombobox.DataSource = filter_list;
            LoadFilterCombobox.DisplayMember = "FilterName";
            maxWidth = 0;
            foreach (ConvolutionFilterBase filter in filter_list)
            {
                int width = TextRenderer.MeasureText(filter.FilterName, LoadFilterCombobox.Font).Width;
                if (width > maxWidth)
                {
                    maxWidth = width;
                }
            }
            LoadFilterCombobox.DropDownWidth = maxWidth;
            LoadFilterCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            RowComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    newmatrix[i, j] = 1;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // hotkeys
            if (keyData == (Keys.Control | Keys.U))
            {
                UploadButton.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveButton.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Q))
            {
                ExitButton.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        // event handlers for functional buttons

        private void UploadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                original = new Bitmap(fileDialog.FileName);
                output = new Bitmap(fileDialog.FileName);
                OriginalPictureBox.Image = original;
                OutputPictureBox.Image = output;
                OriginalPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                OutputPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                sent = true;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit? All unsaved progress will be lost.", "Exit", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(sent == false)
            {
                MessageBox.Show("There's nothing to save!", "Save error", MessageBoxButtons.OK);
            }
            else
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Title = "Save Image...";
                save.Filter = "bmp files (*.bmp)|*.bmp; jpeg files (*.jpeg)|*.jpeg; png files (*.png)|*.png";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    output.Save(save.FileName);
                }
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to restart to!", "Restart error", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to return your edit to original state? This operation cannot be undone.", "Restart", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    output = (Bitmap)original.Clone();
                    OutputPictureBox.Image = output;
                    Suwak.Visible = false;
                    NumericSuwak.Visible = false;
                    SaveChangesButton.Visible = false;
                    FilterLabel.Text = "";
                    FilterLabel.Visible = false;
                }
            }
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            output = (Bitmap)output_edit.Clone();
            OutputPictureBox.Image = output;
            Suwak.Visible = false;
            NumericSuwak.Visible = false;
            SaveChangesButton.Visible = false;
            CancelButton.Visible = false;
            FilterLabel.Text = "";
            FilterLabel.Visible = false;
            filter = mode.None;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            OutputPictureBox.Image = output;
            Suwak.Visible = false;
            NumericSuwak.Visible = false;
            SaveChangesButton.Visible = false;
            CancelButton.Visible = false;
            FilterLabel.Text = "";
            FilterLabel.Visible = false;
            filter = mode.None;
        }

        private void CustomButton_Click(object sender, EventArgs e)
        {
            if (filter == mode.None)
            {
                //turn on custom
                CustomButton.BackColor = Color.FromArgb(255, 128, 128, 255);
                filter = mode.Custom;
                CustomMatrixTable.Visible = true;
                CustomMenuTable.Visible = true;
                CustomSettingsTable.Visible = true;
                if (sent == true)
                {
                    OutputPictureBox.Image = output;
                }

                for (int col = 0; col < 9; col++)
                {
                    for (int row = 0; row < 9; row++)
                    {
                        if (row >= 4 - RowComboBox.SelectedIndex && row <= 4 + RowComboBox.SelectedIndex && col >= 4 - ColumnComboBox.SelectedIndex && col <= 4 + ColumnComboBox.SelectedIndex)
                        {
                            matrix[row, col].Visible = true;
                        }
                        else matrix[row, col].Visible = false;
                    }
                }


            }
            else if (filter == mode.Custom)
            {
                //turn off custom
                CustomButton.BackColor = Color.FromArgb(255, 192, 192, 255);
                filter = mode.None;
                CustomMatrixTable.Visible = false;
                CustomMenuTable.Visible = false;
                CustomSettingsTable.Visible = false;
                if (sent == true)
                {
                    OutputPictureBox.Image = output;
                }
            }
        }

        private void AutoDivisorButton_Click(object sender, EventArgs e)
        {
            bool valid = true;
            bool numeric;
            double val;
            double sum = 0;
            //validation of a matrix
            for (int col = 4 - ColumnComboBox.SelectedIndex; col <= 4 + ColumnComboBox.SelectedIndex; col++)
            {
                for (int row = 4 - RowComboBox.SelectedIndex; row <= 4 + RowComboBox.SelectedIndex; row++)
                {
                    if (matrix[row, col].Text == "") // matrix is not filled
                    {
                        MessageBox.Show("You must input all values in the matrix!", "Auto divisor error", MessageBoxButtons.OK);
                        valid = false;
                        break;
                    }
                    numeric = double.TryParse(matrix[row, col].Text, out val);
                    if (numeric == false)
                    {
                        MessageBox.Show("You must input only numerical values!", "Auto divisor error", MessageBoxButtons.OK);
                        valid = false;
                        break;
                    }
                    else
                    {
                        sum += val;
                    }
                }
                if (valid == false) break;
            }
            if (valid)
            {
                if (sum == 0) sum = 1;
                DivisorTextbox.Text = $"{sum}";
            }
        }

        private void SaveCustomButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to filter!", "Custom filter error", MessageBoxButtons.OK);
            }
            else
            {
                bool rc, gc, bc;
                int offr, offg, offb;
                rc = int.TryParse(RedOffsetTextbox.Text, out offr);
                gc = int.TryParse(GreenOffsetTextbox.Text, out offg);
                bc = int.TryParse(BlueOffsetTextbox.Text, out offb);
                if (rc == false || gc == false || bc == false)
                {
                    MessageBox.Show("The offset values must be numeric!", "Custom filter error", MessageBoxButtons.OK);
                }
                else
                {
                    bool divisor_correct;
                    double divisor_value;
                    divisor_correct = double.TryParse(DivisorTextbox.Text, out divisor_value);
                    if (DivisorTextbox.Text == "0")
                    {
                        MessageBox.Show("The divisor cannot be equal to 0!", "Custom filter error", MessageBoxButtons.OK);
                    }
                    else if (divisor_correct == false)
                    {
                        MessageBox.Show("The divisor must be a numerical value!", "Custom filter error", MessageBoxButtons.OK);
                    }
                    else
                    {
                        int anchor_row, anchor_col;
                        bool anchor_correct_row, anchor_correct_col;
                        anchor_correct_row = int.TryParse(AnchorRowTextbox.Text, out anchor_row);
                        anchor_correct_col = int.TryParse(AnchorColTextbox.Text, out anchor_col);
                        if (anchor_correct_row == false || anchor_correct_col == false)
                        {
                            MessageBox.Show("The anchor must be a numerical value!", "Custom filter error", MessageBoxButtons.OK);
                        }
                        else if (anchor_row < 1 || anchor_row > RowComboBox.SelectedIndex * 2 + 1 || anchor_col < 1 || anchor_col > ColumnComboBox.SelectedIndex * 2 + 1)
                        {
                            MessageBox.Show("The anchor must be inside the matrix!", "Custom filter error", MessageBoxButtons.OK);
                        }
                        else
                        {
                            bool approved = true;
                            bool numeric;
                            double val;
                            for (int col = 4 - ColumnComboBox.SelectedIndex; col <= 4 + ColumnComboBox.SelectedIndex; col++)
                            {
                                for (int row = 4 - RowComboBox.SelectedIndex; row <= 4 + RowComboBox.SelectedIndex; row++)
                                {
                                    if (matrix[row, col].Text == "")
                                    {
                                        MessageBox.Show("You must input all values in the matrix!", "Custom filter error", MessageBoxButtons.OK);
                                        approved = false;
                                        break;
                                    }
                                    numeric = double.TryParse(matrix[row, col].Text, out val);
                                    if (numeric == false)
                                    {
                                        MessageBox.Show("You must input only numerical values!", "Custom filter error", MessageBoxButtons.OK);
                                        approved = false;
                                        break;
                                    }
                                }
                                if (approved == false) break;
                            }
                            if (approved)
                            {
                                using (var form = new FilterNameForm())
                                {
                                    var result = form.ShowDialog();
                                    if (result == DialogResult.OK)
                                    { 
                                        string filterName = form.FilterName;
                                        if (filterName == "") filterName = $"Filter {custom_filter_index++}";

                                        double[,] buffer = new double[RowComboBox.SelectedIndex * 2 + 1, ColumnComboBox.SelectedIndex * 2 + 1];
                                        for (int col = 4 - ColumnComboBox.SelectedIndex; col <= 4 + ColumnComboBox.SelectedIndex; col++)
                                        {
                                            for (int row = 4 - RowComboBox.SelectedIndex; row <= 4 + RowComboBox.SelectedIndex; row++)
                                            {
                                                buffer[row - (4 - RowComboBox.SelectedIndex), col - (4 - ColumnComboBox.SelectedIndex)] = double.Parse(matrix[row, col].Text);
                                            }
                                        }
                                        custom_matrix = buffer;

                                        CustomFilter new_custom = new CustomFilter(filterName,
                                                       offr,
                                                       offg,
                                                       offb,
                                                       anchor_row,
                                                       anchor_col,
                                                       divisor_value,
                                                       custom_matrix);
                                        filter_list.Add(new_custom);
                                        LoadFilterCombobox.DataSource = null;
                                        LoadFilterCombobox.DataSource = filter_list;
                                        LoadFilterCombobox.DisplayMember = "FilterName";
                                        maxWidth = 0;
                                        foreach (ConvolutionFilterBase filter in filter_list)
                                        {
                                            int width = TextRenderer.MeasureText(filter.FilterName, LoadFilterCombobox.Font).Width;
                                            if (width > maxWidth)
                                            {
                                                maxWidth = width;
                                            }
                                        }
                                        LoadFilterCombobox.DropDownWidth = maxWidth;
                                        MessageBox.Show("Your filter was succesfully saved!", "Custom filter error", MessageBoxButtons.OK);
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }


        // functions processing filters (with loading bar)

        void Inverse_Process()
        {
            for (int y = 0; (y <= (output.Height - 1)); y++)
            {
                for (int x = 0; (x <= (output.Width - 1)); x++)
                {
                    Color inv = output.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    output.SetPixel(x, y, inv);
                }
            }
            OutputPictureBox.Image = output;
        }

        void Brightness_Process()
        {
            int suwak_value = 0;
            Suwak.Invoke(new Action(() => { suwak_value = Suwak.Value; }));
            for (int y = 0; (y <= (output.Height - 1)); y++)
            {
                for (int x = 0; (x <= (output.Width - 1)); x++)
                {
                    Color px = output.GetPixel(x, y);
                    int newr = px.R + suwak_value;
                    int newg = px.G + suwak_value;
                    int newb = px.B + suwak_value;
                    if (newr > 255) newr = 255;
                    if (newr < 0) newr = 0;
                    if (newg > 255) newg = 255;
                    if (newg < 0) newg = 0;
                    if (newb > 255) newb = 255;
                    if (newb < 0) newb = 0;
                    px = Color.FromArgb(255, newr, newg, newb);
                    output_edit.SetPixel(x, y, px);
                }
            }
            OutputPictureBox.Image = output_edit;
        }

        void Contrast_Process()
        {
            int suwak_value = 0;
            Suwak.Invoke(new Action(() => { suwak_value = Suwak.Value; }));
            for (int y = 0; (y <= (output.Height - 1)); y++)
            {
                for (int x = 0; (x <= (output.Width - 1)); x++)
                {
                    Color px = output.GetPixel(x, y);
                    double contrastLvl = Math.Pow(((100.0 + suwak_value) / 100.0), 2);
                    int newr = (int)(((((px.R / 255.0) - 0.5) * contrastLvl) + 0.5) * 255.0);
                    int newg = (int)(((((px.G / 255.0) - 0.5) * contrastLvl) + 0.5) * 255.0);
                    int newb = (int)(((((px.B / 255.0) - 0.5) * contrastLvl) + 0.5) * 255.0);
                    if (newr > 255) newr = 255;
                    if (newr < 0) newr = 0;
                    if (newg > 255) newg = 255;
                    if (newg < 0) newg = 0;
                    if (newb > 255) newb = 255;
                    if (newb < 0) newb = 0;
                    px = Color.FromArgb(255, newr, newg, newb);
                    output_edit.SetPixel(x, y, px);
                }
            }
            OutputPictureBox.Image = output_edit;
        }

        void Gamma_Process()
        {
            int suwak_value = 0;
            Suwak.Invoke(new Action(() => { suwak_value = Suwak.Value; }));
            int width = output.Width;
            int height = output.Height;
            BitmapData srcData = output.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            output.UnlockBits(srcData);
            int current = 0;
            int cChannels = 3;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    current = y * srcData.Stride + x * 4;
                    for (int i = 0; i < cChannels; i++)
                    {
                        double range = (double)buffer[current + i] / 255;
                        double correction = 1 * Math.Pow(range, (double)suwak_value / 100);
                        result[current + i] = (byte)(correction * 255);
                    }
                    result[current + 3] = 255;
                }
            }
            Bitmap resImg = new Bitmap(width, height);
            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            output_edit = (Bitmap)resImg.Clone();
            OutputPictureBox.Image = resImg;
        }

        void Blur_Process()
        {
            Bitmap bitmap_clone;
            lock (lockObject)
            {
                bitmap_clone = (Bitmap)output.Clone();
            }
            output_edit = bitmap_clone.ConvolutionFilter(new Blur());
            OutputPictureBox.Invoke(new Action(() => { OutputPictureBox.Image = output_edit; }));
            Suwak.Invoke(new Action(() => { Suwak.Visible = false; }));
            NumericSuwak.Invoke(new Action(() => { NumericSuwak.Visible = false; }));
            SaveChangesButton.Invoke(new Action(() => { SaveChangesButton.Visible = true; }));
            CancelButton.Invoke(new Action(() => { CancelButton.Visible = true; }));
            FilterLabel.Invoke(new Action(() => {
                FilterLabel.Text = "Blur result:";
                FilterLabel.Visible = true;
            }));
        }

        void Gaussian_Process()
        {
            Bitmap bitmap_clone;
            lock (lockObject)
            {
                bitmap_clone = (Bitmap)output.Clone();
            }
            output_edit = bitmap_clone.ConvolutionFilter(new GaussianBlur());
            OutputPictureBox.Invoke(new Action(() => { OutputPictureBox.Image = output_edit; }));
            Suwak.Invoke(new Action(() => { Suwak.Visible = false; }));
            NumericSuwak.Invoke(new Action(() => { NumericSuwak.Visible = false; }));
            SaveChangesButton.Invoke(new Action(() => { SaveChangesButton.Visible = true; }));
            CancelButton.Invoke(new Action(() => { CancelButton.Visible = true; }));
            FilterLabel.Invoke(new Action(() => {
                FilterLabel.Text = "Gaussian Blur result:";
                FilterLabel.Visible = true;
            }));
        }

        void Sharpen_Process()
        {
            Bitmap bitmap_clone;
            lock (lockObject)
            {
                bitmap_clone = (Bitmap)output.Clone();
            }
            output_edit = bitmap_clone.ConvolutionFilter(new Sharpening());
            OutputPictureBox.Invoke(new Action(() => { OutputPictureBox.Image = output_edit; }));
            Suwak.Invoke(new Action(() => { Suwak.Visible = false; }));
            NumericSuwak.Invoke(new Action(() => { NumericSuwak.Visible = false; }));
            SaveChangesButton.Invoke(new Action(() => { SaveChangesButton.Visible = true; }));
            CancelButton.Invoke(new Action(() => { CancelButton.Visible = true; }));
            FilterLabel.Invoke(new Action(() => {
                FilterLabel.Text = "Sharpening result:";
                FilterLabel.Visible = true;
            }));
        }

        void EdgeDetection_Process()
        {
            Bitmap bitmap_clone;
            lock (lockObject)
            {
                bitmap_clone = (Bitmap)output.Clone();
            }
            output_edit = bitmap_clone.ConvolutionFilter(new EdgeDetection());
            OutputPictureBox.Invoke(new Action(() => { OutputPictureBox.Image = output_edit; }));
            Suwak.Invoke(new Action(() => { Suwak.Visible = false; }));
            NumericSuwak.Invoke(new Action(() => { NumericSuwak.Visible = false; }));
            SaveChangesButton.Invoke(new Action(() => { SaveChangesButton.Visible = true; }));
            CancelButton.Invoke(new Action(() => { CancelButton.Visible = true; }));
            FilterLabel.Invoke(new Action(() => {
                FilterLabel.Text = "Edge detection result:";
                FilterLabel.Visible = true;
            }));
        }

        void Emboss_Process()
        {
            Bitmap bitmap_clone;
            lock (lockObject)
            {
                bitmap_clone = (Bitmap)output.Clone();
            }
            output_edit = bitmap_clone.ConvolutionFilter(new Emboss());
            OutputPictureBox.Invoke(new Action(() => { OutputPictureBox.Image = output_edit; }));
            Suwak.Invoke(new Action(() => { Suwak.Visible = false; }));
            NumericSuwak.Invoke(new Action(() => { NumericSuwak.Visible = false; }));
            SaveChangesButton.Invoke(new Action(() => { SaveChangesButton.Visible = true; }));
            CancelButton.Invoke(new Action(() => { CancelButton.Visible = true; }));
            FilterLabel.Invoke(new Action(() => {
                FilterLabel.Text = "Emboss result:";
                FilterLabel.Visible = true;
            }));
        }

        void Custom_Process()
        {
            Bitmap bitmap_clone;
            lock (lockObject)
            {
                bitmap_clone = (Bitmap)output.Clone();
            }
            CustomFilter new_custom = new CustomFilter($"Filter",
                                                       custom_offset_red,
                                                       custom_offset_green,
                                                       custom_offset_blue,
                                                       custom_anchor_row,
                                                       custom_anchor_col,
                                                       custom_divisor,
                                                       custom_matrix);
            output_edit = bitmap_clone.ConvolutionFilter(new_custom);
            OutputPictureBox.Invoke(new Action(() => { OutputPictureBox.Image = output_edit; }));
            Suwak.Invoke(new Action(() => { Suwak.Visible = false; }));
            NumericSuwak.Invoke(new Action(() => { NumericSuwak.Visible = false; }));
            SaveChangesButton.Invoke(new Action(() => { SaveChangesButton.Visible = true; }));
            CancelButton.Invoke(new Action(() => { CancelButton.Visible = true; }));
            FilterLabel.Invoke(new Action(() => {
                FilterLabel.Text = "Your custom filter result:";
                FilterLabel.Visible = true;
            }));
        }

        void Distance_Process()
        {
            BitmapData sourceData = output.LockBits(new Rectangle(0, 0,
                                        output.Width, output.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            output.UnlockBits(sourceData);


            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;


            int filterWidth = newmatrix.GetLength(1);
            int filterHeight = newmatrix.GetLength(0);


            int filterOffsetX = (filterWidth - 1) / 2;
            int filterOffsetY = (filterHeight - 1) / 2;
            int calcOffset = 0;


            int byteOffset = 0;


            for (int offsetY = 2; offsetY <
                 output.Height + 2 - filterHeight; offsetY++)
            {
                for (int offsetX = 2; offsetX <
                     output.Width + 2 - filterWidth; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;

                    Color center_px = output.GetPixel(offsetX, offsetY);

                    int newdivisor = 0;

                    byteOffset = offsetY *
                                    sourceData.Stride +
                                    offsetX * 4;


                    for (int filterY = -2;
                         filterY <= 2 * filterOffsetY - 2; filterY++)
                    {
                        for (int filterX = -2;
                             filterX <= 2 * filterOffsetX - 2; filterX++)
                        {
                            Color current_px = output.GetPixel(offsetX + filterX, offsetY + filterY);

                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);

                            if(Math.Abs(center_px.R - current_px.R) + Math.Abs(center_px.G - current_px.G) + Math.Abs(center_px.B - current_px.B) < M)
                            {
                                newdivisor++;
                                blue += (double)(pixelBuffer[calcOffset]) *
                                    newmatrix[filterY + 2,
                                    filterX + 2];


                                green += (double)(pixelBuffer[calcOffset + 1]) *
                                          newmatrix[filterY + 2,
                                          filterX + 2];


                                red += (double)(pixelBuffer[calcOffset + 2]) *
                                        newmatrix[filterY + 2,
                                        filterX + 2];
                            }
                        }
                    }

                    blue = (blue / newdivisor) + commonoffset;
                    green = (green / newdivisor) + commonoffset;
                    red = (red / newdivisor) + commonoffset;


                    if (blue > 255)
                    { blue = 255; }
                    else if (blue < 0)
                    { blue = 0; }


                    if (green > 255)
                    { green = 255; }
                    else if (green < 0)
                    { green = 0; }


                    if (red > 255)
                    { red = 255; }
                    else if (red < 0)
                    { red = 0; }


                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }


            Bitmap resultBitmap = new Bitmap(output.Width, output.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            output_edit = resultBitmap;
            OutputPictureBox.Image = output_edit;
            SaveChangesButton.Invoke(new Action(() => { SaveChangesButton.Visible = true; }));
            CancelButton.Invoke(new Action(() => { CancelButton.Visible = true; }));
            FilterLabel.Invoke(new Action(() => {
                FilterLabel.Text = "Result of distance filter:";
                FilterLabel.Visible = true;
            }));
        }

        // filter buttons event handlers

        private void InverseButton_Click(object sender, EventArgs e)
        {
            if(sent==false)
            {
                MessageBox.Show("You didn't upload any picture to inverse!","Upload error",MessageBoxButtons.OK);
            }
            else
            {
                using(LoadingForm frm = new LoadingForm(Inverse_Process))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private void BrightnessButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to brighten!", "Brightness error", MessageBoxButtons.OK);
            }
            else
            {
                output_edit = (Bitmap)output.Clone();
                OutputPictureBox.Image = output_edit;

                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.Brightness;

                Suwak.Minimum = -127;
                Suwak.Maximum = 127;
                NumericSuwak.Minimum = -127;
                NumericSuwak.Maximum = 127;
                NumericSuwak.DecimalPlaces = 0;
                NumericSuwak.Increment = 1M;
                Suwak.Value = 0;
                NumericSuwak.Value = 0;
                Suwak.Visible = true;
                NumericSuwak.Visible = true;
                SaveChangesButton.Visible = true;
                CancelButton.Visible = true;
                FilterLabel.Text = "Brighness level:";
                FilterLabel.Visible = true;
            }
        }

        private void ContrastButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to contrast!", "Contrast error", MessageBoxButtons.OK);
            }
            else
            {
                output_edit = (Bitmap)output.Clone();
                OutputPictureBox.Image = output_edit;

                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.Contrast;

                Suwak.Minimum = -127;
                Suwak.Maximum = 127;
                NumericSuwak.Minimum = -127;
                NumericSuwak.Maximum = 127;
                NumericSuwak.DecimalPlaces = 0;
                NumericSuwak.Increment = 1M;
                Suwak.Value = 0;
                NumericSuwak.Value = 0;
                Suwak.Visible = true;
                NumericSuwak.Visible = true;
                SaveChangesButton.Visible = true;
                CancelButton.Visible = true;
                FilterLabel.Text = "Contrast level:";
                FilterLabel.Visible = true;
            }
        }

        private void GammaCorrectionButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to filter!", "Gamma Correction error", MessageBoxButtons.OK);
            }
            else
            {
                output_edit = (Bitmap)output.Clone();
                OutputPictureBox.Image = output_edit;

                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.GammaCorr;

                Suwak.Minimum = 0;
                Suwak.Maximum = 300;
                NumericSuwak.Minimum = 0;
                NumericSuwak.Maximum = 3;
                Suwak.Value = 1;
                NumericSuwak.Value = 1;
                NumericSuwak.DecimalPlaces = 2;
                NumericSuwak.Increment = 0.01M;
                Suwak.Visible = true;
                NumericSuwak.Visible = true;
                SaveChangesButton.Visible = true;
                CancelButton.Visible = true;
                FilterLabel.Text = "Gamma value:";
                FilterLabel.Visible = true;
            }
        }

        private void BlurButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to blur!", "Blur error", MessageBoxButtons.OK);
            }
            else
            {
                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.Blur;
                using (LoadingForm frm = new LoadingForm(Blur_Process))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private void GaussianBlurButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to apply Gaussian Blur to!", "Gaussian Blur error", MessageBoxButtons.OK);
            }
            else
            {
                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.GaussBlur;
                using (LoadingForm frm = new LoadingForm(Gaussian_Process))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private void SharpenButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to sharpen!", "Sharpening error", MessageBoxButtons.OK);
            }
            else
            {
                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.Sharpen;
                using (LoadingForm frm = new LoadingForm(Sharpen_Process))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private void EdgeButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to find edges of!", "Edge detection error", MessageBoxButtons.OK);
            }
            else
            {
                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.EdgeDet;
                using (LoadingForm frm = new LoadingForm(EdgeDetection_Process))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private void EmbossButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to emboss!", "Emboss error", MessageBoxButtons.OK);
            }
            else
            {
                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.Emboss;
                using (LoadingForm frm = new LoadingForm(Emboss_Process))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private void ApplyCustomButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to filter!", "Custom filter error", MessageBoxButtons.OK);
            }
            else
            {
                bool rc, gc, bc;
                int offr, offg, offb;
                rc = int.TryParse(RedOffsetTextbox.Text, out offr);
                gc = int.TryParse(GreenOffsetTextbox.Text, out offg);
                bc = int.TryParse(BlueOffsetTextbox.Text, out offb);
                if (rc == false || gc == false || bc == false)
                {
                    MessageBox.Show("The offset values must be numeric!", "Custom filter error", MessageBoxButtons.OK);
                }
                else
                {
                    bool divisor_correct;
                    double divisor_value;
                    divisor_correct = double.TryParse(DivisorTextbox.Text, out divisor_value);
                    if (DivisorTextbox.Text == "0")
                    {
                        MessageBox.Show("The divisor cannot be equal to 0!", "Custom filter error", MessageBoxButtons.OK);
                    }
                    else if (divisor_correct == false)
                    {
                        MessageBox.Show("The divisor must be a numerical value!", "Custom filter error", MessageBoxButtons.OK);
                    }
                    else
                    {
                        int anchor_row, anchor_col;
                        bool anchor_correct_row, anchor_correct_col;
                        anchor_correct_row = int.TryParse(AnchorRowTextbox.Text, out anchor_row);
                        anchor_correct_col = int.TryParse(AnchorColTextbox.Text, out anchor_col);
                        if (anchor_correct_row == false || anchor_correct_col == false)
                        {
                            MessageBox.Show("The anchor must be a numerical value!", "Custom filter error", MessageBoxButtons.OK);
                        }
                        else if (anchor_row < 1 || anchor_row > RowComboBox.SelectedIndex * 2 + 1 || anchor_col < 1 || anchor_col > ColumnComboBox.SelectedIndex * 2 + 1)
                        {
                            MessageBox.Show("The anchor must be inside the matrix!", "Custom filter error", MessageBoxButtons.OK);
                        }
                        else
                        {
                            bool approved = true;
                            bool numeric;
                            double val;
                            for (int col = 4 - ColumnComboBox.SelectedIndex; col <= 4 + ColumnComboBox.SelectedIndex; col++)
                            {
                                for (int row = 4 - RowComboBox.SelectedIndex; row <= 4 + RowComboBox.SelectedIndex; row++)
                                {
                                    if (matrix[row, col].Text == "")
                                    {
                                        MessageBox.Show("You must input all values in the matrix!", "Custom filter error", MessageBoxButtons.OK);
                                        approved = false;
                                        break;
                                    }
                                    numeric = double.TryParse(matrix[row, col].Text, out val);
                                    if (numeric == false)
                                    {
                                        MessageBox.Show("You must input only numerical values!", "Custom filter error", MessageBoxButtons.OK);
                                        approved = false;
                                        break;
                                    }
                                }
                                if (approved == false) break;
                            }
                            if (approved)
                            {
                                // create and apply proper filter
                                custom_offset_red = offr;
                                custom_offset_green = offg;
                                custom_offset_blue = offb;
                                custom_anchor_row = anchor_row;
                                custom_anchor_col = anchor_col;
                                custom_divisor = divisor_value;

                                double[,] buffer = new double[RowComboBox.SelectedIndex * 2 + 1, ColumnComboBox.SelectedIndex * 2 + 1];
                                for (int col = 4 - ColumnComboBox.SelectedIndex; col <= 4 + ColumnComboBox.SelectedIndex; col++)
                                {
                                    for (int row = 4 - RowComboBox.SelectedIndex; row <= 4 + RowComboBox.SelectedIndex; row++)
                                    {
                                        buffer[row - (4 - RowComboBox.SelectedIndex), col - (4 - ColumnComboBox.SelectedIndex)] = double.Parse(matrix[row, col].Text);
                                    }
                                }
                                custom_matrix = buffer;
                                using (LoadingForm frm = new LoadingForm(Custom_Process))
                                {
                                    frm.ShowDialog(this);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DistanceFilterButton_Click(object sender, EventArgs e)
        {
            if (sent == false)
            {
                MessageBox.Show("You didn't upload any picture to compute!", "Distance filter error", MessageBoxButtons.OK);
            }
            else
            {
                if (filter == mode.Custom) CustomButton.PerformClick();
                filter = mode.Distance;
                using (LoadingForm frm = new LoadingForm(Distance_Process))
                {
                    frm.ShowDialog(this);
                }
            }
        }


        // events handling manipulating parameters

        private void NumericBar_ValueChanged(object sender, EventArgs e)
        {
            if (filter != mode.GammaCorr)
            {
                Suwak.Value = (int)NumericSuwak.Value;
                //Suwak_MouseCaptureChanged(sender, e);
            }
            else
            {
                Suwak.Value = (int)(NumericSuwak.Value * 100M);
                //Suwak_MouseCaptureChanged(sender, e);
            }
        }

        private void Suwak_MouseCaptureChanged(object sender, EventArgs e)
        {
            switch (filter)
            {
                case mode.None:
                    break;
                case mode.Brightness:
                    using (LoadingForm frm = new LoadingForm(Brightness_Process))
                    {
                        frm.ShowDialog(this);
                    }
                    break;
                case mode.Contrast:
                    using (LoadingForm frm = new LoadingForm(Contrast_Process))
                    {
                        frm.ShowDialog(this);
                    }
                    break;
                case mode.GammaCorr:
                    using (LoadingForm frm = new LoadingForm(Gamma_Process))
                    {
                        frm.ShowDialog(this);
                    }
                    break;
            }
        }

        private void RowComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RowComboBox.SelectedIndex*2+1 < int.Parse(AnchorRowTextbox.Text))
            {
                AnchorRowTextbox.Text = $"{RowComboBox.SelectedIndex+1}";
            }
            for(int col=0; col<9; col++)
            {
                for(int row=0; row<9; row++)
                {
                    if (row >= 4 - RowComboBox.SelectedIndex && row <= 4 + RowComboBox.SelectedIndex && col >= 4 - ColumnComboBox.SelectedIndex && col <= 4 + ColumnComboBox.SelectedIndex)
                    {
                        matrix[row, col].Visible = true;
                    }
                    else
                    {
                        if(matrix[row, col].BackColor == Color.FromArgb(255, 255, 224, 192))
                        {
                            matrix[row, col].BackColor = Color.FromKnownColor(KnownColor.Window);
                            matrix[4, 4].BackColor = Color.FromArgb(255, 255, 224, 192);
                            old_kernel_matrixcol = 4;
                            old_kernel_matrixrow = 4;
                            AnchorRowTextbox.Text = $"{RowComboBox.SelectedIndex + 1}";
                            AnchorColTextbox.Text = $"{ColumnComboBox.SelectedIndex + 1}";
                        }
                        matrix[row, col].Visible = false;
                    }
                }
            }
            AnchorRowTextbox_TextChanged(null, EventArgs.Empty);
            AnchorColTextbox_TextChanged(null, EventArgs.Empty);
        }

        private void ColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value;
            bool okay = int.TryParse(AnchorColTextbox.Text, out value);
            if(okay)
            {
                if (ColumnComboBox.SelectedIndex * 2 + 1 < value)
                {
                    AnchorColTextbox.Text = $"{ColumnComboBox.SelectedIndex + 1}";
                }
            }
            for (int col = 0; col < 9; col++)
            {
                for (int row = 0; row < 9; row++)
                {
                    if (row >= 4 - RowComboBox.SelectedIndex && row <= 4 + RowComboBox.SelectedIndex && col >= 4 - ColumnComboBox.SelectedIndex && col <= 4 + ColumnComboBox.SelectedIndex)
                    {
                        matrix[row, col].Visible = true;
                    }
                    else
                    {
                        if (matrix[row, col].BackColor == Color.FromArgb(255, 255, 224, 192))
                        {
                            matrix[row, col].BackColor = Color.FromKnownColor(KnownColor.Window);
                            matrix[4, 4].BackColor = Color.FromArgb(255, 255, 224, 192);
                            old_kernel_matrixcol = 4;
                            old_kernel_matrixrow = 4;
                            AnchorRowTextbox.Text = $"{RowComboBox.SelectedIndex + 1}";
                            AnchorColTextbox.Text = $"{ColumnComboBox.SelectedIndex + 1}";
                        }
                        matrix[row, col].Visible = false;
                    }
                }
            }
            AnchorRowTextbox_TextChanged(null, EventArgs.Empty);
            AnchorColTextbox_TextChanged(null, EventArgs.Empty);
        }

        private void DivisorTextbox_TextChanged(object sender, EventArgs e)
        {
            double value;
            bool legal = double.TryParse(DivisorTextbox.Text, out value);
            if(!legal)
            {
                if(DivisorTextbox.Text != "" && DivisorTextbox.Text != "-")
                {
                    MessageBox.Show("The divisor must be numerical!", "Divisor error", MessageBoxButtons.OK);
                    DivisorTextbox.Text = "";
                }
            }
            else if(value == 0)
            {
                MessageBox.Show("The divisor cannot be equal to 0!", "Divisor error", MessageBoxButtons.OK);
                DivisorTextbox.Text = "";
            }
        }

        private void RedOffsetTextbox_TextChanged(object sender, EventArgs e)
        {
            bool legal = double.TryParse(RedOffsetTextbox.Text, out _);
            if (!legal)
            {
                if(RedOffsetTextbox.Text != "" && RedOffsetTextbox.Text != "-")
                {
                    MessageBox.Show("The offset must be numerical!", "Offset error", MessageBoxButtons.OK);
                    RedOffsetTextbox.Text = "";
                }
            }
        }

        private void GreenOffsetTextbox_TextChanged(object sender, EventArgs e)
        {
            bool legal = double.TryParse(GreenOffsetTextbox.Text, out _);
            if (!legal)
            {
                if(GreenOffsetTextbox.Text != "" && GreenOffsetTextbox.Text != "-")
                {
                    MessageBox.Show("The offset must be numerical!", "Offset error", MessageBoxButtons.OK);
                    GreenOffsetTextbox.Text = "";
                }
            }
        }

        private void BlueOffsetTextbox_TextChanged(object sender, EventArgs e)
        {
            bool legal = double.TryParse(BlueOffsetTextbox.Text, out _);
            if (!legal)
            {
                if (BlueOffsetTextbox.Text != "" && BlueOffsetTextbox.Text != "-")
                {
                    MessageBox.Show("The offset must be numerical!", "Offset error", MessageBoxButtons.OK);
                    BlueOffsetTextbox.Text = "";
                }
            }
        }

        private void AnchorRowTextbox_TextChanged(object sender, EventArgs e)
        {
            int max = RowComboBox.SelectedIndex * 2 + 1;
            int value;
            if(RowComboBox.SelectedIndex != -1)
            {
                bool legal = int.TryParse(AnchorRowTextbox.Text, out value);
                if (!legal)
                {
                    if (AnchorRowTextbox.Text != "")
                    {
                        MessageBox.Show("The anchor must be numerical!", "Anchor error", MessageBoxButtons.OK);
                        AnchorRowTextbox.Text = "";
                    }
                }
                else if (value < 1 || value > max)
                {
                    MessageBox.Show("The anchor must be somewhere in the matrix!", "Anchor error", MessageBoxButtons.OK);
                    AnchorRowTextbox.Text = "";
                }
                else
                {
                    // changing mark of anchor on matrix
                    matrix[old_kernel_matrixrow, old_kernel_matrixcol].BackColor = Color.FromKnownColor(KnownColor.Window);
                    matrix[3 - RowComboBox.SelectedIndex + value, old_kernel_matrixcol].BackColor = Color.FromArgb(255, 255, 224, 192);
                    old_kernel_matrixrow = 3 - RowComboBox.SelectedIndex + value;
                }
            }
            else AnchorRowTextbox.Text = "2";
        }

        private void AnchorColTextbox_TextChanged(object sender, EventArgs e)
        {
            int max = ColumnComboBox.SelectedIndex * 2 + 1;
            int value;
            if (ColumnComboBox.SelectedIndex != -1)
            {
                bool legal = int.TryParse(AnchorColTextbox.Text, out value);
                if (!legal)
                {
                    if (AnchorColTextbox.Text != "")
                    {
                        MessageBox.Show("The anchor must be numerical!", "Anchor error", MessageBoxButtons.OK);
                        AnchorColTextbox.Text = "";
                    }
                }
                else if (value < 1 || value > max)
                {
                    // TextBox criminal = (TextBox)sender;
                    MessageBox.Show($"The anchor must be somewhere in the matrix!", "Anchor error", MessageBoxButtons.OK);
                    AnchorColTextbox.Text = "";
                    // \nvalue = {value}\nmax = {max}\nselected column combobox index = {ColumnComboBox.SelectedIndex}\nsender = {criminal.Name}, text = {criminal.Text}
                }
                else
                {
                    // changing mark of anchor on matrix
                    matrix[old_kernel_matrixrow, old_kernel_matrixcol].BackColor = Color.FromKnownColor(KnownColor.Window);
                    matrix[old_kernel_matrixrow, 3 - ColumnComboBox.SelectedIndex + value].BackColor = Color.FromArgb(255, 255, 224, 192);
                    old_kernel_matrixcol = 3 - ColumnComboBox.SelectedIndex + value;
                }
            }
            else AnchorColTextbox.Text = "2";
        }

        private void LoadFilterCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadFilterCombobox.DataSource != null)
            {
                ConvolutionFilterBase selected = (ConvolutionFilterBase)LoadFilterCombobox.SelectedItem;
                RedOffsetTextbox.Text = $"{selected.OffsetRed}";
                GreenOffsetTextbox.Text = $"{selected.OffsetGreen}";
                BlueOffsetTextbox.Text = $"{selected.OffsetBlue}";
                DivisorTextbox.Text = $"{selected.Divisor}";

                int rowset = selected.FilterMatrix.GetLength(0);
                int colset = selected.FilterMatrix.GetLength(1);

                RowComboBox.SelectedIndex = (rowset - 1) / 2;
                ColumnComboBox.SelectedIndex = (colset - 1) / 2;

                AnchorRowTextbox.Text = $"{selected.AnchorRow}";
                AnchorColTextbox.Text = $"{selected.AnchorCol}";

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++) 
                    {
                        matrix[i, j].Text = "";
                    }
                }

                for (int col = 4 - ColumnComboBox.SelectedIndex; col <= 4 + ColumnComboBox.SelectedIndex; col++)
                {
                    for (int row = 4 - RowComboBox.SelectedIndex; row <= 4 + RowComboBox.SelectedIndex; row++)
                    {
                        matrix[row, col].Text = $"{selected.FilterMatrix[row - (4 - RowComboBox.SelectedIndex), col - (4 - ColumnComboBox.SelectedIndex)]}";
                    }
                }
            }
        }


        // useless functions that, when deleted, crash the program so they need to stay here

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Suwak_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            if (filter != mode.GammaCorr)
            {
                NumericSuwak.Value = Suwak.Value;
            }
            else
            {
                NumericSuwak.Value = Suwak.Value / 100M;
            }
        }
    }
}
