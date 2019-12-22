using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poe_backend.Database;
using poe_backend.Models.ItemData;

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

        private void InsertBaseItem(PoeAppDbContext context, JProperty data)
        {
            var baseItem = CreateWithBasicData<BaseItem>(data);

            context.BaseItems.Add(baseItem);
        }

        public void InitializeBaseItemTable(DbContextOptions options)
        {
            using var context = new PoeAppDbContext(options);
            using var client = new WebClient();

            // todo: dont leave hardcoded url here
            const string address =
                "https://raw.githubusercontent.com/brather1ng/RePoE/master/RePoE/data/base_items.json";
            var jsonData = client.DownloadString(address);

            var deserializeObject = JsonConvert.DeserializeObject<JObject>(jsonData);

            foreach (var child in deserializeObject.Properties())
            {
                InsertBaseItem(context, child);
            }

            context.SaveChanges();
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

            // ----- PoeTag section

            var dataTags = dataHolder.Value.Value<JArray>("tags");

            foreach (var dataTag in dataTags)
            {
                var tag = RetrieveOrCreateData<PoeTag>("PoeTag", dataTag.Value<string>());

                var newItemTag = new ItemTag
                {
                    Item = item,
                    Tag = tag
                };

                if (item.PoeTagsLink == null)
                {
                    item.PoeTagsLink = new List<ItemTag>();
                }

                var duplicateItemTagFound = false;

                foreach (var itemTag in item.PoeTagsLink)
                {
                    if (itemTag.Item == newItemTag.Item
                        && itemTag.Tag == newItemTag.Tag)
                    {
                        duplicateItemTagFound = true;
                        break;
                    }
                }

                if (duplicateItemTagFound)
                {
                    continue;
                }

                item.PoeTagsLink.Add(newItemTag);
            }

            // ---- ItemClass section

            var itemClassData = dataHolder.Value.Value<string>("item_class");

            var itemClass = RetrieveOrCreateData<ItemClass>("ItemClass", itemClassData);

            item.ItemClass = itemClass;

            // ---- SpriteData section

            var identityHolder = dataHolder.Value.Value<JObject>("visual_identity");

            if (identityHolder.Children().Count() > 2)
            {
                throw new PoeParsingException("Visual identity contains more than 2 children");
            }

            var spriteDataId = identityHolder.Value<string>("id");
            var spriteDataDdsFile = identityHolder.Value<string>("dds_file");

            // todo: is cacheable data really needed here ?
            var spriteDate =
                RetrieveOrCreateData<SpriteData>("SpriteData",
                                                 $"{spriteDataId}{SpriteData.Delimiter}{spriteDataDdsFile}");

            item.VisualIdentity = spriteDate;

            // ---- ItemRequirements

            var requirementsHolder = dataHolder.Value.Value<JObject>("requirements");

            // todo: not sure what happens when null (is it actually null ???)
            // ReSharper disable once InvertIf
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

        private readonly Dictionary<string, List<CacheableData>> _cachedData =
            new Dictionary<string, List<CacheableData>>();

        private T RetrieveOrCreateData<T>(string dataIdentifier, string dataKey) where T : CacheableData, new()
        {
            if (!_cachedData.ContainsKey(dataIdentifier))
            {
                _cachedData[dataIdentifier] = new List<CacheableData>();
            }

            var cache = _cachedData[dataIdentifier];

            foreach (T cachedData in cache)
            {
                if (cachedData.Key.Equals(dataKey))
                {
                    return cachedData;
                }
            }

            var data = new T
            {
                Key = dataKey
            };

            cache.Add(data);

            return data;
        }
    }
}