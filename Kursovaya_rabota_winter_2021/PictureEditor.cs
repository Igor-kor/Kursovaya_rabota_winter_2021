using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Kursovaya_rabota_winter_2021
{
    class PictureEditor
    {
        Control.ControlCollection Controls;
        List<PictureEditorItem> pictures;

        public PictureEditor(FlowLayoutPanel flowLayoutPanel)
        {
            Controls = flowLayoutPanel.Controls;
            pictures = new List<PictureEditorItem>();
        }
        public void AddItem(string file)
        {
            PictureEditorItem picture = new PictureEditorItem(file);
            pictures.Add(picture);
            picture.DrawPicture(this.Controls);
        }

        public void GeneratePicture()
        {
            ImageMagick.MagickNET.Initialize();
            var image = new ImageMagick.MagickImageCollection();

            SortedDictionary<int, PictureEditorItem> saved = new SortedDictionary<int, PictureEditorItem>();

            foreach (PictureEditorItem picture in pictures)
            {
                if (Controls.Find(picture.box.Name, false).Length > 0)
                {
                    saved.Add(Controls.IndexOf(Controls.Find(picture.box.Name, false)[0]), picture);                  
                }
            }

            foreach (var img in saved.Values)
            {
                image.Add(img.picture.ImageLocation);
                image[image.Count - 1].AnimationDelay = Convert.ToInt32(img.delay.Text);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "*.gif";
            saveFileDialog.Filter = "Gif files(*.gif)|*.gif|All files(*.*)|*.*";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "Сохранить gif изображение";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel) return;
            if (saveFileDialog.FileName.Length == 0) return;
            image.Write(saveFileDialog.FileName);
        }
    }
}
