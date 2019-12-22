using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace poe_backend.Models.ItemData
{
    public class ItemClass : ICacheableData
    {
        // public int id { get; set; }

        [Key]
        public string ItemClassValue { get; set; }

        [NotMapped]
        public string Key
        {
            get => ItemClassValue;
            set => ItemClassValue = value;
        }
    }
}