using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ldjam_2024
{
	[Tool]
	public class Inventory : Node
	{

		public Gun GunSlot1 { get; set; }
		public Gun GunSlot2 { get; set; }
		
		[Export]
		public NodePath AllSlotsPath { get; set; }
		public GridContainer AllSlots { get; set; }
		
		List<Panel> panels = new List<Panel>();

		public void InitDefaultLoadOut()
		{
			if(!panels.Any())
				throw new Exception("No panels have been initialized.");
			string machineGunRes = "res://gun1.png";
			string shotgunRes = "res://ShotgunStatic.png";
			
			StyleBoxTexture machineGunTex = new StyleBoxTexture();
			machineGunTex.Texture = ResourceLoader.Load<Texture>(machineGunRes);
			panels[0].AddStyleboxOverride("panel", machineGunTex); 
			
			StyleBoxTexture shotgunTex = new StyleBoxTexture();
			shotgunTex.Texture = ResourceLoader.Load<Texture>(shotgunRes);
			panels[1].AddStyleboxOverride("panel", shotgunTex);
		}

		public override void _Ready()
		{
			if(AllSlotsPath == null)
				throw new Exception("AllSlotsPath is null.");
			AllSlots = GetNode<GridContainer>(AllSlotsPath);
			panels = AllSlots.GetChildren().OfType<Panel>()
				.Where(p => p.Name.StartsWith("ItemPanel"))
				.ToList();
			
			InitDefaultLoadOut();
		}
	}
}
