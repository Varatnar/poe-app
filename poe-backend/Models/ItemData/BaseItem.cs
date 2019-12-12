using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    //todo: might? create on class for each type (weapon/armor/flask/etc)
    public class BaseItem
    {
        [Key]
        public string Key { get; set; }

        //----

        public int DroptLevel { get; set; }

        //todo: Change string for actual implicit mod object
        public List<string> Implicits { get; set; }

        //todo: Maybe change the both of those thing for one Dimension class
        public int InventoryHeight { get; set; }
        public int InventoryWidth { get; set; }

        //todo: Change string for Item Class object
        public string ItemClass { get; set; }

        public string Name { get; set; }

        //todo: see https://github.com/brather1ng/RePoE/blob/master/RePoE/constants.py#L173 for possible values
        public string ReleaseState { get; set; }

        //todo: Change for list of Tag object
        public List<string> Tags { get; set; }

        public SpriteData VisualIdentity { get; set; }

//        public ItemRequirements? Requirements { get; set; }

//        public IItemProperties Properties { get; set; }

        //todo: seems to only be with flask, will comment for now
//        public Buff GrantsBuff { get; set; }

        public string Domain { get; set; }
    }
}