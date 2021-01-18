using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StuffDatabase
{
    public class DictionaryPropertyGridAdapter : ICustomTypeDescriptor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("Fields")]
        private readonly Dictionary<string, object> Fields = new Dictionary<string, object>();

        //public IDictionary Fields { get; set; } = new Dictionary<string, object>();

        public DictionaryPropertyGridAdapter()
        {
            foreach (var pi in this.GetType().GetProperties())
            {
                pi.GetValue(this);
            }
        }

        protected void SetPar<T>(T value, [CallerMemberName] string propertyName = null)
        {
            Fields[propertyName] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected T GetPar<T>(T defVal = default(T), [CallerMemberName] string propertyName = null)
        {
            if (!Fields.ContainsKey(propertyName))
                Fields[propertyName] = defVal;
            return (T)Fields[propertyName];
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            ArrayList properties = new ArrayList();
            foreach (var e in Fields)
            {
                DictionaryPropertyDescriptor descr = new DictionaryPropertyDescriptor(Fields, e.Key, this);
                descr.PropertyChanged +=  (a,b) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((string)e.Key));
                properties.Add(descr);
            }

            PropertyDescriptor[] props =
                (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

            return new PropertyDescriptorCollection(props);
        }
    }
}
