using System.ComponentModel;

namespace CodeGenerator.Property
{
    public enum AccessModifier { Default, Public, Internal, Protected, Private }

    public class Property : INotifyPropertyChanged
    {
        private bool propChange, withBody;
        private string datatype, name;
        private AccessModifier geterModifier, seterModifier;

        public bool PropChange
        {
            get => propChange;
            set
            {
                if (value == propChange) return;

                propChange = value;
                OnPropertyChanged(nameof(PropChange));
            }
        }

        public bool WithBody
        {
            get => withBody;
            set
            {
                if (value == withBody) return;

                withBody = value;
                OnPropertyChanged(nameof(WithBody));
            }
        }

        public string Datatype
        {
            get => datatype;
            set
            {
                if (value == datatype) return;

                datatype = value;
                OnPropertyChanged(nameof(Datatype));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;

                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public AccessModifier GeterModifier
        {
            get => geterModifier;
            set
            {
                if (value == geterModifier) return;

                geterModifier = value;
                OnPropertyChanged(nameof(GeterModifier));
            }
        }

        public AccessModifier SeterModifier
        {
            get => seterModifier;
            set
            {
                if (value == seterModifier) return;

                seterModifier = value;
                OnPropertyChanged(nameof(SeterModifier));
            }
        }

        //public IEnumerable<AccessModifier> AccessModifiers => Enum.GetValues(typeof(AccessModifier)).Cast<AccessModifier>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
