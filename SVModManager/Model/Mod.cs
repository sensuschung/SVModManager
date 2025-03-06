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
        public string path { get; set; }

        public int? NexusId { get; set; }

        public DateTime CreateOn { get; set; }
        public DateTime LastModified { get; set; }

        public ICollection<Tag>? Tags { get; set; }

        public bool IsEnabled { get; set; } = true;

    }
}
