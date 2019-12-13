using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    public class PoeImplicit
    {
        [Key]
        public string Implicit { get; set; }
    }
}