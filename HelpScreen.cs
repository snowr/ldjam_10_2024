using Godot;

namespace ldjam_2024
{
	public class HelpScreen : Node2D
	{
		public override void _Ready()
		{
			PauseMode = PauseModeEnum.Process;
			Visible = true;
			GetTree().Paused = true;
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsAction("show_help") && @event.IsPressed() && !@event.IsEcho())
			{
				Visible = !Visible;
				GD.Print(Visible);
				if (!Visible)
				{
					GetTree().Paused = false;
				}
				else
				{
					GetTree().Paused = true;
				}
			}
		}
	}
}
