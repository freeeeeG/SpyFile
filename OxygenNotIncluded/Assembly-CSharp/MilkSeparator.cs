using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000654 RID: 1620
public class MilkSeparator : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>
{
	// Token: 0x06002AB2 RID: 10930 RVA: 0x000E39B8 File Offset: 0x000E1BB8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.noOperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.RefreshMeters));
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).PlayAnim("on").DefaultState(this.operational.idle);
		this.operational.idle.EventTransition(GameHashes.OnStorageChange, this.operational.working.pre, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanBeginSeparate)).EnterTransition(this.operational.full, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.RequiresEmptying));
		this.operational.working.pre.QueueAnim("separating_pre", false, null).OnAnimQueueComplete(this.operational.working.work);
		this.operational.working.work.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.BeginSeparation)).PlayAnim("separating_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.operational.working.post, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanNOTKeepSeparating)).Exit(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.EndSeparation));
		this.operational.working.post.QueueAnim("separating_pst", false, null).OnAnimQueueComplete(this.operational.idle);
		this.operational.full.PlayAnim("ready").ToggleRecurringChore(new Func<MilkSeparator.Instance, Chore>(MilkSeparator.CreateEmptyChore), null).WorkableCompleteTransition((MilkSeparator.Instance smi) => smi.workable, this.operational.emptyComplete).ToggleStatusItem(Db.Get().BuildingStatusItems.MilkSeparatorNeedsEmptying, null);
		this.operational.emptyComplete.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.DropMilkFat)).ScheduleActionNextFrame("AfterMilkFatDrop", delegate(MilkSeparator.Instance smi)
		{
			smi.GoTo(this.operational.idle);
		});
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000E3BF5 File Offset: 0x000E1DF5
	public static void BeginSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000E3C04 File Offset: 0x000E1E04
	public static void EndSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000E3C13 File Offset: 0x000E1E13
	public static bool CanBeginSeparate(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000E3C2B File Offset: 0x000E1E2B
	public static bool CanKeepSeparating(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.CanConvertAtAll();
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x000E3C42 File Offset: 0x000E1E42
	public static bool CanNOTKeepSeparating(MilkSeparator.Instance smi)
	{
		return !MilkSeparator.CanKeepSeparating(smi);
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000E3C4D File Offset: 0x000E1E4D
	public static bool RequiresEmptying(MilkSeparator.Instance smi)
	{
		return smi.MilkFatLimitReached;
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x000E3C55 File Offset: 0x000E1E55
	public static bool ThereIsCapacityForMilkFat(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached;
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x000E3C60 File Offset: 0x000E1E60
	public static void DropMilkFat(MilkSeparator.Instance smi)
	{
		smi.DropMilkFat();
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000E3C68 File Offset: 0x000E1E68
	public static void RefreshMeters(MilkSeparator.Instance smi)
	{
		smi.RefreshMeters();
	}

	// Token: 0x06002ABC RID: 10940 RVA: 0x000E3C70 File Offset: 0x000E1E70
	private static Chore CreateEmptyChore(MilkSeparator.Instance smi)
	{
		return new WorkChore<EmptyMilkSeparatorWorkable>(Db.Get().ChoreTypes.EmptyStorage, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x040018EE RID: 6382
	public const string WORK_PRE_ANIM_NAME = "separating_pre";

	// Token: 0x040018EF RID: 6383
	public const string WORK_ANIM_NAME = "separating_loop";

	// Token: 0x040018F0 RID: 6384
	public const string WORK_POST_ANIM_NAME = "separating_pst";

	// Token: 0x040018F1 RID: 6385
	public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State noOperational;

	// Token: 0x040018F2 RID: 6386
	public MilkSeparator.OperationalStates operational;

	// Token: 0x0200132E RID: 4910
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600801A RID: 32794 RVA: 0x002EFB5E File Offset: 0x002EDD5E
		public Def()
		{
			this.MILK_FAT_TAG = ElementLoader.FindElementByHash(SimHashes.MilkFat).tag;
			this.MILK_TAG = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		}

		// Token: 0x040061B3 RID: 25011
		public float MILK_FAT_CAPACITY = 100f;

		// Token: 0x040061B4 RID: 25012
		public Tag MILK_TAG;

		// Token: 0x040061B5 RID: 25013
		public Tag MILK_FAT_TAG;
	}

	// Token: 0x0200132F RID: 4911
	public class WorkingStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x040061B6 RID: 25014
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State pre;

		// Token: 0x040061B7 RID: 25015
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State work;

		// Token: 0x040061B8 RID: 25016
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State post;
	}

	// Token: 0x02001330 RID: 4912
	public class OperationalStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x040061B9 RID: 25017
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State idle;

		// Token: 0x040061BA RID: 25018
		public MilkSeparator.WorkingStates working;

		// Token: 0x040061BB RID: 25019
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State full;

		// Token: 0x040061BC RID: 25020
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State emptyComplete;
	}

	// Token: 0x02001331 RID: 4913
	public new class Instance : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.GameInstance
	{
		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x0600801D RID: 32797 RVA: 0x002EFBAB File Offset: 0x002EDDAB
		public float MilkFatStored
		{
			get
			{
				return this.storage.GetAmountAvailable(base.def.MILK_FAT_TAG);
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x0600801E RID: 32798 RVA: 0x002EFBC3 File Offset: 0x002EDDC3
		public float MilkFatStoragePercentage
		{
			get
			{
				return Mathf.Clamp(this.MilkFatStored / base.def.MILK_FAT_CAPACITY, 0f, 1f);
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x0600801F RID: 32799 RVA: 0x002EFBE6 File Offset: 0x002EDDE6
		public bool MilkFatLimitReached
		{
			get
			{
				return this.MilkFatStored >= base.def.MILK_FAT_CAPACITY;
			}
		}

		// Token: 0x06008020 RID: 32800 RVA: 0x002EFC00 File Offset: 0x002EDE00
		public Instance(IStateMachineTarget master, MilkSeparator.Def def) : base(master, def)
		{
			KAnimControllerBase component = base.GetComponent<KBatchedAnimController>();
			this.fatMeter = new MeterController(component, "meter_target_1", "meter_fat", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_1"
			});
		}

		// Token: 0x06008021 RID: 32801 RVA: 0x002EFC43 File Offset: 0x002EDE43
		public override void StartSM()
		{
			base.StartSM();
			this.workable.OnWork_PST_Begins = new System.Action(this.Play_Empty_MeterAnimation);
			this.RefreshMeters();
		}

		// Token: 0x06008022 RID: 32802 RVA: 0x002EFC68 File Offset: 0x002EDE68
		private void Play_Empty_MeterAnimation()
		{
			this.fatMeter.SetPositionPercent(0f);
			this.fatMeter.meterController.Play("meter_fat_empty", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x002EFCA0 File Offset: 0x002EDEA0
		public void DropMilkFat()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Drop(base.def.MILK_FAT_TAG, list);
			Vector3 dropSpawnLocation = this.GetDropSpawnLocation();
			foreach (GameObject gameObject in list)
			{
				gameObject.transform.position = dropSpawnLocation;
			}
		}

		// Token: 0x06008024 RID: 32804 RVA: 0x002EFD18 File Offset: 0x002EDF18
		private Vector3 GetDropSpawnLocation()
		{
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(new HashedString("milkfat"), out flag).GetColumn(3);
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				return vector;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x06008025 RID: 32805 RVA: 0x002EFD84 File Offset: 0x002EDF84
		public void RefreshMeters()
		{
			if (this.fatMeter.meterController.currentAnim != "meter_fat")
			{
				this.fatMeter.meterController.Play("meter_fat", KAnim.PlayMode.Paused, 1f, 0f);
			}
			this.fatMeter.SetPositionPercent(this.MilkFatStoragePercentage);
		}

		// Token: 0x040061BD RID: 25021
		[MyCmpGet]
		public EmptyMilkSeparatorWorkable workable;

		// Token: 0x040061BE RID: 25022
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040061BF RID: 25023
		[MyCmpGet]
		public ElementConverter elementConverter;

		// Token: 0x040061C0 RID: 25024
		[MyCmpGet]
		private Storage storage;

		// Token: 0x040061C1 RID: 25025
		private MeterController fatMeter;
	}
}
