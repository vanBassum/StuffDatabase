using STDLib.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace StuffDatabase
{
    public class Descriptor : PropertySensitive
    {
        public static HashSet<int> ids = new HashSet<int>();

        public int ID { get; }
        public string Name { get => GetPar<string>("New type"); set => SetPar(value); }

        [TypeConverter(typeof(DescriptorListStringConverter))]
        public Descriptor Inherits { get => GetPar<Descriptor>(null);  set => SetPar(value);  }
        public bool UseAsComponent { get => GetPar<bool>(true); set => SetPar(value); }
        public BindingList<DynamicProperty> Properties { get; } = new BindingList<DynamicProperty>();

        public Descriptor()
        {
            int id = 0;
            while (ids.Contains(id))
                id++;
            ID = id;
            ids.Add(id);

            Properties.ListChanged += Properties_ListChanged;
        }

        private void Properties_ListChanged(object sender, ListChangedEventArgs e)
        {
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Properties)));
            /*
            switch(e.ListChangedType)
            {
                case ListChangedType.
            }*/
        }

        public override string ToString()
        {
            return Name;
        }
    }



    public class DescriptorListStringConverter : StringConverter
    {
        public static BindingList<Descriptor> Objects = new BindingList<Descriptor>();

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Objects);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return Objects.FirstOrDefault(a=>a.Name == value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return base.GetStandardValuesExclusive(context);
        }
    }
}
