using Godot;
using Godot.Collections;

namespace ldjam_2024
{
	public class InventorySlot : Panel 
	{
		public bool Empty { get; set; }

		public void SetItem(string texture, WeaponType weaponType)
		{
			WeaponType = weaponType;
			TexturePath = texture;
			StyleBoxTexture itemTexture = new StyleBoxTexture();
							itemTexture.Texture = ResourceLoader.Load<Texture>(TexturePath);
							Icon = itemTexture.Texture;
							AddStyleboxOverride("panel", itemTexture);
							GD.Print("Set Item");
		}

		public WeaponType WeaponType { get; set; }
		public string TexturePath { get; set; }
		private Texture Icon { get; set; }

		public override void _Ready()
		{
		}

		public override void _GuiInput(InputEvent @event)
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
			{
				

			}
		}

		public override object GetDragData(Vector2 position)
		{
			Dictionary<string, WeaponType>dragData =
				new Dictionary<string, WeaponType>();
			dragData.Add("weapon_dragged", WeaponType);
			Control dragPrev = null;
			if (Icon != null)
			{
				dragPrev = new TextureRect()
				{
					Texture = Icon,
					StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
					Visible = true,
								Modulate = new Color(1, 1, 1, 0.7f)  // Make it slightly transparent
					
				};

				SetDragPreview(dragPrev);
				GD.Print("Hello");
				return dragData;
			}

			return null;
		}
	}
}
