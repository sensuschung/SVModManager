using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SVModManager.Model
{
    public class Mod : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        private int? _nexusId;
        private string? _author;
        private string? _version;
        private string? _description;
        private DateTime _createOn;
        private DateTime _lastModified;
        private ICollection<Tag>? _tags;
        private bool _isEnabled;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Key]
        [Required]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required]
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? NexusId
        {
            get => _nexusId;
            set
            {
                if (_nexusId != value)
                {
                    _nexusId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Author
        {
            get => _author;
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Version
        {
            get => _version;
            set
            {
                if (_version != value)
                {
                    _version = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreateOn
        {
            get => _createOn;
            set
            {
                if (_createOn != value)
                {
                    _createOn = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime LastModified
        {
            get => _lastModified;
            set
            {
                if (_lastModified != value)
                {
                    _lastModified = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICollection<Tag>? Tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public Mod()
        {
            Tags = new List<Tag>();
        }

        public Mod(string name, string path, string? author, string? version, string? description, int? id, DateTime createOn, DateTime lastModified, bool isEnabled)
        {
            Name = name;
            Path = path;
            NexusId = id;
            Author = author;
            Version = version;
            Description = description;
            CreateOn = createOn;
            LastModified = lastModified;
            Tags = new List<Tag>();
            IsEnabled = isEnabled;
        }

        public List<Tag> getTags()
        {
            return this.Tags.ToList();
        }
    }
}
