namespace FileOrder.Services
{
    public struct ProgressResult
    {
        public int Percent { get; private set;}
        public string CurrentFile { get; private set;}

        public ProgressResult(int percent, string currentFile)
        {
            Percent = percent;
            CurrentFile = currentFile;
        }
    }
}