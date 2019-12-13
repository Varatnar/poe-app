﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace poe_backend.Models.ItemData
{
    //todo: might? create one class for each type (weapon/armor/flask/etc)
    public abstract class BaseItem
    {
        [Key]
        public string Key { get; set; }

        //----

        public int DropLevel { get; set; }


//        public List<PoeImplicit> Implicits { get; set; }

        public int InventoryHeight { get; set; }
        public int InventoryWidth { get; set; }

        public abstract ItemClass ItemClass { get; }

        public string Name { get; set; }

        //todo: see https://github.com/brather1ng/RePoE/blob/master/RePoE/constants.py#L173 for possible values
        public string ReleaseState { get; set; }

        public List<ItemTag> PoeTagsLink { get; set; }

        public SpriteData VisualIdentity { get; set; }

        public ItemRequirements Requirements { get; set; }

//        public IItemProperties Properties { get; set; }

        //todo: seems to only be with flask, will comment for now
//        public Buff GrantsBuff { get; set; }

        public string Domain { get; set; }
    }
}