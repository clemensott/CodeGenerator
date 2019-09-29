using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Simple
{
    public class SimpleService : SingleCodeObjectService<Simple>
    {
        public SimpleService()
        {
            CodeObject = new Simple();
        }

        protected override bool TryParse(string line, out Simple simple)
        {
            string[] parts = line.Split(' ');

            if (parts.Length == 1)
            {
                simple = new Simple() { CompleteCommend = parts[0] };
                return true;
            }

            simple = null;
            return false;
        }

        protected override IEnumerable<Func<Simple, string>> GetObjectsCodeGenerators()
        {
            yield return new Func<Simple, string>(GetCode);
        }

        private string GetCode(Simple simple)
        {
            switch (simple.GetCommend().ToLower())
            {
                case "sgt":
                    return GetSingleton(simple.GetArgs());

                case "xcd":
                    return GetXmalColumnDefinition(simple.GetArgs());

                case "xrd":
                    return GetXmalRowDefinition(simple.GetArgs());
            }

            throw new ArgumentException("Commend \"{0}\" is not implemented,", simple.GetCommend());
        }

        public string GetSingleton(string[] args)
        {
            string format = "\r\n";
            format += "\t\tprivate static {2} instance;\r\n";
            format += "\r\n";
            format += "\t\tpublic static {2} Current\r\n";
            format += "\t\t{0}\r\n";
            format += "\t\t\tget\r\n";
            format += "\t\t\t{0}\r\n";
            format += "\t\t\t\tif (instance == null) instance = new {2}();\r\n";
            format += "\r\n";
            format += "\t\t\t\treturn instance;\r\n";
            format += "\t\t\t{1}\r\n";
            format += "\t\t{1}\r\n";
            format += "\r\n";
            format += "\t\tprivate {2}()\r\n";
            format += "\t\t{0}\r\n";
            format += "\t\t{1}\r\n";

            return string.Format(format, "{", "}", args[0]);
        }

        private static string GetXmalColumnDefinition(string[] args)
        {
            IEnumerable<string> columnWidths = args.SelectMany(ToXamlGridLengthTexts);

            string code = "<Grid.ColumnDefinitions>\r\n";

            code += string.Concat(columnWidths.Select(w => "<ColumnDefinition Width=\"" + w + "\" />\r\n"));
            code += "</Grid.ColumnDefinitions>\r\n\r\n";

            return code;
        }

        private string GetXmalRowDefinition(string[] args)
        {
            IEnumerable<string> rowHeights = args.SelectMany(ToXamlGridLengthTexts);

            string code = "<Grid.RowDefinitions>\r\n";

            code += string.Concat(rowHeights.Select(w => "<RowDefinition Height=\"" + w + "\" />\r\n"));
            code += "</Grid.RowDefinitions>\r\n\r\n";

            return code;
        }

        private static IEnumerable<string> ToXamlGridLengthTexts(string arg)
        {
            int i = 0, count;
            string countString = string.Empty;

            if (arg.Contains("x") || arg.Contains("a"))
            {
                foreach (char c in arg)
                {
                    i++;

                    if (c == 'x' || c == 'a') break;

                    countString += c;
                }

                if (countString.Length == 0 || !int.TryParse(countString, out count)) count = 1;
            }
            else count = 1;

            if (arg.Contains("a"))
            {
                return Enumerable.Repeat("Auto", count);
            }

            return Enumerable.Repeat(arg.Substring(i), count);
        }
    }
}
