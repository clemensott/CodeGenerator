using System.ComponentModel;
using System.Windows.Input;

namespace CodeGenerator
{
    public abstract class MultipleCodeObjectsService<T> : CodeObjectsService<T> where T : INotifyPropertyChanged
    {
        private string parseText;

        public string ParseText
        {
            get { return parseText; }
            set
            {
                if (value == parseText) return;

                T newCodeObject;
                if (value.Contains("\n") && TryParse(value.Replace("\n", "").Replace("\r", ""), out newCodeObject))
                {
                    CodeObjects.Add(newCodeObject);

                    if (!Keyboard.IsKeyDown(Key.LeftShift)) parseText = string.Empty;
                }
                else parseText = value;

                OnPropertyChanged("ParseText");
            }
        }

        protected abstract bool TryParse(string line, out T obj);
    }
}
