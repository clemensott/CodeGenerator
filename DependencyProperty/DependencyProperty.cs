using System.ComponentModel;

namespace CodeGenerator.DependencyProperty
{
    public class DependencyProperty : INotifyPropertyChanged
    {
        private bool withBody, withPropertyChanged, isReadonly, withValidation, withNewValue, withOldValue;
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

        public bool WithPropertyChanged
        {
            get { return withPropertyChanged; }
            set
            {
                if (value == withPropertyChanged) return;

                withPropertyChanged = value;
                OnPropertyChanged(nameof(WithPropertyChanged));
            }
        }
        public bool IsReadonly
        {
            get => isReadonly;
            set
            {
                if (value == isReadonly) return;

                isReadonly = value;
                OnPropertyChanged(nameof(IsReadonly));
            }
        }

        public bool WithValidation
        {
            get => withValidation;
            set
            {
                if (value == withValidation) return;

                withValidation = value;
                OnPropertyChanged(nameof(WithValidation));
            }
        }
        public bool WithNewValue
        {
            get => withNewValue;
            set
            {
                if (value == withNewValue) return;

                withNewValue = value;
                OnPropertyChanged(nameof(WithNewValue));
            }
        }

        public bool WithOldValue
        {
            get => withOldValue;
            set
            {
                if (value == withOldValue) return;

                withOldValue = value;
                OnPropertyChanged(nameof(WithOldValue));
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
