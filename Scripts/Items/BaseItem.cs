namespace Scripts.Items {
    public class BaseItem{
        public string Name { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public float BaseValue { get; set; }
        public Weight WeightClass { get; set; }
    }

    public enum ItemType {
        Consumable, Armor, Weapon, Shield, Accessory
    }

    public enum Weight {
        Light, Medium, Heavy
    }
}
