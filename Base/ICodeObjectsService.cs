namespace CodeGenerator
{
    interface ICodeObjectsService
    {
        int CodePartsIndex { get; }
        bool IsCopying { get; }

        string GetWholeCode();
        string GetNextCodePart(out bool isLastPart);
        void StopCopying();
    }
}
