using System;
using System.Windows.Forms;

namespace Kursovaya_rabota_winter_2021
{
    class PictureEditorItem 
    {
        public PictureBox picture;
        Button buttondelete;
        public TextBox delay;
        Control.ControlCollection controlForm;
        public GroupBox box;
        bool moved = false;

        public PictureEditorItem(string filename)
        {
            picture = new PictureBox();
            buttondelete = new Button();
            delay = new TextBox();
            box = new GroupBox();
            box.Name = filename;
            delay.Text = "100";
            buttondelete.Text = "Удалить";
            picture.Click += new EventHandler(picture_DoubleClick);
            buttondelete.Click += new EventHandler(button_Click);
            picture.ImageLocation = System.IO.Path.GetFullPath(filename);
            picture.Width = 200;
            picture.Height = 200;
            box.Width = 200;
            box.Height = 280;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            }

        private void button_Click(object sender, EventArgs e)
        {
            Remove();
        }

        public void Remove()
        {
            controlForm.Remove(box);
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
