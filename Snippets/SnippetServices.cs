using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StdOttStandard;

namespace CodeGenerator.Snippets
{
    public class SnippetServices : IEnumerable<SnippetService>
    {
        private readonly IEnumerable<SnippetService> services;

        public SnippetServices()
        {
            string[] fileLines = File.ReadAllLines("snippets.txt");
            services = GetServices(fileLines).ToArray();
        }

        private static IEnumerable<SnippetService> GetServices(string[] lines)
        {
            foreach (IList<string> serviceLines in Split(lines, "snippet"))
            {
                SnippetService service;
                if (TryGetSnippetService(serviceLines, out service)) yield return service;
            }
        }

        private static IEnumerable<IList<string>> Split(IList<string> lines, string propertyName)
        {
            int i = 0;

            while (i < lines.Count)
            {
                if (lines[i++].Trim().ToLower() != "#" + propertyName) continue;

                List<string> propertyLines = new List<string>();

                while (true)
                {
                    if (i == lines.Count) yield break;
                    if (lines[i].Trim().ToLower() == "#end" + propertyName)
                    {
                        yield return propertyLines;
                        break;
                    }

                    propertyLines.Add(lines[i++]);
                }
            }
        }

        private static bool TryGetSnippetService(IList<string> lines, out SnippetService service)
        {
            service = null;

            string name, wholeCode;
            IList<string> names;
            if (!Split(lines, "name").TryFirst(out names) || !names.TryFirst(out name)) return false;

            IList<string> codeParts = Split(lines, "part").Select(part => string.Join("\r\n", part)).ToArray();
            IList<string> wholeCodes = Split(lines, "code").Select(part => string.Join("\r\n", part)).ToArray();

            if (codeParts.Count == 0 && wholeCodes.Count == 0) return false;

            if (!wholeCodes.TryFirst(out wholeCode)) wholeCode = string.Join("\r\n", codeParts);
            if (codeParts.Count == 0) codeParts = new string[] { wholeCode };

            service = new SnippetService(name, wholeCode, codeParts);
            return true;
        }

        public IEnumerator<SnippetService> GetEnumerator()
        {
            return services.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
