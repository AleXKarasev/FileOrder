using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileOrder.Services
{
    public interface ICopyService
    {
        Task Copy(DirectoryInfo source, DirectoryInfo target, string options, IProgress<ProgressResult> progress, CancellationToken token);
    }
}