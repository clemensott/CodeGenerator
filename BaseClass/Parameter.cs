using System.ComponentModel;

namespace CodeGenerator.BaseClass
{
    public class Parameter : INotifyPropertyChanged
    {
        private string dataType, name;

        public string DataType
        {
            get => dataType;
            set
            {
                if (value == dataType) return;

                dataType = value;
                OnPropertyChanged(nameof(DataType));
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return DataType + " " + Name;
        }
    }
}