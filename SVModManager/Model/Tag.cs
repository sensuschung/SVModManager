using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModManager.Model
{
    public class Tag
    {
        [Key]
        [Required]
        public string Name { get; set; }

        public string Color { get; set; }

        public Tag()
        {
        }

        public Tag(string name, string color)
        {
            Name = name;
            Color = color;
        }
    }
}
