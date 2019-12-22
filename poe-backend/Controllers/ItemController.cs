using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using poe_backend.Database;
using poe_backend.Models.ItemData;

namespace poe_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        [HttpGet]
        [Route("bases")]
        public IEnumerable<BaseItem> GetAllOneHandedSwords()
        {
            using var context = new PoeAppDbContext();
            return context.BaseItems.ToList();
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