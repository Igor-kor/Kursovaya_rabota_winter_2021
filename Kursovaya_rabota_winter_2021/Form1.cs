using System;
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
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
     
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
           
            string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
            foreach (string file in files)
            {
                PictureEditor picture = new PictureEditor(file);
                picture.DrawPicture(this.Controls);
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

        private void Form1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
           
        }
    }

    class PictureEditor
    {
        PictureBox picture;
        Button buttondelete;
        Control.ControlCollection controlForm;

        public PictureEditor(string filename)
        {
            picture = new PictureBox();
            buttondelete = new Button();
            buttondelete.Text = "Delete";
            buttondelete.Click += new EventHandler(button_Click);
            picture.ImageLocation = System.IO.Path.GetFullPath(filename);
            picture.Width = 100;
            picture.Height = 100;
        }

        private void button_Click(object sender, EventArgs e)
        {
            controlForm.Remove(picture);
            controlForm.Remove(buttondelete);
        }

        public void DrawPicture(Control.ControlCollection Controls)
        {
            controlForm = Controls;
            Controls.Add(picture);
            Controls.Add(buttondelete);
            picture.Location = new System.Drawing.Point(0, 0);
            buttondelete.Location = new System.Drawing.Point(0, 100);
        }
    }
}
