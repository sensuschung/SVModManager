using SVModManager.Services;
using SVModManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;

namespace SVModManager.ViewModel
{
    class ModListVM: ViewModelBase
    {
        private readonly ModService _modService;
        private readonly TagService _tagService;
        private string currentSelectedTag;
        public string CurrentSelectedTag
        {
            get => currentSelectedTag;
            set
            {
                currentSelectedTag = value;
                OnPropertyChanged(nameof(CurrentSelectedTag));
                freshModList(currentSelectedTag);
            }
        }
        private List<Tag> tags;
        public List<Tag> Tags
        {
            get => tags;
            set
            {
                tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        private List<Mod> currentMods;
        public List<Mod> CurrentMods
        {
            get => currentMods;
        }

        public ICommand AddTagCommand { get; }
        public ICommand TagClickCommand { get; }
        public ICommand ShowModDetailsCommand { get; }
        public ICommand EnableModCommand { get; }
        public ICommand DisableModCommand { get; }
        public ICommand AddTagtoModCommand { get; set; }
        public ICommand RemoveTagFromModCommand { get; set; }

        public ModListVM(ModService modService, TagService tagService)
        {
            _modService = modService;
            _tagService = tagService;
            currentSelectedTag = "全部";

            tags = _tagService.GetAllTags();
            currentMods = _modService.getAllMods();

            AddTagCommand = new RelayCommand(OpenTagInputWindow);
            TagClickCommand = new RelayCommand<string>(OnTagClick);
            ShowModDetailsCommand = new RelayCommand<Mod>(ShowModDetails);
            EnableModCommand = new RelayCommand<string>(ExecuteEnableMod);
            DisableModCommand = new RelayCommand<string>(ExecuteDisableMod);
            AddTagtoModCommand = new RelayCommand<Tuple<string, Tag>>(addTagToMod);
            RemoveTagFromModCommand = new RelayCommand<Tuple<string, Tag>>(removeTagFromMod);
        }

        private void addTag(string TagName)
        {
            if(TagName=="全部"|| TagName == "未分类")
            {
                MessageBox.Show($"该标签名不可添加", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (_tagService.isTagExist(TagName))
                {
                    MessageBox.Show($"该标签名已存在", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _tagService.AddTag(TagName);
                    Tags = _tagService.GetAllTags();
                }
            }
        }

        private void OpenTagInputWindow()
        {
            var tagInputWindow = new TagInputWindow();
            tagInputWindow.Owner = Application.Current.MainWindow;
            tagInputWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? result = tagInputWindow.ShowDialog();
            if (result == true)
            {
                string tagName = tagInputWindow.TagName;
                addTag(tagName);
            }
        }

        private void freshModList(string tag)
        {
            if (tag == "全部")
            {
                currentMods = _modService.getAllMods();
            }
            else if (tag == "未分类")
            {
                currentMods = _modService.getModsWithoutTag();
            }
            else
            {
                currentMods = _modService.getModsByTag(tag);
            }
            OnPropertyChanged(nameof(CurrentMods));
        }

        private void OnTagClick(string tagName)
        {
            CurrentSelectedTag = tagName; 
        }

        private void ShowModDetails(Mod selectedMod)
        {
            ModDetailsWindow detailsWindow = new ModDetailsWindow(selectedMod,EnableModCommand,DisableModCommand,AddTagtoModCommand,RemoveTagFromModCommand);
            detailsWindow.Owner = Application.Current.MainWindow;
            detailsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            detailsWindow.AllTags = Tags;
            detailsWindow.ShowDialog();
        }

        private void ExecuteEnableMod(string modName)
        {
            _modService.enableMod(modName);
            freshModList(currentSelectedTag);
        }


        private void ExecuteDisableMod(string modName)
        {
            _modService.disableMod(modName);
            freshModList(currentSelectedTag);
        }

        private void addTagToMod(Tuple<string, Tag> parameters)
        {
            string modName = parameters.Item1;
            Tag selectedTag = parameters.Item2;
            _modService.addTagToMod(modName, selectedTag);
        }

        private void removeTagFromMod(Tuple<string, Tag> parameters)
        {
            string modName = parameters.Item1;
            Tag selectedTag = parameters.Item2;
            _modService.removeTagFromMod(modName, selectedTag.Name);
        }


    }
}
