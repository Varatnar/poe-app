using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    /// <summary>
    /// Documentation can be found at https://github.com/brather1ng/RePoE/blob/master/RePoE/docs/base_items.md
    ///
    /// </summary>
    public class BaseItem
    {
        [Key]
        public string Key { get; set; } //done

        //----

        public int DropLevel { get; set; } //done


        // todo: implicit will probably be referencing mod id in an other table (doing them later)
//        public List<PoeImplicit> Implicits { get; set; }

        public int InventoryHeight { get; set; } //done
        public int InventoryWidth { get; set; } //done

        public ItemClass ItemClass { get; set; }

        public string Name { get; set; } //done

        public string ReleaseState { get; set; } //done

        public List<ItemTag> PoeTagsLink { get; set; } //done

        public SpriteData VisualIdentity { get; set; } //done

        public ItemRequirements Requirements { get; set; }

//        public abstract IItemProperties Properties { get; set; }

        //todo: seems to only be with flask, will comment for now
//        public Buff GrantsBuff { get; set; }

        public string Domain { get; set; } //done
    }
}