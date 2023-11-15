using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000653 RID: 1619
public class MilkFeeder : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>
{
	// Token: 0x06002AAA RID: 10922 RVA: 0x000E35FC File Offset: 0x000E17FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Enter(delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		}).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		});
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (MilkFeeder.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).EventTransition(GameHashes.OperationalChanged, this.on.pst, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() != this.on.pre).EventTransition(GameHashes.OperationalChanged, this.off, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() == this.on.pre);
		this.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
		this.on.working.PlayAnim("on").DefaultState(this.on.working.empty);
		this.on.working.empty.PlayAnim("empty").EnterTransition(this.on.working.refilling, (MilkFeeder.Instance smi) => smi.HasEnoughMilkForOneFeeding()).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			if (smi.HasEnoughMilkForOneFeeding())
			{
				smi.GoTo(this.on.working.refilling);
			}
		});
		this.on.working.refilling.PlayAnim("fill").OnAnimQueueComplete(this.on.working.full);
		this.on.working.full.PlayAnim("full").Enter(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(true, smi, false);
		}).Exit(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(false, smi, false);
		}).ParamTransition<DrinkMilkStates.Instance>(this.currentFeedingCritter, this.on.working.emptying, (MilkFeeder.Instance smi, DrinkMilkStates.Instance val) => val != null);
		this.on.working.emptying.EnterTransition(this.on.working.full, delegate(MilkFeeder.Instance smi)
		{
			DrinkMilkMonitor.Instance smi2 = this.currentFeedingCritter.Get(smi).GetSMI<DrinkMilkMonitor.Instance>();
			return smi2 != null && !smi2.def.consumesMilk;
		}).PlayAnim("emptying").OnAnimQueueComplete(this.on.working.empty).Exit(delegate(MilkFeeder.Instance smi)
		{
			smi.StopFeeding();
		});
		this.on.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040018EA RID: 6378
	private GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State off;

	// Token: 0x040018EB RID: 6379
	private MilkFeeder.OnState on;

	// Token: 0x040018EC RID: 6380
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.BoolParameter isReadyToStartFeeding;

	// Token: 0x040018ED RID: 6381
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.ObjectParameter<DrinkMilkStates.Instance> currentFeedingCritter;

	// Token: 0x0200132A RID: 4906
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008003 RID: 32771 RVA: 0x002EF868 File Offset: 0x002EDA68
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			go.GetSMI<MilkFeeder.Instance>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(CREATURES.MODIFIERS.GOTMILK.NAME, "", Descriptor.DescriptorType.Effect);
			list.Add(item);
			Effect.AddModifierDescriptions(list, "HadMilk", true, "STRINGS.CREATURES.STATS.");
			return list;
		}
	}

	// Token: 0x0200132B RID: 4907
	public class OnState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
	{
		// Token: 0x040061A7 RID: 24999
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pre;

		// Token: 0x040061A8 RID: 25000
		public MilkFeeder.OnState.WorkingState working;

		// Token: 0x040061A9 RID: 25001
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pst;

		// Token: 0x020020FF RID: 8447
		public class WorkingState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
		{
			// Token: 0x040093C1 RID: 37825
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State empty;

			// Token: 0x040093C2 RID: 37826
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State refilling;

			// Token: 0x040093C3 RID: 37827
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State full;

			// Token: 0x040093C4 RID: 37828
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State emptying;
		}
	}

	// Token: 0x0200132C RID: 4908
	public new class Instance : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.GameInstance
	{
		// Token: 0x06008006 RID: 32774 RVA: 0x002EF8C8 File Offset: 0x002EDAC8
		public Instance(IStateMachineTarget master, MilkFeeder.Def def) : base(master, def)
		{
			this.milkStorage = base.GetComponent<Storage>();
			this.storageMeter = new MeterController(base.smi.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06008007 RID: 32775 RVA: 0x002EF906 File Offset: 0x002EDB06
		public void UpdateStorageMeter()
		{
			this.storageMeter.SetPositionPercent(1f - Mathf.Clamp01(this.milkStorage.RemainingCapacity() / this.milkStorage.capacityKg));
		}

		// Token: 0x06008008 RID: 32776 RVA: 0x002EF935 File Offset: 0x002EDB35
		public bool IsOperational()
		{
			return base.GetComponent<Operational>().IsOperational;
		}

		// Token: 0x06008009 RID: 32777 RVA: 0x002EF942 File Offset: 0x002EDB42
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x0600800A RID: 32778 RVA: 0x002EF950 File Offset: 0x002EDB50
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				global::Debug.Assert(!base.HasTag(GameTags.Creatures.ReservedByCreature));
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a MilkFeeder that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x0600800B RID: 32779 RVA: 0x002EF9BD File Offset: 0x002EDBBD
		public bool IsReadyToStartFeeding()
		{
			return this.IsOperational() && base.sm.isReadyToStartFeeding.Get(base.smi);
		}

		// Token: 0x0600800C RID: 32780 RVA: 0x002EF9DF File Offset: 0x002EDBDF
		public void RequestToStartFeeding(DrinkMilkStates.Instance feedingCritter)
		{
			base.sm.currentFeedingCritter.Set(feedingCritter, base.smi, false);
		}

		// Token: 0x0600800D RID: 32781 RVA: 0x002EF9FC File Offset: 0x002EDBFC
		public void StopFeeding()
		{
			DrinkMilkStates.Instance instance = base.sm.currentFeedingCritter.Get(base.smi);
			if (instance != null)
			{
				instance.RequestToStopFeeding();
			}
			base.sm.currentFeedingCritter.Set(null, base.smi, false);
		}

		// Token: 0x0600800E RID: 32782 RVA: 0x002EFA42 File Offset: 0x002EDC42
		public bool HasEnoughMilkForOneFeeding()
		{
			return this.milkStorage.GetAmountAvailable(MilkFeederConfig.MILK_TAG) >= 5f;
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x002EFA5E File Offset: 0x002EDC5E
		public void ConsumeMilkForOneFeeding()
		{
			this.milkStorage.ConsumeIgnoringDisease(MilkFeederConfig.MILK_TAG, 5f);
		}

		// Token: 0x06008010 RID: 32784 RVA: 0x002EFA78 File Offset: 0x002EDC78
		public bool IsInCreaturePenRoom()
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			return roomOfGameObject != null && roomOfGameObject.roomType == Db.Get().RoomTypes.CreaturePen;
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x002EFAB8 File Offset: 0x002EDCB8
		public CellOffset GetCellOffsetToDrinkCell(bool isGassyMoo, bool isGassyMooCramped)
		{
			Rotatable component = base.GetComponent<Rotatable>();
			CellOffset rotatedCellOffset = component.GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
			if (isGassyMoo && component.IsRotated)
			{
				rotatedCellOffset.x--;
			}
			if (isGassyMoo && isGassyMooCramped)
			{
				if (component.IsRotated)
				{
					rotatedCellOffset.x += 2;
				}
				else
				{
					rotatedCellOffset.x -= 2;
				}
			}
			return rotatedCellOffset;
		}

		// Token: 0x040061AA RID: 25002
		public Storage milkStorage;

		// Token: 0x040061AB RID: 25003
		public MeterController storageMeter;
	}
}
