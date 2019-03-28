using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.BaseClass
{
    public class CodeBaseClassService : MultipleCodeObjectsService<BaseClassElement>
    {
        private const string indexerFormat = "{3}{4} {5}[{6}] {0} {7} {8} {1}",
            propertyFormat = "{3}{4} {5} {0} {7} {8} {1}",
            methodFormat = "{3}{4} {5}({6}) => {7}",
            propertySeterWithINotifyPropertyChangedFormat = "\r\n" +
            "\t\t\t{4}set\r\n" +
            "\t\t\t{0}\r\n" +
            "\t\t\t\tif (value == {2}.{3}) return;\r\n" +
            "\r\n" +
            "\t\t\t\t{2}.{3} = value;\r\n" +
            "\t\t\t\tOnPropertyChanged(nameof({2}.{3}));\r\n" +
            "\t\t\t{1}\r\n\t\t";

        public const string RaisePropertyChangedCode = "\r\n" +
            "\t\tprivate void Base_PropertyChanged(object sender, PropertyChangedEventArgs e)\r\n" +
            "\t\t{\r\n" +
            "\t\t\tOnPropertyChanged(e.PropertyName);\r\n" +
            "\t\t}\r\n" +
            "\r\n" +
            "\t\tpublic event PropertyChangedEventHandler PropertyChanged;\r\n" +
            "\r\n" +
            "\t\tprivate void OnPropertyChanged(string name)\r\n" +
            "\t\t{\r\n" +
            "\t\t\tPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\r\n" +
            "\t\t}";

        private bool implementINotifyPropertyChanged, implementINotifyPropertyChangedOnBase;
        private AccessModifier baseMainModifier, baseGeterModifier, baseSeterModifier;
        private string baseClassName, parseText, codeOnly;

        public bool ImplementINotifyPropertyChanged
        {
            get { return implementINotifyPropertyChanged; }
            set
            {
                if (value == implementINotifyPropertyChanged) return;

                implementINotifyPropertyChanged = value;
                OnPropertyChanged(nameof(ImplementINotifyPropertyChanged));
            }
        }

        public bool ImplementINotifyPropertyChangedOnBase
        {
            get { return implementINotifyPropertyChangedOnBase; }
            set
            {
                if (value == implementINotifyPropertyChangedOnBase) return;

                implementINotifyPropertyChangedOnBase = value;
                OnPropertyChanged(nameof(ImplementINotifyPropertyChangedOnBase));
            }
        }

        public AccessModifier BaseMainModifier
        {
            get { return baseMainModifier; }
            set
            {
                if (value == baseMainModifier) return;

                baseMainModifier = value;
                OnPropertyChanged(nameof(BaseMainModifier));
            }
        }

        public AccessModifier BaseGeterModifier
        {
            get { return baseGeterModifier; }
            set
            {
                if (value == baseGeterModifier) return;

                baseGeterModifier = value;
                OnPropertyChanged(nameof(BaseGeterModifier));
            }
        }

        public AccessModifier BaseSeterModifier
        {
            get { return baseSeterModifier; }
            set
            {
                if (value == baseSeterModifier) return;

                baseSeterModifier = value;
                OnPropertyChanged(nameof(BaseSeterModifier));
            }
        }

        public string BaseClassName
        {
            get { return baseClassName; }
            set
            {
                if (value == baseClassName) return;

                baseClassName = value;
                OnPropertyChanged(nameof(BaseClassName));
            }
        }

        public override string ParseText
        {
            get { return parseText; }
            set
            {
                Code code = new Code(value);
                System.Diagnostics.Debug.WriteLine(code.Source);

                if (code.Source == codeOnly) return;

                try
                {
                    List<BaseClassElement> codeObjs;
                    if (TryParse(code, out codeObjs))
                    {
                        CodeObjects.Clear();

                        foreach (BaseClassElement obj in codeObjs) CodeObjects.Add(obj);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }

                parseText = value;
                codeOnly = code.Source;

                OnPropertyChanged(nameof(ParseText));
            }
        }

        public CodeBaseClassService()
        {
            BaseMainModifier = AccessModifier.Public;
        }

        protected override IEnumerable<Func<Func<BaseClassElement, string>, string>> GetCodePartGenerators()
        {
            if (implementINotifyPropertyChanged) yield return GetBaseVarCode;
            yield return GetBasePropertyCode;
            yield return LoopForeach;
        }

        protected override IEnumerable<Func<BaseClassElement, string>> GetObjectsCodeGenerators()
        {
            if (implementINotifyPropertyChanged) yield return null;
            yield return null;
            yield return GetElementCode;
        }

        private string GetBaseVarCode(Func<BaseClassElement, string> func)
        {
            return "\t\tprivate " + BaseClassName + " @base;\r\n";
        }

        private string GetBasePropertyCode(Func<BaseClassElement, string> func)
        {
            string mainModifier = GetAccessModifierCode(BaseMainModifier);
            string geterModifier = GetAccessModifierCode(BaseGeterModifier);
            string seterModifier = GetAccessModifierCode(BaseSeterModifier);
            string format;

            if (ImplementINotifyPropertyChanged && ImplementINotifyPropertyChangedOnBase)
            {
                format = "\t\t{2}{3} Base\r\n";
                format += "\t\t{0}\r\n";
                format += "\t\t\t{4}get => @base;\r\n";
                format += "\t\t\t{5}set\r\n";
                format += "\t\t\t{0}\r\n";
                format += "\t\t\t\tif(value == @base) return;\r\n";
                format += "\r\n";
                format += "\t\t\t\tif(@base != null) @base.PropertyChanged -= Base_PropertyChanged;\r\n";
                format += "\t\t\t\t@base = value;\r\n";
                format += "\t\t\t\tif(@base != null) @base.PropertyChanged += Base_PropertyChanged;\r\n";
                format += "\t\t\t{1}\r\n";
                format += "\t\t{1}\r\n";
            }
            else format = "\t\t{2}{3} Base {0} {4}get; {5}set; {1}";

            return "\r\n" + string.Format(format, '{', '}', mainModifier, BaseClassName, geterModifier, seterModifier) + "\r\n";
        }

        private string GetElementCode(BaseClassElement e)
        {
            string baseName = "Base";
            string mainModifier = GetAccessModifierCode(e.AccessModifier);
            string returnType = e.DataType;
            string name = e.Name;
            string parameterDecl = string.Join(", ", e.Parameters.Select(p => p.ToString()));
            string geterModifier = e.GeterModifier.HasValue ? GetAccessModifierCode(e.GeterModifier.Value) : null;
            string seterModifier = e.SeterModifier.HasValue ? GetAccessModifierCode(e.SeterModifier.Value) : null;
            string geter = string.Empty;
            string seter = string.Empty;
            string format;

            switch (e.Type)
            {
                case ElementType.Indexer:
                    format = indexerFormat;

                    string indexerCall = string.Join(", ", e.Parameters.Select(p => p.Name));

                    if (geterModifier != null)
                    {
                        geter = string.Format("{0}get => {1}.{2}[{3}]; ", geterModifier, baseName, name, indexerCall);
                    }

                    if (seterModifier != null)
                    {
                        seter = string.Format("{0}set => {1}.{2}[{3}] = value; ", seterModifier, baseName, name, indexerCall);
                    }
                    break;

                case ElementType.Property:
                    format = propertyFormat;

                    if (geterModifier != null) geter = string.Format("{0}get => {1}.{2}; ", geterModifier, baseName, name);
                    if (seterModifier != null)
                    {
                        seter = ImplementINotifyPropertyChanged ^ !ImplementINotifyPropertyChangedOnBase ?
                            string.Format("{0}set => {1}.{2} = value; ", seterModifier, baseName, name) :
                            string.Format(propertySeterWithINotifyPropertyChangedFormat, '{', '}', baseName, name, seterModifier);
                    }
                    break;

                case ElementType.Method:
                    format = methodFormat;

                    string methodCall = string.Join(", ", e.Parameters.Select(p => p.Name));
                    geter = string.Format("{0}.{1}({2});", baseName, name, methodCall);
                    break;

                default:
                    throw new Exception();
            }

            return "\r\n\t\t" + string.Format(format, '{', '}', baseName,
                mainModifier, returnType, name, parameterDecl, geter, seter) + "\r\n";
        }

        private string GetAccessModifierCode(AccessModifier modifier)
        {
            switch (modifier)
            {
                case AccessModifier.Default:
                    return string.Empty;

                case AccessModifier.Public:
                    return "public ";

                case AccessModifier.Internal:
                    return "internal ";

                case AccessModifier.Protected:
                    return "protected ";

                case AccessModifier.Private:
                    return "private ";

                default:
                    throw new NotImplementedException();
            }
        }

        protected override bool TryParse(string line, out BaseClassElement obj)
        {
            throw new NotImplementedException();
        }

        private bool TryParse(Code code, out List<BaseClassElement> objs)
        {
            objs = new List<BaseClassElement>();
            bool isClass;

            Code classCode = code.Clone(), interfaceCode = code.Clone();
            int classIndex = classCode.SetNextIndexOfInCode(" class ");
            int interfaceIndex = interfaceCode.SetNextIndexOfInCode(" interface ");

            if (classIndex >= 0 && classCode.Position < interfaceCode.Position)
            {
                isClass = true;
                code = classCode;
            }
            else if (interfaceIndex >= 0)
            {
                isClass = false;
                code = interfaceCode;
            }
            else return false;

            int baseClassNameStartIndex = code.Position + 1;

            if (code.SetNextIndexOfInCode(Bracket.Curly) == -1) return false;

            string classHeader = code.ToStringFromTo(baseClassNameStartIndex, code.Position - 1);
            (string className, string[] bases) = GetClassNameAndInherents(classHeader);

            BaseClassName = className;
            ImplementINotifyPropertyChanged = bases.Contains("INotifyPropertyChanged");

            code = new Code(code.GetCodeFromToEndOfLevel());

            BaseClassElement element;
            while (TryGetNextBaseClassElement(code, out element))
            {
                if (element.IsStatic || string.IsNullOrWhiteSpace(element.DataType)) continue;

                AccessModifier mod = element.AccessModifier;

                if (isClass && (mod == AccessModifier.Public || mod == AccessModifier.Internal)) objs.Add(element);
                else if (!isClass && mod == AccessModifier.Default) objs.Add(element);
            }

            return true;
        }

        private static (string className, string[] bases) GetClassNameAndInherents(string text)
        {
            int doublePointsIndex = text.IndexOf(':');
            string className = GetDataType(doublePointsIndex == -1 ? text : text.Remove(doublePointsIndex)).datatype;

            if (text.Length >= doublePointsIndex + 1) return (className, new string[0]);

            text = text.Substring(doublePointsIndex + 1);

            int whereIndex = text.IndexOf(" where ");
            string basesText = whereIndex == -1 ? text : text.Remove(whereIndex);
            string[] bases = basesText.Split(',').Select(t => t.Trim()).ToArray();

            return (className, bases);
        }

        private static bool TryGetNextBaseClassElement(Code code, out BaseClassElement element)
        {
            element = null;

            string header;
            if (!TryGetNextHeader(code, out header)) return false;

            (AccessModifier mainModifier, int mainModifierIndex) = GetAccessModifier(header);
            if (mainModifierIndex != -1) header = header.Substring(mainModifierIndex + mainModifier.ToString().Length);

            int staticIndex;
            if (TryIndexOfKeyword(header, "static", out staticIndex)) header = header.Substring(staticIndex + " static ".Length);

            (ElementType type, int parameterIndex, int parameterLength) = GetElementType(header);

            Parameter[] parameters;
            switch (type)
            {
                case ElementType.Indexer:
                    parameters = GetParameters(header.Substring(parameterIndex + 1, parameterLength - 1)).ToArray();
                    header = header.Remove(parameterIndex);
                    break;

                case ElementType.Method:
                    parameters = GetParameters(header.Substring(parameterIndex + 1, parameterLength - 1)).ToArray();
                    header = header.Remove(parameterIndex);
                    break;

                default:
                    parameters = new Parameter[0];
                    break;
            }

            (string name, string returnType) = GetNameAndDataType(header);

            bool isCurlyBody;
            string nextBody;
            if (!TryGetNextBody(code, out nextBody, out isCurlyBody)) return false;

            AccessModifier? geterModifier = null, seterModifier = null;
            if (type == ElementType.Indexer || type == ElementType.Property)
            {
                if (!isCurlyBody) geterModifier = AccessModifier.Default;
                else
                {
                    geterModifier = GetModifierOf(nextBody, "get");
                    seterModifier = GetModifierOf(nextBody, "set");
                }
            }

            element = new BaseClassElement()
            {
                AccessModifier = mainModifier,
                DataType = returnType,
                GeterModifier = geterModifier,
                IsStatic = staticIndex != -1,
                Name = name,
                Parameters = parameters,
                SeterModifier = seterModifier,
                Type = type
            };

            return true;
        }

        private static bool TryGetNextBody(Code code, out string nextBody, out bool isCurly)
        {
            isCurly = false;
            nextBody = null;

            Code curlyCode = code.Clone(), expressionCode = code.Clone();

            int curlyIndex = curlyCode.SetNextIndexOfInCodeOnLevel(Bracket.Curly, curlyCode.Brackets.Count - 1);
            int expressionIndex = expressionCode.SetNextIndexOfInCodeOnLevel(" => ");

            if (curlyIndex >= 0 && curlyCode.Position < expressionCode.Position)
            {
                if (curlyCode.SetNextIndexOfInCodeOnLevel('}', curlyCode.Brackets.Count - 1) == -1) return false;

                nextBody = curlyCode.ToStringFromTo(curlyIndex + 1, curlyCode.Position);
                code.Position = curlyCode.Position + 1;
                isCurly = true;

                return true;
            }
            else if (expressionIndex >= 0)
            {
                if (expressionCode.SetNextIndexOfInCodeOnLevel(';') == -1) return false;

                nextBody = expressionCode.ToStringFromTo(expressionIndex + 3, expressionCode.Position);
                code.Position = expressionCode.Position + 1;
                isCurly = false;

                return true;
            }

            nextBody = null;
            return false;
        }

        private static bool TryGetNextHeader(Code code, out string header)
        {
            header = null;

            Code curly = code.Clone();
            Code expression = code.Clone();
            Code set = code.Clone();
            Code semi = code.Clone();
            List<Code> codes = new List<Code>();

            if (curly.SetNextIndexOfInCodeOnLevel(Bracket.Curly) != -1) codes.Add(curly);
            if (expression.SetNextIndexOfInCodeOnLevel(" => ") != -1) codes.Add(expression);
            if (set.SetNextIndexOfInCodeOnLevel(" = ") != -1) codes.Add(set);
            if (semi.SetNextIndexOfInCodeOnLevel("; ") != -1) codes.Add(semi);

            if (codes.Count == 0) return false;

            Code first = codes.OrderBy(c => c.Position).First();

            if (first == curly)
            {
                header = curly.ToStringFromTo(code.Position, curly.Position);
                code.Position = curly.Position;
                return true;
            }
            else if (first == expression)
            {
                header = expression.ToStringFromTo(code.Position, expression.Position);
                code.Position = expression.Position;
                return true;
            }
            else if (first == set)
            {
                if (set.SetNextIndexOfInCodeOnLevel(';') == -1 || set.Position + 1 >= set.Length) return false;

                do
                {
                    if (!set.MoveNext()) return false;
                } while (set.Brackets.Count > code.Brackets.Count);

                if (!TryGetNextHeader(set, out header)) return false;

                code.Position = set.Position;
                return true;
            }
            else
            {
                if (semi.Position + 1 >= semi.Length) return false;

                do
                {
                    if (!semi.MoveNext()) return false;
                } while (semi.Brackets.Count > code.Brackets.Count);

                if (!TryGetNextHeader(semi, out header)) return false;

                code.Position = semi.Position;
                return true;
            }

            throw new NotImplementedException();
        }

        private static (AccessModifier modifier, int startIndex) GetAccessModifier(string header)
        {
            int index;

            if (TryIndexOfKeyword(header, " public ", out index)) return (AccessModifier.Public, index + 1);
            if (TryIndexOfKeyword(header, " internal ", out index)) return (AccessModifier.Internal, index + 1);
            if (TryIndexOfKeyword(header, " protected ", out index)) return (AccessModifier.Protected, index + 1);
            if (TryIndexOfKeyword(header, " private ", out index)) return (AccessModifier.Private, index + 1);

            return (AccessModifier.Default, -1);
        }

        private static bool TryIndexOfKeyword(string s, string keyword, out int index)
        {
            index = s.IndexOf(keyword);

            return index >= 0;
        }

        private static (ElementType type, int parameterOpenIndex, int parameterLength) GetElementType(string header)
        {
            if (header.Count(c => c == '(') != header.Count(c => c == ')')) throw new Exception();
            if (header.Count(c => c == '[') != header.Count(c => c == ']')) throw new Exception();
            if (header.Count(c => c == '<') != header.Count(c => c == '>')) throw new Exception();

            Code code = new Code(header);
            int lastLevel = 0;
            int methodOpenIndex = -1;
            int indexerOpenIndex = -1;

            do
            {
                if (lastLevel == 0 && code.Brackets.Count == 1)
                {
                    if (code.Peek == Bracket.Round) methodOpenIndex = code.Position;
                    else if (code.Peek == Bracket.Square) indexerOpenIndex = code.Position;
                }

                lastLevel = code.Brackets.Count;
            } while (code.MoveNext());

            int methodCloseIndex = header.LastIndexOf(')');
            int indexerCloseIndex = header.LastIndexOf(']');
            int maxCloseIndex = Math.Max(methodCloseIndex, indexerCloseIndex);

            if (maxCloseIndex == -1) return header.Any(IsVarNameBegin) ? (ElementType.Property, -1, -1) : throw new Exception();
            else if (header.Substring(maxCloseIndex).Any(IsVarNameBegin)) return (ElementType.Property, -1, -1);

            return methodCloseIndex > indexerCloseIndex ?
                (ElementType.Method, methodOpenIndex, methodCloseIndex - methodOpenIndex) :
                (ElementType.Indexer, indexerOpenIndex, indexerCloseIndex - indexerOpenIndex);
        }

        private static bool IsVarNameBegin(char c)
        {
            return char.IsLetter(c) || c == '_' || c == '@';
        }

        private static bool IsVarNameBody(char c)
        {
            return char.IsLetter(c) || char.IsDigit(c) || c == '_' || c == '[' || c == ']' || c == '?';
        }

        public static (string name, string dataType) GetNameAndDataType(string header)
        {
            (string name, int index) = GetName(header);
            string type = GetDataType(header.Remove(index)).datatype;

            return (name, type);
        }

        private static (string name, int startIndex) GetName(string header)
        {
            bool skip = false, startAgain = true;
            int startIndex = -1;
            string name = string.Empty;
            Code code = new Code(header);

            do
            {
                if (code.Current == ' ' || code.Current == '\r' || code.Current == '\n')
                {
                    skip = false;
                    startAgain = true;
                    continue;
                }
                else if (skip) continue;

                if (startAgain)
                {
                    startAgain = false;
                    startIndex = code.Position;

                    if (IsVarNameBegin(code.Current)) name = code.Current.ToString();
                    else skip = true;
                }
                else if (IsVarNameBody(code.Current)) name += code.Current;
                else skip = true;
            } while (code.MoveNext());

            return (name, startIndex);
        }

        private static (string datatype, int startIndex) GetDataType(string header)
        {
            bool skip = false, startAgain = true, inRoundBracket = false;
            int startIndex = -1, thanBracketLevel = 0;
            string type = string.Empty;
            Code code = new Code(header);

            do
            {
                if (code.Current == '<') thanBracketLevel++;

                if (inRoundBracket || thanBracketLevel > 0)
                {
                    type += code.Current;

                    if (code.Current == '>' && --thanBracketLevel < 0) throw new Exception();

                    if (code.Brackets.Count > 0 || thanBracketLevel > 0) continue;
                    if (code.Length > code.Position + 1 && (code[code.Position] == ' ' ||
                        code[code.Position] == '\r' || code[code.Position] == '\n')) throw new Exception();

                    inRoundBracket = false;
                }

                if (code.Current == ' ' || code.Current == '\r' || code.Current == '\n')
                {
                    skip = false;
                    startAgain = true;
                    continue;
                }
                else if (skip) continue;

                if (startAgain)
                {
                    startAgain = false;
                    startIndex = code.Position;

                    if (IsVarNameBegin(code.Current)) type = code.Current.ToString();
                    else skip = true;
                }
                else if (IsVarNameBody(code.Current)) type += code.Current;
                else if (code.Peek == Bracket.Round)
                {
                    inRoundBracket = true;
                    type += code.Current;
                }
                else skip = true;

            } while (code.MoveNext());

            if (type.Count(c => c == '[') != type.Count(c => c == ']')) throw new Exception();

            return (type, startIndex);
        }

        public static IEnumerable<Parameter> GetParameters(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) yield break;

            bool skip = false;
            int thanLevel = 0;
            string rawParam = string.Empty;
            Code code = new Code(text);

            do
            {
                if (code.Current == '<') thanLevel++;
                if (code.Current == '>') thanLevel--;
                if (thanLevel < 0) throw new Exception();

                if (code.Brackets.Count == 0 && thanLevel == 0 && code.Current == ',')
                {
                    yield return GetParameter(GetNameAndDataType(rawParam));

                    skip = false;
                    rawParam = string.Empty;
                }
                else if (code.Current == '=') skip = true;
                else if (!skip) rawParam += code.Current;

            } while (code.MoveNext());

            yield return GetParameter(GetNameAndDataType(rawParam));
        }

        private static Parameter GetParameter((string name, string dataType) tuple)
        {
            return new Parameter()
            {
                DataType = tuple.dataType,
                Name = tuple.name
            };
        }

        private static AccessModifier? GetModifierOf(string body, string getSet)
        {
            Code code = new Code(body);
            int index = code.SetNextIndexOfInCodeOnLevel(" " + getSet);

            if (index == -1) return null;

            if (code.Before(" public")) return AccessModifier.Public;
            if (code.Before(" internal")) return AccessModifier.Internal;
            if (code.Before(" protected")) return AccessModifier.Protected;
            if (code.Before(" private")) return AccessModifier.Private;

            return AccessModifier.Default;
        }
    }
}
