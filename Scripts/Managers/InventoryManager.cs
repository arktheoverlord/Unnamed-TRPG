using Godot;
using System.Collections.Generic;
using TRPG.Items;

namespace TRPG.Managers {
    public class InventoryManager : Node {
        private List<Item> items;

        public override void _Ready() {
            items = new List<Item>();
        }
    }
}
