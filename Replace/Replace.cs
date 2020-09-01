using StdOttStandard;
using StdOttStandard.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CodeGenerator.Replace
{
    public enum Case { Ignore, Sensitive, KeepFirstLetter }

    public class Replace : INotifyPropertyChanged
    {

        private Case caseSensitive;
        private string searchText;
        private string[] replaceTexts;

        public Case CaseSensitive
        {
            get => caseSensitive;
            set
            {
                if (value == caseSensitive) return;

                caseSensitive = value;
                OnPropertyChanged(nameof(CaseSensitive));
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                if (value == searchText) return;

                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string ReplaceText
        {
            get => Serializer.Serialize(ReplaceTexts.ToNotNull(), '#');
            set => ReplaceTexts = Serializer.Deserialize(value, '#').ToArray();
        }

        public string[] ReplaceTexts
        {
            get => replaceTexts;
            set
            {
                if (value == replaceTexts) return;

                replaceTexts = value;
                OnPropertyChanged(nameof(ReplaceTexts));
                OnPropertyChanged(nameof(ReplaceText));
            }
        }

        public Replace()
        {
        }

        public Replace(string searchText, Case caseSensitive, IEnumerable<string> replaceTexts)
        {
            SearchText = searchText;
            CaseSensitive = caseSensitive;
            ReplaceTexts = replaceTexts.ToArray();
        }

        public bool TryGetReplaceIndex(string text, int replaceIndex, out int index, out string replaceText)
        {
            string searchText = SearchText;
            replaceText = ReplaceTexts.ElementAtOrDefault(replaceIndex) ?? "{null}";

            switch (CaseSensitive)
            {
                case Case.Ignore:
                    index = text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
                    break;

                case Case.Sensitive:
                    index = text.IndexOf(searchText, StringComparison.Ordinal);
                    break;

                case Case.KeepFirstLetter:
                    int lowerIndex = text.IndexOf(ToLowerAt(searchText, 0), StringComparison.Ordinal);
                    int upperIndex = text.IndexOf(ToUpperAt(searchText, 0), StringComparison.Ordinal);

                    if (lowerIndex == -1 && upperIndex == -1) index = -1;
                    else if ((lowerIndex != -1 && lowerIndex < upperIndex) || upperIndex == -1)
                    {
                        index = lowerIndex;
                        replaceText = ToLowerAt(replaceText, 0);
                    }
                    else
                    {
                        index = upperIndex;
                        replaceText = ToUpperAt(replaceText, 0);
                    }
                    break;

                default:
                    throw new ArgumentException(nameof(CaseSensitive));
            }

            return index != -1;
        }

        private static string ToLowerAt(string text, int index)
        {
            if (char.IsLower(text[index])) return text;

            char c = char.ToLower(text[index]);
            string output = text.Remove(index) + c;

            if (index + 1 < text.Length) output += text.Substring(index + 1);

            return output;
        }

        private static string ToUpperAt(string text, int index)
        {
            if (char.IsUpper(text[index])) return text;

            char c = char.ToLower(text[index]);
            string output = text.Remove(index) + c;

            if (index + 1 < text.Length) output += text.Substring(index + 1);

            return output;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
