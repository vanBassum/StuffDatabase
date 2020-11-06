using DYMO.Label.Framework;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StuffDatabase.Components
{
    public class Transistor : BaseComponent, ILabelConvertable
    {
        [Category("Specifics")]
        public double HFE { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double VCE { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double PD { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double IC { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double FT { get { return GetPar<double>(0); } set { SetPar<double>(value); } }


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

                        case nameof(Transistor.HFE):
                            label.FillLabelObject(name, $"HFE {Spacing(HFE)}x");
                            break;

                        case nameof(Transistor.VCE):
                            label.FillLabelObject(name, $"Vce {Spacing(VCE)}V");
                            break;

                        case nameof(Transistor.PD):
                            label.FillLabelObject(name, $"Pd  {Spacing(PD)}W");
                            break;

                        case nameof(Transistor.IC):
                            label.FillLabelObject(name, $"Ic  {Spacing(IC)}A");
                            break;

                        case nameof(Transistor.FT):
                            label.FillLabelObject(name, $"Ft  {Spacing(FT)}Hz");
                            break;

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

        public static Transistor ParseFromAlltransistor(string page)
        {
            Transistor tor  = new Transistor();

            Match m = Regex.Match(page, "Type Designator: (.+)");
            if (!m.Success)
                return null;
            tor.Name = m.Groups[1].Value;

            m = Regex.Match(page, "Polarity: (.+)");
            if (!m.Success)
                return null;
            tor.Function = m.Groups[1].Value;

            tor.PD = ParseDouble(page, @"Collector Power Dissipation[^\d]+([\d\.]+)");
            tor.VCE = ParseDouble(page, @"Collector-Emitter Voltage[^\d]+([\d\.]+)");
            tor.IC = ParseDouble(page, @"Collector Current[^\d]+([\d\.]+)");
            tor.FT = ParseDouble(page, @"Transition Frequency[^\d]+([\d\.]+)");
            tor.HFE = ParseDouble(page, @"Forward Current Transfer Ratio[^\d]+([\d\.]+)");
            return tor;
        }

    }
}
