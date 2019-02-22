using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.BaseClass
{
    static class CodeExtention
    {
        public static int SetNextIndexOf(this Code code, char c)
        {
            return code.Position = code.String.IndexOf(c);
        }

        public static int SetNextIndexOf(this Code code, string text)
        {
            return code.Position = code.String.IndexOf(text);
        }

        public static int NextIndexOfInCode(this Code code, char c)
        {
            return SetNextIndexOfInCode(code.Clone(), c);
        }

        public static int SetNextIndexOfInCode(this Code code, char c)
        {
            do
            {
                if (code.IsInCode && code.Current == c) return code.Position;
            }
            while (code.MoveNext());

            return -1;
        }

        public static int NextIndexOfInCode(this Code code, string text)
        {
            return SetNextIndexOfInCode(code.Clone(), text);
        }

        public static int SetNextIndexOfInCode(this Code code, string value)
        {
            do
            {
                if (code.IsInCode && code.ContinuesWith(value)) return code.Position;
            }
            while (code.MoveNext());

            return -1;
        }

        public static int NextIndexOfInCode(this Code code, Bracket bracket)
        {
            return SetNextIndexOfInCode(code.Clone(), bracket);
        }

        public static int SetNextIndexOfInCode(this Code code, Bracket bracket)
        {
            int level = code.Brackets.Count;

            while (code.MoveNext())
            {
                if (level < code.Brackets.Count && code.Peek == bracket) return code.Position;

                level = code.Brackets.Count;
            }

            return -1;
        }

        public static int SetNextIndexOfInCodeOnLevel(this Code code, char c)
        {
            return SetNextIndexOfInCodeOnLevel(code, c, code.Brackets.Count);
        }

        public static int SetNextIndexOfInCodeOnLevel(this Code code, char c, int level)
        {
            do
            {
                if (code.Brackets.Count == level && code.IsInCode && code.Current == c) return code.Position;
            }
            while (code.MoveNext());

            return -1;
        }

        public static int SetNextIndexOfInCodeOnLevel(this Code code, string value)
        {
            return SetNextIndexOfInCodeOnLevel(code, value, code.Brackets.Count);
        }

        public static int SetNextIndexOfInCodeOnLevel(this Code code, string value, int level)
        {
            do
            {
                if (code.Brackets.Count == level && code.IsInCode && code.ContinuesWith(value)) return code.Position;
            }
            while (code.MoveNext());

            return -1;
        }

        public static int SetNextIndexOfInCodeOnLevel(this Code code, Bracket bracket)
        {
            return SetNextIndexOfInCodeOnLevel(code, bracket, code.Brackets.Count);
        }

        public static int SetNextIndexOfInCodeOnLevel(this Code code, Bracket bracket, int level)
        {
            do
            {
                if (code.Brackets.Count == level + 1 && code.Peek == bracket) return code.Position;
            }
            while (code.MoveNext());

            return -1;
        }

        public static string GetCodeFromToEndOfLevel(this Code code)
        {
            code = code.Clone();
            int startPos = code.Position + 1, startLevel = code.Brackets.Count;

            while (code.MoveNext() && code.Brackets.Count >= startLevel) ;

            return code.ToStringFromTo(startPos, code.Position);
        }

        public static bool Before(this Code code, string text)
        {
            if (code.Position + 1 < text.Length) return false;

            code = code.Clone();
            code.Position -= text.Length;

            return code.ContinuesWith(text);
        }
    }
}
