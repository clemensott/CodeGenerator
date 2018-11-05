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

        public override bool TryParse(string line, out Singleton singleton)
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

        protected override IEnumerable<Func<Singleton, string>> GetObjectsCodeGenerators()
        {
            yield return new Func<Singleton, string>(GetCode);
        }

        private string GetCode(Singleton singleton)
        {
            string format = "private static {0} instance;\r\n\r\n";
            format += "public static {0} Current\r\n{1}\r\nget\r\n{1}\r\n";
            format += "if(instance==null)instance=new {0}();\r\n\r\nreturn instance;";
            format += "\r\n{2}\r\n{2}";

            return string.Format(format, singleton.ClassName, "{", "}");
        }
    }
}
