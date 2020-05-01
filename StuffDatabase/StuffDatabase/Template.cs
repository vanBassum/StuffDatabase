using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace StuffDatabase
{
    public class Template<T>
    {
        public string File { get; set; }

        public Template()
        {

        }

        public Template(string file)
        {
            File = file;
        }

        public DYMO.Label.Framework.ILabel GetLabel(T chem)
        {
            DYMO.Label.Framework.ILabel label = DYMO.Label.Framework.DieCutLabel.Open(File);

            foreach (DYMO.Label.Framework.ILabelObject obj in label.Objects)
            {
                string name = obj.Name;

                PropertyInfo propInfo = typeof(T).GetProperty(name);

                if (propInfo != null)
                {
                    object prop = propInfo.GetValue(chem);

                    switch (prop)
                    {
                        case string v:
                            label.SetObjectText(name, v);
                            break;
                        case Image i:
                            label.SetImagePngData("GHSimg", ImgToStream(i, ImageFormat.Png));
                            break;
                    }
                }

            }
            return label;
        }

        Stream ImgToStream(Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }


        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(File);
        }

    }

}
