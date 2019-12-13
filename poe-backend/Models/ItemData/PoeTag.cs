using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    public class PoeTag
    {
        [Key]
        public int Id { get; set; }

        public string Tag { get; set; }
        
        public List<ItemTag> BaseItemsLink { get; set; }
    }
}