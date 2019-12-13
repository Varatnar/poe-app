namespace poe_backend.Models.ItemData
{
    public class ItemTag
    {
        public int TagId { get; set; }
        public PoeTag Tag { get; set; }

        public string ItemKey { get; set; }
        public BaseItem Item { get; set; }
    }
}