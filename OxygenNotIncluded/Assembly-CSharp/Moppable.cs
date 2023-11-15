using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
[AddComponentMenu("KMonoBehaviour/Workable/Moppable")]
public class Moppable : Workable, ISim1000ms, ISim200ms
{
	// Token: 0x06001C68 RID: 7272 RVA: 0x000980F0 File Offset: 0x000962F0
	private Moppable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x0009814C File Offset: 0x0009634C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x000981D0 File Offset: 0x000963D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.IsThereLiquid())
		{
			base.gameObject.DeleteObject();
			return;
		}
		Grid.Objects[Grid.PosToCell(base.gameObject), 8] = base.gameObject;
		new WorkChore<Moppable>(Db.Get().ChoreTypes.Mop, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForMop, null);
		base.Subscribe<Moppable>(493375141, Moppable.OnRefreshUserMenuDelegate);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_mop_dirtywater_kanim")
		};
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Moppable.OnSpawn", base.gameObject, new Extents(Grid.PosToCell(this), new CellOffset[]
		{
			new CellOffset(0, 0)
		}), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		this.Refresh();
		base.Subscribe<Moppable>(-1432940121, Moppable.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(this).StartSM();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x0009831C File Offset: 0x0009651C
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELMOP.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELMOP.TOOLTIP, true), 1f);
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x00098376 File Offset: 0x00096576
	private void OnCancel()
	{
		DetailsScreen.Instance.Show(false);
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x00098394 File Offset: 0x00096594
	protected override void OnStartWork(Worker worker)
	{
		SimAndRenderScheduler.instance.Add(this, false);
		this.Refresh();
		this.MopTick(this.amountMoppedPerTick);
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x000983B4 File Offset: 0x000965B4
	protected override void OnStopWork(Worker worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x000983C1 File Offset: 0x000965C1
	protected override void OnCompleteWork(Worker worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x000983CE File Offset: 0x000965CE
	public override bool InstantlyFinish(Worker worker)
	{
		this.MopTick(1000f);
		return true;
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x000983DC File Offset: 0x000965DC
	public void Sim1000ms(float dt)
	{
		if (this.amountMopped > 0f)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, GameUtil.GetFormattedMass(-this.amountMopped, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), base.transform, 1.5f, false);
			this.amountMopped = 0f;
		}
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x00098436 File Offset: 0x00096636
	public void Sim200ms(float dt)
	{
		if (base.worker != null)
		{
			this.Refresh();
			this.MopTick(this.amountMoppedPerTick);
		}
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x00098458 File Offset: 0x00096658
	private void OnCellMopped(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		if (this == null)
		{
			return;
		}
		if (mass_cb_info.mass > 0f)
		{
			this.amountMopped += mass_cb_info.mass;
			int cell = Grid.PosToCell(this);
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(ElementLoader.elements[(int)mass_cb_info.elemIdx], mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
			substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
		}
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x00098510 File Offset: 0x00096710
	public static void MopCell(int cell, float amount, Action<Sim.MassConsumedCallback, object> cb)
	{
		if (Grid.Element[cell].IsLiquid)
		{
			int callbackIdx = -1;
			if (cb != null)
			{
				callbackIdx = Game.Instance.massConsumedCallbackManager.Add(cb, null, "Moppable").index;
			}
			SimMessages.ConsumeMass(cell, Grid.Element[cell].id, amount, 1, callbackIdx);
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x00098564 File Offset: 0x00096764
	private void MopTick(float mopAmount)
	{
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid)
			{
				Moppable.MopCell(num, mopAmount, new Action<Sim.MassConsumedCallback, object>(this.OnCellMopped));
			}
		}
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000985C0 File Offset: 0x000967C0
	private bool IsThereLiquid()
	{
		int cell = Grid.PosToCell(this);
		bool result = false;
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid && Grid.Mass[num] <= MopTool.maxMopAmt)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x00098620 File Offset: 0x00096820
	private void Refresh()
	{
		if (!this.IsThereLiquid())
		{
			if (!this.destroyHandle.IsValid)
			{
				this.destroyHandle = GameScheduler.Instance.Schedule("DestroyMoppable", 1f, delegate(object moppable)
				{
					this.TryDestroy();
				}, this, null);
				return;
			}
		}
		else if (this.destroyHandle.IsValid)
		{
			this.destroyHandle.ClearScheduler();
		}
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x00098683 File Offset: 0x00096883
	private void OnLiquidChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x0009868B File Offset: 0x0009688B
	private void TryDestroy()
	{
		if (this != null)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x000986A1 File Offset: 0x000968A1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000986BC File Offset: 0x000968BC
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			bool flag = (bool)data;
			if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
			{
				return;
			}
			KSelectable component = base.GetComponent<KSelectable>();
			if (flag)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, false);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, this);
			GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
			}, null, null);
			material.color = Game.Instance.uiColours.Dig.unreachable;
		}
	}

	// Token: 0x04000FB4 RID: 4020
	[MyCmpReq]
	private KSelectable Selectable;

	// Token: 0x04000FB5 RID: 4021
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04000FB6 RID: 4022
	public float amountMoppedPerTick = 1000f;

	// Token: 0x04000FB7 RID: 4023
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000FB8 RID: 4024
	private SchedulerHandle destroyHandle;

	// Token: 0x04000FB9 RID: 4025
	private float amountMopped;

	// Token: 0x04000FBA RID: 4026
	private MeshRenderer childRenderer;

	// Token: 0x04000FBB RID: 4027
	private CellOffset[] offsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04000FBC RID: 4028
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04000FBD RID: 4029
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnReachableChanged(data);
	});
}
