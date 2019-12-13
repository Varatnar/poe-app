using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    public class PoeTag
    {
        [Key]
        public string Tag { get; set; }
        
        public List<ItemTag> ItemTags { get; set; }
    }
}