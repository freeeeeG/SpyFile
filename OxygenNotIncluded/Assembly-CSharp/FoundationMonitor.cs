using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000723 RID: 1827
[AddComponentMenu("KMonoBehaviour/scripts/FoundationMonitor")]
public class FoundationMonitor : KMonoBehaviour
{
	// Token: 0x0600322C RID: 12844 RVA: 0x0010A84C File Offset: 0x00108A4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.position = Grid.PosToCell(base.gameObject);
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("FoundationMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
			this.OnGroundChanged(null);
		}
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x0010A8F0 File Offset: 0x00108AF0
	protected override void OnCleanUp()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x0010A950 File Offset: 0x00108B50
	public bool CheckFoundationValid()
	{
		return !this.needsFoundation || this.IsSuitableFoundation(this.position);
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x0010A968 File Offset: 0x00108B68
	public bool IsSuitableFoundation(int cell)
	{
		bool flag = true;
		foreach (CellOffset offset in this.monitorCells)
		{
			if (!Grid.IsCellOffsetValid(cell, offset))
			{
				return false;
			}
			int i2 = Grid.OffsetCell(cell, offset);
			flag = Grid.Solid[i2];
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x0010A9BC File Offset: 0x00108BBC
	public void OnGroundChanged(object callbackData)
	{
		if (!this.hasFoundation && this.CheckFoundationValid())
		{
			this.hasFoundation = true;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.HasNoFoundation);
			base.Trigger(-1960061727, null);
		}
		if (this.hasFoundation && !this.CheckFoundationValid())
		{
			this.hasFoundation = false;
			base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.HasNoFoundation, false);
			base.Trigger(-1960061727, null);
		}
	}

	// Token: 0x04001E1A RID: 7706
	private int position;

	// Token: 0x04001E1B RID: 7707
	[Serialize]
	public bool needsFoundation = true;

	// Token: 0x04001E1C RID: 7708
	[Serialize]
	private bool hasFoundation = true;

	// Token: 0x04001E1D RID: 7709
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x04001E1E RID: 7710
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();
}
