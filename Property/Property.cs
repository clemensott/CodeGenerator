using System.ComponentModel;

namespace CodeGenerator.Property
{
    public class Property : INotifyPropertyChanged
    {
        private bool propChange;
        private string datatype, name;

        public bool PropChange
        {
            get { return propChange; }
            set
            {
                if (value == propChange) return;

                propChange = value;
                OnPropertyChanged(nameof(PropChange));
            }
        }

        public string Datatype
        {
            get { return datatype; }
            set
            {
                if (value == datatype) return;

                datatype = value;
                OnPropertyChanged(nameof(Datatype));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
