using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FileOrder.Services;

namespace FileOrder.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly CopyViewModel _copyViewModel;

        private string _txtSource;
        private string _txtTarget;
        private string _txtCopyOptions;

        public string TxtSource
        {
            get { return _txtSource; }
            set
            {
                _txtSource = value;
                NotifyOfPropertyChange(() => TxtSource);
            }
        }
        public string TxtTarget
        {
            get { return _txtTarget; }
            set
            {
                _txtTarget = value;
                NotifyOfPropertyChange(() => TxtTarget);
            }
        }
        public string TxtCopyOptions
        {
            get { return _txtCopyOptions; }
            set
            {
                _txtCopyOptions = value;
                NotifyOfPropertyChange(() => TxtCopyOptions);
            }
        }

        public ShellViewModel(IWindowManager windowManager, CopyViewModel copyViewModel)
        {
            _windowManager = windowManager;
            _copyViewModel = copyViewModel;
        }

        public async Task Run()
        {
            if (!IsAllParametersValid())
            {
                return;
            }

            _copyViewModel.Init(new DirectoryInfo(TxtSource), new DirectoryInfo(TxtTarget));
            var result = await _windowManager.ShowDialogAsync(_copyViewModel).ConfigureAwait(true);
            if (result.Value)
            {
                MessageBox.Show("Done!", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SourceChoose()
        {
            TxtSource = GetFolderPath();
        }

        public void TargetChoose()
        {
            TxtTarget = GetFolderPath();
        }

        private string GetFolderPath()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.SelectedPath.ToString();
                }
            }

            return null;
        }

        private bool IsAllParametersValid()
        {
            if (string.IsNullOrWhiteSpace(TxtSource))
            {
                MessageBox.Show("Source folder is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtTarget))
            {
                MessageBox.Show("Target folder is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Directory.Exists(TxtSource))
            {
                MessageBox.Show("Source folder is not exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Directory.Exists(TxtTarget))
            {
                MessageBox.Show("Target folder is not exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // if (string.IsNullOrWhiteSpace(TxtCopyOptions))
            // {
            //     MessageBox.Show("Option settings are empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //     return false;
            // }

            return true;
        }
    }
}