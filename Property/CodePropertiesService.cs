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

        protected override IEnumerable<Func<Property, string>> GetObjectsCodeGenerators(bool getWhole)
        {
            yield return p => throw new NotImplementedException();
            yield return new Func<Property, string>(GetPropertyCodePart2);
        }

        protected override IEnumerable<Func<Func<Property, string>, string>> GetCodePartGenerators(bool getWhole)
        {

            yield return getWhole ? (Func<Func<Property, string>, string>)GetCodePart1Whole : GetCodePart1Parts;
        }

        private string GetCodePart1Whole(Func<Property, string> func)
        {
            return GetCodePart1(func, true);
        }

        private string GetCodePart1Parts(Func<Property, string> func)
        {
            return GetCodePart1(func, false);
        }

        private string GetCodePart1(Func<Property, string> func, bool getWhole)
        {
            string code = "";

            foreach (var propertyGroup in CodeObjects.GroupBy(p => p.Datatype))
            {
                code += "\r\n";
                code += "\t\tprivate " + propertyGroup.Key;

                int i = 0;
                foreach (Property property in propertyGroup)
                {
                    string nameWithLowerStart = char.ToLower(property.Name.First()) + property.Name.Remove(0, 1);

                    code += " " + nameWithLowerStart;
                    i++;

                    if (i < propertyGroup.Count()) code += ",";
                    else code += ";";
                }
            }

            if (getWhole) code += "\r\n";

            return code;
        }

        private static string GetPropertyCodePart2(Property property)
        {
            string nameLowerStart = char.ToLower(property.Name.First()) + property.Name.Remove(0, 1);
            string nameUpperStart = char.ToUpper(property.Name.First()) + property.Name.Remove(0, 1);
            string propChange = !property.PropChange ? string.Empty :
                string.Format("\t\t\t\tOnPropertyChanged(nameof({0}));\r\n", nameUpperStart);
            string geterModifier = GetAccessModifierCode(property.GeterModifier);
            string seterModifier = GetAccessModifierCode(property.SeterModifier);

            string format = "\r\n";
            format += "\t\tpublic {2} {3}\r\n";
            format += "\t\t{0}\r\n";
            format += property.WithBody ? "\t\t\t{6}get {0} return {4}; {1}\r\n" : "\t\t\t{6}get => {4};\r\n";
            format += "\t\t\t{7}set \r\n";
            format += "\t\t\t{0}\r\n";
            format += "\t\t\t\tif (value == {4}) return;\r\n";
            format += "\r\n";
            format += "\t\t\t\t{4} = value;\r\n";
            format += "{5}";
            format += "\t\t\t{1}\r\n";
            format += "\t\t{1}\r\n";

            return string.Format(format, "{", "}", property.Datatype, nameUpperStart,
                nameLowerStart, propChange, geterModifier, seterModifier);
        }

        private static string GetAccessModifierCode(AccessModifier modifier)
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

                if (data.Length < 2 || data.Length > 6) return false;

                bool propChange = true, withBody = false;
                string datatype = data[0], name = data[1];
                AccessModifier geterModifier = AccessModifier.Default, seterModifier = AccessModifier.Default;

                if (data.Length > 2 && !TryConvertToBoolean(data[2], ref propChange)) return false;
                if (data.Length > 3 && !TryConvertToBoolean(data[3], ref withBody)) return false;
                if (data.Length > 4 && !TryConvertToAccessModifier(data[4], ref geterModifier)) return false;
                if (data.Length > 5 && !TryConvertToAccessModifier(data[5], ref seterModifier)) return false;

                property = new Property()
                {
                    Datatype = datatype,
                    Name = name,
                    PropChange = propChange,
                    WithBody = withBody,
                    GeterModifier = geterModifier,
                    SeterModifier = seterModifier
                };
                return true;
            }
            catch { }

            return false;
        }

        private static bool TryConvertToBoolean(string s, ref bool value)
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

        private static bool TryConvertToAccessModifier(string s, ref AccessModifier value)
        {
            if ("default".StartsWith(s.ToLower())) value = AccessModifier.Default;
            else if ("public".StartsWith(s.ToLower())) value = AccessModifier.Public;
            else if ("internal".StartsWith(s.ToLower())) value = AccessModifier.Internal;
            else if ("protected".StartsWith(s.ToLower())) value = AccessModifier.Protected;
            else if ("private".StartsWith(s.ToLower())) value = AccessModifier.Private;
            else if (s.Length != 0) return false;

            return true;
        }
    }
}
