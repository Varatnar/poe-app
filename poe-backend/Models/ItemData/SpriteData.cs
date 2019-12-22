using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace poe_backend.Models.ItemData
{
    public class SpriteData : ICacheableData
    {
        public const string Delimiter = "}{";


        public string Id { get; set; }
        public string DdsFile { get; set; }

        [NotMapped]
        public string Key
        {
            get => $"{Id}{Delimiter}{DdsFile}";
            set
            {
                var items = value.Split(Delimiter);

                if (items.Length != 2)
                {
                    throw new Exception($"Cant parse \"{value}\" for SpriteData");
                }

                Id = items[0];
                DdsFile = items[1];
            }
        }
    }
}