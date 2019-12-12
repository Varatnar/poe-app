using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using poe_backend.Database;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IPoeAppDatabase _databaseService;

        public ItemController(IPoeAppDatabase databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("oneHandSwords")]
        public IEnumerable<OneHandedSword> GetAllOneHandedSwords()
        {
            return _databaseService.GetAllOneHandedSword();
        }

        [HttpGet]
        [Route("twoHandSwords")]
        public IEnumerable<TwoHandedSword> GetAllTwoHandedSwords()
        {
            return _databaseService.GetAllTwoHandedSwords();
        }


        [HttpGet]
        [Route("add")]
        public void Add()
        {
            var oneWeapon = new OneHandedSword
            {
                Key = "Metadata/Items/Weapons/OneHandWeapons/OneHandSwords/OneHandSword18",
                Domain = "item",
                DropLevel = 60,
                InventoryHeight = 2,
                InventoryWidth = 2,
                Name = "Gladius"
            };

            var twoWeapon = new TwoHandedSword
            {
                Key = "Metadata/Items/Weapons/OneHandWeapons/OneHandSwords/TowHandSword18",
                Domain = "item",
                DropLevel = 60,
                InventoryHeight = 4,
                InventoryWidth = 2,
                Name = "Gladius"
            };

            _databaseService.AddOneHandedWeapon(oneWeapon);
            _databaseService.AddTwoHandedWeapon(twoWeapon);
        }
    }
}