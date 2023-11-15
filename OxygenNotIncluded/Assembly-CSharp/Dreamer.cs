using System;

// Token: 0x020004AA RID: 1194
public class Dreamer : GameStateMachine<Dreamer, Dreamer.Instance>
{
	// Token: 0x06001B0E RID: 6926 RVA: 0x00091270 File Offset: 0x0008F470
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notDreaming;
		this.notDreaming.OnSignal(this.startDreaming, this.dreaming, (Dreamer.Instance smi) => smi.currentDream != null);
		this.dreaming.Enter(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(Dreamer.PrepareDream)).OnSignal(this.stopDreaming, this.notDreaming).Update(new Action<Dreamer.Instance, float>(this.UpdateDream), UpdateRate.SIM_EVERY_TICK, false).Exit(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(this.RemoveDream));
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x00091309 File Offset: 0x0008F509
	private void RemoveDream(Dreamer.Instance smi)
	{
		smi.SetDream(null);
		NameDisplayScreen.Instance.StopDreaming(smi.gameObject);
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x00091322 File Offset: 0x0008F522
	private void UpdateDream(Dreamer.Instance smi, float dt)
	{
		NameDisplayScreen.Instance.DreamTick(smi.gameObject, dt);
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x00091335 File Offset: 0x0008F535
	private static void PrepareDream(Dreamer.Instance smi)
	{
		NameDisplayScreen.Instance.SetDream(smi.gameObject, smi.currentDream);
	}

	// Token: 0x04000F06 RID: 3846
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal stopDreaming;

	// Token: 0x04000F07 RID: 3847
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal startDreaming;

	// Token: 0x04000F08 RID: 3848
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State notDreaming;

	// Token: 0x04000F09 RID: 3849
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State dreaming;

	// Token: 0x0200115B RID: 4443
	public class DreamingState : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005C0D RID: 23565
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State hidden;

		// Token: 0x04005C0E RID: 23566
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State visible;
	}

	// Token: 0x0200115C RID: 4444
	public new class Instance : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007939 RID: 31033 RVA: 0x002D87F8 File Offset: 0x002D69F8
		public Instance(IStateMachineTarget master) : base(master)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x002D8813 File Offset: 0x002D6A13
		public void SetDream(Dream dream)
		{
			this.currentDream = dream;
		}

		// Token: 0x0600793B RID: 31035 RVA: 0x002D881C File Offset: 0x002D6A1C
		public void StartDreaming()
		{
			base.sm.startDreaming.Trigger(base.smi);
		}

		// Token: 0x0600793C RID: 31036 RVA: 0x002D8834 File Offset: 0x002D6A34
		public void StopDreaming()
		{
			this.SetDream(null);
			base.sm.stopDreaming.Trigger(base.smi);
		}

		// Token: 0x04005C0F RID: 23567
		public Dream currentDream;
	}
}
