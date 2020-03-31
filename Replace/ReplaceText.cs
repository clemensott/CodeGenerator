using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Replace
{
    internal class ReplaceText
    {
        private readonly List<(bool replaced, string text)> parts;

        public ReplaceText(string text)
        {
            parts = new List<(bool replaced, string text)>();
            parts.Add((false, text));
        }

        public void Replace(Replace replace, int replaceTextIndex)
        {
            int index;
            string replaceText;

            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].replaced) continue;

                string src = parts[i].text;
                if (!replace.TryGetReplaceIndex(src, replaceTextIndex, out index, out replaceText)) continue;

                string start = src.Remove(index);
                string end = src.Substring(index + replace.SearchText.Length);

                parts[i] = (false, start);
                parts.Insert(i + 1, (true, replaceText));
                parts.Insert(i + 2, (false, end));
            }
        }

        public override string ToString()
        {
            return string.Concat(parts.Select(p => p.text));
        }
    }
}
