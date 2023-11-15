using System;
using UnityEngine;

// Token: 0x020008C6 RID: 2246
public class SocialChoreTracker
{
	// Token: 0x06004118 RID: 16664 RVA: 0x0016C8CC File Offset: 0x0016AACC
	public SocialChoreTracker(GameObject owner, CellOffset[] chore_offsets)
	{
		this.owner = owner;
		this.choreOffsets = chore_offsets;
		this.chores = new Chore[this.choreOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(owner), this.choreOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("PrintingPodSocialize", owner, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
	}

	// Token: 0x06004119 RID: 16665 RVA: 0x0016C940 File Offset: 0x0016AB40
	public void Update(bool update = true)
	{
		if (this.updating)
		{
			return;
		}
		this.updating = true;
		int num = 0;
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			CellOffset offset = this.choreOffsets[i];
			Chore chore = this.chores[i];
			if (update && num < this.choreCount && this.IsOffsetValid(offset))
			{
				num++;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = ((this.CreateChoreCB != null) ? this.CreateChoreCB(i) : null);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
		this.updating = false;
	}

	// Token: 0x0600411A RID: 16666 RVA: 0x0016C9F1 File Offset: 0x0016ABF1
	private void OnCellChanged(object data)
	{
		if (this.owner.HasTag(GameTags.Operational))
		{
			this.Update(true);
		}
	}

	// Token: 0x0600411B RID: 16667 RVA: 0x0016CA0C File Offset: 0x0016AC0C
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.Update(false);
	}

	// Token: 0x0600411C RID: 16668 RVA: 0x0016CA28 File Offset: 0x0016AC28
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this.owner), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, true);
	}

	// Token: 0x04002A60 RID: 10848
	public Func<int, Chore> CreateChoreCB;

	// Token: 0x04002A61 RID: 10849
	public int choreCount;

	// Token: 0x04002A62 RID: 10850
	private GameObject owner;

	// Token: 0x04002A63 RID: 10851
	private CellOffset[] choreOffsets;

	// Token: 0x04002A64 RID: 10852
	private Chore[] chores;

	// Token: 0x04002A65 RID: 10853
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04002A66 RID: 10854
	private bool updating;
}
