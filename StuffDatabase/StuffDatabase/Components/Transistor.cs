using System.ComponentModel;

namespace StuffDatabase.Components
{
    public class Transistor : BaseComponent
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
    }
}
