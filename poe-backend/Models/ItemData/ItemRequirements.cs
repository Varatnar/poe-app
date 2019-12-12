namespace poe_backend.Models.ItemData
{
    public class ItemRequirements
    {
        public int Id { get; set; }

        public int RequiredLevel { get; set; }
        public int RequiredStrength { get; set; }
        public int RequiredDexterity { get; set; }
        public int RequiredIntelligence { get; set; }
    }
}