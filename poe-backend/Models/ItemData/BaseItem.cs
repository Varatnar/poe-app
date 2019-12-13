using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    //todo: might? create one class for each type (weapon/armor/flask/etc)
    public abstract class BaseItem
    {
        [Key]
        public string Key { get; set; } //done

        //----

        public int DropLevel { get; set; } //done


//        public List<PoeImplicit> Implicits { get; set; }

        public int InventoryHeight { get; set; } //done
        public int InventoryWidth { get; set; } //done

        public abstract ItemClass ItemClass { get; } //todo: verify

        public string Name { get; set; } //done

        public string ReleaseState { get; set; } //done

        public List<ItemTag> PoeTagsLink { get; set; } //done

        public SpriteData VisualIdentity { get; set; } //done

        public ItemRequirements Requirements { get; set; }

//        public IItemProperties Properties { get; set; }

        //todo: seems to only be with flask, will comment for now
//        public Buff GrantsBuff { get; set; }

        public string Domain { get; set; } //done
    }
}