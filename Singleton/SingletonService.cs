using System;
using System.Collections.Generic;

namespace CodeGenerator.Singleton
{
    public class SingletonService : SingleCodeObjectService<Singleton>
    {
        public SingletonService()
        {
            CodeObject = new Singleton();
        }

        protected override bool TryParse(string line, out Singleton singleton)
        {
            string[] parts = line.Split(' ');

            if (parts.Length == 1)
            {
                singleton = new Singleton() { ClassName = parts[0] };
                return true;
            }

            singleton = null;
            return false;
        }

        protected override IEnumerable<Func<Singleton, string>> GetObjectsCodeGenerators(bool getWhole)
        {
            yield return new Func<Singleton, string>(GetCodePart1);
            yield return new Func<Singleton, string>(GetCodePart2);
        }

        private static string GetCodePart1(Singleton singleton)
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

            return string.Format(format, "{", "}", singleton.ClassName);
        }

        private static string GetCodePart2(Singleton singleton)
        {
            string format = "\r\n";
            format += "\t\tprivate {2}()\r\n";
            format += "\t\t{0}\r\n";
            format += "\t\t{1}\r\n";

            return string.Format(format, "{", "}", singleton.ClassName);
        }
    }
}
