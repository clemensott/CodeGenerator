using CodeGenerator.BaseClass;
using CodeGenerator.DependencyProperty;
using CodeGenerator.Property;
using CodeGenerator.Replace;
using CodeGenerator.Simple;
using CodeGenerator.Singleton;
using CodeGenerator.Snippets;
using System;
using System.ComponentModel;
using System.Windows;

namespace CodeGenerator
{
    public class ViewModel : INotifyPropertyChanged
    {
        private SimpleService simple;
        private ReplaceService replace;
        private CodePropertiesService property;
        private SingletonService singleton;
        private CodeDependencyPropertiesService dependencyProperty;
        private CodeBaseClassService baseClass;
        private SnippetServices snippets;

        public SimpleService Simple
        {
            get => simple;
            set
            {
                if (value == simple) return;

                simple = value;
                OnPropertyChanged(nameof(Simple));
            }
        }

        public ReplaceService Replace
        {
            get => replace;
            set
            {
                if (value == replace) return;

                replace = value;
                OnPropertyChanged(nameof(Replace));
            }
        }

        public CodePropertiesService Property
        {
            get => property;
            set
            {
                if (value == property) return;

                property = value;
                OnPropertyChanged(nameof(Property));
            }
        }

        public SingletonService Singleton
        {
            get => singleton;
            set
            {
                if (value == singleton) return;

                singleton = value;
                OnPropertyChanged(nameof(Singleton));
            }
        }

        public CodeDependencyPropertiesService DependencyProperty
        {
            get => dependencyProperty;
            set
            {
                if (value == dependencyProperty) return;

                dependencyProperty = value;
                OnPropertyChanged(nameof(DependencyProperty));
            }
        }

        public CodeBaseClassService BaseClass
        {
            get => baseClass;
            set
            {
                if (value == baseClass) return;

                baseClass = value;
                OnPropertyChanged(nameof(BaseClass));
            }
        }

        public SnippetServices Snippets
        {
            get => snippets;
            set
            {
                if (value == snippets) return;

                snippets = value;
                OnPropertyChanged(nameof(Snippets));
            }
        }

        public ViewModel()
        {
            simple = new SimpleService();
            replace = new ReplaceService();
            property = new CodePropertiesService();
            singleton = new SingletonService();
            dependencyProperty = new CodeDependencyPropertiesService();
            baseClass = new CodeBaseClassService();

            try
            {
                snippets = new SnippetServices();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception on new SnippetServices()");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
