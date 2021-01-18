using Newtonsoft.Json;
using STDLib.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace StuffDatabase
{

    public class Part : PGA//DictionaryPropertyGridAdapter
    {
        [JsonIgnore]
        [Category("Always")]
        public string Name  { get { return GetPar<string>(""); } set { SetPar(value); } }

        public static Part Create(Descriptor descriptor)
        {
            Part part = new Part();
            part.SetDescriptor(descriptor);
            return part;
        }

        public override string ToString()
        {
            return Name;
        }
    }





    public class PGA : ICustomTypeDescriptor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("Fields")]
        private readonly Dictionary<string, object> Fields = new Dictionary<string, object>();

        [JsonIgnore]
        public static BindingList<Descriptor> Descriptors { get; set; }

        [Browsable(false)]
        [JsonProperty("DescriptorID")]
        int DescriptorID { get; set; } = -1;


        public PGA()
        {

        }



        public void SetDescriptor(Descriptor descriptor)
        {
            DescriptorID = descriptor.ID;
        }

        public Descriptor GetDescriptor()
        {
            return Descriptors.FirstOrDefault(a => a.ID == DescriptorID);
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

        public PropertyDescriptorCollection GetProperties()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {

            List<PropertyDescriptor> descriptors = new List<PropertyDescriptor>();

            foreach (var pi in this.GetType().GetProperties())
            {
                BrowsableAttribute browsable = null;

                if ((browsable = pi.GetCustomAttribute<BrowsableAttribute>()) != null)
                {
                    if (!browsable.Browsable)
                        continue;
                }

                PD pd = new PD(pi.Name, attributes, pi.PropertyType);

                CategoryAttribute cat = null;
                if ((cat = pi.GetCustomAttribute<CategoryAttribute>()) != null)
                {
                    pd.SetCategory(cat.Category);
                }


                pd.Getter = () => pi.GetValue(this);
                pd.Setter = (a) => SetPar(a, pi.Name);
                descriptors.Add(pd);

            }

            LoopDescriptors(descriptors, attributes);

            return new PropertyDescriptorCollection(descriptors.ToArray());

        }


        void LoopDescriptors(List<PropertyDescriptor> collection, Attribute[] attributes)
        {
            Descriptor descriptor = GetDescriptor();

            while (descriptor != null)
            {
                foreach (var p in descriptor.Properties)
                {
                    if (p.PropertyName != null)
                    {
                        if (!collection.Any(a => a.Name == p.PropertyName))
                        {
                            PD pd = new PD(p.PropertyName, attributes, p.GetSystemType());
                            pd.SetCategory(descriptor.Name);
                            pd.Getter = () => GetPar<object>(null, p.PropertyName);//{ return Fields.ContainsKey(p.PropertyName) ? Fields[p.PropertyName] : null; };
                            pd.Setter = (a) => SetPar(a, p.PropertyName);
                            collection.Add(pd);
                        }
                    }
                }

                descriptor = descriptor.Inherits;
            }
        }
    }




    public class PD : PropertyDescriptor
    {
        Type type;
        public Action<object> Setter;
        public Func<object> Getter;

        public override Type ComponentType => throw new NotImplementedException();

        public override bool IsReadOnly => false;

        public override Type PropertyType => type;

        string _Category = null;
        public override string Category { get { return _Category; } }

        public void SetCategory(string cat)
        {
            _Category = cat;
        }

        public PD(string name, Attribute[] attributes, Type type) : base(name, attributes)
        {
            this.type = type;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return Getter();
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            Setter(value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }




}
