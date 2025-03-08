using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVModManager.Model;

namespace SVModManager.Services
{
    public class TagService
    {
        private readonly DbService _dbService;

        public TagService(DbService dbService)
        {
            _dbService = dbService;
        }

        private string generateColor()
        {
            Random random = new Random();
            int r, g, b;
            double brightness;
            do
            {
                r = random.Next(0, 256);
                g = random.Next(0, 256);
                b = random.Next(0, 256);
                brightness = 0.299 * r + 0.587 * g + 0.114 * b;
            } while (brightness < 70 || brightness > 180);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        public void AddTag(string name)
        {
            if (_dbService.QueryItem<Tag>(m => m.Name == name) != null)
            {
                return;
            }
            _dbService.InsertItem(new Tag(name, generateColor()));
        }

        public void RemoveTag(string name)
        {
            var tag = _dbService.QueryItem<Tag>(m => m.Name == name);
            if (tag != null)
            {
                var mods = _dbService.QueryItems<Mod>(m => m.Tags.Contains(tag));
                foreach (var mod in mods)
                {
                    mod.Tags.Remove(tag);
                    _dbService.UpdateItem(mod);
                }
                _dbService.DeleteItem<Tag>(tag);
            }
        }


    }
}
