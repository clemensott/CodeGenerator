using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Simple
{
    public class Simple : INotifyPropertyChanged
    {
        private string completeCommend;

        public string CompleteCommend
        {
            get { return completeCommend; }
            set
            {
                if (value == completeCommend) return;

                completeCommend = value;
                OnPropertyChanged(nameof(CompleteCommend));
            }
        }

        public string GetCommend()
        {
            return CompleteCommend.Split(' ')[0];
        }

        public string[] GetArgs()
        {
            return CompleteCommend.Split(' ').Skip(1).ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
