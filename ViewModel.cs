using CodeGenerator.DependencyProperty;
using CodeGenerator.Property;
using CodeGenerator.Replace;
using CodeGenerator.Simple;
using CodeGenerator.Singleton;
using System.ComponentModel;

namespace CodeGenerator
{
    public class ViewModel : INotifyPropertyChanged
    {
        private SimpleService simple;
        private ReplaceService replace;
        private CodePropertiesService property;
        private SingletonService singleton;
        private CodeDependencyPropertiesService dependencyProperty;

        public SimpleService Simple
        {
            get { return simple; }
            set
            {
                if (value == simple) return;

                simple = value;
                OnPropertyChanged(nameof(Simple));
            }
        }

        public ReplaceService Replace
        {
            get { return replace; }
            set
            {
                if (value == replace) return;

                replace = value;
                OnPropertyChanged(nameof(Replace));
            }
        }

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
            simple = new SimpleService();
            replace = new ReplaceService();
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
