using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeGenerator
{
    public abstract class CodeObjectsService<T> : ICodeObjectsService, INotifyPropertyChanged where T : INotifyPropertyChanged
    {
        private IEnumerator<string> codePartsEnumerator;
        private int codePartsIndex;

        public int CodePartsIndex
        {
            get { return codePartsIndex; }
            private set
            {
                if (value == codePartsIndex) return;

                codePartsIndex = value;
                OnPropertyChanged(nameof(CodePartsIndex));
                OnPropertyChanged(nameof(IsCopying));
            }
        }

        public bool IsCopying { get { return codePartsIndex >= 0; } }

        public ObservableCollection<T> CodeObjects { get; }

        protected CodeObjectsService()
        {
            CodePartsIndex = -1;

            CodeObjects = new ObservableCollection<T>();
        }

        public string GetWholeCode()
        {
            CodePartsIndex = -1;

            return string.Concat(GetCodeParts(true));
        }

        public string GetNextCodePart(out bool isLastPart)
        {
            int count = Math.Max(GetCodePartGenerators(false).Count(), GetObjectsCodeGenerators(false).Count());

            if (CodePartsIndex == -1)
            {
                codePartsEnumerator = GetCodeParts(false).GetEnumerator();
                codePartsEnumerator.MoveNext();

                CodePartsIndex = 0;
            }
            else
            {
                codePartsEnumerator.MoveNext();
                CodePartsIndex++;
            }

            if (CodePartsIndex + 1 >= count) CodePartsIndex = -1;

            isLastPart = CodePartsIndex == -1;

            return codePartsEnumerator.Current;
        }

        public void StopCopying()
        {
            CodePartsIndex = -1;
        }

        private IEnumerable<string> GetCodeParts(bool getWhole)
        {
            Func<T, string>[] codeGenerators = GetObjectsCodeGenerators(getWhole).ToArray();
            Func<Func<T, string>, string>[] partGenerators = GetCodePartGenerators(getWhole).ToArray();

            for (int i = 0; i < codeGenerators.Length || i < partGenerators.Length; i++)
            {
                Func<T, string> codeGenerator = codeGenerators.ElementAtOrDefault(i);

                if (i < partGenerators.Length) yield return partGenerators[i](codeGenerator);
                else yield return LoopForeach(codeGenerator);
            }
        }

        protected abstract IEnumerable<Func<T, string>> GetObjectsCodeGenerators(bool getWhole);

        protected virtual IEnumerable<Func<Func<T, string>, string>> GetCodePartGenerators(bool getWhole)
        {
            foreach (Func<T, string> func in GetObjectsCodeGenerators(getWhole)) yield return LoopForeach;
        }

        protected string LoopForeach(Func<T, string> func)
        {
            return string.Concat(CodeObjects.Select(o => func(o)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
