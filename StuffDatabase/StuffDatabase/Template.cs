using DYMO.Label.Framework;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StuffDatabase
{
    public interface ILabelConvertable
    {
        void PopulateLabel(DYMO.Label.Framework.ILabel label);
    }


    public class Template
    {
        public string File { get; set; }

        public Template()
        {

        }

        public Template(string file)
        {
            File = file;
        }

        public DYMO.Label.Framework.ILabel GetLabel(object obj)
        {
            DYMO.Label.Framework.ILabel label = DYMO.Label.Framework.DieCutLabel.Open(File);

            switch (obj)
            {

                case ILabelConvertable v:
                    v.PopulateLabel(label);
                    break;
                default:
                    foreach (DYMO.Label.Framework.ILabelObject labelObj in label.Objects)
                    {
                        string name = labelObj.Name;
                        Match m = Regex.Match(name, @"([^_ \r\n]+)");

                        if (m.Success)
                        {
                            PropertyInfo propInfo = obj.GetType().GetProperty(m.Groups[1].Value);
                            if (propInfo != null)
                            {
                                object propVal = propInfo.GetValue(obj);
                                switch (propVal)
                                {
                                    case double v:
                                        label.FillLabelObject(name, v.ToHumanReadable());
                                        break;
                                    default:
                                        label.FillLabelObject(name, propVal);
                                        break;
                                }

                                
                            }
                        }
                    }
                    break;
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
