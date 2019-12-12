using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using poe_backend.Database;
using poe_backend.Models.ItemData;

namespace poe_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseItemController : ControllerBase
    {
        private readonly IPoeAppDatabase _databaseService;

        public BaseItemController(IPoeAppDatabase databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("items")]
        public IEnumerable<BaseItem> GetAll()
        {
            return _databaseService.GetAll();
        }
    }
}