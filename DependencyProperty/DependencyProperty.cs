using System.ComponentModel;

namespace CodeGenerator.DependencyProperty
{
    public class DependencyProperty : INotifyPropertyChanged
    {
        private bool withBody, ignoreChanges;
        private string name, propertyType, defaultValue;

        public bool WithBody
        {
            get { return withBody; }
            set
            {
                if (value == withBody) return;

                withBody = value;
                OnPropertyChanged(nameof(WithBody));
            }
        }

        public bool IgnoreChanges
        {
            get { return ignoreChanges; }
            set
            {
                if (value == ignoreChanges) return;

                ignoreChanges = value;
                OnPropertyChanged(nameof(IgnoreChanges));
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

        public string PropertyType
        {
            get { return propertyType; }
            set
            {
                if (value == propertyType) return;

                propertyType = value;
                OnPropertyChanged(nameof(PropertyType));
            }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set
            {
                if (value == defaultValue) return;

                defaultValue = value;
                OnPropertyChanged(nameof(DefaultValue));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
