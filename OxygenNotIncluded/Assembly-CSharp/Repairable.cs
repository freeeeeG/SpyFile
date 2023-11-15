using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Repairable")]
public class Repairable : Workable
{
	// Token: 0x06001DB4 RID: 7604 RVA: 0x0009DF5C File Offset: 0x0009C15C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		base.Subscribe<Repairable>(493375141, Repairable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x0009E00C File Offset: 0x0009C20C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Repairable.SMInstance(this);
		this.smi.StartSM();
		this.workTime = float.PositiveInfinity;
		this.workTimeRemaining = float.PositiveInfinity;
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x0009E041 File Offset: 0x0009C241
	private void OnProxyStorageChanged(object data)
	{
		base.Trigger(-1697596308, data);
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x0009E04F File Offset: 0x0009C24F
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x0009E05E File Offset: 0x0009C25E
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("Destroy Repairable");
		}
		base.OnCleanUp();
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x0009E080 File Offset: 0x0009C280
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject != null && this.smi != null)
		{
			if (this.smi.GetCurrentState() == this.smi.sm.forbidden)
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.NAME, new System.Action(this.AllowRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.NAME, new System.Action(this.CancelRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
		}
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x0009E164 File Offset: 0x0009C364
	private void AllowRepair()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.hp.Repair(this.hp.MaxHitPoints);
			this.OnCompleteWork(null);
		}
		this.smi.sm.allow.Trigger(this.smi);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x0009E1B7 File Offset: 0x0009C3B7
	public void CancelRepair()
	{
		if (this.smi != null)
		{
			this.smi.sm.forbid.Trigger(this.smi);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x0009E1E4 File Offset: 0x0009C3E4
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, false);
		}
		this.smi.sm.worker.Set(worker, this.smi);
		this.timeSpentRepairing = 0f;
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x0009E23C File Offset: 0x0009C43C
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		float num = Mathf.Sqrt(base.GetComponent<PrimaryElement>().Mass);
		float num2 = ((this.expectedRepairTime < 0f) ? num : this.expectedRepairTime) * 0.1f;
		if (this.timeSpentRepairing >= num2)
		{
			this.timeSpentRepairing -= num2;
			int num3 = 0;
			if (worker != null)
			{
				num3 = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
			}
			int repair_amount = Mathf.CeilToInt((float)(10 + Math.Max(0, num3 * 10)) * 0.1f);
			this.hp.Repair(repair_amount);
			if (this.hp.HitPoints >= this.hp.MaxHitPoints)
			{
				return true;
			}
		}
		this.timeSpentRepairing += dt;
		return false;
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x0009E304 File Offset: 0x0009C504
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x0009E334 File Offset: 0x0009C534
	protected override void OnCompleteWork(Worker worker)
	{
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x0009E360 File Offset: 0x0009C560
	public void CreateStorageProxy()
	{
		if (this.storageProxy == null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(RepairableStorageProxy.ID), base.transform.gameObject, null);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			this.storageProxy = gameObject.GetComponent<Storage>();
			this.storageProxy.prioritizable = base.transform.GetComponent<Prioritizable>();
			this.storageProxy.prioritizable.AddRef();
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x0009E3E8 File Offset: 0x0009C5E8
	[OnSerializing]
	private void OnSerializing()
	{
		this.storedData = null;
		if (this.storageProxy != null && !this.storageProxy.IsEmpty())
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					this.storageProxy.Serialize(binaryWriter);
				}
				this.storedData = memoryStream.ToArray();
			}
		}
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x0009E470 File Offset: 0x0009C670
	[OnSerialized]
	private void OnSerialized()
	{
		this.storedData = null;
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x0009E47C File Offset: 0x0009C67C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.storedData != null)
		{
			FastReader reader = new FastReader(this.storedData);
			this.CreateStorageProxy();
			this.storageProxy.Deserialize(reader);
			this.storedData = null;
		}
	}

	// Token: 0x04001087 RID: 4231
	public float expectedRepairTime = -1f;

	// Token: 0x04001088 RID: 4232
	[MyCmpGet]
	private BuildingHP hp;

	// Token: 0x04001089 RID: 4233
	private Repairable.SMInstance smi;

	// Token: 0x0400108A RID: 4234
	private Storage storageProxy;

	// Token: 0x0400108B RID: 4235
	[Serialize]
	private byte[] storedData;

	// Token: 0x0400108C RID: 4236
	private float timeSpentRepairing;

	// Token: 0x0400108D RID: 4237
	private static readonly Operational.Flag repairedFlag = new Operational.Flag("repaired", Operational.Flag.Type.Functional);

	// Token: 0x0400108E RID: 4238
	private static readonly EventSystem.IntraObjectHandler<Repairable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Repairable>(delegate(Repairable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x02001196 RID: 4502
	public class SMInstance : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.GameInstance
	{
		// Token: 0x06007A0F RID: 31247 RVA: 0x002DAB7B File Offset: 0x002D8D7B
		public SMInstance(Repairable smi) : base(smi)
		{
		}

		// Token: 0x06007A10 RID: 31248 RVA: 0x002DAB84 File Offset: 0x002D8D84
		public bool HasRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			return primaryElement != null && primaryElement.Mass >= num;
		}

		// Token: 0x06007A11 RID: 31249 RVA: 0x002DABD8 File Offset: 0x002D8DD8
		public KeyValuePair<Tag, float> GetRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float value = (primaryElement != null) ? Math.Max(0f, num - primaryElement.Mass) : num;
			return new KeyValuePair<Tag, float>(component.Element.tag, value);
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x002DAC45 File Offset: 0x002D8E45
		public void ConsumeRepairMaterials()
		{
			base.smi.master.storageProxy.ConsumeAllIgnoringDisease();
		}

		// Token: 0x06007A13 RID: 31251 RVA: 0x002DAC5C File Offset: 0x002D8E5C
		public void DestroyStorageProxy()
		{
			if (base.smi.master.storageProxy != null)
			{
				base.smi.master.transform.GetComponent<Prioritizable>().RemoveRef();
				List<GameObject> list = new List<GameObject>();
				base.smi.master.storageProxy.DropAll(false, false, default(Vector3), true, list);
				GameObject gameObject = base.smi.sm.worker.Get(base.smi);
				if (gameObject != null)
				{
					foreach (GameObject go in list)
					{
						go.Trigger(580035959, gameObject.GetComponent<Worker>());
					}
				}
				base.smi.sm.worker.Set(null, base.smi);
				Util.KDestroyGameObject(base.smi.master.storageProxy.gameObject);
			}
		}

		// Token: 0x06007A14 RID: 31252 RVA: 0x002DAD6C File Offset: 0x002D8F6C
		public bool NeedsRepairs()
		{
			return base.smi.master.GetComponent<BuildingHP>().NeedsRepairs;
		}

		// Token: 0x04005CB6 RID: 23734
		private const float REQUIRED_MASS_SCALE = 0.1f;
	}

	// Token: 0x02001197 RID: 4503
	public class States : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable>
	{
		// Token: 0x06007A15 RID: 31253 RVA: 0x002DAD84 File Offset: 0x002D8F84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.repaired;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.forbidden.OnSignal(this.allow, this.repaired);
			this.allowed.Enter(delegate(Repairable.SMInstance smi)
			{
				smi.master.CreateStorageProxy();
			}).DefaultState(this.allowed.needMass).EventHandler(GameHashes.BuildingFullyRepaired, delegate(Repairable.SMInstance smi)
			{
				smi.ConsumeRepairMaterials();
			}).EventTransition(GameHashes.BuildingFullyRepaired, this.repaired, null).OnSignal(this.forbid, this.forbidden).Exit(delegate(Repairable.SMInstance smi)
			{
				smi.DestroyStorageProxy();
			});
			this.allowed.needMass.Enter(delegate(Repairable.SMInstance smi)
			{
				Prioritizable.AddRef(smi.master.storageProxy.transform.parent.gameObject);
			}).Exit(delegate(Repairable.SMInstance smi)
			{
				if (!smi.isMasterNull && smi.master.storageProxy != null)
				{
					Prioritizable.RemoveRef(smi.master.storageProxy.transform.parent.gameObject);
				}
			}).EventTransition(GameHashes.OnStorageChange, this.allowed.repairable, (Repairable.SMInstance smi) => smi.HasRequiredMass()).ToggleChore(new Func<Repairable.SMInstance, Chore>(this.CreateFetchChore), this.allowed.repairable, this.allowed.needMass).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForRepairMaterials, (Repairable.SMInstance smi) => smi.GetRequiredMass());
			this.allowed.repairable.ToggleRecurringChore(new Func<Repairable.SMInstance, Chore>(this.CreateRepairChore), null).ToggleStatusItem(Db.Get().BuildingStatusItems.PendingRepair, null);
			this.repaired.EventTransition(GameHashes.BuildingReceivedDamage, this.allowed, (Repairable.SMInstance smi) => smi.NeedsRepairs()).OnSignal(this.allow, this.allowed).OnSignal(this.forbid, this.forbidden);
		}

		// Token: 0x06007A16 RID: 31254 RVA: 0x002DAFD0 File Offset: 0x002D91D0
		private Chore CreateFetchChore(Repairable.SMInstance smi)
		{
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			PrimaryElement primaryElement = smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float amount = component.Mass * 0.1f - ((primaryElement != null) ? primaryElement.Mass : 0f);
			HashSet<Tag> tags = new HashSet<Tag>
			{
				GameTagExtensions.Create(component.ElementID)
			};
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storageProxy, amount, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x06007A17 RID: 31255 RVA: 0x002DB06C File Offset: 0x002D926C
		private Chore CreateRepairChore(Repairable.SMInstance smi)
		{
			WorkChore<Repairable> workChore = new WorkChore<Repairable>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			Deconstructable component = smi.master.GetComponent<Deconstructable>();
			if (component != null)
			{
				workChore.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
			}
			Breakable component2 = smi.master.GetComponent<Breakable>();
			if (component2 != null)
			{
				workChore.AddPrecondition(Repairable.States.IsNotBeingAttacked, component2);
			}
			workChore.AddPrecondition(Repairable.States.IsNotAngry, null);
			return workChore;
		}

		// Token: 0x04005CB7 RID: 23735
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal allow;

		// Token: 0x04005CB8 RID: 23736
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal forbid;

		// Token: 0x04005CB9 RID: 23737
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State forbidden;

		// Token: 0x04005CBA RID: 23738
		public Repairable.States.AllowedState allowed;

		// Token: 0x04005CBB RID: 23739
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repaired;

		// Token: 0x04005CBC RID: 23740
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.TargetParameter worker;

		// Token: 0x04005CBD RID: 23741
		public static readonly Chore.Precondition IsNotBeingAttacked = new Chore.Precondition
		{
			id = "IsNotBeingAttacked",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				bool result = true;
				if (data != null)
				{
					result = (((Breakable)data).worker == null);
				}
				return result;
			}
		};

		// Token: 0x04005CBE RID: 23742
		public static readonly Chore.Precondition IsNotAngry = new Chore.Precondition
		{
			id = "IsNotAngry",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_ANGRY,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Traits traits = context.consumerState.traits;
				AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject);
				return !(traits != null) || amountInstance == null || amountInstance.value < STRESS.ACTING_OUT_RESET || !traits.HasTrait("Aggressive");
			}
		};

		// Token: 0x0200209C RID: 8348
		public class AllowedState : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State
		{
			// Token: 0x040091B1 RID: 37297
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State needMass;

			// Token: 0x040091B2 RID: 37298
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repairable;
		}
	}
}
