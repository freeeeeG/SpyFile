using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006A8 RID: 1704
public class Toilet : StateMachineComponent<Toilet.StatesInstance>, ISaveLoadable, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06002E23 RID: 11811 RVA: 0x000F3C63 File Offset: 0x000F1E63
	// (set) Token: 0x06002E24 RID: 11812 RVA: 0x000F3C6B File Offset: 0x000F1E6B
	public int FlushesUsed
	{
		get
		{
			return this._flushesUsed;
		}
		set
		{
			this._flushesUsed = value;
			base.smi.sm.flushes.Set(value, base.smi, false);
		}
	}

	// Token: 0x06002E25 RID: 11813 RVA: 0x000F3C94 File Offset: 0x000F1E94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.GetComponent<ToiletWorkableUse>().trackUses = true;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		this.FlushesUsed = this._flushesUsed;
		base.Subscribe<Toilet>(493375141, Toilet.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06002E26 RID: 11814 RVA: 0x000F3D47 File Offset: 0x000F1F47
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
	}

	// Token: 0x06002E27 RID: 11815 RVA: 0x000F3D65 File Offset: 0x000F1F65
	public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

	// Token: 0x06002E28 RID: 11816 RVA: 0x000F3D78 File Offset: 0x000F1F78
	public void Flush(Worker worker)
	{
		this.FlushesUsed++;
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		float num = 0f;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
		float num2;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(tag, base.smi.DirtUsedPerFlush(), out num2, out diseaseInfo, out num);
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		float mass = base.smi.MassPerFlush() + num2;
		GameObject gameObject = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).substance.SpawnResource(base.transform.GetPosition(), mass, this.solidWasteTemperature, index, this.diseasePerFlush, true, false, false);
		gameObject.GetComponent<PrimaryElement>().AddDisease(diseaseInfo.idx, diseaseInfo.count, "Toilet.Flush");
		this.storage.Store(gameObject, false, false, true, false);
		worker.GetComponent<PrimaryElement>().AddDisease(index, this.diseaseOnDupePerFlush, "Toilet.Flush");
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, this.diseasePerFlush + this.diseaseOnDupePerFlush), base.transform, Vector3.up, 1.5f, false, false);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
	}

	// Token: 0x06002E29 RID: 11817 RVA: 0x000F3EF8 File Offset: 0x000F20F8
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.IsSoiled || base.smi.cleanChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", UI.USERMENUACTIONS.CLEANTOILET.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyclean);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x06002E2A RID: 11818 RVA: 0x000F3F8A File Offset: 0x000F218A
	private void SpawnMonster()
	{
		GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), base.smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
	}

	// Token: 0x06002E2B RID: 11819 RVA: 0x000F3FBC File Offset: 0x000F21BC
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = base.GetComponent<ManualDeliveryKG>().RequestedItemTag.ProperName();
		float mass = base.smi.DirtUsedPerFlush();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

	// Token: 0x06002E2C RID: 11820 RVA: 0x000F4040 File Offset: 0x000F2240
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).tag.ProperName();
		float mass = base.smi.MassPerFlush() + base.smi.DirtUsedPerFlush();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

	// Token: 0x06002E2D RID: 11821 RVA: 0x000F4155 File Offset: 0x000F2355
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x04001B2D RID: 6957
	[SerializeField]
	public Toilet.SpawnInfo solidWastePerUse;

	// Token: 0x04001B2E RID: 6958
	[SerializeField]
	public float solidWasteTemperature;

	// Token: 0x04001B2F RID: 6959
	[SerializeField]
	public Toilet.SpawnInfo gasWasteWhenFull;

	// Token: 0x04001B30 RID: 6960
	[SerializeField]
	public int maxFlushes = 15;

	// Token: 0x04001B31 RID: 6961
	[SerializeField]
	public string diseaseId;

	// Token: 0x04001B32 RID: 6962
	[SerializeField]
	public int diseasePerFlush;

	// Token: 0x04001B33 RID: 6963
	[SerializeField]
	public int diseaseOnDupePerFlush;

	// Token: 0x04001B34 RID: 6964
	[SerializeField]
	public float dirtUsedPerFlush = 13f;

	// Token: 0x04001B35 RID: 6965
	[Serialize]
	public int _flushesUsed;

	// Token: 0x04001B36 RID: 6966
	private MeterController meter;

	// Token: 0x04001B37 RID: 6967
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001B38 RID: 6968
	[MyCmpReq]
	private ManualDeliveryKG manualdeliverykg;

	// Token: 0x04001B39 RID: 6969
	private static readonly EventSystem.IntraObjectHandler<Toilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Toilet>(delegate(Toilet component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x020013D7 RID: 5079
	[Serializable]
	public struct SpawnInfo
	{
		// Token: 0x0600827D RID: 33405 RVA: 0x002F9AB5 File Offset: 0x002F7CB5
		public SpawnInfo(SimHashes element_id, float mass, float interval)
		{
			this.elementID = element_id;
			this.mass = mass;
			this.interval = interval;
		}

		// Token: 0x0400638E RID: 25486
		[HashedEnum]
		public SimHashes elementID;

		// Token: 0x0400638F RID: 25487
		public float mass;

		// Token: 0x04006390 RID: 25488
		public float interval;
	}

	// Token: 0x020013D8 RID: 5080
	public class StatesInstance : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.GameInstance
	{
		// Token: 0x0600827E RID: 33406 RVA: 0x002F9ACC File Offset: 0x002F7CCC
		public StatesInstance(Toilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x0600827F RID: 33407 RVA: 0x002F9AEB File Offset: 0x002F7CEB
		public bool IsSoiled
		{
			get
			{
				return base.master.FlushesUsed > 0;
			}
		}

		// Token: 0x06008280 RID: 33408 RVA: 0x002F9AFB File Offset: 0x002F7CFB
		public int GetFlushesRemaining()
		{
			return base.master.maxFlushes - base.master.FlushesUsed;
		}

		// Token: 0x06008281 RID: 33409 RVA: 0x002F9B14 File Offset: 0x002F7D14
		public bool RequiresDirtDelivery()
		{
			return base.master.storage.IsEmpty() || !base.master.storage.Has(GameTags.Dirt) || (base.master.storage.GetAmountAvailable(GameTags.Dirt) < base.master.manualdeliverykg.capacity && !this.IsSoiled);
		}

		// Token: 0x06008282 RID: 33410 RVA: 0x002F9B85 File Offset: 0x002F7D85
		public float MassPerFlush()
		{
			return base.master.solidWastePerUse.mass;
		}

		// Token: 0x06008283 RID: 33411 RVA: 0x002F9B97 File Offset: 0x002F7D97
		public float DirtUsedPerFlush()
		{
			return base.master.dirtUsedPerFlush;
		}

		// Token: 0x06008284 RID: 33412 RVA: 0x002F9BA4 File Offset: 0x002F7DA4
		public bool IsToxicSandRemoved()
		{
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			return base.master.storage.FindFirst(tag) == null;
		}

		// Token: 0x06008285 RID: 33413 RVA: 0x002F9BE0 File Offset: 0x002F7DE0
		public void CreateCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("dupe");
			}
			ToiletWorkableClean component = base.master.GetComponent<ToiletWorkableClean>();
			this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, component, null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06008286 RID: 33414 RVA: 0x002F9C48 File Offset: 0x002F7E48
		public void CancelCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}
		}

		// Token: 0x06008287 RID: 33415 RVA: 0x002F9C6C File Offset: 0x002F7E6C
		private void DropFromStorage(Tag tag)
		{
			ListPool<GameObject, Toilet>.PooledList pooledList = ListPool<GameObject, Toilet>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x06008288 RID: 33416 RVA: 0x002F9CE8 File Offset: 0x002F7EE8
		private void OnCleanComplete(Chore chore)
		{
			this.cleanChore = null;
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			Tag tag2 = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
			this.DropFromStorage(tag);
			this.DropFromStorage(tag2);
			base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
		}

		// Token: 0x06008289 RID: 33417 RVA: 0x002F9D5C File Offset: 0x002F7F5C
		public void Flush()
		{
			Worker worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

		// Token: 0x04006391 RID: 25489
		public Chore cleanChore;

		// Token: 0x04006392 RID: 25490
		public List<Chore> activeUseChores;

		// Token: 0x04006393 RID: 25491
		public float monsterSpawnTime = 1200f;
	}

	// Token: 0x020013D9 RID: 5081
	public class States : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet>
	{
		// Token: 0x0600828A RID: 33418 RVA: 0x002F9D88 File Offset: 0x002F7F88
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.needsdirt;
			this.root.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.needsdirt, (Toilet.StatesInstance smi) => smi.RequiresDirtDelivery()).EventTransition(GameHashes.OperationalChanged, this.notoperational, (Toilet.StatesInstance smi) => !smi.Get<Operational>().IsOperational);
			this.needsdirt.Enter(delegate(Toilet.StatesInstance smi)
			{
				if (smi.RequiresDirtDelivery())
				{
					smi.master.manualdeliverykg.RequestDelivery();
				}
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.ready, (Toilet.StatesInstance smi) => !smi.RequiresDirtDelivery());
			this.ready.ParamTransition<int>(this.flushes, this.full, (Toilet.StatesInstance smi, int p) => smi.GetFlushesRemaining() <= 0).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Toilet, null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateBreakUseChore), null).ToggleTag(GameTags.Usable).EventHandler(GameHashes.Flush, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.Flush();
			});
			this.earlyclean.PlayAnims((Toilet.StatesInstance smi) => Toilet.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForClean);
			this.earlyWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved());
			this.full.PlayAnims((Toilet.StatesInstance smi) => Toilet.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForClean);
			this.fullWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved()).Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.Schedule(smi.monsterSpawnTime, delegate
				{
					smi.master.SpawnMonster();
				}, null);
			});
			this.empty.PlayAnim("off").Enter("ClearFlushes", delegate(Toilet.StatesInstance smi)
			{
				smi.master.FlushesUsed = 0;
			}).GoTo(this.needsdirt);
			this.notoperational.EventTransition(GameHashes.OperationalChanged, this.needsdirt, (Toilet.StatesInstance smi) => smi.Get<Operational>().IsOperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null);
		}

		// Token: 0x0600828B RID: 33419 RVA: 0x002FA190 File Offset: 0x002F8390
		private Chore CreateUrgentUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

		// Token: 0x0600828C RID: 33420 RVA: 0x002FA1CC File Offset: 0x002F83CC
		private Chore CreateBreakUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Hygiene);
			return chore;
		}

		// Token: 0x0600828D RID: 33421 RVA: 0x002FA220 File Offset: 0x002F8420
		private Chore CreateUseChore(Toilet.StatesInstance smi, ChoreType choreType)
		{
			WorkChore<ToiletWorkableUse> workChore = new WorkChore<ToiletWorkableUse>(choreType, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
			smi.activeUseChores.Add(workChore);
			WorkChore<ToiletWorkableUse> workChore2 = workChore;
			workChore2.onExit = (Action<Chore>)Delegate.Combine(workChore2.onExit, new Action<Chore>(delegate(Chore exiting_chore)
			{
				smi.activeUseChores.Remove(exiting_chore);
			}));
			workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
			workChore.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
			return workChore;
		}

		// Token: 0x04006394 RID: 25492
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State needsdirt;

		// Token: 0x04006395 RID: 25493
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State empty;

		// Token: 0x04006396 RID: 25494
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State notoperational;

		// Token: 0x04006397 RID: 25495
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State ready;

		// Token: 0x04006398 RID: 25496
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyclean;

		// Token: 0x04006399 RID: 25497
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyWaitingForClean;

		// Token: 0x0400639A RID: 25498
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State full;

		// Token: 0x0400639B RID: 25499
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State fullWaitingForClean;

		// Token: 0x0400639C RID: 25500
		private static readonly HashedString[] FULL_ANIMS = new HashedString[]
		{
			"full_pre",
			"full"
		};

		// Token: 0x0400639D RID: 25501
		public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter flushes = new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter(0);

		// Token: 0x02002139 RID: 8505
		public class ReadyStates : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State
		{
			// Token: 0x040094F6 RID: 38134
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State idle;

			// Token: 0x040094F7 RID: 38135
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State inuse;

			// Token: 0x040094F8 RID: 38136
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State flush;
		}
	}
}
