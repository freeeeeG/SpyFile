using System;
using UnityEngine;

// Token: 0x020008BF RID: 2239
public class ObjectLayerListItem
{
	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x060040D4 RID: 16596 RVA: 0x0016A2B2 File Offset: 0x001684B2
	// (set) Token: 0x060040D5 RID: 16597 RVA: 0x0016A2BA File Offset: 0x001684BA
	public ObjectLayerListItem previousItem { get; private set; }

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x060040D6 RID: 16598 RVA: 0x0016A2C3 File Offset: 0x001684C3
	// (set) Token: 0x060040D7 RID: 16599 RVA: 0x0016A2CB File Offset: 0x001684CB
	public ObjectLayerListItem nextItem { get; private set; }

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x060040D8 RID: 16600 RVA: 0x0016A2D4 File Offset: 0x001684D4
	// (set) Token: 0x060040D9 RID: 16601 RVA: 0x0016A2DC File Offset: 0x001684DC
	public GameObject gameObject { get; private set; }

	// Token: 0x060040DA RID: 16602 RVA: 0x0016A2E5 File Offset: 0x001684E5
	public ObjectLayerListItem(GameObject gameObject, ObjectLayer layer, int new_cell)
	{
		this.gameObject = gameObject;
		this.layer = layer;
		this.Refresh(new_cell);
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x0016A30E File Offset: 0x0016850E
	public void Clear()
	{
		this.Refresh(Grid.InvalidCell);
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x0016A31C File Offset: 0x0016851C
	public bool Refresh(int new_cell)
	{
		if (this.cell != new_cell)
		{
			if (this.cell != Grid.InvalidCell && Grid.Objects[this.cell, (int)this.layer] == this.gameObject)
			{
				GameObject value = null;
				if (this.nextItem != null && this.nextItem.gameObject != null)
				{
					value = this.nextItem.gameObject;
				}
				Grid.Objects[this.cell, (int)this.layer] = value;
			}
			if (this.previousItem != null)
			{
				this.previousItem.nextItem = this.nextItem;
			}
			if (this.nextItem != null)
			{
				this.nextItem.previousItem = this.previousItem;
			}
			this.previousItem = null;
			this.nextItem = null;
			this.cell = new_cell;
			if (this.cell != Grid.InvalidCell)
			{
				GameObject gameObject = Grid.Objects[this.cell, (int)this.layer];
				if (gameObject != null && gameObject != this.gameObject)
				{
					ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
					this.nextItem = objectLayerListItem;
					objectLayerListItem.previousItem = this;
				}
				Grid.Objects[this.cell, (int)this.layer] = this.gameObject;
			}
			return true;
		}
		return false;
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x0016A460 File Offset: 0x00168660
	public bool Update(int cell)
	{
		return this.Refresh(cell);
	}

	// Token: 0x04002A38 RID: 10808
	private int cell = Grid.InvalidCell;

	// Token: 0x04002A39 RID: 10809
	private ObjectLayer layer;
}
