namespace poe_backend.Models.ItemData
{
    /// <summary>
    /// Used to cache data in the retriever to insert most data in one shot.
    /// </summary>
    public interface CacheableData
    {
        string Key { get; set; }
    }
}