using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poe_backend.Database;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.DataRetrieving
{
    public class Retriever
    {
        public void InitializeBaseItemTable()
        {
            InitializeBaseItemTable(new DbContextOptionsBuilder<PoeAppDbContext>()
                                    .UseSqlite(@"Data Source=Data.db;")
                                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                    .EnableSensitiveDataLogging()
                                    .Options);
        }

        public void InitializeBaseItemTable(DbContextOptions options)
        {
            using var context = new PoeAppDbContext(options);
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
                        InsertTwoHandedWeapons(context, child);
                        break;

                    case "Metadata/Items/Weapons/OneHandWeapons/OneHandSwords":
                        InsertOneHandedWeapons(context, child);
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

            context.SaveChanges();
        }

        private void InsertTwoHandedWeapons(PoeAppDbContext context, JProperty data)
        {
            var twoHandWeapon = CreateWithBasicData<TwoHandedSword>(context, data);

            context.TwoHandedSwords.Add(twoHandWeapon);
        }

        private void InsertOneHandedWeapons(PoeAppDbContext context, JProperty data)
        {
            var oneHandWeapon = CreateWithBasicData<OneHandedSword>(context, data);

            context.OneHandedSwords.Add(oneHandWeapon);
        }

        private T CreateWithBasicData<T>(PoeAppDbContext context, JProperty dataHolder) where T : BaseItem, new()
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

//                var tag = new PoeTag
//                {
//                    Tag = dataTag.Value<string>()
//                };

                var newItemTag = new ItemTag
                {
                    Item = item,
                    Tag = tag
                };

                if (item.PoeTagsLink == null)
                {
                    item.PoeTagsLink = new List<ItemTag>();
                }

                item.PoeTagsLink.Add(newItemTag);
            }

            return item;
        }

        private List<PoeTag> _cachedTags = new List<PoeTag>();

        private PoeTag RetrieveOrCreateTag(string tagKey)
        {
            foreach (var cachedTag in _cachedTags)
            {
                if (cachedTag.Tag.Equals(tagKey))
                {
                    return cachedTag;
                }
            }

            var tag = new PoeTag
            {
                Tag = tagKey
            };

            _cachedTags.Add(tag);


            return tag;
        }
    }
}