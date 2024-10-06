using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ldjam_2024
{
	public class HealthBar : Node2D
	{
		public HealthBar()
		{
			Ticks = new List<AnimatedSprite>();
		}
		
		public List<AnimatedSprite> Ticks { get; set; }

		public override void _Ready()
		{
			var ticks = GetChildren().Cast<Node2D>().Where(x => x.Name.StartsWith("Tick")).Cast<AnimatedSprite>().ToList();
			foreach (var t in ticks)
			{
				t.Play("default");
			}

			// I am a programmer with 15 years YOE. 
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick1"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick2"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick3"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick4"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick5"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick6"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick7"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick8"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick9"));
			Ticks.Add(ticks.FirstOrDefault(x => x.Name == "Tick10"));
		}
		
	}
}
