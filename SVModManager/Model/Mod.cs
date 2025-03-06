using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SVModManager.Model
{
    public class Mod
    {
        [Key]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        public int? NexusId { get; set; }

        public DateTime CreateOn { get; set; }
        public DateTime LastModified { get; set; }
        public ICollection<Tag>? Tags { get; set; }

        public bool IsEnabled { get; set; }

        public Mod()
        {
            Tags = new List<Tag>();
        }

        public Mod(string name, string path, int? id, DateTime createOn, DateTime lastModified, bool isEnabled)
        {
            Name = name;
            Path = path;
            NexusId = id;
            CreateOn = createOn;
            LastModified = lastModified;
            Tags = new List<Tag>();
            IsEnabled = isEnabled;
        }
    }
}
