using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public class SleepChoreMonitor : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance>
{
	// Token: 0x06003FCA RID: 16330 RVA: 0x00164BC8 File Offset: 0x00162DC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.AssignablesChanged, delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
		});
		this.satisfied.EventTransition(GameHashes.AddUrge, this.checkforbed, (SleepChoreMonitor.Instance smi) => smi.HasSleepUrge());
		this.checkforbed.Enter("SetBed", delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
			if (smi.GetSMI<StaminaMonitor.Instance>().NeedsToSleep())
			{
				smi.GoTo(this.passingout);
				return;
			}
			if (this.bed.Get(smi) == null || !smi.IsBedReachable())
			{
				smi.GoTo(this.sleeponfloor);
				return;
			}
			smi.GoTo(this.bedassigned);
		});
		this.passingout.ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreatePassingOutChore), this.satisfied, this.satisfied);
		this.sleeponfloor.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventHandlerTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi, object data) => smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepOnFloorChore), this.satisfied, this.satisfied);
		this.bedassigned.ParamTransition<GameObject>(this.bed, this.checkforbed, (SleepChoreMonitor.Instance smi, GameObject p) => p == null).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi) => !smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepChore), this.satisfied, this.satisfied);
	}

	// Token: 0x06003FCB RID: 16331 RVA: 0x00164D84 File Offset: 0x00162F84
	private Chore CreatePassingOutChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreatePassedOutLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, false);
	}

	// Token: 0x06003FCC RID: 16332 RVA: 0x00164DB8 File Offset: 0x00162FB8
	private Chore CreateSleepOnFloorChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreateFloorLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, true);
	}

	// Token: 0x06003FCD RID: 16333 RVA: 0x00164DE9 File Offset: 0x00162FE9
	private Chore CreateSleepChore(SleepChoreMonitor.Instance smi)
	{
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true);
	}

	// Token: 0x0400297A RID: 10618
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400297B RID: 10619
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State checkforbed;

	// Token: 0x0400297C RID: 10620
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout;

	// Token: 0x0400297D RID: 10621
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State sleeponfloor;

	// Token: 0x0400297E RID: 10622
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State bedassigned;

	// Token: 0x0400297F RID: 10623
	public StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.TargetParameter bed;

	// Token: 0x020016AF RID: 5807
	public new class Instance : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BE4 RID: 35812 RVA: 0x00317464 File Offset: 0x00315664
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008BE5 RID: 35813 RVA: 0x00317470 File Offset: 0x00315670
		public void UpdateBed()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			Assignable assignable = soleOwner.GetAssignable(Db.Get().AssignableSlots.MedicalBed);
			Assignable assignable2;
			if (assignable != null && assignable.CanAutoAssignTo(base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().assignableProxy.Get()))
			{
				assignable2 = assignable;
			}
			else
			{
				assignable2 = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);
				if (assignable2 == null)
				{
					assignable2 = soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
					if (assignable2 != null)
					{
						base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>().Update();
					}
				}
			}
			base.smi.sm.bed.Set(assignable2, base.smi);
		}

		// Token: 0x06008BE6 RID: 35814 RVA: 0x0031755A File Offset: 0x0031575A
		public bool HasSleepUrge()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x06008BE7 RID: 35815 RVA: 0x00317578 File Offset: 0x00315778
		public bool IsBedReachable()
		{
			AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
			return sensor.IsReachable(Db.Get().AssignableSlots.Bed) || sensor.IsReachable(Db.Get().AssignableSlots.MedicalBed);
		}

		// Token: 0x06008BE8 RID: 35816 RVA: 0x003175BF File Offset: 0x003157BF
		public GameObject CreatePassedOutLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "PassedOutSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x06008BE9 RID: 35817 RVA: 0x003175FE File Offset: 0x003157FE
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "FloorSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x04006C7D RID: 27773
		private int locatorCell;

		// Token: 0x04006C7E RID: 27774
		public GameObject locator;
	}
}
