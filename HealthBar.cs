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
		
		private List<AnimatedSprite> Ticks { get; set; }

		public override void _Ready()
		{
			Ticks = GetChildren().Cast<AnimatedSprite>().ToList();
			foreach (var t in Ticks)
			{
				t.Play("default");
			}
		}
		
	}
}
