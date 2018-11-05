using CodeGenerator.DependencyProperty;
using CodeGenerator.Property;
using CodeGenerator.Singleton;
using System.ComponentModel;

namespace CodeGenerator
{
    public class ViewModel : INotifyPropertyChanged
    {
        private CodePropertiesService property;
        private SingletonService singleton;
        private CodeDependencyPropertiesService dependencyProperty;

        public CodePropertiesService Property
        {
            get { return property; }
            set
            {
                if (value == property) return;

                property = value;
                OnPropertyChanged(nameof(Property));
            }
        }

        public SingletonService Singleton
        {
            get { return singleton; }
            set
            {
                if (value == singleton) return;

                singleton = value;
                OnPropertyChanged(nameof(Singleton));
            }
        }

        public CodeDependencyPropertiesService DependencyProperty
        {
            get { return dependencyProperty; }
            set
            {
                if (value == dependencyProperty) return;

                dependencyProperty = value;
                OnPropertyChanged(nameof(DependencyProperty));
            }
        }

        public ViewModel()
        {
            property = new CodePropertiesService();
            singleton = new SingletonService();
            dependencyProperty = new CodeDependencyPropertiesService();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
