using ImageMagick;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya_rabota_winter_2021
{
    public partial class Form1 : Form
    {
        PictureEditor pictureEditor;
        System.IO.MemoryStream imageStream;
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.AutoScroll = true;
            pictureEditor = new PictureEditor(flowLayoutPanel1);
            imageStream = new System.IO.MemoryStream();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
            foreach (string file in files)
            {
                pictureEditor.AddItem(file);
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {

        }

        private void Form1_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Form1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureEditor.SavePicture();
        }

        private void Form1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureEditor.GeneratePicture();
            imageStream.Dispose();
            imageStream = new System.IO.MemoryStream();
            pictureEditor.image.Write(imageStream, MagickFormat.Gif);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Image.FromStream(imageStream);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png|All files(*.*)|*.*";           
            openFileDialog.Title = "Открыть изображения";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            if (openFileDialog.FileName.Length == 0) return;

            foreach(string  filename in openFileDialog.FileNames)
            {
                pictureEditor.AddItem(filename);
            }
        }
    }


}
