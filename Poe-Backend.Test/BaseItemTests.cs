using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using poe_backend.Database;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Weapons;

namespace Poe_Backend.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddBaseItemOneHandWeaponTest()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<PoeAppDbContext>()
                          .UseSqlite(connection)
                          .Options;
            try
            {
                using (var context = new PoeAppDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new PoeAppDbContext(options))
                {
                    var baseItem = new OneHandedSword
                    {
                        Domain = "item",
                        Key = "Metadata/Items/Weapons/OneHandWeapons/OneHandSwords/OneHandSword1",
                        DropLevel = 1,
                        InventoryHeight = 3,
                        InventoryWidth = 1,
                        Name = "Rusted Sword",
                        ReleaseState = "released"
                    };

                    context.OneHandedSwords.Add(baseItem);
                    context.SaveChanges();
                }

                using (var context = new PoeAppDbContext(options))
                {
                    Assert.AreEqual(1, context.OneHandedSwords.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public void AddWeaponWithTags()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<PoeAppDbContext>()
                          .UseSqlite(connection)
                          .Options;
            try
            {
                using (var context = new PoeAppDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new PoeAppDbContext(options))
                {
                    var baseItem = new OneHandedSword
                    {
                        Domain = "item",
                        Key = "Metadata/Items/Weapons/OneHandWeapons/OneHandSwords/OneHandSword1",
                        DropLevel = 1,
                        InventoryHeight = 3,
                        InventoryWidth = 1,
                        Name = "Rusted Sword",
                        ReleaseState = "released"
                    };

                    var tag1 = new PoeTag
                    {
                        Tag = "sword"
                    };
                    var tag2 = new PoeTag
                    {
                        Tag = "one_hand_weapon"
                    };
                    var tag3 = new PoeTag
                    {
                        // ReSharper disable once StringLiteralTypo
                        Tag = "onehand"
                    };
                    var tag4 = new PoeTag
                    {
                        Tag = "weapon"
                    };
                    var tag5 = new PoeTag
                    {
                        Tag = "default"
                    };

                    baseItem.PoeTagsLink = new List<ItemTag>
                    {
                        new ItemTag
                        {
                            Item = baseItem,
                            Tag = tag1
                        },
                        new ItemTag
                        {
                            Item = baseItem,
                            Tag = tag2
                        },
                        new ItemTag
                        {
                            Item = baseItem,
                            Tag = tag3
                        },
                        new ItemTag
                        {
                            Item = baseItem,
                            Tag = tag4
                        },
                        new ItemTag
                        {
                            Item = baseItem,
                            Tag = tag5
                        }
                    };


                    context.OneHandedSwords.Add(baseItem);
                    context.SaveChanges();
                }

                using (var context = new PoeAppDbContext(options))
                {
                    Assert.AreEqual(1, context.OneHandedSwords.Count());
                    Assert.AreEqual(5, context.PoeTags.Count());
                    Assert.AreEqual(5, context.ItemTags.Count());


                    var data = context.OneHandedSwords.Include(sword => sword.PoeTagsLink);

                    Assert.AreEqual(5, data.Single().PoeTagsLink.Count);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}