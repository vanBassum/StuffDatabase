using System.Drawing;
using System.IO;
using System.Linq;

namespace StuffDatabase
{
    public class Dymo
    {
        public static Image Render<T>(Template<T> template, T comp)
        {
            DYMO.Label.Framework.IPrinter printer = DYMO.Label.Framework.Framework.GetPrinters().FirstOrDefault();
            Image image = Image.FromStream(new MemoryStream(template.GetLabel(comp).RenderAsPng(printer, new DYMO.Label.Framework.LabelRenderParams())));
            return image;
        }

        public static void Print<T>(Template<T> template, T comp)
        {
            DYMO.Label.Framework.IPrinter printer = DYMO.Label.Framework.Framework.GetPrinters().FirstOrDefault();
            template.GetLabel(comp).Print(printer);
        }

    }

}
