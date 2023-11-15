using System;
using UnityEngine;

// Token: 0x020004DA RID: 1242
[AddComponentMenu("KMonoBehaviour/scripts/AntiCluster")]
public class MoverLayerOccupier : KMonoBehaviour, ISim200ms
{
	// Token: 0x06001C90 RID: 7312 RVA: 0x00098DE0 File Offset: 0x00096FE0
	private void RefreshCellOccupy()
	{
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in this.cellOffsets)
		{
			int current_cell = Grid.OffsetCell(cell, offset);
			if (this.previousCell != Grid.InvalidCell)
			{
				int previous_cell = Grid.OffsetCell(this.previousCell, offset);
				this.UpdateCell(previous_cell, current_cell);
			}
			else
			{
				this.UpdateCell(this.previousCell, current_cell);
			}
		}
		this.previousCell = cell;
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x00098E56 File Offset: 0x00097056
	public void Sim200ms(float dt)
	{
		this.RefreshCellOccupy();
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x00098E60 File Offset: 0x00097060
	private void UpdateCell(int previous_cell, int current_cell)
	{
		foreach (ObjectLayer layer in this.objectLayers)
		{
			if (previous_cell != Grid.InvalidCell && previous_cell != current_cell && Grid.Objects[previous_cell, (int)layer] == base.gameObject)
			{
				Grid.Objects[previous_cell, (int)layer] = null;
			}
			GameObject gameObject = Grid.Objects[current_cell, (int)layer];
			if (gameObject == null)
			{
				Grid.Objects[current_cell, (int)layer] = base.gameObject;
			}
			else
			{
				KPrefabID component = base.GetComponent<KPrefabID>();
				KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
				if (component.InstanceID > component2.InstanceID)
				{
					Grid.Objects[current_cell, (int)layer] = base.gameObject;
				}
			}
		}
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x00098F18 File Offset: 0x00097118
	private void CleanUpOccupiedCells()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		foreach (CellOffset offset in this.cellOffsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			foreach (ObjectLayer layer in this.objectLayers)
			{
				if (Grid.Objects[cell2, (int)layer] == base.gameObject)
				{
					Grid.Objects[cell2, (int)layer] = null;
				}
			}
		}
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x00098FA8 File Offset: 0x000971A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.RefreshCellOccupy();
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x00098FB6 File Offset: 0x000971B6
	protected override void OnCleanUp()
	{
		this.CleanUpOccupiedCells();
		base.OnCleanUp();
	}

	// Token: 0x04000FC6 RID: 4038
	private int previousCell = Grid.InvalidCell;

	// Token: 0x04000FC7 RID: 4039
	public ObjectLayer[] objectLayers;

	// Token: 0x04000FC8 RID: 4040
	public CellOffset[] cellOffsets;
}
