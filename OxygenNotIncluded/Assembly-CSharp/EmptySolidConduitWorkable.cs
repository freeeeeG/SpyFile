using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200078B RID: 1931
[AddComponentMenu("KMonoBehaviour/Workable/EmptySolidConduitWorkable")]
public class EmptySolidConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x06003592 RID: 13714 RVA: 0x001225C4 File Offset: 0x001207C4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptySolidConduitWorkable>(2127324410, EmptySolidConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptySolidConduitWorkable.emptySolidConduitStatusItem == null)
		{
			EmptySolidConduitWorkable.emptySolidConduitStatusItem = new StatusItem("EmptySolidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, 32770, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x00122684 File Offset: 0x00120884
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x001226A0 File Offset: 0x001208A0
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x001226DC File Offset: 0x001208DC
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid();
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x00122713 File Offset: 0x00120913
	private void CancelEmptying()
	{
		this.CleanUpVisualization();
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel");
			this.chore = null;
			this.shouldShowSkillPerkStatusItem = false;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x00122748 File Offset: 0x00120948
	private void CleanUpVisualization()
	{
		StatusItem statusItem = this.GetStatusItem();
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.ToggleStatusItem(statusItem, false, null);
		}
		this.elapsedTime = -1f;
		if (this.chore != null)
		{
			base.GetComponent<Prioritizable>().RemoveRef();
		}
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x00122794 File Offset: 0x00120994
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x001227A2 File Offset: 0x001209A2
	private SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x001227AE File Offset: 0x001209AE
	private void OnEmptyConduitCancelled(object data)
	{
		this.CancelEmptying();
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x001227B6 File Offset: 0x001209B6
	private StatusItem GetStatusItem()
	{
		return EmptySolidConduitWorkable.emptySolidConduitStatusItem;
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x001227C0 File Offset: 0x001209C0
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptySolidConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x00122850 File Offset: 0x00120A50
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.elapsedTime == -1f)
		{
			return true;
		}
		bool result = false;
		this.elapsedTime += dt;
		if (!this.emptiedPipe)
		{
			if (this.elapsedTime > 4f)
			{
				this.EmptyContents();
				this.emptiedPipe = true;
				this.elapsedTime = 0f;
			}
		}
		else if (this.elapsedTime > 2f)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			if (this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid())
			{
				this.elapsedTime = 0f;
				this.emptiedPipe = false;
			}
			else
			{
				this.CleanUpVisualization();
				this.chore = null;
				result = true;
				this.shouldShowSkillPerkStatusItem = false;
				this.UpdateStatusItem(null);
			}
		}
		return result;
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x00122919 File Offset: 0x00120B19
	public override bool InstantlyFinish(Worker worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x00122928 File Offset: 0x00120B28
	public void EmptyContents()
	{
		int cell_idx = Grid.PosToCell(base.transform.GetPosition());
		this.GetFlowManager().RemovePickupable(cell_idx);
		this.elapsedTime = 0f;
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x0012295E File Offset: 0x00120B5E
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x040020C6 RID: 8390
	[MyCmpReq]
	private SolidConduit conduit;

	// Token: 0x040020C7 RID: 8391
	private static StatusItem emptySolidConduitStatusItem;

	// Token: 0x040020C8 RID: 8392
	private Chore chore;

	// Token: 0x040020C9 RID: 8393
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x040020CA RID: 8394
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x040020CB RID: 8395
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x040020CC RID: 8396
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x040020CD RID: 8397
	private bool emptiedPipe = true;

	// Token: 0x040020CE RID: 8398
	private static readonly EventSystem.IntraObjectHandler<EmptySolidConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptySolidConduitWorkable>(delegate(EmptySolidConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
