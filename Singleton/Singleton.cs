using System.ComponentModel;

namespace CodeGenerator.Singleton
{
    public class Singleton : INotifyPropertyChanged
    {
        private string className;

        public string ClassName
        {
            get { return className; }
            set
            {
                if (value == className) return;

                className = value;
                OnPropertyChanged(nameof(ClassName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
