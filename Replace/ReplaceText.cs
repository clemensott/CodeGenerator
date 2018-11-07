using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Replace
{
    class ReplaceText
    {
        private List<Tuple<bool, string>> parts;

        public ReplaceText(string text)
        {
            parts = new List<Tuple<bool, string>>();
            parts.Add(new Tuple<bool, string>(false, text));
        }

        public void Replace(Replace replace, int replaceTextIndex)
        {
            int index;
            string replaceText;

            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].Item1) continue;

                string src = parts[i].Item2;
                if (!replace.TryGetReplaceIndex(src, replaceTextIndex, out index, out replaceText)) continue;

                string start = src.Remove(index);
                string end = src.Substring(index + replace.SearchText.Length);

                parts[i] = new Tuple<bool, string>(false, start);
                parts.Insert(i + 1, new Tuple<bool, string>(true, replaceText));
                parts.Insert(i + 2, new Tuple<bool, string>(false, end));
            }
        }

        public override string ToString()
        {
            return string.Concat(parts.Select(p => p.Item2));
        }
    }
}
