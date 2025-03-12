using CommunityToolkit.Mvvm.Input;
using SVModManager.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SVModManager.ViewModel
{
    /// <summary>
    /// ModDetailsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModDetailsWindow : Window
    {

        public static readonly DependencyProperty TagsProperty =
        DependencyProperty.Register(nameof(AllTags), typeof(List<Tag>), typeof(ModDetailsWindow), new PropertyMetadata(null));

        public List<Tag> AllTags
        {
            get { return (List<Tag>)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
            }
        }

        public ICommand EnableModCommand { get; set; }
        public ICommand DisableModCommand { get; set; }
        public ICommand AddTagtoModCommand { get; set; }
        public ICommand RemoveTagCommand_high_level { get; set; }
        public ICommand RemoveTagCommand { get; set; }

        public ModDetailsWindow(Mod selectedMod, ICommand EnableMod,ICommand DisableMod,ICommand AddTagtoMod,ICommand RemoveTag)
        {
            InitializeComponent();
            DataContext = selectedMod;
            //EnableModCommand = EnableMod;
            //DisableModCommand = DisableMod;
            EnableModCommand = EnableMod;
            DisableModCommand = DisableMod;
            AddTagtoModCommand = AddTagtoMod;
            RemoveTagCommand_high_level = RemoveTag;
            RemoveTagCommand = new RelayCommand<Tag>(RemoveTagClick);
        }

        private void EnableButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Mod selectedMod)
            {
                selectedMod.IsEnabled = true;
                this.DataContext = selectedMod;
                this.UpdateLayout();
            }
        }

        private void DisableButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Mod selectedMod)
            {
                selectedMod.IsEnabled = false;
                this.DataContext = selectedMod;
                this.UpdateLayout();
            }
        }

        private void OnAddTagButtonClick(object sender, RoutedEventArgs e)
        {
            string modName;
            if (DataContext is Mod selectedMod)
            {
                modName = selectedMod.Name;
                if(selectedMod.Tags.Any(t => t.Name == SelectedTag.Name) == false)
                {
                    //foreach (var c in selectedMod.Tags.First().Name)
                    //{
                    //    Console.WriteLine($"Character: {c}, Unicode: {(int)c}");
                    //}

                    //foreach (var c in SelectedTag.Name)
                    //{
                    //    Console.WriteLine($"Character: {c}, Unicode: {(int)c}");
                    //}

                    var commandParameter = new Tuple<string, Tag>(modName, SelectedTag);
                    AddTagtoModCommand.Execute(commandParameter);
                    List<Tag> tags = selectedMod.getTags();
                    if (tags.Any(tags => tags.Name == SelectedTag.Name) == false)
                    {
                        tags.Add(SelectedTag);
                    }
                    selectedMod.Tags = tags;
                    this.DataContext = selectedMod;
                    this.UpdateLayout();
                }
            }

        }

        private void RemoveTagClick(Tag tag)
        {
            string modName;
            if (DataContext is Mod selectedMod)
            {
                modName = selectedMod.Name;
                if (selectedMod.Tags.Any(t => t.Name == tag.Name) == true)
                {
                    var commandParameter = new Tuple<string, Tag>(modName, tag);
                    RemoveTagCommand_high_level.Execute(commandParameter);
                    List<Tag> tags = selectedMod.getTags();
                    tags.Remove(tag);
                    selectedMod.Tags = tags;
                    this.DataContext = selectedMod;
                    this.UpdateLayout();
                    //MessageBox.Show($"删除标签{tag.Name}", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
