using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Property
{
    public class CodePropertiesService : MultipleCodeObjectsService<Property>
    {
        public const string NotifyPropertyChangeText = ": INotifyPropertyChanged\r\n{\r\n\r\n" +
            "\t\tpublic event PropertyChangedEventHandler PropertyChanged;\r\n" +
            "\r\n" +
            "\t\tprivate void OnPropertyChanged(string name)\r\n" +
            "\t\t{\r\n" +
            "\t\t\tPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\r\n" +
            "\t\t}";

        public IEnumerable<AccessModifier> AccessModifiers => Enum.GetValues(typeof(AccessModifier)).Cast<AccessModifier>();

        public CodePropertiesService()
        {
        }

        protected override IEnumerable<Func<Property, string>> GetObjectsCodeGenerators()
        {
            yield return new Func<Property, string>(GetPropertyCodePart1);
            yield return new Func<Property, string>(GetPropertyCodePart2);
        }

        protected override IEnumerable<Func<Func<Property, string>, string>> GetCodePartGenerators()
        {
            yield return new Func<Func<Property, string>, string>(GetCodePart1);
        }

        private string GetCodePart1(Func<Property, string> func)
        {
            string code = "\r\n";

            foreach (var propertyGroup in CodeObjects.GroupBy(p => p.Datatype))
            {
                code += "   private " + propertyGroup.Key;

                int i = 0;
                foreach (Property property in propertyGroup)
                {
                    string nameWithLowerStart = char.ToLower(property.Name.First()) + property.Name.Remove(0, 1);

                    code += " " + nameWithLowerStart;
                    i++;

                    if (i < propertyGroup.Count()) code += ",";
                    else code += ";\r\n";
                }
            }

            return code;
        }

        private string GetPropertyCodePart1(Property property)
        {
            //string nameWithLowerStart = char.ToLower(property.Name.First()) + property.Name.Remove(0, 1);

            //return string.Format("\r\nprivate {0} {1};", property.Datatype, nameWithLowerStart);
            throw new NotImplementedException();
        }

        private string GetPropertyCodePart2(Property property)
        {
            string nameLowerStart = char.ToLower(property.Name.First()) + property.Name.Remove(0, 1);
            string nameUpperStart = char.ToUpper(property.Name.First()) + property.Name.Remove(0, 1);
            string propChange = !property.PropChange ? string.Empty :
                string.Format("\r\nOnPropertyChanged(nameof({0}));", nameUpperStart);
            string geterModifier = GetAccessModifierCode(property.GeterModifier);
            string seterModifier = GetAccessModifierCode(property.GeterModifier);

            string format = "\r\npublic {2} {3}\r\n{0}\r\n{6}get {0} return {4}; {1}\r\n" +
                "{7}set \r\n{0}\r\nif(value == {4})return;\r\n\r\n{4} = value;{5}\r\n{1}\r\n{1}\r\n";

            return string.Format(format, "{", "}", property.Datatype, nameUpperStart,
                nameLowerStart, propChange, geterModifier, seterModifier);
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

        protected override bool TryParse(string line, out Property property)
        {
            property = null;

            try
            {
                string[] data = line.Split(' ');

                if (data.Length < 2 || data.Length > 5) return false;

                bool propChange = false;
                string datatype = data[0], name = data[1];
                AccessModifier geterModifier = AccessModifier.Default, seterModifier = AccessModifier.Default;

                if (data.Length > 2 && !TryConvertToBoolean(data[2], ref propChange)) return false;
                if (data.Length > 3 && !TryConvertToAccessModifier(data[3], ref geterModifier)) return false;
                if (data.Length > 4 && !TryConvertToAccessModifier(data[4], ref seterModifier)) return false;

                property = new Property()
                {
                    Datatype = datatype,
                    Name = name,
                    PropChange = propChange,
                    GeterModifier = geterModifier,
                    SeterModifier = seterModifier
                };
                return true;
            }
            catch { }

            return false;
        }

        private bool TryConvertToBoolean(string s, ref bool value)
        {
            switch (s.ToLower())
            {
                case "":
                    break;

                case "0":
                case "false":
                    value = false;
                    break;

                case "1":
                case "true":
                    value = true;
                    break;

                default:
                    return false;
            }

            return true;
        }

        private bool TryConvertToAccessModifier(string s, ref AccessModifier value)
        {
            switch (s.ToLower())
            {
                case "":
                    break;

                case "default":
                    value = AccessModifier.Default;
                    break;

                case "public":
                    value = AccessModifier.Public;
                    break;

                case "internal":
                    value = AccessModifier.Public;
                    break;

                case "protected":
                    value = AccessModifier.Public;
                    break;

                case "private":
                    value = AccessModifier.Public;
                    break;

                default:
                    return false;
            }

            return true;
        }
    }
}
