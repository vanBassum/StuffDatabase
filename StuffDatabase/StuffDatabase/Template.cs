using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

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

        public DYMO.Label.Framework.ILabel GetLabel(T obj)
        {
            DYMO.Label.Framework.ILabel label = DYMO.Label.Framework.DieCutLabel.Open(File);

            foreach (DYMO.Label.Framework.ILabelObject labelObj in label.Objects)
            {
                string name = labelObj.Name;
                Match m = Regex.Match(name, @"([^_ \r\n]+)");

                if (m.Success)
                {
                    PropertyInfo propInfo = obj.GetType().GetProperty(m.Groups[1].Value);
                    if (propInfo != null)
                    {
                        string txt = propInfo.GetValue(obj).ToString();
                        label.FillLabelObject(name, txt);
                    }
                }

            }
            return label;
        }

        public DYMO.Label.Framework.ILabel GetLabel(T[] objs)
        {
            DYMO.Label.Framework.ILabel label = DYMO.Label.Framework.DieCutLabel.Open(File);

            foreach (DYMO.Label.Framework.ILabelObject labelObj in label.Objects)
            {
                string name = labelObj.Name;

                Match m = Regex.Match(name, @"(.+?)_(\d+)");

                if (m.Success)
                {
                    PropertyInfo propInfo = typeof(T).GetProperty(m.Groups[1].Value);
                    int ind = int.Parse(m.Groups[2].Value);
                    if (propInfo != null)
                        label.FillLabelObject(name, propInfo.GetValue(objs[ind]));
                }
            }
            return label;
        }




        


        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(File);
        }

    }

    public static class DymoExt
    {
        public static void FillLabelObject(this DYMO.Label.Framework.ILabel label, string name, object value)
        {
            switch (value)
            {
                case string v:
                    label.SetObjectText(name, v);
                    break;

                case Image i:
                    label.SetImagePngData("GHSimg", ImgToStream(i, ImageFormat.Png));
                    break;
            }
        }

        static Stream ImgToStream(Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }
    }

}
