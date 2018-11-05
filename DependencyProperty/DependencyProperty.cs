using System.ComponentModel;

namespace CodeGenerator.DependencyProperty
{
    public class DependencyProperty : INotifyPropertyChanged
    {
        private string name, propertyType, controlType, defaultValue;

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

        public string ControlType
        {
            get { return controlType; }
            set
            {
                if (value == controlType) return;

                controlType = value;
                OnPropertyChanged(nameof(ControlType));
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
