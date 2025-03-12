using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModManager.Model
{
    public class Tag : INotifyPropertyChanged
    {
        private string _name;
        private string _color;
        private ICollection<Mod> _mods;

        [Key]
        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        public ICollection<Mod> Mods
        {
            get { return _mods; }
            set
            {
                if (_mods != value)
                {
                    _mods = value;
                    OnPropertyChanged(nameof(Mods));
                    OnPropertyChanged(nameof(ModsCount));
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public Tag()
        {
            Mods = new List<Mod>();
        }

        public Tag(string name, string color)
        {
            Name = name;
            Color = color;
            Mods = new List<Mod>();
        }

        public int ModsCount
        {
            get { return Mods?.Count ?? 0; }
        }

        public string DisplayName
        {
            get { return $"{Name}"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
