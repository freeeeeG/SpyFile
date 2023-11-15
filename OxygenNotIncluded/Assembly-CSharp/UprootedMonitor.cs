using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200073D RID: 1853
[AddComponentMenu("KMonoBehaviour/scripts/UprootedMonitor")]
public class UprootedMonitor : KMonoBehaviour
{
	// Token: 0x1700039E RID: 926
	// (get) Token: 0x060032F1 RID: 13041 RVA: 0x0010E3CF File Offset: 0x0010C5CF
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x0010E3EC File Offset: 0x0010C5EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UprootedMonitor>(-216549700, UprootedMonitor.OnUprootedDelegate);
		this.position = Grid.PosToCell(base.gameObject);
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
			this.OnGroundChanged(null);
		}
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x0010E4A0 File Offset: 0x0010C6A0
	protected override void OnCleanUp()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		base.OnCleanUp();
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x0010E500 File Offset: 0x0010C700
	public bool CheckTileGrowable()
	{
		return !this.canBeUprooted || (!this.uprooted && this.IsSuitableFoundation(this.position));
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x0010E528 File Offset: 0x0010C728
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

	// Token: 0x060032F6 RID: 13046 RVA: 0x0010E579 File Offset: 0x0010C779
	public void OnGroundChanged(object callbackData)
	{
		if (!this.CheckTileGrowable())
		{
			this.uprooted = true;
		}
		if (this.uprooted)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			base.Trigger(-216549700, null);
		}
	}

	// Token: 0x04001E92 RID: 7826
	private int position;

	// Token: 0x04001E93 RID: 7827
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x04001E94 RID: 7828
	[Serialize]
	private bool uprooted;

	// Token: 0x04001E95 RID: 7829
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x04001E96 RID: 7830
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();

	// Token: 0x04001E97 RID: 7831
	private static readonly EventSystem.IntraObjectHandler<UprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<UprootedMonitor>(delegate(UprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
