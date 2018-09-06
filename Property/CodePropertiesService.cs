using StdOttWpfLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Property
{
    public class CodePropertiesService : MultipleCodeObjectsService<Property>
    {
        private const string notifyPropertyChangeText = ": INotifyPropertyChanged\r\n{\r\n\r\n" +
            "public event PropertyChangedEventHandler PropertyChanged;\r\n\r\n" +
            "private void OnPropertyChanged(string name)\r\n" +
            "{\r\nPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\r\n}";

        public string NotifyPropertyChangeText { get { return notifyPropertyChangeText; } }

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
            throw Utils.GetNotImplementedExeption(this, nameof(GetCodePart1));
        }

        private string GetPropertyCodePart2(Property property)
        {
            string nameWithLowerStart = char.ToLower(property.Name.First()) + property.Name.Remove(0, 1);
            string nameWithUpperStart = char.ToUpper(property.Name.First()) + property.Name.Remove(0, 1);
            string propChange = !property.PropChange ? string.Empty :
                string.Format("\r\nOnPropertyChanged(nameof({0}));", nameWithUpperStart);

            string format = "\r\npublic {2} {3}\r\n{0}\r\nget {0} return {4}; {1}\r\n" +
                "set \r\n{0}\r\nif(value == {4})return;\r\n\r\n{4} = value;{5}\r\n{1}\r\n{1}\r\n";

            return string.Format(format, "{", "}", property.Datatype, nameWithUpperStart, nameWithLowerStart, propChange);
        }

        protected override bool TryParse(string line, out Property property)
        {
            property = null;

            try
            {
                string[] data = line.Split(' ');

                bool propChange = false;
                string datatype = data[0], name = data[1];

                if (datatype.Length == 0 || name.Length == 0) return false;
                if (data.Length > 2) propChange = ConvertToBoolean(data[2]);


                property = new Property() { Datatype = datatype, Name = name, PropChange = propChange };
                return true;
            }
            catch { }

            return false;
        }

        private bool ConvertToBoolean(string s)
        {
            if (s == "0") return false;
            else if (s == "1") return true;
            else if (s.ToLower() == "false") return false;
            else if (s.ToLower() == "true") return true;

            return false;
        }
    }
}
