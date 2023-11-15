using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A10 RID: 2576
[AddComponentMenu("KMonoBehaviour/Workable/Uprootable")]
public class Uprootable : Workable
{
	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06004D1D RID: 19741 RVA: 0x001B0D71 File Offset: 0x001AEF71
	public bool IsMarkedForUproot
	{
		get
		{
			return this.isMarkedForUproot;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x06004D1E RID: 19742 RVA: 0x001B0D79 File Offset: 0x001AEF79
	public Storage GetPlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
	}

	// Token: 0x06004D1F RID: 19743 RVA: 0x001B0D84 File Offset: 0x001AEF84
	protected Uprootable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.buttonLabel = UI.USERMENUACTIONS.UPROOT.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.UPROOT.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELUPROOT.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELUPROOT.TOOLTIP;
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
	}

	// Token: 0x06004D20 RID: 19744 RVA: 0x001B0E24 File Offset: 0x001AF024
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
		base.Subscribe<Uprootable>(1309017699, Uprootable.OnPlanterStorageDelegate);
	}

	// Token: 0x06004D21 RID: 19745 RVA: 0x001B0ED8 File Offset: 0x001AF0D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Uprootable>(2127324410, Uprootable.ForceCancelUprootDelegate);
		base.SetWorkTime(12.5f);
		base.Subscribe<Uprootable>(2127324410, Uprootable.OnCancelDelegate);
		base.Subscribe<Uprootable>(493375141, Uprootable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Components.Uprootables.Add(this);
		this.area = base.GetComponent<OccupyArea>();
		Prioritizable.AddRef(base.gameObject);
		base.gameObject.AddTag(GameTags.Plant);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForUproot)
		{
			this.MarkForUproot(true);
		}
	}

	// Token: 0x06004D22 RID: 19746 RVA: 0x001B0FC8 File Offset: 0x001AF1C8
	private void OnPlanterStorage(object data)
	{
		this.planterStorage = (Storage)data;
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.showIcon = (this.planterStorage == null);
		}
	}

	// Token: 0x06004D23 RID: 19747 RVA: 0x001B1003 File Offset: 0x001AF203
	public bool IsInPlanterBox()
	{
		return this.planterStorage != null;
	}

	// Token: 0x06004D24 RID: 19748 RVA: 0x001B1014 File Offset: 0x001AF214
	public void Uproot()
	{
		this.isMarkedForUproot = false;
		this.chore = null;
		this.uprootComplete = true;
		base.Trigger(-216549700, this);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06004D25 RID: 19749 RVA: 0x001B108F File Offset: 0x001AF28F
	public void SetCanBeUprooted(bool state)
	{
		this.canBeUprooted = state;
		if (this.canBeUprooted)
		{
			this.SetUprootedComplete(false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06004D26 RID: 19750 RVA: 0x001B10BC File Offset: 0x001AF2BC
	public void SetUprootedComplete(bool state)
	{
		this.uprootComplete = state;
	}

	// Token: 0x06004D27 RID: 19751 RVA: 0x001B10C8 File Offset: 0x001AF2C8
	public void MarkForUproot(bool instantOnDebug = true)
	{
		if (!this.canBeUprooted)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Uproot();
		}
		else if (this.chore == null)
		{
			this.chore = new WorkChore<Uprootable>(Db.Get().ChoreTypes.Uproot, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
		this.isMarkedForUproot = true;
	}

	// Token: 0x06004D28 RID: 19752 RVA: 0x001B113D File Offset: 0x001AF33D
	protected override void OnCompleteWork(Worker worker)
	{
		this.Uproot();
	}

	// Token: 0x06004D29 RID: 19753 RVA: 0x001B1148 File Offset: 0x001AF348
	private void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		}
		this.isMarkedForUproot = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06004D2A RID: 19754 RVA: 0x001B11AC File Offset: 0x001AF3AC
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x06004D2B RID: 19755 RVA: 0x001B11B9 File Offset: 0x001AF3B9
	private void OnClickUproot()
	{
		this.MarkForUproot(true);
	}

	// Token: 0x06004D2C RID: 19756 RVA: 0x001B11C2 File Offset: 0x001AF3C2
	protected void OnClickCancelUproot()
	{
		this.OnCancel(null);
	}

	// Token: 0x06004D2D RID: 19757 RVA: 0x001B11CB File Offset: 0x001AF3CB
	public virtual void ForceCancelUproot(object data = null)
	{
		this.OnCancel(null);
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x001B11D4 File Offset: 0x001AF3D4
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		if (this.uprootComplete)
		{
			if (this.deselectOnUproot)
			{
				KSelectable component = base.GetComponent<KSelectable>();
				if (component != null && SelectTool.Instance.selected == component)
				{
					SelectTool.Instance.Select(null, false);
				}
			}
			return;
		}
		if (!this.canBeUprooted)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_uproot", this.cancelButtonLabel, new System.Action(this.OnClickCancelUproot), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_uproot", this.buttonLabel, new System.Action(this.OnClickUproot), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x001B12AE File Offset: 0x001AF4AE
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Components.Uprootables.Remove(this);
	}

	// Token: 0x06004D30 RID: 19760 RVA: 0x001B12D1 File Offset: 0x001AF4D1
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
	}

	// Token: 0x04003248 RID: 12872
	[Serialize]
	protected bool isMarkedForUproot;

	// Token: 0x04003249 RID: 12873
	protected bool uprootComplete;

	// Token: 0x0400324A RID: 12874
	[MyCmpReq]
	private Prioritizable prioritizable;

	// Token: 0x0400324B RID: 12875
	[Serialize]
	protected bool canBeUprooted = true;

	// Token: 0x0400324C RID: 12876
	public bool deselectOnUproot = true;

	// Token: 0x0400324D RID: 12877
	protected Chore chore;

	// Token: 0x0400324E RID: 12878
	private string buttonLabel;

	// Token: 0x0400324F RID: 12879
	private string buttonTooltip;

	// Token: 0x04003250 RID: 12880
	private string cancelButtonLabel;

	// Token: 0x04003251 RID: 12881
	private string cancelButtonTooltip;

	// Token: 0x04003252 RID: 12882
	private StatusItem pendingStatusItem;

	// Token: 0x04003253 RID: 12883
	public OccupyArea area;

	// Token: 0x04003254 RID: 12884
	private Storage planterStorage;

	// Token: 0x04003255 RID: 12885
	public bool showUserMenuButtons = true;

	// Token: 0x04003256 RID: 12886
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003257 RID: 12887
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnPlanterStorageDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnPlanterStorage(data);
	});

	// Token: 0x04003258 RID: 12888
	private static readonly EventSystem.IntraObjectHandler<Uprootable> ForceCancelUprootDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.ForceCancelUproot(data);
	});

	// Token: 0x04003259 RID: 12889
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x0400325A RID: 12890
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
