using DYMO.Label.Framework;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StuffDatabase.Components
{
    public class Opamp : BaseComponent, ILabelConvertable
    {
        [Category("Specifics")]
        public double UnityGainBandwidth { get { return GetPar<double>(0); } set { SetPar<double>(value); } }

        [Category("Specifics")]
        public double Slewrate { get { return GetPar<double>(0); } set { SetPar<double>(value); } }

        [Category("Specifics")]
        public double IOut { get { return GetPar<double>(0); } set { SetPar<double>(value); } }





        string Spacing(double var, int space = 8)
        {
            string hrd = Ext.ToHumanReadable(var, 0);
            int i = space - Regex.Match(hrd, @"[\d\.]+").Value.Length;
            return new string(' ', i) + hrd;
        }

        public void PopulateLabel(ILabel label)
        {
            foreach (DYMO.Label.Framework.ILabelObject labelObj in label.Objects)
            {
                string name = labelObj.Name;
                Match m = Regex.Match(name, @"([^_ \r\n]+)");

                if (m.Success)
                {
                    switch (m.Groups[1].Value)
                    {
                        default:
                            PropertyInfo propInfo = this.GetType().GetProperty(m.Groups[1].Value);
                            if (propInfo != null)
                            {
                                string txt = propInfo.GetValue(this).ToString();
                                label.FillLabelObject(name, txt);
                            }
                            break;
                    }
                }
            }
        }
    }
}
