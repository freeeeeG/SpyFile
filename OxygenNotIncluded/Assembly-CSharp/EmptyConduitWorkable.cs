using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200078A RID: 1930
[AddComponentMenu("KMonoBehaviour/Workable/EmptyConduitWorkable")]
public class EmptyConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x06003581 RID: 13697 RVA: 0x001220D4 File Offset: 0x001202D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptyConduitWorkable>(2127324410, EmptyConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptyConduitWorkable.emptyLiquidConduitStatusItem == null)
		{
			EmptyConduitWorkable.emptyLiquidConduitStatusItem = new StatusItem("EmptyLiquidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, 66, true, null);
			EmptyConduitWorkable.emptyGasConduitStatusItem = new StatusItem("EmptyGasConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, 130, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x001221C8 File Offset: 0x001203C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x001221E4 File Offset: 0x001203E4
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x00122220 File Offset: 0x00120420
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).mass > 0f;
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x00122259 File Offset: 0x00120459
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

	// Token: 0x06003586 RID: 13702 RVA: 0x00122290 File Offset: 0x00120490
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

	// Token: 0x06003587 RID: 13703 RVA: 0x001222DC File Offset: 0x001204DC
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x001222EA File Offset: 0x001204EA
	private ConduitFlow GetFlowManager()
	{
		if (this.conduit.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x0012230F File Offset: 0x0012050F
	private void OnEmptyConduitCancelled(object data)
	{
		this.CancelEmptying();
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x00122318 File Offset: 0x00120518
	private StatusItem GetStatusItem()
	{
		ConduitType type = this.conduit.type;
		StatusItem result;
		if (type != ConduitType.Gas)
		{
			if (type != ConduitType.Liquid)
			{
				throw new ArgumentException();
			}
			result = EmptyConduitWorkable.emptyLiquidConduitStatusItem;
		}
		else
		{
			result = EmptyConduitWorkable.emptyGasConduitStatusItem;
		}
		return result;
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x00122354 File Offset: 0x00120554
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptyConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x001223E4 File Offset: 0x001205E4
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
			if (this.GetFlowManager().GetContents(cell).mass > 0f)
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

	// Token: 0x0600358D RID: 13709 RVA: 0x001224AD File Offset: 0x001206AD
	public override bool InstantlyFinish(Worker worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x001224BC File Offset: 0x001206BC
	public void EmptyContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		ConduitFlow.ConduitContents conduitContents = this.GetFlowManager().RemoveElement(cell, float.PositiveInfinity);
		this.elapsedTime = 0f;
		if (conduitContents.mass > 0f && conduitContents.element != SimHashes.Vacuum)
		{
			ConduitType type = this.conduit.type;
			IChunkManager instance;
			if (type != ConduitType.Gas)
			{
				if (type != ConduitType.Liquid)
				{
					throw new ArgumentException();
				}
				instance = LiquidSourceManager.Instance;
			}
			else
			{
				instance = GasSourceManager.Instance;
			}
			instance.CreateChunk(conduitContents.element, conduitContents.mass, conduitContents.temperature, conduitContents.diseaseIdx, conduitContents.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore)).Trigger(580035959, base.worker);
		}
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x0012257B File Offset: 0x0012077B
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x040020BC RID: 8380
	[MyCmpReq]
	private Conduit conduit;

	// Token: 0x040020BD RID: 8381
	private static StatusItem emptyLiquidConduitStatusItem;

	// Token: 0x040020BE RID: 8382
	private static StatusItem emptyGasConduitStatusItem;

	// Token: 0x040020BF RID: 8383
	private Chore chore;

	// Token: 0x040020C0 RID: 8384
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x040020C1 RID: 8385
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x040020C2 RID: 8386
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x040020C3 RID: 8387
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x040020C4 RID: 8388
	private bool emptiedPipe = true;

	// Token: 0x040020C5 RID: 8389
	private static readonly EventSystem.IntraObjectHandler<EmptyConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptyConduitWorkable>(delegate(EmptyConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
