using Godot;
using Godot.Collections;

namespace Scripts.Combat.Mapping {
    public class Tile : Area {
        [Export]
        public float Height { get; set; }
        [Export]
        public bool IsObstruction { get; set; } = false;
        [Export]
        public ObstructionType Obstruction { get; set; } = ObstructionType.None;
        [Export]
        public bool IsTreasure { get; set; } = false;
        [Export]
        public TreasureTier Treasure { get; set; } = TreasureTier.None;
        [Export]
        public bool IsFloor { get; set; } = true;
        [Export]
        public bool IsOpeque { get; set; } = true;
        [Export]
        public bool IsHalfBlock { get; set; } = false;

        private PhysicsDirectSpaceState spaceState;
        private Sprite3D northSprite;
        private Sprite3D southSprite;
        private Sprite3D eastSprite;
        private Sprite3D westSprite;

        public override void _Ready() {
            spaceState = GetWorld().GetDirectSpaceState();
            northSprite = GetNode<Sprite3D>("Sprites/North");
            southSprite = GetNode<Sprite3D>("Sprites/South");
            eastSprite = GetNode<Sprite3D>("Sprites/East");
            westSprite = GetNode<Sprite3D>("Sprites/West");
            SetVisableSides();
        }

        private void SetVisableSides() {
            var self = new Array() { this };
            var center = Translation - new Vector3(0, 0.5f, 0);
            var cast = spaceState.IntersectRay(center, center + new Vector3(1, 0, 0), self, collideWithAreas: true);
            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                northSprite.Visible = false;
            }
            cast = spaceState.IntersectRay(center, center + new Vector3(-1, 0, 0), self, collideWithAreas: true);
            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                southSprite.Visible = false;
            }
            cast = spaceState.IntersectRay(center, center + new Vector3(0, 0, 1), self, collideWithAreas: true);
            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                eastSprite.Visible = false;
            }
            cast = spaceState.IntersectRay(center, center + new Vector3(0, 0, -1), self, collideWithAreas: true);
            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                westSprite.Visible = false;
            }
            
        }

        public int GetHighestTile(int tileCount = 0) {
            if (tileCount > 100) {
                return tileCount;
            }
            tileCount++;
            var cast = spaceState.IntersectRay(Translation - new Vector3(0, 0.5f, 0), Translation + new Vector3(0, 1f, 0), collideWithAreas: true);

            if (cast != null && cast.Count > 0 && cast["collider"].GetType() == typeof(Tile)) {
                tileCount = ((Tile)cast["collider"]).GetHighestTile(tileCount);
            }
            return tileCount;
        }

        public enum ObstructionType {
            None, Rock, Tree
        }

        public enum TreasureTier {
            None, Normal, Rare, Epic, Legendary
        }
    }
}