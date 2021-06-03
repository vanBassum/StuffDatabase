using STDLib.Misc;
using System.ComponentModel;

namespace StuffDatabase
{
    public class PartType : PropertySensitive
    {
        public string Name { get => GetPar(""); set => SetPar(value); }

        
        public BindingList<PartParameter> Parameters { get; } = new BindingList<PartParameter>();

        public override string ToString()
        {
            return Name;
        }
    }






}
