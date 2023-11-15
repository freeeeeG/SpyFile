using System;

// Token: 0x020009B8 RID: 2488
public class SimpleDoorController : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>
{
	// Token: 0x06004A1F RID: 18975 RVA: 0x001A17A4 File Offset: 0x0019F9A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inactive;
		this.inactive.TagTransition(GameTags.RocketOnGround, this.active, false);
		this.active.DefaultState(this.active.closed).TagTransition(GameTags.RocketOnGround, this.inactive, true).Enter(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Register();
		}).Exit(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Unregister();
		});
		this.active.closed.PlayAnim((SimpleDoorController.StatesInstance smi) => smi.GetDefaultAnim(), KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.opening, (SimpleDoorController.StatesInstance smi, int p) => p > 0);
		this.active.opening.PlayAnim("enter_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.open);
		this.active.open.PlayAnim("enter_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.closedelay, (SimpleDoorController.StatesInstance smi, int p) => p == 0);
		this.active.closedelay.ParamTransition<int>(this.numOpens, this.active.open, (SimpleDoorController.StatesInstance smi, int p) => p > 0).ScheduleGoTo(0.5f, this.active.closing);
		this.active.closing.PlayAnim("enter_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.closed);
	}

	// Token: 0x040030C7 RID: 12487
	public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State inactive;

	// Token: 0x040030C8 RID: 12488
	public SimpleDoorController.ActiveStates active;

	// Token: 0x040030C9 RID: 12489
	public StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.IntParameter numOpens;

	// Token: 0x0200183F RID: 6207
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001840 RID: 6208
	public class ActiveStates : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State
	{
		// Token: 0x04007170 RID: 29040
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closed;

		// Token: 0x04007171 RID: 29041
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State opening;

		// Token: 0x04007172 RID: 29042
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State open;

		// Token: 0x04007173 RID: 29043
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closedelay;

		// Token: 0x04007174 RID: 29044
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closing;
	}

	// Token: 0x02001841 RID: 6209
	public class StatesInstance : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.GameInstance, INavDoor
	{
		// Token: 0x06009145 RID: 37189 RVA: 0x003295E1 File Offset: 0x003277E1
		public StatesInstance(IStateMachineTarget master, SimpleDoorController.Def def) : base(master, def)
		{
		}

		// Token: 0x06009146 RID: 37190 RVA: 0x003295EC File Offset: 0x003277EC
		public string GetDefaultAnim()
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				return component.initialAnim;
			}
			return "idle_loop";
		}

		// Token: 0x06009147 RID: 37191 RVA: 0x0032961C File Offset: 0x0032781C
		public void Register()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = true;
		}

		// Token: 0x06009148 RID: 37192 RVA: 0x0032964C File Offset: 0x0032784C
		public void Unregister()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = false;
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06009149 RID: 37193 RVA: 0x0032967B File Offset: 0x0032787B
		public bool isSpawned
		{
			get
			{
				return base.master.gameObject.GetComponent<KMonoBehaviour>().isSpawned;
			}
		}

		// Token: 0x0600914A RID: 37194 RVA: 0x00329692 File Offset: 0x00327892
		public void Close()
		{
			base.sm.numOpens.Delta(-1, base.smi);
		}

		// Token: 0x0600914B RID: 37195 RVA: 0x003296AC File Offset: 0x003278AC
		public bool IsOpen()
		{
			return base.IsInsideState(base.sm.active.open) || base.IsInsideState(base.sm.active.closedelay);
		}

		// Token: 0x0600914C RID: 37196 RVA: 0x003296DE File Offset: 0x003278DE
		public void Open()
		{
			base.sm.numOpens.Delta(1, base.smi);
		}
	}
}
