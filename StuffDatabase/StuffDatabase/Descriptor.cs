using Newtonsoft.Json;
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
        public string Name { get => GetPar<string>("New type"); set => SetPar(value); }
        public bool UseAsComponent { get => GetPar<bool>(true); set => SetPar(value); }
        public BindingList<DynamicProperty> Properties { get; } = new BindingList<DynamicProperty>();

        [Browsable(false)]
        [JsonProperty("DescriptorID")]
        public int ID { get; private set; } = -1;


        [Browsable(false)]
        [JsonProperty("InheritedDescriptorID")]
        int InheritedDescriptorID { get; set; } = -1;

        [TypeConverter(typeof(DescriptorListStringConverter))]
        [JsonIgnore]
        public Descriptor Inherits { get => GetInheritedDescriptor(); set => SetInheritedDescriptor(value); }




        [JsonIgnore]
        public static BindingList<Descriptor> Descriptors { get; set; }





        public void SetInheritedDescriptor(Descriptor descriptor)
        {
            if (descriptor == null)
                InheritedDescriptorID = -1;
            else
                InheritedDescriptorID = descriptor.ID;
        }

        public Descriptor GetInheritedDescriptor()
        {
            Descriptor d = null;
            if (Descriptors != null)
                d = Descriptors.FirstOrDefault(a => a.ID == InheritedDescriptorID);
            return d;
        }

        public Descriptor()
        {
            Properties.ListChanged += Properties_ListChanged;
        }


        public static Descriptor Create()
        {
            Descriptor d = new Descriptor();

            int id = 0;
            while (Descriptors.Any(a=>a.ID == id))
                id++;
            d.ID = id;

            return d;
        }



        private void Properties_ListChanged(object sender, ListChangedEventArgs e)
        {
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(Properties)));
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
