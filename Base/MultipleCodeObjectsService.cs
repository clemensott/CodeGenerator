using System.ComponentModel;
using System.Windows.Input;

namespace CodeGenerator
{
    public abstract class MultipleCodeObjectsService<T> : CodeObjectsService<T> where T : INotifyPropertyChanged
    {
        private string parseText;

        public virtual string ParseText
        {
            get { return parseText; }
            set
            {
                if (value == parseText) return;

                bool tryToParse = value.Contains("\n");
                value = value.Replace("\n", "").Replace("\r", "");

                T newCodeObject;
                if (tryToParse && TryParse(value, out newCodeObject))
                {
                    CodeObjects.Add(newCodeObject);

                    if (!Keyboard.IsKeyDown(Key.LeftShift)) parseText = string.Empty;
                }
                else parseText = value;

                OnPropertyChanged(nameof(ParseText));
            }
        }

        protected abstract bool TryParse(string line, out T obj);
    }
}
