using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
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


}
