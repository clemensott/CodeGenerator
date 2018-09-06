using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OnPropertyChanged("PropChange");
            }
        }

        public string Datatype
        {
            get { return datatype; }
            set
            {
                if (value == datatype) return;

                datatype = value;
                OnPropertyChanged("Datatype");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value == name) return;

                name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
