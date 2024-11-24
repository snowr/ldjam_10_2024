using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ldjam_2024
{
	// Inventory operations:
	// 1. Add item to inventory
	// 2. Destroy item from inventory
	// 3. Swap items in inventory slots
	// Equipping items is taken care of by the Player class.
	[Tool]
	public class Inventory : Node
	{
		public InventorySlot EquippedSlot1 { get; set; }
		public InventorySlot EquippedSlot2 { get; set; }

		
		[Export]
		public NodePath AllSlotsPath { get; set; }
		
		[Export]
		public NodePath EquippedSlotsPath { get; set; }
		
		public GridContainer AllSlots { get; set; }

		List<InventorySlot> InventorySlots { get; set; } = new List<InventorySlot>();

		public void InitDefaultLoadOut()
		{
			if (!InventorySlots.Any())
				throw new Exception("No panels have been initialized.");
			string machineGunRes = "res://gun1.png";
			string shotgunRes = "res://ShotgunStatic.png";

			PackedScene machineGunScene = ResourceLoader.Load<PackedScene>("res://Gun1.tscn");
			PackedScene shotgunScene = ResourceLoader.Load<PackedScene>("res://Gun1.tscn");
			Gun machineGun = machineGunScene.Instance<Gun>();
			Gun shotgun = shotgunScene.Instance<Gun>();
			machineGun.RateOfFire = 100;
			shotgun.RateOfFire = 500;
			machineGun.InventoryTexturePath = machineGunRes;
			shotgun.InventoryTexturePath = shotgunRes;

			InventorySlots[0].SetItem(machineGun);
			InventorySlots[1].SetItem(shotgun);
			
			foreach (var inventorySlot in InventorySlots)
			{
				inventorySlot.Connect(nameof(InventorySlot.InventorySlotChanged), this, nameof(OnInventorySlotChanged));
			}
		}

		public override void _Ready()
		{
			if(AllSlotsPath == null)
				throw new Exception("AllSlotsPath is null.");
			if(EquippedSlotsPath == null)
				throw new Exception("EquippedSlotsPath is null.");
			
			AllSlots = GetNode<GridContainer>(AllSlotsPath);
			InventorySlots = AllSlots.GetChildren().OfType<InventorySlot>()
				.Where(p => p.Name.StartsWith("ItemPanel"))
				.ToList();

			try
			{
				var allEquippedSlots = GetNode<GridContainer>(EquippedSlotsPath).GetChildren().OfType<InventorySlot>();
				EquippedSlot1 = allEquippedSlots.FirstOrDefault(x => x.Name == "EquippedSlot1");
				EquippedSlot2 = allEquippedSlots.FirstOrDefault(x => x.Name == "EquippedSlot2");

				if (EquippedSlot1 == null || EquippedSlot2 == null)
				{
					throw new Exception("No equipped slots have been initialized.");
				}

				foreach (var slot in allEquippedSlots)
				{
					slot.Connect(nameof(InventorySlot.InventorySlotChanged), this, nameof(OnInventorySlotChanged));
				}
			}
			catch (Exception ex)
			{
				GD.Print($"Failed to get all inventory slots: {ex.Message}");
				throw;
			}

			GD.Print(InventorySlots.Count);
			InitDefaultLoadOut();
		}

		public void AddItem(Item item)
		{
			var slot = GetEmptySlot();
			if (slot == null)
				return;
			slot.SetItem(item.InventoryTexturePath, item);
		}
		
		public InventorySlot GetEmptySlot()
		{
			return InventorySlots.FirstOrDefault(s => s.IsEmpty);
		}

		public void OnInventorySlotChanged(Item newItem)
		{
		}
	}
}
