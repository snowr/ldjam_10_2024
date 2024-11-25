using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ldjam_2024
{

	public class InventorySlot : Panel 
	{

		[Signal]
		public delegate void InventorySlotChanged(InventorySlotChangedEvent e);
		
		[Export]
		public InventorySlotType SlotType { get; set; }
		
		public bool IsEmpty
		{
			get
			{
				return SlotItem == null;
			}
		}

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
			AddChild(item);
			// We're hiding the item because we're using the InventoryTexturePath to display its thumbnail
			item.Hide();
		}
		
		public void UnSet()
		{
			TexturePath = "";
			Icon = null;
			RemoveStyleboxOverride("panel");
			if(!IsEmpty)
				RemoveChild(SlotItem);
			SlotItem = null;
		}
		public string TexturePath { get; set; }
		private Texture Icon { get; set; }

		public override void _Ready()
		{
			GD.Print($"--------- {Name}");
		}

		public override void _GuiInput(InputEvent @event)
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
			{
				

			}
		}

		public override object GetDragData(Vector2 position)
		{
			Godot.Collections.Dictionary<string, object> dragData =
				new Godot.Collections.Dictionary<string, object>();
			dragData.Add("weapon_dragged", SlotItem);
			dragData.Add("source", this);
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

		public override bool CanDropData(Vector2 position, object data)
		{
			return IsEmpty;
		}

		public override void DropData(Vector2 position, object data)
		{
			GD.Print($"Dropping {data.GetType()} at position {position}");
			Godot.Collections.Dictionary droppedItems = data as Godot.Collections.Dictionary;
			if (droppedItems != null)
			{
				var source = (droppedItems["source"] as InventorySlot);
				GD.Print("Success Drop");
				source.UnSet();
				SetItem(droppedItems["weapon_dragged"] as Item);
				OnItemChanged(source);
			}
		}

		public void OnItemChanged(InventorySlot source)
		{
			EmitSignal(nameof(InventorySlotChanged), new InventorySlotChangedEvent()
			{
				NewItem = SlotItem,
				TargetSlotType = SlotType,
				SourceSlotType = source.SlotType
			});
		}
	}
}
