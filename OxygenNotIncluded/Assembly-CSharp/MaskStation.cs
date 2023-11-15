using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200064C RID: 1612
public class MaskStation : StateMachineComponent<MaskStation.SMInstance>, IBasicBuilding
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06002A66 RID: 10854 RVA: 0x000E2602 File Offset: 0x000E0802
	// (set) Token: 0x06002A67 RID: 10855 RVA: 0x000E260F File Offset: 0x000E080F
	private bool isRotated
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000E2619 File Offset: 0x000E0819
	// (set) Token: 0x06002A69 RID: 10857 RVA: 0x000E2626 File Offset: 0x000E0826
	private bool isOperational
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
		}
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x000E2630 File Offset: 0x000E0830
	public void UpdateOperational()
	{
		bool flag = this.GetTotalOxygenAmount() >= this.oxygenConsumedPerMask * (float)this.maxUses;
		this.shouldPump = this.IsPumpable();
		if (this.operational.IsOperational && this.shouldPump && !flag)
		{
			this.operational.SetActive(true, false);
		}
		else
		{
			this.operational.SetActive(false, false);
		}
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidMaskStationConsumptionState, this.noElementStatusGuid, !this.shouldPump, null);
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x000E26C8 File Offset: 0x000E08C8
	private bool IsPumpable()
	{
		ElementConsumer[] components = base.GetComponents<ElementConsumer>();
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool result = false;
		foreach (ElementConsumer elementConsumer in components)
		{
			for (int j = 0; j < (int)elementConsumer.consumptionRadius; j++)
			{
				for (int k = 0; k < (int)elementConsumer.consumptionRadius; k++)
				{
					int num2 = num + k + Grid.WidthInCells * j;
					bool flag = Grid.Element[num2].IsState(Element.State.Gas);
					bool flag2 = Grid.Element[num2].id == elementConsumer.elementToConsume;
					if (flag && flag2)
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x000E276C File Offset: 0x000E096C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, null, false, fetch_chore_type);
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000E27A8 File Offset: 0x000E09A8
	private List<GameObject> GetPossibleMaterials()
	{
		List<GameObject> result = new List<GameObject>();
		this.materialStorage.Find(this.materialTag, result);
		return result;
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x000E27CF File Offset: 0x000E09CF
	private float GetTotalMaterialAmount()
	{
		return this.materialStorage.GetMassAvailable(this.materialTag);
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x000E27E2 File Offset: 0x000E09E2
	private float GetTotalOxygenAmount()
	{
		return this.oxygenStorage.GetMassAvailable(this.oxygenTag);
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x000E27F8 File Offset: 0x000E09F8
	private void RefreshMeters()
	{
		float num = this.GetTotalMaterialAmount();
		num = Mathf.Clamp01(num / ((float)this.maxUses * this.materialConsumedPerMask));
		float num2 = this.GetTotalOxygenAmount();
		num2 = Mathf.Clamp01(num2 / ((float)this.maxUses * this.oxygenConsumedPerMask));
		this.materialsMeter.SetPositionPercent(num);
		this.oxygenMeter.SetPositionPercent(num2);
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x000E2858 File Offset: 0x000E0A58
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.CreateNewReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<MaskStation>(-592767678, MaskStation.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<MaskStation>(-1643076535, MaskStation.OnRotatedDelegate);
		this.materialsMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_resources_target", "meter_resources", this.materialsMeterOffset, Grid.SceneLayer.BuildingBack, new string[]
		{
			"meter_resources_target"
		});
		this.oxygenMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_oxygen_target", "meter_oxygen", this.oxygenMeterOffset, Grid.SceneLayer.BuildingFront, new string[]
		{
			"meter_oxygen_target"
		});
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
		base.Subscribe<MaskStation>(-1697596308, MaskStation.OnStorageChangeDelegate);
		this.RefreshMeters();
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000E2964 File Offset: 0x000E0B64
	private void Update()
	{
		float a = this.GetTotalMaterialAmount() / this.materialConsumedPerMask;
		float b = this.GetTotalOxygenAmount() / this.oxygenConsumedPerMask;
		int fullLockerCount = (int)Mathf.Min(a, b);
		int emptyLockerCount = 0;
		Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x000E29B0 File Offset: 0x000E0BB0
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
		}
		base.OnCleanUp();
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x000E29FC File Offset: 0x000E0BFC
	private void OnOperationalChanged(bool isOperational)
	{
		this.isOperational = isOperational;
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000E2A05 File Offset: 0x000E0C05
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000E2A0D File Offset: 0x000E0C0D
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000E2A31 File Offset: 0x000E0C31
	private void CreateNewReactable()
	{
		this.reactable = new MaskStation.OxygenMaskReactable(this);
	}

	// Token: 0x040018C3 RID: 6339
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x040018C4 RID: 6340
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnOperationalChanged((bool)data);
	});

	// Token: 0x040018C5 RID: 6341
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x040018C6 RID: 6342
	public float materialConsumedPerMask = 1f;

	// Token: 0x040018C7 RID: 6343
	public float oxygenConsumedPerMask = 1f;

	// Token: 0x040018C8 RID: 6344
	public Tag materialTag = GameTags.Metal;

	// Token: 0x040018C9 RID: 6345
	public Tag oxygenTag = GameTags.Breathable;

	// Token: 0x040018CA RID: 6346
	public int maxUses = 10;

	// Token: 0x040018CB RID: 6347
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040018CC RID: 6348
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040018CD RID: 6349
	public Storage materialStorage;

	// Token: 0x040018CE RID: 6350
	public Storage oxygenStorage;

	// Token: 0x040018CF RID: 6351
	private bool shouldPump;

	// Token: 0x040018D0 RID: 6352
	private MaskStation.OxygenMaskReactable reactable;

	// Token: 0x040018D1 RID: 6353
	private MeterController materialsMeter;

	// Token: 0x040018D2 RID: 6354
	private MeterController oxygenMeter;

	// Token: 0x040018D3 RID: 6355
	public Meter.Offset materialsMeterOffset = Meter.Offset.Behind;

	// Token: 0x040018D4 RID: 6356
	public Meter.Offset oxygenMeterOffset;

	// Token: 0x040018D5 RID: 6357
	public string choreTypeID;

	// Token: 0x040018D6 RID: 6358
	protected FilteredStorage filteredStorage;

	// Token: 0x040018D7 RID: 6359
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x040018D8 RID: 6360
	private int cell;

	// Token: 0x040018D9 RID: 6361
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x040018DA RID: 6362
	private Guid noElementStatusGuid;

	// Token: 0x040018DB RID: 6363
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x0200131F RID: 4895
	private class OxygenMaskReactable : Reactable
	{
		// Token: 0x06007FB7 RID: 32695 RVA: 0x002EDAFC File Offset: 0x002EBCFC
		public OxygenMaskReactable(MaskStation mask_station) : base(mask_station.gameObject, "OxygenMask", Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.maskStation = mask_station;
		}

		// Token: 0x06007FB8 RID: 32696 RVA: 0x002EDB50 File Offset: 0x002EBD50
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.maskStation == null)
			{
				base.Cleanup();
				return false;
			}
			bool flag = !new_reactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			int x = transition.navGridTransition.x;
			if (x == 0)
			{
				return false;
			}
			if (!flag)
			{
				return (x >= 0 || !this.maskStation.isRotated) && (x <= 0 || this.maskStation.isRotated);
			}
			return this.maskStation.smi.IsReady() && (x <= 0 || !this.maskStation.isRotated) && (x >= 0 || this.maskStation.isRotated);
		}

		// Token: 0x06007FB9 RID: 32697 RVA: 0x002EDC20 File Offset: 0x002EBE20
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.maskStation.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.maskStation.CreateNewReactable();
		}

		// Token: 0x06007FBA RID: 32698 RVA: 0x002EDCB4 File Offset: 0x002EBEB4
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.maskStation)
			{
				facing.SetFacing(this.maskStation.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				this.Run();
				base.Cleanup();
			}
		}

		// Token: 0x06007FBB RID: 32699 RVA: 0x002EDD2C File Offset: 0x002EBF2C
		private void Run()
		{
			GameObject reactor = this.reactor;
			Equipment equipment = reactor.GetComponent<MinionIdentity>().GetEquipment();
			bool flag = !equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			Navigator component = reactor.GetComponent<Navigator>();
			bool flag2 = component != null && (component.flags & this.maskStation.PathFlag) > PathFinder.PotentialPath.Flags.None;
			if (flag)
			{
				if (!this.maskStation.smi.IsReady())
				{
					return;
				}
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Oxygen_Mask".ToTag()), null, null);
				gameObject.SetActive(true);
				SimHashes elementID = this.maskStation.GetPossibleMaterials()[0].GetComponent<PrimaryElement>().ElementID;
				gameObject.GetComponent<PrimaryElement>().SetElement(elementID, false);
				SuitTank component2 = gameObject.GetComponent<SuitTank>();
				this.maskStation.materialStorage.ConsumeIgnoringDisease(this.maskStation.materialTag, this.maskStation.materialConsumedPerMask);
				this.maskStation.oxygenStorage.Transfer(component2.storage, component2.elementTag, this.maskStation.oxygenConsumedPerMask, false, true);
				Equippable component3 = gameObject.GetComponent<Equippable>();
				component3.Assign(equipment.GetComponent<IAssignableIdentity>());
				component3.isEquipped = true;
			}
			if (!flag)
			{
				Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
				assignable.Unassign();
				if (!flag2)
				{
					Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
					assignable.GetComponent<Notifier>().Add(notification, "");
				}
			}
		}

		// Token: 0x06007FBC RID: 32700 RVA: 0x002EDED3 File Offset: 0x002EC0D3
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.maskStation.interactAnim);
			}
		}

		// Token: 0x06007FBD RID: 32701 RVA: 0x002EDEFE File Offset: 0x002EC0FE
		protected override void InternalCleanup()
		{
		}

		// Token: 0x0400617F RID: 24959
		private MaskStation maskStation;

		// Token: 0x04006180 RID: 24960
		private float startTime;
	}

	// Token: 0x02001320 RID: 4896
	public class SMInstance : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.GameInstance
	{
		// Token: 0x06007FBE RID: 32702 RVA: 0x002EDF00 File Offset: 0x002EC100
		public SMInstance(MaskStation master) : base(master)
		{
		}

		// Token: 0x06007FBF RID: 32703 RVA: 0x002EDF09 File Offset: 0x002EC109
		private bool HasSufficientMaterials()
		{
			return base.master.GetTotalMaterialAmount() >= base.master.materialConsumedPerMask;
		}

		// Token: 0x06007FC0 RID: 32704 RVA: 0x002EDF26 File Offset: 0x002EC126
		private bool HasSufficientOxygen()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask;
		}

		// Token: 0x06007FC1 RID: 32705 RVA: 0x002EDF43 File Offset: 0x002EC143
		public bool OxygenIsFull()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask * (float)base.master.maxUses;
		}

		// Token: 0x06007FC2 RID: 32706 RVA: 0x002EDF6D File Offset: 0x002EC16D
		public bool IsReady()
		{
			return this.HasSufficientMaterials() && this.HasSufficientOxygen();
		}
	}

	// Token: 0x02001321 RID: 4897
	public class States : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation>
	{
		// Token: 0x06007FC3 RID: 32707 RVA: 0x002EDF84 File Offset: 0x002EC184
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notOperational;
			this.notOperational.PlayAnim("off").TagTransition(GameTags.Operational, this.charging, false);
			this.charging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.notCharging, (MaskStation.SMInstance smi) => smi.OxygenIsFull() || !smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (smi.OxygenIsFull() || !smi.master.shouldPump)
				{
					smi.GoTo(this.notCharging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.charging.openChargingPre);
					return;
				}
				smi.GoTo(this.charging.closedChargingPre);
			});
			this.charging.opening.QueueAnim("opening_charging", false, null).OnAnimQueueComplete(this.charging.open);
			this.charging.open.PlayAnim("open_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.charging.closing.QueueAnim("closing_charging", false, null).OnAnimQueueComplete(this.charging.closed);
			this.charging.closed.PlayAnim("closed_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.charging.openChargingPre.PlayAnim("open_charging_pre").OnAnimQueueComplete(this.charging.open);
			this.charging.closedChargingPre.PlayAnim("closed_charging_pre").OnAnimQueueComplete(this.charging.closed);
			this.notCharging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.charging, (MaskStation.SMInstance smi) => !smi.OxygenIsFull() && smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (!smi.OxygenIsFull() && smi.master.shouldPump)
				{
					smi.GoTo(this.charging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.notCharging.openChargingPst);
					return;
				}
				smi.GoTo(this.notCharging.closedChargingPst);
			});
			this.notCharging.opening.PlayAnim("opening_not_charging").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.open.PlayAnim("open_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.notCharging.closing.PlayAnim("closing_not_charging").OnAnimQueueComplete(this.notCharging.closed);
			this.notCharging.closed.PlayAnim("closed_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.notCharging.openChargingPst.PlayAnim("open_charging_pst").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.closedChargingPst.PlayAnim("closed_charging_pst").OnAnimQueueComplete(this.notCharging.closed);
		}

		// Token: 0x04006181 RID: 24961
		public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State notOperational;

		// Token: 0x04006182 RID: 24962
		public MaskStation.States.ChargingStates charging;

		// Token: 0x04006183 RID: 24963
		public MaskStation.States.NotChargingStates notCharging;

		// Token: 0x020020F6 RID: 8438
		public class ChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x0400938E RID: 37774
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x0400938F RID: 37775
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x04009390 RID: 37776
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x04009391 RID: 37777
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x04009392 RID: 37778
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPre;

			// Token: 0x04009393 RID: 37779
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPre;
		}

		// Token: 0x020020F7 RID: 8439
		public class NotChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x04009394 RID: 37780
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x04009395 RID: 37781
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x04009396 RID: 37782
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x04009397 RID: 37783
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x04009398 RID: 37784
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPst;

			// Token: 0x04009399 RID: 37785
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPst;
		}
	}
}
