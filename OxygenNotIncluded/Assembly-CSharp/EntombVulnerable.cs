using System;
using KSerialization;
using STRINGS;

// Token: 0x0200071F RID: 1823
public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06003212 RID: 12818 RVA: 0x00109EA5 File Offset: 0x001080A5
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06003213 RID: 12819 RVA: 0x00109EC7 File Offset: 0x001080C7
	public bool GetEntombed
	{
		get
		{
			return this.isEntombed;
		}
	}

	// Token: 0x06003214 RID: 12820 RVA: 0x00109ED0 File Offset: 0x001080D0
	public void SetStatusItem(StatusItem si)
	{
		bool flag = this.showStatusItemOnEntombed;
		this.SetShowStatusItemOnEntombed(false);
		this.EntombedStatusItem = si;
		this.SetShowStatusItemOnEntombed(flag);
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x00109EFC File Offset: 0x001080FC
	public void SetShowStatusItemOnEntombed(bool val)
	{
		this.showStatusItemOnEntombed = val;
		if (this.isEntombed && this.EntombedStatusItem != null)
		{
			if (this.showStatusItemOnEntombed)
			{
				this.selectable.AddStatusItem(this.EntombedStatusItem, null);
				return;
			}
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06003216 RID: 12822 RVA: 0x00109F4F File Offset: 0x0010814F
	public string WiltStateString
	{
		get
		{
			return Db.Get().CreatureStatusItems.Entombed.resolveStringCallback(CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, base.gameObject);
		}
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06003217 RID: 12823 RVA: 0x00109F7A File Offset: 0x0010817A
	public WiltCondition.Condition[] Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Entombed
			};
		}
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x00109F88 File Offset: 0x00108188
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.EntombedStatusItem == null)
		{
			this.EntombedStatusItem = this.DefaultEntombedStatusItem;
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add("EntombVulnerable", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.CheckEntombed();
		if (this.isEntombed)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			base.Trigger(-1089732772, true);
		}
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x0010A01B File Offset: 0x0010821B
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x0010A033 File Offset: 0x00108233
	private void OnSolidChanged(object data)
	{
		this.CheckEntombed();
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x0010A03C File Offset: 0x0010823C
	private void CheckEntombed()
	{
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		if (!this.IsCellSafe(cell))
		{
			if (!this.isEntombed)
			{
				this.isEntombed = true;
				if (this.showStatusItemOnEntombed)
				{
					this.selectable.AddStatusItem(this.EntombedStatusItem, base.gameObject);
				}
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				base.Trigger(-1089732772, true);
			}
		}
		else if (this.isEntombed)
		{
			this.isEntombed = false;
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			base.Trigger(-1089732772, false);
		}
		if (this.operational != null)
		{
			this.operational.SetFlag(EntombVulnerable.notEntombedFlag, !this.isEntombed);
		}
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x0010A131 File Offset: 0x00108331
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, null, EntombVulnerable.IsCellSafeCBDelegate);
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x0010A145 File Offset: 0x00108345
	private static bool IsCellSafeCB(int cell, object data)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell];
	}

	// Token: 0x04001E04 RID: 7684
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001E05 RID: 7685
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001E06 RID: 7686
	private OccupyArea _occupyArea;

	// Token: 0x04001E07 RID: 7687
	[Serialize]
	private bool isEntombed;

	// Token: 0x04001E08 RID: 7688
	private StatusItem DefaultEntombedStatusItem = Db.Get().CreatureStatusItems.Entombed;

	// Token: 0x04001E09 RID: 7689
	[NonSerialized]
	private StatusItem EntombedStatusItem;

	// Token: 0x04001E0A RID: 7690
	private bool showStatusItemOnEntombed = true;

	// Token: 0x04001E0B RID: 7691
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x04001E0C RID: 7692
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001E0D RID: 7693
	private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (int cell, object data) => EntombVulnerable.IsCellSafeCB(cell, data);
}
