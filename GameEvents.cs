using Godot;

namespace ldjam_2024
{
	public class InventorySlotChangedEvent : Reference
	{
		public Item NewItem { get; set; }
		public InventorySlotType SourceSlotType { get; set; }
		
		public InventorySlotType TargetSlotType { get; set; }
	}
}