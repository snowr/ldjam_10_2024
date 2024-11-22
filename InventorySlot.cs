using Godot;
using Godot.Collections;

namespace ldjam_2024
{
	public class InventorySlot : Panel 
	{
		public bool Empty { get; set; }
		public Item SlotItem { get; set; }

		public void SetItem(string texture, Item item)
		{
			SlotItem = item;
			TexturePath = texture;
			StyleBoxTexture itemTexture = new StyleBoxTexture();
			itemTexture.Texture = ResourceLoader.Load<Texture>(TexturePath);
			Icon = itemTexture.Texture;
			AddStyleboxOverride("panel", itemTexture);
			GD.Print("Set Item");
			Empty = false;

		}

		public void SetItem(Item item)
		{
			SlotItem = item;
			TexturePath = item.InventoryTexturePath;
			StyleBoxTexture itemTexture = new StyleBoxTexture();
			itemTexture.Texture = ResourceLoader.Load<Texture>(TexturePath);
			Icon = itemTexture.Texture;
			AddStyleboxOverride("panel", itemTexture);
			GD.Print("Set Item");
			Empty = false;
		}
		
		public void UnSet()
		{
			Empty = true;
			TexturePath = "";
			Icon = null;
			RemoveStyleboxOverride("panel");
		}
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
			Dictionary<string, Item>dragData =
				new Dictionary<string, Item>();
			dragData.Add("weapon_dragged", SlotItem);
			Control dragPrev = null;
			if (Icon != null)
			{
				// outer Control to allow us to manipulate the position of the drag preview
				Control ctl = new Control();
				dragPrev = new TextureRect()
				{
					Texture = Icon,
					StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
					Visible = true, 
					Modulate = new Color(1, 1, 1, 0.7f),  // Make it slightly transparent
				};
				
				ctl.AddChild(dragPrev);
				dragPrev.RectPosition = new Vector2(0, -10);

				SetDragPreview(ctl);
				GD.Print("Hello");
				return dragData;
			}

			return null;
		}
	}
}
