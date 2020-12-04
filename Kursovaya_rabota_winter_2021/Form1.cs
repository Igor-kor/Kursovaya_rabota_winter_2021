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
        PictureEditor pictureEditor;
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.AutoScroll = true;
            pictureEditor = new PictureEditor(flowLayoutPanel1.Controls);
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
            pictureEditor.GeneratePicture();
        }

        private void Form1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
           
        }
    }

    class PictureEditor
    {
        int x, y;
        Dictionary<string, PictureEditorItem> pictureEditorItems;
        Control.ControlCollection Controls;

        public PictureEditor(Control.ControlCollection _controls)
        {
            Controls = _controls;
            pictureEditorItems = new Dictionary<string, PictureEditorItem>();
            x = 0;
            y = 50;
        }
        public void AddItem(string file)
        {
            PictureEditorItem picture = new PictureEditorItem(file);
            pictureEditorItems.Add(x+" "+y,picture);
            picture.DrawPicture(this.Controls);
            x += 200;         
        }

        // находим элемент, удаляем, все остальные координаты смещаем и удаляем из контрола

        public void GeneratePicture()
        {
            ImageMagick.MagickNET.Initialize();
            var images = pictureEditorItems.ToArray();
            var image = new ImageMagick.MagickImageCollection();
            foreach (var img in images)
            {
                image.Add(img.Value.picture.ImageLocation);
                image[image.Count - 1].AnimationDelay = Convert.ToInt32(img.Value.delay.Text);
            }
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "*.gif";
            saveFileDialog.Filter = "Gif files(*.gif)|*.gif|All files(*.*)|*.*";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "Сохранить gif изображение";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)return;
            if (saveFileDialog.FileName.Length == 0) return;
            image.Write(saveFileDialog.FileName);
        }
    }

    class PictureEditorItem
    {
        public PictureBox picture;
        Button buttondelete;
        public TextBox delay;
        Control.ControlCollection controlForm;
        GroupBox box;

        public PictureEditorItem(string filename)
        {
            picture = new PictureBox();
            buttondelete = new Button();
            delay = new TextBox();
            box = new GroupBox();
            delay.Text = "100";
            buttondelete.Text = "Delete";
            picture.Click += new EventHandler(picture_DoubleClick);
            buttondelete.Click += new EventHandler(button_Click);
            picture.ImageLocation = System.IO.Path.GetFullPath(filename);
            picture.Width = 200;
            picture.Height = 200;
            box.Width = 200;
            box.Height = 400;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button_Click(object sender, EventArgs e)
        {
            Remove();
        }

        public void Remove()
        {
            //controlForm.Remove(picture);
            //controlForm.Remove(buttondelete);
           // controlForm.Remove(delay);
            controlForm.Remove(box);
            // удалится из списка надо
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

        public void DrawPicture(Control.ControlCollection Controls)
        {
            controlForm = Controls;
            box.Controls.Add(picture);
            box.Controls.Add(buttondelete);
            box.Controls.Add(delay);
            Controls.Add(box);
            picture.Location = new System.Drawing.Point(0,0);
            buttondelete.Location = new System.Drawing.Point(0,200);
            delay.Location = new System.Drawing.Point(0,230);           
        }
    }
}
