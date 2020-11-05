using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya_rabota_winter_2021
{
    public partial class Form1 : Form
    {
        int x, y;
        Dictionary <int,PictureEditor> pictureEditor; 
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.AutoScroll = true;
            pictureEditor = new Dictionary<int, PictureEditor>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
     
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            int i = 0;
            string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
            foreach (string file in files)
            {
                PictureEditor picture = new PictureEditor(file);
                pictureEditor.Add(i++, picture);
                picture.DrawPicture(this.Controls,x,y);
                x += 200;
                if(x > 500)
                {
                    x = 0;
                    y += 220;
                }
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
            ImageMagick.MagickNET.Initialize();
            var images = pictureEditor.ToArray();
            var image = new ImageMagick.MagickImageCollection();
            foreach (var img in images){
                image.Add(img.Value.picture.ImageLocation);
                image[image.Count - 1].AnimationDelay = 50;
            }
            image.Write("result.gif");
        }

        private void Form1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
           
        }
    }

    class PictureEditor
    {
        public PictureBox picture;
        Button buttondelete;
        Control.ControlCollection controlForm;

        public PictureEditor(string filename)
        {
            picture = new PictureBox();
            buttondelete = new Button();
            buttondelete.Text = "Delete";
            picture.Click += new EventHandler(picture_DoubleClick);
            buttondelete.Click += new EventHandler(button_Click);
            picture.ImageLocation = System.IO.Path.GetFullPath(filename);
            picture.Width = 200;
            picture.Height = 200;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button_Click(object sender, EventArgs e)
        {
            controlForm.Remove(picture);
            controlForm.Remove(buttondelete);
        }

        private void picture_DoubleClick(object sender, EventArgs e)
        {
            FullPicture fullPicture = new FullPicture();
            PictureBox picturefull = new PictureBox();
            picturefull.Image = picture.Image;
            picturefull.Width = picture.Image.Width;
            picturefull.Height = picture.Image.Height;
            fullPicture.Controls.Add(picturefull);
            fullPicture.Visible = true;
            fullPicture.AutoScroll = true;
            fullPicture.Width = picture.Image.Width;
            fullPicture.Height = picture.Image.Height;
        }

        public void DrawPicture(Control.ControlCollection Controls,int x,int y)
        {
            controlForm = Controls;
            Controls.Add(picture);
            Controls.Add(buttondelete);
            picture.Location = new System.Drawing.Point(x, y);
            buttondelete.Location = new System.Drawing.Point(x, y+200);
        }
    }
}
