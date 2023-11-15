using System;

// Token: 0x02000A04 RID: 2564
[SkipSaveFileSerialization]
public class Thriver : StateMachineComponent<Thriver.StatesInstance>
{
	// Token: 0x06004CA9 RID: 19625 RVA: 0x001AE037 File Offset: 0x001AC237
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0200188E RID: 6286
	public class StatesInstance : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.GameInstance
	{
		// Token: 0x06009215 RID: 37397 RVA: 0x0032B4A0 File Offset: 0x003296A0
		public StatesInstance(Thriver master) : base(master)
		{
		}

		// Token: 0x06009216 RID: 37398 RVA: 0x0032B4AC File Offset: 0x003296AC
		public bool IsStressed()
		{
			StressMonitor.Instance smi = base.master.GetSMI<StressMonitor.Instance>();
			return smi != null && smi.IsStressed();
		}
	}

	// Token: 0x0200188F RID: 6287
	public class States : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver>
	{
		// Token: 0x06009217 RID: 37399 RVA: 0x0032B4D0 File Offset: 0x003296D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.EventTransition(GameHashes.NotStressed, this.idle, null).EventTransition(GameHashes.Stressed, this.stressed, null).EventTransition(GameHashes.StressedHadEnough, this.stressed, null).Enter(delegate(Thriver.StatesInstance smi)
			{
				StressMonitor.Instance smi2 = smi.master.GetSMI<StressMonitor.Instance>();
				if (smi2 != null && smi2.IsStressed())
				{
					smi.GoTo(this.stressed);
				}
			});
			this.idle.DoNothing();
			this.stressed.ToggleEffect("Thriver");
			this.toostressed.DoNothing();
		}

		// Token: 0x04007251 RID: 29265
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State idle;

		// Token: 0x04007252 RID: 29266
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State stressed;

		// Token: 0x04007253 RID: 29267
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State toostressed;
	}
}
