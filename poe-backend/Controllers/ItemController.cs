using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using poe_backend.Database;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        [HttpGet]
        [Route("oneHandSwords")]
        public IEnumerable<OneHandedSword> GetAllOneHandedSwords()
        {
            using var context = new PoeAppDbContext();
            return context.OneHandedSwords.ToList();
        }

        [HttpGet]
        [Route("twoHandSwords")]
        public IEnumerable<TwoHandedSword> GetAllTwoHandedSwords()
        {
            using var context = new PoeAppDbContext();
            return context.TwoHandedSwords.ToList();
        }

        [HttpGet]
        [Route("tags")]
        public IEnumerable<PoeTag> GetTags()
        {
            using var context = new PoeAppDbContext();
            return context.PoeTags.ToList();
        }
    }
}