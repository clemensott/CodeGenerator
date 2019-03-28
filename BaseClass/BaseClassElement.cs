using System.ComponentModel;
using System.Linq;

namespace CodeGenerator.BaseClass
{
    public enum ElementType { Indexer, Property, Method }

    public enum AccessModifier { Default, Public, Internal, Protected, Private }

    public class BaseClassElement : INotifyPropertyChanged
    {
        private ElementType type;
        private bool isStatic;
        private string datatype, name;
        private AccessModifier accessModifier;
        private AccessModifier? geterModifier, seterModifier;
        private Parameter[] parameters;

        public ElementType Type
        {
            get { return type; }
            set
            {
                if (value == type) return;

                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public bool IsStatic
        {
            get { return isStatic; }
            set
            {
                if (value == isStatic) return;

                isStatic = value;
                OnPropertyChanged(nameof(IsStatic));
            }
        }

        public string DataType
        {
            get { return datatype; }
            set
            {
                if (value == datatype) return;

                datatype = value;
                OnPropertyChanged(nameof(DataType));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value == name) return;

                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public AccessModifier AccessModifier
        {
            get { return accessModifier; }
            set
            {
                if (value == accessModifier) return;

                accessModifier = value;
                OnPropertyChanged(nameof(AccessModifier));
            }
        }

        public AccessModifier? GeterModifier
        {
            get { return geterModifier; }
            set
            {
                if (value == geterModifier) return;

                geterModifier = value;
                OnPropertyChanged(nameof(GeterModifier));
            }
        }

        public AccessModifier? SeterModifier
        {
            get { return seterModifier; }
            set
            {
                if (value == seterModifier) return;

                seterModifier = value;
                OnPropertyChanged(nameof(SeterModifier));
            }
        }

        public Parameter[] Parameters
        {
            get { return parameters; }
            set
            {
                if (value == parameters) return;

                parameters = value;
                OnPropertyChanged(nameof(Parameters));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            string parameters = string.Join(", ", Parameters.Select(p => p.DataType + " " + p.Name));
            string format = "{0} {1} {2} {3} ({4}) {5} {6}";
            string geter = (GeterModifier?.ToString() ?? "No") + " Geter";
            string seter = (SeterModifier?.ToString() ?? "No") + " Seter";

            return string.Format(format, Type, AccessModifier, DataType, Name, parameters, geter, seter);
        }
    }
}
