namespace Scripts.Tiles {
    public class Tile {
        public int ID { get; set; }
        public int Name { get; set; }
        public bool Passable { get; set; }
        public bool ProvidesStealth { get; set; }
        public bool StealthBonus { get; set; }
        public bool ProvidesCover { get; set; }
    }
}