using System.ComponentModel;

namespace CodeGenerator.Snippets
{
    public class Snippet : INotifyPropertyChanged
    {
        public string WholeCode { get; }

        public string CodePart { get; }

        public Snippet(string codePart)
        {
            WholeCode = CodePart = codePart;
        }

        public Snippet(string wholeCode, string codePart)
        {
            WholeCode = wholeCode;
            CodePart = codePart;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
