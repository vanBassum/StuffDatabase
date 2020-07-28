using STDLib.Misc;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace StuffDatabase
{
    public class Chemical : PropertySensitive
    {
        public string Name { get { return GetPar(" new"); } set { SetPar(value); } }
        public string Formula { get { return GetPar(""); } set { SetPar(value); } }
        public string Description { get { return GetPar(""); } set { SetPar(value); } }
        public List<GHS> GHSitems { get { return GetPar(new List<GHS>()); } set { SetPar(value); } }
        public Image GHSimg { get { return CreateGHSImage(); } }



        Image CreateGHSImage()
        {
            Image img = new Bitmap(150 * GHSitems.Count + 1, 150);
            int xPos = 0;
            using (Graphics g = Graphics.FromImage(img))
            {
                foreach (GHS ghs in GHSitems)
                {
                    string fullPath = Path.Combine(Settings.ChemicalSymbols, ghs.ImageFile);
                    Image ghsImg = Image.FromFile(fullPath);
                    g.DrawImage(ghsImg, xPos, 0);
                    xPos += ghsImg.Width;
                    ghsImg.Dispose();
                }
                
            }
            return img;
        }


        public override string ToString()
        {
            return Name;
        }
    }
    public class GHS : PropertySensitive
    {
        public string Name { get { return GetPar(""); } set { SetPar(value); } }
        public string ImageFile { get { return GetPar(""); } set { SetPar(value); } }

    }
}
