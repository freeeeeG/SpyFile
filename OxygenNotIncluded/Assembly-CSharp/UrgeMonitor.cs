using System;
using Klei.AI;

// Token: 0x020008A1 RID: 2209
public class UrgeMonitor : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance>
{
	// Token: 0x0600400C RID: 16396 RVA: 0x001667DC File Offset: 0x001649DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.hasurge, (UrgeMonitor.Instance smi) => smi.HasUrge(), UpdateRate.SIM_200ms);
		this.hasurge.Transition(this.satisfied, (UrgeMonitor.Instance smi) => !smi.HasUrge(), UpdateRate.SIM_200ms).ToggleUrge((UrgeMonitor.Instance smi) => smi.GetUrge());
	}

	// Token: 0x040029B6 RID: 10678
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040029B7 RID: 10679
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State hasurge;

	// Token: 0x020016D3 RID: 5843
	public new class Instance : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C98 RID: 35992 RVA: 0x00319630 File Offset: 0x00317830
		public Instance(IStateMachineTarget master, Urge urge, Amount amount, ScheduleBlockType schedule_block, float in_schedule_threshold, float out_of_schedule_threshold, bool is_threshold_minimum) : base(master)
		{
			this.urge = urge;
			this.scheduleBlock = schedule_block;
			this.schedulable = base.GetComponent<Schedulable>();
			this.amountInstance = base.gameObject.GetAmounts().Get(amount);
			this.isThresholdMinimum = is_threshold_minimum;
			this.inScheduleThreshold = in_schedule_threshold;
			this.outOfScheduleThreshold = out_of_schedule_threshold;
		}

		// Token: 0x06008C99 RID: 35993 RVA: 0x0031968E File Offset: 0x0031788E
		private float GetThreshold()
		{
			if (this.schedulable.IsAllowed(this.scheduleBlock))
			{
				return this.inScheduleThreshold;
			}
			return this.outOfScheduleThreshold;
		}

		// Token: 0x06008C9A RID: 35994 RVA: 0x003196B0 File Offset: 0x003178B0
		public Urge GetUrge()
		{
			return this.urge;
		}

		// Token: 0x06008C9B RID: 35995 RVA: 0x003196B8 File Offset: 0x003178B8
		public bool HasUrge()
		{
			if (this.isThresholdMinimum)
			{
				return this.amountInstance.value >= this.GetThreshold();
			}
			return this.amountInstance.value <= this.GetThreshold();
		}

		// Token: 0x04006D07 RID: 27911
		private AmountInstance amountInstance;

		// Token: 0x04006D08 RID: 27912
		private Urge urge;

		// Token: 0x04006D09 RID: 27913
		private ScheduleBlockType scheduleBlock;

		// Token: 0x04006D0A RID: 27914
		private Schedulable schedulable;

		// Token: 0x04006D0B RID: 27915
		private float inScheduleThreshold;

		// Token: 0x04006D0C RID: 27916
		private float outOfScheduleThreshold;

		// Token: 0x04006D0D RID: 27917
		private bool isThresholdMinimum;
	}
}
