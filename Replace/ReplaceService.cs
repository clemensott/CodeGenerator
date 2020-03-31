using StdOttFramework.Converters;
using StdOttStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using CodeGenerator.Base;

namespace CodeGenerator.Replace
{
    public class ReplaceService : MultipleCodeObjectsService<Replace>
    {
        private string srcText;

        public string SrcText
        {
            get => srcText;
            set
            {
                if (value == srcText) return;

                srcText = value;
                OnPropertyChanged(nameof(SrcText));
            }
        }

        protected override bool TryParse(string line, out Replace obj)
        {
            obj = null;

            string[] parts = Serializer.Deserialize(line, '#').ToArray();

            if (parts.Length < 3) return false;

            Case caseSensitive;
            string searchText = parts[1];

            if (!EnumToStringConverter.TryParse(parts[0], out caseSensitive)) return false;

            obj = new Replace(searchText, caseSensitive, parts.Skip(2));
            return true;
        }

        protected override IEnumerable<Func<Replace, string>> GetObjectsCodeGenerators(bool getWhole)
        {
            yield break;
        }

        protected override IEnumerable<Func<Func<Replace, string>, string>> GetCodePartGenerators(bool getWhole)
        {
            int count = CodeObjects.Min(o => o.ReplaceTexts.Length);

            for (int i = 0; i < count; i++)
            {
                int index2 = i;
                yield return (f) => GetReplacedText(index2);
            }
        }

        private string GetReplacedText(int index)
        {
            ReplaceText text = new ReplaceText(SrcText);

            foreach (Replace replace in CodeObjects)
            {
                text.Replace(replace, index);
            }

            return text.ToString();
        }
    }
}