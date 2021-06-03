using STDLib.Misc;

namespace StuffDatabase
{
    public class PartItem : PropertySensitive
    {
        public string Name { get => GetPar(""); set => SetPar(value); }
        public PartType Type { get => GetPar<PartType>(); set => SetPar(value); }

        public override string ToString()
        {
            return Name;
        }
    }



}
