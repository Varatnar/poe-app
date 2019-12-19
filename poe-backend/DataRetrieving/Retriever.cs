using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poe_backend.Database;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Armours;
using poe_backend.Models.ItemData.Jewelery;
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
            InitializeBaseItemTable(options, out _);
        }

        public void InitializeBaseItemTable(DbContextOptions options, out HashSet<string> unprocessedKeyOut)
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
                    case "Metadata/items/Weapons/OneHandWeapons/OneHandSwords":
                        InsertOneHandedWeapons(context, child);
                        break;
                    case "Metadata/Items/Amulets":
                    case "Metadata/Items/Amulet":
                        InsertAmulet(context, child);
                        break;
                    case "Metadata/Items/Amulets/Talismans":
                        InsertTalisman(context, child);
                        break;
                    case "Metadata/Items/Armours/BodyArmours":
                        InsertBodyArmour(context, child);
                        break;
                    case "Metadata/Items/Armours/Boots":
                        InsertBoots(context, child);
                        break;
                    case "Metadata/Items/Armours/Gloves":
                        InsertGloves(context, child);
                        break;
                    case "Metadata/Items/Armours/Helmets":
                        InsertHelmet(context, child);
                        break;
                    case "Metadata/Items/Armours/Shields":
                        InsertShield(context, child);
                        break;
                    case "Metadata/Items/Belts":
                        InsertBelt(context, child);
                        break;
                    case "Metadata/Items/Quivers":
                        InsertQuiver(context, child);
                        break;
                    case "Metadata/Items/Rings":
                        InsertRing(context, child);
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
            unprocessedKeyOut = unprocessedKey;

            foreach (var key in unprocessedKey)
            {
                Console.WriteLine(key);
            }

            context.SaveChanges();
        }

        // ---- ARMOURS

        private void InsertBodyArmour(PoeAppDbContext context, JProperty data)
        {
            var bodyArmour = CreateWithBasicData<BodyArmour>(data);

            context.BodyArmours.Add(bodyArmour);
        }

        private void InsertBoots(PoeAppDbContext context, JProperty data)
        {
            var boots = CreateWithBasicData<Boot>(data);

            context.Boots.Add(boots);
        }

        private void InsertGloves(PoeAppDbContext context, JProperty data)
        {
            var gloves = CreateWithBasicData<Glove>(data);

            context.Gloves.Add(gloves);
        }

        private void InsertHelmet(PoeAppDbContext context, JProperty data)
        {
            var helmet = CreateWithBasicData<Helmet>(data);

            context.Helmets.Add(helmet);
        }

        public void InsertQuiver(PoeAppDbContext context, JProperty data)
        {
            var quiver = CreateWithBasicData<Quiver>(data);

            context.Quivers.Add(quiver);
        }

        public void InsertShield(PoeAppDbContext context, JProperty data)
        {
            var shield = CreateWithBasicData<Shield>(data);

            context.Shields.Add(shield);
        }

        // ---- JEWELERY

        private void InsertAmulet(PoeAppDbContext context, JProperty data)
        {
            var amulet = CreateWithBasicData<Amulet>(data);

            context.Amulets.Add(amulet);
        }

        private void InsertBelt(PoeAppDbContext context, JProperty data)
        {
            var belt = CreateWithBasicData<Belt>(data);

            context.Belts.Add(belt);
        }

        private void InsertRing(PoeAppDbContext context, JProperty data)
        {
            var ring = CreateWithBasicData<Ring>(data);

            context.Rings.Add(ring);
        }

        private void InsertTalisman(PoeAppDbContext context, JProperty data)
        {
            var talisman = CreateWithBasicData<Talisman>(data);

            context.Talismans.Add(talisman);
        }

        // ---- WEAPONS

        private void InsertTwoHandedWeapons(PoeAppDbContext context, JProperty data)
        {
            var twoHandWeapon = CreateWithBasicData<TwoHandedSword>(data);

            context.TwoHandedSwords.Add(twoHandWeapon);
        }

        private void InsertOneHandedWeapons(PoeAppDbContext context, JProperty data)
        {
            var oneHandWeapon = CreateWithBasicData<OneHandedSword>(data);

            context.OneHandedSwords.Add(oneHandWeapon);
        }

        // ---- Global 

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

            // ----- PoeTag section

            var dataTags = dataHolder.Value.Value<JArray>("tags");

            foreach (var dataTag in dataTags)
            {
                var tag = RetrieveOrCreateTag(dataTag.Value<string>());

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

            // ---- SpriteData section

            var identityHolder = dataHolder.Value.Value<JObject>("visual_identity");
            var spriteDataId = identityHolder.Value<string>("id");
            var spriteDataDdsFile = identityHolder.Value<string>("dds_file");


            var spriteDate = RetrieveOrCreateSpriteData(spriteDataId, spriteDataDdsFile);

            item.VisualIdentity = spriteDate;

            // ---- ItemRequirements

            var requirementsHolder = dataHolder.Value.Value<JObject>("requirements");

            // todo: not sure what happens when null (is it actually null ???)
            if (requirementsHolder != null)
            {
                var requirements = new ItemRequirements
                {
                    RequiredDexterity = requirementsHolder.Value<int>("dexterity"),
                    RequiredIntelligence = requirementsHolder.Value<int>("intelligence"),
                    RequiredStrength = requirementsHolder.Value<int>("strength"),
                    RequiredLevel = requirementsHolder.Value<int>("level")
                };

                item.Requirements = requirements;
            }


            return item;
        }

        private readonly List<PoeTag> _cachedTags = new List<PoeTag>();

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

        private readonly List<SpriteData> _cachedSpriteData = new List<SpriteData>();

        private SpriteData RetrieveOrCreateSpriteData(string spriteDataId, string spriteDataDds)
        {
            foreach (var cachedSpriteData in _cachedSpriteData)
            {
                if (cachedSpriteData.Id.Equals(spriteDataId))
                {
                    if (!cachedSpriteData.DdsFile.Equals(spriteDataDds))
                    {
                        throw new Exception("Sprite Data Id was no unique !");
                    }

                    return cachedSpriteData;
                }
            }

            var spriteData = new SpriteData
            {
                Id = spriteDataId,
                DdsFile = spriteDataDds
            };

            _cachedSpriteData.Add(spriteData);


            return spriteData;
        }
    }
}