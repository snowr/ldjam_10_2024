using Godot;

namespace ldjam_2024
{
    public class Inventory : Node
    {

        public Gun GunSlot1 { get; set; }
        public Gun GunSlot2 { get; set; }
        public GridContainer AllSlots { get; set; }

        public void InitDefaultLoadOut()
        {
        }
    }
}
