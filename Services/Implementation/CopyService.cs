using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FileOrder.Services.Implementation
{
    internal sealed class CopyService : ICopyService
    {
        public async Task Copy(DirectoryInfo source, DirectoryInfo target, string options, IProgress<ProgressResult> progress, CancellationToken token)
        {
            var inpuFiles = Directory.GetFiles(source.FullName, "*.*", SearchOption.AllDirectories);
            for(var i = 0; i < inpuFiles.Length; i++)
            {
                progress.Report( new ProgressResult(i * 100/inpuFiles.Length, inpuFiles[i]));

                await CopyFile(inpuFiles[i], target).ConfigureAwait(false);

                token.ThrowIfCancellationRequested();
            }
        }

        private async Task CopyFile(string sourcePath, DirectoryInfo targetFolder)
        {
            using (Stream source = File.Open(sourcePath, FileMode.Open, FileAccess.Read))
            {
                using(Stream destination = File.Create(GetDestinationPath(sourcePath, targetFolder)))
                {
                    await source.CopyToAsync(destination);
                }
            }
        }

        private string GetDestinationPath(string sourcePath, DirectoryInfo targetFolder)
        {
            var timeStamp = File.GetCreationTime(sourcePath);
            var pathParts = new string[] { timeStamp.ToString("yyyy"), timeStamp.ToString("yyyy-MM-dd") };

            var resultPath = targetFolder.FullName;
            foreach(var pathPart in pathParts)
            {
                resultPath = Path.Combine(resultPath, pathPart);

                if (!Directory.Exists(resultPath))
                {
                    Directory.CreateDirectory(resultPath);
                }
            }

            return Path.Combine(resultPath, Path.GetFileName(sourcePath));
        }
    }
}