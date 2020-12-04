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
            pictureEditor = new PictureEditor(flowLayoutPanel1);
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
        Control.ControlCollection Controls;
        List<PictureEditorItem> pictures;
        Graphics g;
        // возвращаем старый список всех изображений и с помощьюпоиска формируем изображение

        public PictureEditor(FlowLayoutPanel flowLayoutPanel)
        {
            Controls = flowLayoutPanel.Controls;
            pictures = new List<PictureEditorItem>();
            g = flowLayoutPanel.CreateGraphics();
        }
        public void AddItem(string file)
        {
            PictureEditorItem picture = new PictureEditorItem(file,g);
            pictures.Add(picture);
            picture.DrawPicture(this.Controls);       
        }

        public void MoveItem()
        {
            PictureEditorItem[] array = new PictureEditorItem[this.Controls.Count];
            this.Controls.CopyTo(array,0);
            this.Controls.Clear();
            this.Controls.AddRange(array);
        }

        // находим элемент, удаляем, все остальные координаты смещаем и удаляем из контрола

        public void GeneratePicture()
        {
            ImageMagick.MagickNET.Initialize();
            Control []images = new Control[this.Controls.Count];
            this.Controls.CopyTo(images,0);
            var image = new ImageMagick.MagickImageCollection();
            foreach (var img in images)
            {
                image.Add(pictures[this.Controls.IndexOf(img)].picture.ImageLocation);
                image[image.Count - 1].AnimationDelay = Convert.ToInt32(pictures[this.Controls.IndexOf(img)].delay.Text);
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

    class PictureEditorItem : Control
    {
        public PictureBox picture;
        Button buttondelete;
        public TextBox delay;
        Control.ControlCollection controlForm;
        GroupBox box;
        bool moved = false;
        Graphics layoutg;

        public PictureEditorItem(string filename, Graphics _g)
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
            layoutg = _g;
            }

        private void button_Click(object sender, EventArgs e)
        {
            Remove();
        }

        public void Remove()
        {
            controlForm.Remove(box);
            // удалится из списка
        }

        public void MoveItem(int ToIndex)
        {
           controlForm.SetChildIndex(box, ToIndex);
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

        public void DrawPicture(Control.ControlCollection Controlscol)
        {
            controlForm = Controlscol;
            box.Controls.Add(picture);
            box.Controls.Add(buttondelete);
            box.Controls.Add(delay);
            box.AllowDrop = true;
            picture.MouseDown += new MouseEventHandler(BoxMouseDown);
            picture.MouseUp += new MouseEventHandler(BoxMouseUP);
            picture.MouseMove += new MouseEventHandler(BoxMouseMove);
            Controlscol.Add(box);
            picture.Location = new System.Drawing.Point(0,0);
            buttondelete.Location = new System.Drawing.Point(0,200);
            delay.Location = new System.Drawing.Point(0,230);           
        }
        
        private void BoxMouseDown(object sender, MouseEventArgs e)
        {
            moved = true;
            Cursor.Current = Cursors.SizeWE;
        } 

        private void BoxMouseMove(object sender, MouseEventArgs e)
        {

        }
        private void BoxMouseUP(object sender, MouseEventArgs e)
        {
            if (moved)
            {
                
                if (e.X > 0 && controlForm.IndexOf(box) < (controlForm.Count-1))
                {                   
                    controlForm.SetChildIndex(box, (e.X / box.Width) + controlForm.IndexOf(box));
                }
                if(e.X < 0 && controlForm.IndexOf(box) > 0)
                {
                    controlForm.SetChildIndex(box, (e.X / box.Width - 1) + controlForm.IndexOf(box));
                }
              
               moved = false;
            }
            Cursor.Current = Cursors.Default;
        }
    }


}
