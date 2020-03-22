using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FileOrder.Services;

namespace FileOrder.ViewModels
{
    public class CopyViewModel : Screen
    {
        private readonly ICopyService _copyService;

        private DirectoryInfo _source;
        private DirectoryInfo _target;
        private int _currentProgress;
        private string _currentFile;
        private string _fullCurrentFile;
        private CancellationTokenSource _cancellationTokenSource;

        public int CurrentProgress
        {
            get { return _currentProgress; }
            set
            {
                _currentProgress = value;
                NotifyOfPropertyChange(() => CurrentProgress);
            }
        }

        public string CurrentFile
        {
            get { return _currentFile; }
            set
            {
                _currentFile = value;
                NotifyOfPropertyChange(() => CurrentFile);
            }
        }

        public string FullCurrentFile
        {
            get { return _fullCurrentFile; }
            set
            {
                _fullCurrentFile = value;
                NotifyOfPropertyChange(() => FullCurrentFile);
            }
        }

        public CopyViewModel(ICopyService copyService)
        {
            _copyService = copyService;
        }

        public void Init(DirectoryInfo source, DirectoryInfo target)
        {
            _source = source;
            _target = target;

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await BackgroudTask(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
        }

        public async Task Cancel()
        {
            _cancellationTokenSource.Cancel();
            await TryCloseAsync(false);
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            await Task.CompletedTask;
        }

        private async Task BackgroudTask(CancellationToken token)
        {
            await _copyService.Copy(_source, _target, null, new Progress<ProgressResult>(SetProgressResult), token).ConfigureAwait(true);
            await TryCloseAsync(true);
        }

        private void SetProgressResult(ProgressResult progressResult)
        {
            CurrentProgress = progressResult.Percent;
            FullCurrentFile = progressResult.CurrentFile;
            CurrentFile = Path.GetFileName(progressResult.CurrentFile);
        }
    }
}