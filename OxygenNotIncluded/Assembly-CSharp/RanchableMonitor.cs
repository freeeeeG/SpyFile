using System;

// Token: 0x020004A2 RID: 1186
public class RanchableMonitor : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>
{
	// Token: 0x06001AB4 RID: 6836 RVA: 0x0008EBCD File Offset: 0x0008CDCD
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetRanched, (RanchableMonitor.Instance smi) => smi.ShouldGoGetRanched(), null);
	}

	// Token: 0x0200114D RID: 4429
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200114E RID: 4430
	public new class Instance : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.GameInstance
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600790A RID: 30986 RVA: 0x002D8305 File Offset: 0x002D6505
		// (set) Token: 0x0600790B RID: 30987 RVA: 0x002D830D File Offset: 0x002D650D
		public ChoreConsumer ChoreConsumer { get; private set; }

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600790C RID: 30988 RVA: 0x002D8316 File Offset: 0x002D6516
		public Navigator NavComponent
		{
			get
			{
				return this.navComponent;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x0600790D RID: 30989 RVA: 0x002D831E File Offset: 0x002D651E
		public RanchedStates.Instance States
		{
			get
			{
				if (this.states == null)
				{
					this.states = this.controller.GetSMI<RanchedStates.Instance>();
				}
				return this.states;
			}
		}

		// Token: 0x0600790E RID: 30990 RVA: 0x002D833F File Offset: 0x002D653F
		public Instance(IStateMachineTarget master, RanchableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.navComponent = base.GetComponent<Navigator>();
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x002D8361 File Offset: 0x002D6561
		public bool ShouldGoGetRanched()
		{
			return this.TargetRanchStation != null && this.TargetRanchStation.IsRunning() && this.TargetRanchStation.IsRancherReady;
		}

		// Token: 0x04005BF4 RID: 23540
		public RanchStation.Instance TargetRanchStation;

		// Token: 0x04005BF5 RID: 23541
		private Navigator navComponent;

		// Token: 0x04005BF6 RID: 23542
		private RanchedStates.Instance states;
	}
}
