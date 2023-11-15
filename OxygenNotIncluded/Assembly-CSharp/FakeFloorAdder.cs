using System;

// Token: 0x020007BB RID: 1979
public class FakeFloorAdder : KMonoBehaviour
{
	// Token: 0x060036C0 RID: 14016 RVA: 0x001277F8 File Offset: 0x001259F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.initiallyActive)
		{
			this.SetFloor(true);
		}
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x00127810 File Offset: 0x00125A10
	public void SetFloor(bool active)
	{
		if (this.isActive == active)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		Building component = base.GetComponent<Building>();
		foreach (CellOffset offset in this.floorOffsets)
		{
			CellOffset rotatedOffset = component.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			if (active)
			{
				Grid.FakeFloor.Add(num);
			}
			else
			{
				Grid.FakeFloor.Remove(num);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
		this.isActive = active;
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x00127897 File Offset: 0x00125A97
	protected override void OnCleanUp()
	{
		this.SetFloor(false);
		base.OnCleanUp();
	}

	// Token: 0x040021A7 RID: 8615
	public CellOffset[] floorOffsets;

	// Token: 0x040021A8 RID: 8616
	public bool initiallyActive = true;

	// Token: 0x040021A9 RID: 8617
	private bool isActive;
}
