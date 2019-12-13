using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    public class ItemTag
    {
        [Key]
        public string TagKey { get; set; }
        public PoeTag Tag { get; set; }

        [Key]
        public string ItemKey { get; set; }
        public BaseItem Item { get; set; }
    }
}