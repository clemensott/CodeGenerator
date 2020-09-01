using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.BaseClass
{
    internal enum Bracket { Round, Curly, Square, Than, Quote, NoEscapingQuote, Apostrophe, SingleLineComment, MultiLineComment }

    internal class Code : IEnumerable<char>
    {
        private int position;

        public char this[int index] => Source[index];

        public string Source { get; private set; }

        public int Length => Source.Length;

        public int Position
        {
            get => position;
            set
            {
                if (position > value)
                {
                    position = -1;
                    Brackets.Clear();
                }

                while (position < value && MoveNext()) ;
            }
        }

        public char Current => this[Position];

        public Stack<Bracket> Brackets { get; }

        public Bracket? Peek => Brackets.Count > 0 ? (Bracket?)Brackets.Peek() : null;

        public bool IsInCode => IsCodeBracket(Peek);

        public string String => ToStringFrom(Position);

        private Code(string code, int pos, Stack<Bracket> brackets)
        {
            Source = code;
            position = pos;
            Brackets = brackets;
        }

        public Code(string code)
        {
            Source = code;
            Brackets = new Stack<Bracket>();

            position = -1;

            RemoveNonActualCodeAndLineFeedsAndWhiteSpaces();
        }

        private void RemoveNonActualCodeAndLineFeedsAndWhiteSpaces()
        {
            string code = string.Empty;
            bool wasInCode = IsInCode;
            int index = -1;

            while (MoveNext())
            {
                if (wasInCode || IsInCode)
                {
                    do
                    {
                        index++;

                        code += this[index];
                    }
                    while (index < Position);
                }
                else index = Position;

                wasInCode = IsInCode;
            }

            code = code.Replace("//", "")
                .Replace("/*", "")
                .Replace("*/", "")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("=>", " => ")
                .Replace(";", "; ")
                .Replace("@\"\"", "\"\"")
                .Replace("{", " { ")
                .Replace("}", " } ");

            code = ' ' + code + ' ';

            ReplaceAll(ref code, " ;", ";");
            ReplaceAll(ref code, "  ", " ");

            Source = code;
            Position = 0;
        }

        private void ReplaceAll(ref string text, string oldValue, string newValue)
        {
            while (text.Contains(oldValue)) text = text.Replace(oldValue, newValue);
        }

        public bool MoveNext()
        {
            if (Position + 1 >= Length) return false;

            position++;

            switch (this[position])
            {
                case '(':
                    if (IsInCode) Brackets.Push(Bracket.Round);
                    break;

                case ')':
                    if (IsInCode && Brackets.Pop() != Bracket.Round) throw new Exception();
                    break;

                case '{':
                    if (IsInCode) Brackets.Push(Bracket.Curly);
                    break;

                case '}':
                    if (IsInCode && Brackets.Pop() != Bracket.Curly) throw new Exception();
                    break;

                case '[':
                    if (IsInCode) Brackets.Push(Bracket.Square);
                    break;

                case ']':
                    if (IsInCode && Brackets.Pop() != Bracket.Square) throw new Exception();
                    break;

                case '\"':
                    if (Peek == Bracket.NoEscapingQuote || Peek == Bracket.Quote) Brackets.Pop();
                    else Brackets.Push(Bracket.Quote);
                    break;

                case '\'':
                    if (!IsInCode) break;

                    if (Peek == Bracket.Apostrophe) Brackets.Pop();
                    else Brackets.Push(Bracket.Apostrophe);
                    break;

                case '@':
                    if (!IsInCode) break;
                    if (position + 1 < Length && this[position + 1] == '\"')
                    {
                        position++;
                        Brackets.Push(Bracket.NoEscapingQuote);
                    }
                    break;

                case '\\':
                    if (Peek == Bracket.Apostrophe || Peek == Bracket.Quote) position++;
                    break;

                case '/':
                    if (!IsInCode || position + 1 >= Length) break;

                    if (this[position + 1] == '/')
                    {
                        position++;
                        Brackets.Push(Bracket.SingleLineComment);
                    }
                    else if (this[position + 1] == '*')
                    {
                        position++;
                        Brackets.Push(Bracket.MultiLineComment);
                    }
                    break;

                case '\n':
                    if (Peek == Bracket.SingleLineComment) Brackets.Pop();
                    break;

                case '*':
                    if (Peek == Bracket.MultiLineComment && position + 1 < Length && this[position + 1] == '/')
                    {
                        position++;
                        Brackets.Pop();
                    }
                    break;
            }

            return true;
        }

        public bool ContinuesWith(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (Position + 1 >= Length) return false;
                if (text[i] != this[Position + i]) return false;
            }

            return true;
        }

        public int NextIndexOf(char c)
        {
            return String.IndexOf(c);
        }

        public int NextIndexOf(string text)
        {
            return String.IndexOf(text, StringComparison.Ordinal);
        }

        public bool ContainsBetween(char c, int end)
        {
            return ContainsBetween(c, Position, end);
        }

        public bool ContainsBetween(char c, int start, int end)
        {
            return this.Skip(start + 1).Take(end - start - 1).Contains(c);
        }

        public bool ContainsBetween(string text, int end)
        {
            return ContainsBetween(text, Position, end);
        }

        public bool ContainsBetween(string text, int start, int end)
        {
            return String.Contains(text);
        }

        public string ToStringFrom(int start)
        {
            return ToStringFromTo(start, Length);
        }

        public string ToStringTo(int end)
        {
            return ToStringFromTo(Position, end);
        }

        public string ToStringFromTo(int start, int end)
        {
            return string.Concat(this.Skip(start).Take(end - start));
        }

        private static bool IsCodeBracket(Bracket? bracket)
        {
            if (!bracket.HasValue) return true;

            Bracket b = bracket.Value;
            return b == Bracket.Curly || b == Bracket.Round || b == Bracket.Square || b == Bracket.Than;
        }

        public Code Clone()
        {
            return new Code(Source, Position, new Stack<Bracket>(Brackets));
        }

        public IEnumerator<char> GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
