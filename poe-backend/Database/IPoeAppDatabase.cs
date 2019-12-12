using System.Collections.Generic;
using poe_backend.Models.ItemData;

namespace poe_backend.Database
{
    public interface IPoeAppDatabase
    {
        IEnumerable<BaseItem> GetAll();
    }
}