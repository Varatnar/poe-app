namespace poe_backend.Models.ItemData.ItemProperties
{
    /// <summary>
    /// https://github.com/brather1ng/RePoE/blob/master/RePoE/docs/base_items.md
    ///
    /// Using Strongly typed object mostly for learning to fill those from json
    ///
    /// The situation would probably be better by simply using dictionaries
    ///
    /// </summary>
    public class GlobalItemProperties
    {
        public int Armour { get; set; }
        public int Evasion { get; set; }
        public int EnergyShield { get; set; }

        public int MovementSpeed { get; set; }

        public int Block { get; set; }

        // flasks
        public int LifePerUse { get; set; }
        public int ManaPerUse { get; set; }

        // flasks
        public int Duration { get; set; }
        public int ChargesMax { get; set; }
        public int ChragesPerUse { get; set; }

        public int CriticalStrikeChance { get; set; }

        public int AttackTime { get; set; }

        public int PhysicalDamageMax { get; set; }
        public int PhysicalDamageMin { get; set; }
        
        public int Range { get; set; }
        
        // currency
        public int StackSize { get; set; }
        public int StackSizeCurrencyTab { get; set; }
        
        public string Directions { get; set; }
        public string Description { get; set; }
        
        public string FullStackTurnsInto { get; set; }

    }
}