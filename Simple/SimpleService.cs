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

        public override bool TryParse(string line, out Simple simple)
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
                    return GetSingelton(simple.GetArgs());

                case "xcd":
                    return GetXmalColumnDefinition(simple.GetArgs());

                case "xrd":
                    return GetXmalRowDefinition(simple.GetArgs());
            }

            throw new ArgumentException("Commend \"{0}\" is not implemented,", simple.GetCommend());
        }

        private string GetSingelton(string[] args)
        {
            string format = "private static {0} instance;\r\n\r\n";
            format += "public static {0} Current\r\n{1}\r\nget\r\n{1}\r\n";
            format += "if (instance == null) instance=new {0}();\r\n\r\nreturn instance;";
            format += "\r\n{2}\r\n{2}";

            return string.Format(format, args[0], "{", "}");
        }

        private string GetXmalColumnDefinition(string[] args)
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

        private IEnumerable<string> ToXamlGridLengthTexts(string arg)
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
