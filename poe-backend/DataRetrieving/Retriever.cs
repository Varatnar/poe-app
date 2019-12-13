using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poe_backend.Database;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.DataRetrieving
{
    public class Retriever
    {
        private PoeAppDbContext _context;

        public void InitializeBaseItemTable(PoeAppDbContext context)
        {
            _context = context;

            using var client = new WebClient();

            const string address =
                "https://raw.githubusercontent.com/brather1ng/RePoE/master/RePoE/data/base_items.json";
            var jsonData = client.DownloadString(address);

            var deserializeObject = JsonConvert.DeserializeObject<JObject>(jsonData);

            var unprocessedKey = new HashSet<string>();

            foreach (var child in deserializeObject.Properties())
            {
                var lastSlashIndex = child.Name.LastIndexOf("/", StringComparison.Ordinal);
                var typeKey = child.Name.Substring(0, lastSlashIndex);

                switch (typeKey)
                {
                    case "Metadata/Items/Weapons/TwoHandWeapons/TwoHandSwords":
                        InsertTwoHandedWeapons(child);
                        break;

                    case "Metadata/Items/Weapons/OneHandWeapons/OneHandSwords":
                        InsertOneHandedWeapons(child);
                        break;
                    default:
                        if (!unprocessedKey.Contains(typeKey))
                        {
                            unprocessedKey.Add(typeKey);
                        }

                        break;
                }
            }

            Console.WriteLine("List of unprocessed keys :");

            foreach (var key in unprocessedKey)
            {
                Console.WriteLine(key);
            }
        }

        private void InsertTwoHandedWeapons(JProperty data)
        {
            var twoHandWeapon = CreateWithBasicData<TwoHandedSword>(data);

            _context.TwoHandedSwords.Add(twoHandWeapon);
        }

        private void InsertOneHandedWeapons(JProperty data)
        {
            var oneHandWeapon = CreateWithBasicData<OneHandedSword>(data);

            _context.OneHandedSwords.Add(oneHandWeapon);
        }

        private T CreateWithBasicData<T>(JProperty dataHolder) where T : BaseItem, new()
        {
            var item = new T
            {
                Key = dataHolder.Name,
                Name = dataHolder.Value.Value<string>("name"),
                DropLevel = dataHolder.Value.Value<int>("drop_level"),
                InventoryHeight = dataHolder.Value.Value<int>("inventory_height"),
                InventoryWidth = dataHolder.Value.Value<int>("inventory_width"),
                ReleaseState = dataHolder.Value.Value<string>("release_state"),
                Domain = dataHolder.Value.Value<string>("domain")
            };

            var dataTags = dataHolder.Value.Value<JArray>("tags");

            foreach (var dataTag in dataTags)
            {
                var tag = RetrieveOrCreateTag(dataTag.Value<string>());

                var newItemTag = new ItemTag
                {
                    Item = item,
                    Tag = tag
                };

                if (tag.ItemTags == null)
                {
                    tag.ItemTags = new List<ItemTag>();
                }

                item.ItemTags.Add(newItemTag);
                tag.ItemTags.Add(newItemTag);

            }

            return item;
        }

        private PoeTag RetrieveOrCreateTag(string tagKey)
        {
            var tag = _context.PoeTags.Find(tagKey);

            if (tag == null)
            {
                tag = new PoeTag
                {
                    Tag = tagKey
                };
            }

            return tag;
        }
    }
}