using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using CommunityToolkit.Mvvm.Input;
using SVModManager.Services;

namespace SVModManager.ViewModel
{
    public class SettingsVM: ViewModelBase
    {
        private readonly ConfigService _configService;
        private readonly ModService _modService;
        private string? _modPath;
        public string? ModPath
        {
            get => _modPath;
            set
            {
                _modPath = value;
                OnPropertyChanged(nameof(ModPath));
                OnModPathChanged();
            }
        }
        private string _nexusAPI;
        public string NexusAPI
        {
            get => _nexusAPI;
            set
            {
                _nexusAPI = value;
                OnPropertyChanged(nameof(NexusAPI));
                OnNexusAPIChanged();
            }
        }

        public ICommand AutoSetModPathCommand { get; }
        public ICommand SelectModPathCommand { get; }

        public SettingsVM(ConfigService configService, ModService modService)
        {
            _configService = configService;
            _modService = modService;

            ModPath = _configService.getStardewModPath();
            NexusAPI = _configService.getNexusAPI();

            AutoSetModPathCommand = new RelayCommand(autoSetModPath);
            SelectModPathCommand = new RelayCommand(selectModPath);
        }

        private void autoSetModPath()
        {
            ModPath = _configService.autoGetModPath();
        }

        private void selectModPath()
        {
            using (var dialog = new CommonOpenFileDialog("请选择Mods文件夹"))
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    ModPath = dialog.FileName;
                }
            }
            _configService.setStardewModPath(ModPath);
        }

        private void OnModPathChanged()
        {
            _modService.clearAllMods();
            _modService.ProcessModsInDirectory(ModPath);
        }

        private void OnNexusAPIChanged()
        {
            _configService.setNexusAPI(NexusAPI);
        }
    }
}
