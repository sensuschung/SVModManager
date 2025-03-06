using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModManager.Model
{
    class config
    {
        [Key]
        [Required]
        public string Name { get; set; }

        public string? Value { get; set; }
    }
}
