namespace Scripts.Items {
    public class Item {
        public string Name { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public float BaseValue { get; set; }
    }

    public enum ItemType {
        Consumable, Armor, Weapon, Accessory
    }
}
