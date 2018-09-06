using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace CodeGenerator
{
    public abstract class SingleCodeObjectService<T> : CodeObjectsService<T> where T : INotifyPropertyChanged
    {
        private string parseText;

        public T CodeObject
        {
            get { return CodeObjects.FirstOrDefault(); }
            set
            {
                if (ReferenceEquals(value, CodeObject)) return;

                if (CodeObjects.Count == 0) CodeObjects.Add(value);
                else CodeObjects[0] = value;

                OnPropertyChanged("CodeObject");
            }
        }

        public string ParseText
        {
            get { return parseText; }
            set
            {
                if (value == parseText) return;

                T newCodeObject;
                if (value.Contains("\n") && TryParse(value.Replace("\n", "").Replace("\r", ""), out newCodeObject))
                {
                    CodeObject = newCodeObject;

                    if (!Keyboard.IsKeyDown(Key.RightCtrl)) parseText = string.Empty;
                }
                else parseText = value;

                OnPropertyChanged("ParseText");
            }
        }

        public abstract bool TryParse(string line, out T obj);
    }
}
