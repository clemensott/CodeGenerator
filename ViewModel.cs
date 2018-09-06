using CodeGenerator.DependencyProperty;
using CodeGenerator.Property;
using CodeGenerator.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OnPropertyChanged("Property");
            }
        }

        public SingletonService Singleton
        {
            get { return singleton; }
            set
            {
                if (value == singleton) return;

                singleton = value;
                OnPropertyChanged("Singleton");
            }
        }

        public CodeDependencyPropertiesService DependencyProperty
        {
            get { return dependencyProperty; }
            set
            {
                if (value == dependencyProperty) return;

                dependencyProperty = value;
                OnPropertyChanged("DependencyProperty");
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
