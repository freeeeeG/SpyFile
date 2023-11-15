using System;
using System.Runtime.CompilerServices;
using Klei.AI;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class DrinkMilkMonitor : GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>
{
	// Token: 0x06001A94 RID: 6804 RVA: 0x0008DEF0 File Offset: 0x0008C0F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.lookingToDrinkMilk;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.OnSignal(this.didFinishDrinkingMilk, this.applyEffect).Enter(delegate(DrinkMilkMonitor.Instance smi)
		{
			this.remainingSecondsForEffect.Set(Mathf.Clamp(this.remainingSecondsForEffect.Get(smi), 0f, 600f), smi, false);
		}).ParamTransition<float>(this.remainingSecondsForEffect, this.satisfied, (DrinkMilkMonitor.Instance smi, float val) => val > 0f);
		this.lookingToDrinkMilk.Update(new Action<DrinkMilkMonitor.Instance, float>(DrinkMilkMonitor.FindMilkFeederTarget), UpdateRate.SIM_4000ms, true).ToggleBehaviour(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder, (DrinkMilkMonitor.Instance smi) => !smi.targetMilkFeeder.IsNullOrStopped() && !smi.targetMilkFeeder.IsReserved(), null).Exit(delegate(DrinkMilkMonitor.Instance smi)
		{
			smi.targetMilkFeeder = null;
		});
		this.applyEffect.Enter(delegate(DrinkMilkMonitor.Instance smi)
		{
			this.remainingSecondsForEffect.Set(600f, smi, false);
		}).EnterTransition(this.satisfied, (DrinkMilkMonitor.Instance smi) => true);
		this.satisfied.Enter(delegate(DrinkMilkMonitor.Instance smi)
		{
			if (smi.def.consumesMilk)
			{
				smi.GetComponent<Effects>().Add("HadMilk", false).timeRemaining = this.remainingSecondsForEffect.Get(smi);
			}
		}).Exit(delegate(DrinkMilkMonitor.Instance smi)
		{
			if (smi.def.consumesMilk)
			{
				smi.GetComponent<Effects>().Remove("HadMilk");
			}
			this.remainingSecondsForEffect.Set(-1f, smi, false);
		}).ScheduleGoTo((DrinkMilkMonitor.Instance smi) => this.remainingSecondsForEffect.Get(smi), this.lookingToDrinkMilk).Update(delegate(DrinkMilkMonitor.Instance smi, float deltaSeconds)
		{
			this.remainingSecondsForEffect.Delta(-deltaSeconds, smi);
			if (this.remainingSecondsForEffect.Get(smi) < 0f)
			{
				smi.GoTo(this.lookingToDrinkMilk);
			}
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x0008E064 File Offset: 0x0008C264
	private static void FindMilkFeederTarget(DrinkMilkMonitor.Instance smi, float dt)
	{
		DrinkMilkMonitor.<>c__DisplayClass8_0 CS$<>8__locals1;
		CS$<>8__locals1.smi = smi;
		using (ListPool<MilkFeeder.Instance, DrinkMilkMonitor>.PooledList pooledList = PoolsFor<DrinkMilkMonitor>.AllocateList<MilkFeeder.Instance>())
		{
			int cell = Grid.PosToCell(CS$<>8__locals1.smi.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null && cavityForCell.room.roomType == Db.Get().RoomTypes.CreaturePen)
			{
				foreach (KPrefabID kprefabID in cavityForCell.buildings)
				{
					if (!kprefabID.IsNullOrDestroyed())
					{
						MilkFeeder.Instance smi2 = kprefabID.GetSMI<MilkFeeder.Instance>();
						if (smi2 != null && smi2.IsReadyToStartFeeding())
						{
							pooledList.Add(smi2);
						}
					}
				}
			}
			DrinkMilkMonitor.<>c__DisplayClass8_1 CS$<>8__locals2;
			CS$<>8__locals2.navigator = CS$<>8__locals1.smi.GetComponent<Navigator>();
			CS$<>8__locals2.drowningMonitor = CS$<>8__locals1.smi.GetComponent<DrowningMonitor>();
			CS$<>8__locals2.canDrown = (CS$<>8__locals2.drowningMonitor != null && CS$<>8__locals2.drowningMonitor.canDrownToDeath && !CS$<>8__locals2.drowningMonitor.livesUnderWater);
			CS$<>8__locals1.smi.targetMilkFeeder = null;
			CS$<>8__locals1.smi.doesTargetMilkFeederHaveSpaceForGassyMoo = false;
			CS$<>8__locals2.resultCost = -1;
			foreach (MilkFeeder.Instance milkFeeder in pooledList)
			{
				DrinkMilkMonitor.<>c__DisplayClass8_2 CS$<>8__locals3;
				CS$<>8__locals3.milkFeeder = milkFeeder;
				if (CS$<>8__locals1.smi.def.isGassyMoo)
				{
					if (DrinkMilkMonitor.<FindMilkFeederTarget>g__ConsiderCell|8_0(CS$<>8__locals1.smi.GetDrinkCellOf(CS$<>8__locals3.milkFeeder, false), ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3))
					{
						CS$<>8__locals1.smi.doesTargetMilkFeederHaveSpaceForGassyMoo = false;
					}
					else if (DrinkMilkMonitor.<FindMilkFeederTarget>g__ConsiderCell|8_0(CS$<>8__locals1.smi.GetDrinkCellOf(CS$<>8__locals3.milkFeeder, true), ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3))
					{
						CS$<>8__locals1.smi.doesTargetMilkFeederHaveSpaceForGassyMoo = true;
					}
				}
				else
				{
					DrinkMilkMonitor.<FindMilkFeederTarget>g__ConsiderCell|8_0(CS$<>8__locals1.smi.GetDrinkCellOf(CS$<>8__locals3.milkFeeder, CS$<>8__locals1.smi.doesTargetMilkFeederHaveSpaceForGassyMoo), ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
				}
			}
		}
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x0008E3AC File Offset: 0x0008C5AC
	[CompilerGenerated]
	internal static bool <FindMilkFeederTarget>g__ConsiderCell|8_0(int cell, ref DrinkMilkMonitor.<>c__DisplayClass8_0 A_1, ref DrinkMilkMonitor.<>c__DisplayClass8_1 A_2, ref DrinkMilkMonitor.<>c__DisplayClass8_2 A_3)
	{
		if (A_2.canDrown && !A_2.drowningMonitor.IsCellSafe(cell))
		{
			return false;
		}
		int navigationCost = A_2.navigator.GetNavigationCost(cell);
		if (navigationCost == -1)
		{
			return false;
		}
		if (navigationCost < A_2.resultCost || A_2.resultCost == -1)
		{
			A_2.resultCost = navigationCost;
			A_1.smi.targetMilkFeeder = A_3.milkFeeder;
			return true;
		}
		return false;
	}

	// Token: 0x04000EC8 RID: 3784
	public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State lookingToDrinkMilk;

	// Token: 0x04000EC9 RID: 3785
	public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State applyEffect;

	// Token: 0x04000ECA RID: 3786
	public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State satisfied;

	// Token: 0x04000ECB RID: 3787
	private StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.Signal didFinishDrinkingMilk;

	// Token: 0x04000ECC RID: 3788
	private StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.FloatParameter remainingSecondsForEffect;

	// Token: 0x02001132 RID: 4402
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BA0 RID: 23456
		public bool isGassyMoo;

		// Token: 0x04005BA1 RID: 23457
		public bool consumesMilk = true;
	}

	// Token: 0x02001133 RID: 4403
	public new class Instance : GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.GameInstance
	{
		// Token: 0x060078B7 RID: 30903 RVA: 0x002D76F1 File Offset: 0x002D58F1
		public Instance(IStateMachineTarget master, DrinkMilkMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060078B8 RID: 30904 RVA: 0x002D76FB File Offset: 0x002D58FB
		public void NotifyFinishedDrinkingMilkFrom(MilkFeeder.Instance milkFeeder)
		{
			if (milkFeeder != null && base.def.consumesMilk)
			{
				milkFeeder.ConsumeMilkForOneFeeding();
			}
			base.sm.didFinishDrinkingMilk.Trigger(base.smi);
		}

		// Token: 0x060078B9 RID: 30905 RVA: 0x002D7729 File Offset: 0x002D5929
		public int GetDrinkCellOf(MilkFeeder.Instance milkFeeder, bool isGassyMooCramped)
		{
			return Grid.OffsetCell(Grid.PosToCell(milkFeeder), milkFeeder.GetCellOffsetToDrinkCell(base.def.isGassyMoo, isGassyMooCramped));
		}

		// Token: 0x04005BA2 RID: 23458
		public MilkFeeder.Instance targetMilkFeeder;

		// Token: 0x04005BA3 RID: 23459
		public bool doesTargetMilkFeederHaveSpaceForGassyMoo;
	}
}
