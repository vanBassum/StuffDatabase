using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace StuffDatabase
{
    public class Descriptor
    {
        public string Name { get; set; } = "New component type";

        [TypeConverter(typeof(DescriptorListStringConverter))]
        public Descriptor Inherits { get; set; } = null;
        public List<DynamicProperty> Properties { get; set; } = new List<DynamicProperty>();

        public Descriptor()
        {

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
