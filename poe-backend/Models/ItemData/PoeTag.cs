using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace poe_backend.Models.ItemData
{
    public class PoeTag : CacheableData
    {
        [Key]
        public int Id { get; set; }

        public string Tag { get; set; }

        public List<ItemTag> BaseItemsLink { get; set; }

        [NotMapped]
        public string Key
        {
            get => Tag;
            set => Tag = value;
        }
    }
}