using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Snippets
{
    public class SnippetService : SingleCodeObjectService<Snippet>
    {
        public string Name { get; }

        public SnippetService(string name, string wholeCode, IEnumerable<string> codeParts)
        {
            Name = name;

            foreach (string codePart in codeParts)
            {
                CodeObjects.Add(new Snippet(wholeCode, codePart));
            }
        }

        protected override IEnumerable<Func<Snippet, string>> GetObjectsCodeGenerators(bool getWhole)
        {
            yield break;
        }

        protected override IEnumerable<Func<Func<Snippet, string>, string>> GetCodePartGenerators(bool getWhole)
        {
            if (getWhole)
            {
                return new Func<Func<Snippet, string>, string>[] { f => GetCodeFunc()(CodeObject) };
            }

            return CodeObjects.Select(s => GetPartFuncFunc(s));
        }

        private Func<Snippet, string> GetCodeFunc()
        {
            return s => s.WholeCode;
        }

        private Func<Func<Snippet, string>, string> GetPartFuncFunc(Snippet snippet)
        {
            return s => GetPartFunc()(snippet);
        }

        private Func<Snippet, string> GetPartFunc()
        {
            return s => s.CodePart;
        }

        protected override bool TryParse(string line, out Snippet obj)
        {
            throw new NotImplementedException();
        }
    }
}
