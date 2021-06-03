using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace orm_maker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Image files
        Image _o_img;
        Image _r_img;
        Image _m_img;

        //Bitmaps
        Bitmap result;
        Bitmap _o_map;
        Bitmap _r_map;
        Bitmap _m_map;

        //Delete buttons
        Button _o_btn = new Button();
        Button _r_btn = new Button();
        Button _m_btn = new Button();

        //Flag for images
        bool is_o_image_set = false;
        bool is_r_image_set = false;
        bool is_m_image_set = false;
        bool is_size_set = false;

        //Language setters
        string drag = "drag or press";
        string drop = "drop";
        string del = "delete";
        string wrn = "Warning!";
        string file_one = "drop only 1 file!";
        string file_format = "wrong file format dropped!";
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
        int sz = 1024;

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set output image size
            if (!is_size_set)
            {
                string str = "";
                 do
                 {
                    str = Interaction.InputBox("Enter output file size: \n (256<=size<=16384)");
                    if (str.Length == 0)
                        this.Close();                
                    else 
                        if (int.TryParse(str,out sz))
                            sz = int.Parse(str);
                    } while (!(sz >= 256 && sz <= 16384));
                
                is_size_set = true;
            }

            //Click events to buttons
            _o_btn.Click += new EventHandler(pictureBox1_button_Click);
            _r_btn.Click += new EventHandler(pictureBox2_button_Click);
            _m_btn.Click += new EventHandler(pictureBox3_button_Click);
            //Button parameters
            _o_btn.Size = _r_btn.Size = _m_btn.Size = new Size(80, 40);
            _o_btn.Location = _r_btn.Location = _m_btn.Location = new Point(33, 52);

            //Allow drop
            pictureBox1.AllowDrop = pictureBox2.AllowDrop = pictureBox3.AllowDrop = true;
            pictureBox1.SizeMode = pictureBox2.SizeMode = pictureBox3.SizeMode = pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            label1.AllowDrop = label2.AllowDrop = label3.AllowDrop = true;

            //Bitmaps init
            if (!is_o_image_set)
            {
                _o_map = new Bitmap(sz, sz);
                using (var graphics = Graphics.FromImage(_o_map))
                    graphics.Clear(Color.Black);
                _o_btn.Visible = false;
            }
            if (!is_r_image_set)
            {
                _r_map = new Bitmap(sz, sz);
                using (var graphics = Graphics.FromImage(_r_map))
                    graphics.Clear(Color.Black);
                _r_btn.Visible = false;
            }
            if (!is_m_image_set)
            {
                _m_map = new Bitmap(sz, sz);
                using (var graphics = Graphics.FromImage(_m_map))
                {
                    graphics.Clear(Color.Black);
                }
                _m_btn.Visible = false;
            }
            result = new Bitmap(sz, sz);

            //Progressbar init
            progressBar1.Maximum = sz;
            progressBar1.Visible = false;

            //Language
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
        private void drag_enter(DragEventArgs e, Label lbl)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                lbl.Text = drop;
            }
        }
        private void drag_leave(Label lbl)
        {
            lbl.Text = drag;
        }
        private void button_click(EventArgs e, ref PictureBox box, ref Image img, ref Button btn, ref Label lbl, ref bool flag, ref Bitmap map)
        {
            box.Image = pictureBox4.Image = null;
            img = null;
            btn.Visible = false;
            lbl.Visible = true;
            flag = false;
            map = new Bitmap(sz, sz);
            using (var graphics = Graphics.FromImage(map))
                graphics.Clear(Color.Black);
            //Update final bitmap
            update_bitmap();

        }
        private void picturebox_click(EventArgs e, ref PictureBox box, ref Image img, ref Button btn, ref Label lbl, ref bool flag, ref Bitmap map)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image files (*.JPG, *.PNG)|*.jpg;*.png";
            if (file.ShowDialog() == DialogResult.OK)
            {
                //Controls
                lbl.Visible = false;
                btn.Visible = true;

                //Change flag
                flag = true;

                //Set image
                img = Image.FromFile(file.FileName);
                map = new Bitmap(img, sz, sz);
                box.Image = map;
                btn.Parent = box;

                //Update final bitmap
                update_bitmap();
            }
        }
        private void picturebox_dragdrop(DragEventArgs e, ref PictureBox box, ref Image img, ref Button btn, ref Label lbl, ref bool flag, ref Bitmap map)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1)
            {
                if (files[0].EndsWith(".jpg") || files[0].EndsWith(".JPG") ||
                    files[0].EndsWith(".png") || files[0].EndsWith(".PNG"))
                {
                    //Controls
                    lbl.Visible = false;
                    btn.Visible = true;

                    //Change flag
                    flag = true;

                    //Set image
                    img = Image.FromFile(files[0]);
                    map = new Bitmap(img, sz, sz);
                    box.Image = map;
                    btn.Parent = box;


                    //Update final bitmap
                    update_bitmap();
                }
                else
                    MessageBox.Show(file_format, wrn, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show(file_one, wrn, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            lbl.Text = drag;
        }
        private void update_bitmap() {
            if (!(_o_img == null && _r_img == null && _m_img == null))
            {
                progressBar1.Visible = true;
                for (int i = 0; i < sz; i++)
                {
                    progressBar1.Value++;
                    for (int j = 0; j < sz; j++)
                    {
                        result.SetPixel(i, j, Color.FromArgb(
                                       _o_map.GetPixel(i, j).R,
                                       _r_map.GetPixel(i, j).G,
                                       _m_map.GetPixel(i, j).B
                                   ));
                    }
                }
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                pictureBox4.Image = result;
            }
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            englishToolStripMenuItem.Checked = true;
            russianToolStripMenuItem.Checked = false;
            drag = "drag or press";
            drop = "drop";
            del = "delete";
            wrn = "Warning!";
            file_one = "drop only 1 file!";
            file_format = "wrong file format dropped!";
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
        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            russianToolStripMenuItem.Checked = true;
            englishToolStripMenuItem.Checked = false;
            drag = "перетащи или нажми";
            drop = "отпусти";
            del = "удалить";
            wrn = "Внимание!";
            file_one = "перетащи только 1 файл!";
            file_format = "неправильный формат файла!";
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
        private void aboutORMMAKERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about menu = new about();
            menu.flag = englishToolStripMenuItem.Checked;
            menu.ShowDialog();
        }
        private void goToRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/tokursky/ORM-Maker");
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Image files (*.JPG, *.PNG)|*.jpg;*.png";
            //result.SetResolution(sz, sz);
            if (file.ShowDialog() == DialogResult.OK)
            {
                string file_ext = file.FileName.Remove(0, file.FileName.Length - 3);
                switch(file_ext) {
                    case "jpg":
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        result.Save(file.FileName, jpgEncoder, myEncoderParameters);
                        break;
                    case "png":
                        result.Save(file.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            picturebox_click(e, ref pictureBox1, ref _o_img, ref _o_btn, ref label1, ref is_o_image_set, ref _o_map);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            picturebox_click(e, ref pictureBox2, ref _r_img, ref _r_btn, ref label2, ref is_r_image_set, ref _r_map);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            picturebox_click(e, ref pictureBox3, ref _m_img, ref _m_btn, ref label3, ref is_m_image_set, ref _m_map);
        }
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            picturebox_dragdrop(e, ref pictureBox1, ref _o_img, ref _o_btn, ref label1, ref is_o_image_set, ref _o_map);
        }
        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {
            picturebox_dragdrop(e, ref pictureBox2, ref _r_img, ref _r_btn, ref label2, ref is_r_image_set, ref _r_map);
        }
        private void pictureBox3_DragDrop(object sender, DragEventArgs e)
        {
            picturebox_dragdrop(e, ref pictureBox3, ref _m_img, ref _m_btn, ref label3, ref is_m_image_set, ref _m_map);
        }
        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            drag_enter(e, label1);
        }
        private void pictureBox2_DragEnter(object sender, DragEventArgs e)
        {
            drag_enter(e, label2);
        }
        private void pictureBox3_DragEnter(object sender, DragEventArgs e)
        {
            drag_enter(e, label3);
        }
        private void pictureBox1_DragLeave(object sender, EventArgs e)
        {
            drag_leave(label1);
        }
        private void pictureBox2_DragLeave(object sender, EventArgs e)
        {
            drag_leave(label2);
        }
        private void pictureBox3_DragLeave(object sender, EventArgs e)
        {
            drag_leave(label3);
        }
        private void pictureBox1_button_Click(object sender, EventArgs e)
        {
            button_click(e, ref pictureBox1, ref _o_img, ref _o_btn, ref label1, ref is_o_image_set, ref _o_map);
        }
        private void pictureBox2_button_Click(object sender, EventArgs e)
        {
            button_click(e, ref pictureBox2, ref _r_img, ref _r_btn, ref label2, ref is_r_image_set, ref _r_map);
        }
        private void pictureBox3_button_Click(object sender, EventArgs e)
        {
            button_click(e, ref pictureBox3, ref _m_img, ref _m_btn, ref label3, ref is_m_image_set, ref _m_map);
        }  
    }
}