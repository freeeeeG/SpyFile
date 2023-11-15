using System;

// Token: 0x0200065C RID: 1628
public class ModularConduitPortController : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>
{
	// Token: 0x06002AF0 RID: 10992 RVA: 0x000E4F3C File Offset: 0x000E313C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		ModularConduitPortController.InitializeStatusItems();
		this.off.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on, (ModularConduitPortController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (ModularConduitPortController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.on.idle.PlayAnim("idle").ParamTransition<bool>(this.hasRocket, this.on.finished, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.idleStatusItem, null);
		this.on.finished.PlayAnim("finished", KAnim.PlayMode.Loop).ParamTransition<bool>(this.hasRocket, this.on.idle, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.isUnloading, this.on.unloading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ParamTransition<bool>(this.isLoading, this.on.loading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.loadedStatusItem, null);
		this.on.unloading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("unloading_pre").QueueAnim("unloading_loop", true, null).ParamTransition<bool>(this.isUnloading, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.unloadingStatusItem, null);
		this.on.unloading_pst.PlayAnim("unloading_pst").OnAnimQueueComplete(this.on.finished);
		this.on.loading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("loading_pre").QueueAnim("loading_loop", true, null).ParamTransition<bool>(this.isLoading, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.loadingStatusItem, null);
		this.on.loading_pst.PlayAnim("loading_pst").OnAnimQueueComplete(this.on.finished);
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x000E5244 File Offset: 0x000E3444
	private static void InitializeStatusItems()
	{
		if (ModularConduitPortController.idleStatusItem == null)
		{
			ModularConduitPortController.idleStatusItem = new StatusItem("ROCKET_PORT_IDLE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.unloadingStatusItem = new StatusItem("ROCKET_PORT_UNLOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadingStatusItem = new StatusItem("ROCKET_PORT_LOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadedStatusItem = new StatusItem("ROCKET_PORT_LOADED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	// Token: 0x04001914 RID: 6420
	private GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State off;

	// Token: 0x04001915 RID: 6421
	private ModularConduitPortController.OnStates on;

	// Token: 0x04001916 RID: 6422
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isUnloading;

	// Token: 0x04001917 RID: 6423
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isLoading;

	// Token: 0x04001918 RID: 6424
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter hasRocket;

	// Token: 0x04001919 RID: 6425
	private static StatusItem idleStatusItem;

	// Token: 0x0400191A RID: 6426
	private static StatusItem unloadingStatusItem;

	// Token: 0x0400191B RID: 6427
	private static StatusItem loadingStatusItem;

	// Token: 0x0400191C RID: 6428
	private static StatusItem loadedStatusItem;

	// Token: 0x02001344 RID: 4932
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006213 RID: 25107
		public ModularConduitPortController.Mode mode;
	}

	// Token: 0x02001345 RID: 4933
	public enum Mode
	{
		// Token: 0x04006215 RID: 25109
		Unload,
		// Token: 0x04006216 RID: 25110
		Both,
		// Token: 0x04006217 RID: 25111
		Load
	}

	// Token: 0x02001346 RID: 4934
	private class OnStates : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State
	{
		// Token: 0x04006218 RID: 25112
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State idle;

		// Token: 0x04006219 RID: 25113
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading;

		// Token: 0x0400621A RID: 25114
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading_pst;

		// Token: 0x0400621B RID: 25115
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading;

		// Token: 0x0400621C RID: 25116
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading_pst;

		// Token: 0x0400621D RID: 25117
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State finished;
	}

	// Token: 0x02001347 RID: 4935
	public new class Instance : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.GameInstance
	{
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06008085 RID: 32901 RVA: 0x002F19EB File Offset: 0x002EFBEB
		public ModularConduitPortController.Mode SelectedMode
		{
			get
			{
				return base.def.mode;
			}
		}

		// Token: 0x06008086 RID: 32902 RVA: 0x002F19F8 File Offset: 0x002EFBF8
		public Instance(IStateMachineTarget master, ModularConduitPortController.Def def) : base(master, def)
		{
		}

		// Token: 0x06008087 RID: 32903 RVA: 0x002F1A02 File Offset: 0x002EFC02
		public ConduitType GetConduitType()
		{
			return base.GetComponent<IConduitConsumer>().ConduitType;
		}

		// Token: 0x06008088 RID: 32904 RVA: 0x002F1A0F File Offset: 0x002EFC0F
		public void SetUnloading(bool isUnloading)
		{
			base.sm.isUnloading.Set(isUnloading, this, false);
		}

		// Token: 0x06008089 RID: 32905 RVA: 0x002F1A25 File Offset: 0x002EFC25
		public void SetLoading(bool isLoading)
		{
			base.sm.isLoading.Set(isLoading, this, false);
		}

		// Token: 0x0600808A RID: 32906 RVA: 0x002F1A3B File Offset: 0x002EFC3B
		public void SetRocket(bool hasRocket)
		{
			base.sm.hasRocket.Set(hasRocket, this, false);
		}

		// Token: 0x0600808B RID: 32907 RVA: 0x002F1A51 File Offset: 0x002EFC51
		public bool IsLoading()
		{
			return base.sm.isLoading.Get(this);
		}

		// Token: 0x0400621E RID: 25118
		[MyCmpGet]
		public Operational operational;
	}
}
