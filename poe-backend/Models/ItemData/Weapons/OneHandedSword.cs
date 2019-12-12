using System.ComponentModel.DataAnnotations.Schema;

namespace poe_backend.Models.ItemData.Weapons
{
    [Table("One_Hand_Swords")]
    public class OneHandedSword : BaseItem
    {
        public override ItemClass ItemClass => ItemClass.OneHandedSword;
    }
}