using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace orm_maker {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        //Битмапа выходного файла
        Bitmap result;
        //Флаг, проверяющий установлен ли размер выходного файла
        bool is_size_set = false;
        //Строки для смены языка
        string drag = "drag or press";
        string drop = "drop";
        string del = "delete";
        string wrn = "Warning!";
        string file_one = "drop only 1 file!";
        string file_format = "wrong file format dropped!";
        string file_format_select = "wrong file format selected!";
        string[] words =
        {
            "File",
            "Save as...",
            "Exit",
            "Language",
            "English",
            "Russian",
            "About",
            "Go to Repository ->",
            "About ORM-MAKER"
        };
        int sz = 0;
        enum Channel {
            Red,
            Green,
            Blue,
            Erase_Red,
            Erase_Green,
            Erase_Blue
        }
        private void Form1_Load(object sender, EventArgs e) {
            //Установить размер выходного файла
            if (!is_size_set) {
                do {
                    var str = Interaction.InputBox("Enter output file size: \n (from 128 to 16384)", "", "1024");
                    if (str.Length == 0) {
                        this.Close();
                        return;
                    }
                    else if (int.TryParse(str, out sz))
                        sz = int.Parse(str);
                } while (!(sz >= 128 && sz <= 16384));
                is_size_set = true;
                result = new Bitmap(sz, sz);
            }
            pictureBox1.AllowDrop = pictureBox2.AllowDrop = pictureBox3.AllowDrop = true;
            pictureBox1.SizeMode = pictureBox2.SizeMode = pictureBox3.SizeMode = pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            label1.AllowDrop = label2.AllowDrop = label3.AllowDrop = true;
            progressBar1.Maximum = sz;
            progressBar1.Visible = false;
            //Язык
            _o_btn.Text = _r_btn.Text = _m_btn.Text = del;
            label1.Text = label2.Text = label3.Text = drag;
            fileToolStripMenuItem.Text = words[0];
            saveAsToolStripMenuItem.Text = words[1];
            exitToolStripMenuItem.Text = words[2];
            languageToolStripMenuItem.Text = words[3];
            englishToolStripMenuItem.Text = words[4];
            russianToolStripMenuItem.Text = words[5];
            aboutToolStripMenuItem.Text = words[6];
            goToRepositoryToolStripMenuItem.Text = words[7];
            aboutORMMAKERToolStripMenuItem.Text = words[8];
        }
        private void drag_enter(DragEventArgs e, Label lbl) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
                lbl.Text = drop;
            }
        }
        private void drag_leave(Label lbl) {
            lbl.Text = drag;
        }
        private void button_click(EventArgs e, Channel ch) {
            switch (ch) {
                case Channel.Erase_Red:
                    label1.Visible = true;
                    _o_btn.Visible = false;
                    pictureBox1.Image = null;
                    break;
                case Channel.Erase_Green:
                    label2.Visible = true;
                    _r_btn.Visible = false;
                    pictureBox2.Image = null;
                    break;
                case Channel.Erase_Blue:
                    label3.Visible = true;
                    _m_btn.Visible = false;
                    pictureBox3.Image = null;
                    break;
                default:
                    break;
            }
            dispose_bitmap(ch);
        }
        private void picturebox_click(EventArgs e, Channel ch) {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tiff)|*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tiff";
            if (file.ShowDialog() == DialogResult.OK) {
                Image img = Image.FromFile(file.FileName);
                Bitmap map = new Bitmap(img, sz, sz);
                Bitmap thumbnail = new Bitmap(img, 150, 150);
                img.Dispose();
                //Установить картинку в picturebox в зависимости от канала
                switch (ch) {
                    case Channel.Red:
                        label1.Visible = false;
                        _o_btn.Visible = true;
                        pictureBox1.Image = thumbnail;
                        break;
                    case Channel.Green:
                        label2.Visible = false;
                        _r_btn.Visible = true;
                        pictureBox2.Image = thumbnail;
                        break;
                    case Channel.Blue:
                        label3.Visible = false;
                        _m_btn.Visible = true;
                        pictureBox3.Image = thumbnail;
                        break;
                    default:
                        break;
                }
                //Обновить выходной файл
                update_bitmap(ref map, ch);
                //Очистить память
                map.Dispose();
            }
        }
        private void picturebox_dragdrop(DragEventArgs e, Channel ch) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1) {
                if (files[0].EndsWith(".jpg") || files[0].EndsWith(".JPG")  ||
                    files[0].EndsWith(".jpeg")|| files[0].EndsWith(".JPEG") ||
                    files[0].EndsWith(".png") || files[0].EndsWith(".PNG")  ||
                    files[0].EndsWith(".bmp") || files[0].EndsWith(".BMP")  ||
                    files[0].EndsWith(".gif") || files[0].EndsWith(".GIF")  ||
                    files[0].EndsWith(".tiff")|| files[0].EndsWith(".TIFF")) {
                    Image img = Image.FromFile(files[0]);
                    Bitmap map = new Bitmap(img, sz, sz);
                    Bitmap thumbnail = new Bitmap(img, 150, 150);
                    img.Dispose();
                    //Установить картинку в picturebox в зависимости от канала
                    switch (ch) {
                        case Channel.Red:
                            label1.Visible = false;
                            _o_btn.Visible = true;
                            pictureBox1.Image = thumbnail;
                            break;
                        case Channel.Green:
                            label2.Visible = false;
                            _r_btn.Visible = true;
                            pictureBox2.Image = thumbnail;
                            break;
                        case Channel.Blue:
                            label3.Visible = false;
                            _m_btn.Visible = true;
                            pictureBox3.Image = thumbnail;
                            break;
                        default:
                            break;
                    }
                    //Обновить выходной файл
                    update_bitmap(ref map, ch);
                    //Очистить память
                    map.Dispose();
                }
                else
                    MessageBox.Show(file_format, wrn, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show(file_one, wrn, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            switch (ch) {
                case Channel.Red:
                    label1.Text = drag;
                    break;
                case Channel.Green:
                    label2.Text = drag;
                    break;
                case Channel.Blue:
                    label3.Text = drag;
                    break;
                default:
                    break;
            }
        }
        private void update_bitmap(ref Bitmap map, Channel ch) {
            progressBar1.Visible = true;
            BmpPixelSnoop result_map = new BmpPixelSnoop(result);
            BmpPixelSnoop pic_map = new BmpPixelSnoop(map);
            for (int i = 0; i < sz; i++) {
                for (int j = 0; j < sz; j++) {
                    Color clr = pic_map.GetPixel(i, j);
                    int grayScale = (int)((clr.R + clr.G + clr.B) / 3);
                    Color cur_clr = result_map.GetPixel(i, j);
                    Color new_clr = cur_clr;
                    //В зависимости от выбранного канала, записать цвет
                    switch (ch) {
                        case Channel.Red:
                            new_clr = Color.FromArgb(grayScale, cur_clr.G, cur_clr.B);
                            break;
                        case Channel.Green:
                            new_clr = Color.FromArgb(cur_clr.R, grayScale, cur_clr.B);
                            break;
                        case Channel.Blue:
                            new_clr = Color.FromArgb(cur_clr.R, cur_clr.G, grayScale);
                            break;
                        default:
                            break;
                    }
                    result_map.SetPixel(i, j, new_clr);
                }
                progressBar1.Value++;
            }
            //Очистить BmpPixelSnoop
            result_map.Dispose();
            pic_map.Dispose();
            pictureBox4.Image = result;
            progressBar1.Value = 0;
            progressBar1.Visible = false;
        }
        private void dispose_bitmap(Channel ch) {
            progressBar1.Visible = true;
            BmpPixelSnoop result_map = new BmpPixelSnoop(result);
            for (int i = 0; i < sz; i++) {
                for (int j = 0; j < sz; j++) {
                    Color cur_clr = result_map.GetPixel(i, j);
                    Color new_clr = cur_clr;
                    //В зависимости от выбранного канала, записать цвет
                    switch (ch) {
                        case Channel.Erase_Red:
                            new_clr = Color.FromArgb(0, cur_clr.G, cur_clr.B);
                            break;
                        case Channel.Erase_Green:
                            new_clr = Color.FromArgb(cur_clr.R, 0, cur_clr.B);
                            break;
                        case Channel.Erase_Blue:
                            new_clr = Color.FromArgb(cur_clr.R, cur_clr.G, 0);
                            break;
                        default:
                            break;
                    }
                    result_map.SetPixel(i, j, new_clr);
                }
                progressBar1.Value++;
            }
            //Очистить BmpPixelSnoop
            result_map.Dispose();
            pictureBox4.Image = result;
            progressBar1.Value = 0;
            progressBar1.Visible = false;
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e) {
            englishToolStripMenuItem.Checked = true;
            russianToolStripMenuItem.Checked = false;
            drag = "drag or press";
            drop = "drop";
            del = "delete";
            wrn = "Warning!";
            file_one = "drop only 1 file!";
            file_format = "wrong file format dropped!";
            file_format_select = "wrong file format selected!";
            words[0] = "File";
            words[1] = "Save as...";
            words[2] = "Exit";
            words[3] = "Language";
            words[4] = "English";
            words[5] = "Russian";
            words[6] = "About";
            words[7] = "Go to Repository ->";
            words[8] = "About ORM-MAKER";
            this.OnLoad(e);
        }
        private void russianToolStripMenuItem_Click(object sender, EventArgs e) {
            russianToolStripMenuItem.Checked = true;
            englishToolStripMenuItem.Checked = false;
            drag = "перетащи или нажми";
            drop = "отпусти";
            del = "удалить";
            wrn = "Внимание!";
            file_one = "перетащи только 1 файл!";
            file_format = "неправильный формат файла!";
            file_format_select = "неправильный формат файла!";
            words[0] = "Файл";
            words[1] = "Сохранить как...";
            words[2] = "Выход";
            words[3] = "Язык";
            words[4] = "Английский";
            words[5] = "Русский";
            words[6] = "О программе";
            words[7] = "Перейти в Репозиторий ->";
            words[8] = "Об ORM-MAKER";
            this.OnLoad(e);
        }
        private void aboutORMMAKERToolStripMenuItem_Click(object sender, EventArgs e) {
            about menu = new about();
            menu.flag = englishToolStripMenuItem.Checked;
            menu.ShowDialog();
        }
        private void goToRepositoryToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/tokursky/orm-maker");
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tiff)|*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tiff";
            file.OverwritePrompt = true;
            file.RestoreDirectory = true;
            file.AddExtension = true;
            if (file.ShowDialog() == DialogResult.OK) {
                string ext = System.IO.Path.GetExtension(file.FileName);
                switch (ext) {
                    case ".jpeg":
                    case ".jpg":
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        result.Save(file.FileName, jpgEncoder, myEncoderParameters);
                        break;
                    case ".png":
                        result.Save(file.FileName, ImageFormat.Png);
                        break;
                    case ".bmp":
                        result.Save(file.FileName, ImageFormat.Bmp);
                        break;
                    case ".gif":
                        result.Save(file.FileName, ImageFormat.Gif);
                        break;
                    case ".tiff":
                        result.Save(file.FileName, ImageFormat.Tiff);
                        break;
                    default:
                        MessageBox.Show(file_format_select, wrn, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format) {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FormatID == format.Guid) {
                    return codec;
                }
            }
            return null;
        }
        private void pictureBox1_Click(object sender, EventArgs e) {
            picturebox_click(e, Channel.Red);
        }
        private void pictureBox2_Click(object sender, EventArgs e) {
            picturebox_click(e, Channel.Green);
        }
        private void pictureBox3_Click(object sender, EventArgs e) {
            picturebox_click(e, Channel.Blue);
        }
        private void pictureBox1_DragDrop(object sender, DragEventArgs e) {
            picturebox_dragdrop(e, Channel.Red);
        }
        private void pictureBox2_DragDrop(object sender, DragEventArgs e) {
            picturebox_dragdrop(e, Channel.Green);
        }
        private void pictureBox3_DragDrop(object sender, DragEventArgs e) {
            picturebox_dragdrop(e, Channel.Blue);
        }
        private void pictureBox1_DragEnter(object sender, DragEventArgs e) {
            drag_enter(e, label1);
        }
        private void pictureBox2_DragEnter(object sender, DragEventArgs e) {
            drag_enter(e, label2);
        }
        private void pictureBox3_DragEnter(object sender, DragEventArgs e) {
            drag_enter(e, label3);
        }
        private void pictureBox1_DragLeave(object sender, EventArgs e) {
            drag_leave(label1);
        }
        private void pictureBox2_DragLeave(object sender, EventArgs e) {
            drag_leave(label2);
        }
        private void pictureBox3_DragLeave(object sender, EventArgs e) {
            drag_leave(label3);
        }
        private void pictureBox1_button_Click(object sender, EventArgs e) {
            button_click(e, Channel.Erase_Red);
        }
        private void pictureBox2_button_Click(object sender, EventArgs e) {
            button_click(e, Channel.Erase_Green);
        }
        private void pictureBox3_button_Click(object sender, EventArgs e) {
            button_click(e, Channel.Erase_Blue);
        }
    }
}