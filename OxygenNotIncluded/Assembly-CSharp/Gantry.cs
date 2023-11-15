using System;
using STRINGS;

// Token: 0x02000606 RID: 1542
public class Gantry : Switch
{
	// Token: 0x060026B2 RID: 9906 RVA: 0x000D2278 File Offset: 0x000D0478
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (Gantry.infoStatusItem == null)
		{
			Gantry.infoStatusItem = new StatusItem("GantryAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Gantry.infoStatusItem.resolveStringCallback = new Func<string, object, string>(Gantry.ResolveInfoStatusItemString);
		}
		base.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 0.5f;
		this.smi = new Gantry.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		base.GetComponent<KSelectable>().ToggleStatusItem(Gantry.infoStatusItem, true, this.smi);
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x000D2315 File Offset: 0x000D0515
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		base.OnCleanUp();
	}

	// Token: 0x060026B4 RID: 9908 RVA: 0x000D2335 File Offset: 0x000D0535
	public void SetWalkable(bool active)
	{
		this.fakeFloorAdder.SetFloor(active);
	}

	// Token: 0x060026B5 RID: 9909 RVA: 0x000D2343 File Offset: 0x000D0543
	protected override void Toggle()
	{
		base.Toggle();
		this.smi.SetSwitchState(this.switchedOn);
	}

	// Token: 0x060026B6 RID: 9910 RVA: 0x000D235C File Offset: 0x000D055C
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x000D2372 File Offset: 0x000D0572
	protected override void UpdateSwitchStatus()
	{
	}

	// Token: 0x060026B8 RID: 9912 RVA: 0x000D2374 File Offset: 0x000D0574
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		Gantry.Instance instance = (Gantry.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.GANTRY.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.GANTRY.MANUAL_CONTROL;
		string arg = instance.IsExtended() ? BUILDING.STATUSITEMS.GANTRY.EXTENDED : BUILDING.STATUSITEMS.GANTRY.RETRACTED;
		return string.Format(format, arg);
	}

	// Token: 0x04001630 RID: 5680
	public static readonly HashedString PORT_ID = "Gantry";

	// Token: 0x04001631 RID: 5681
	[MyCmpReq]
	private Building building;

	// Token: 0x04001632 RID: 5682
	[MyCmpReq]
	private FakeFloorAdder fakeFloorAdder;

	// Token: 0x04001633 RID: 5683
	private Gantry.Instance smi;

	// Token: 0x04001634 RID: 5684
	private static StatusItem infoStatusItem;

	// Token: 0x020012AF RID: 4783
	public class States : GameStateMachine<Gantry.States, Gantry.Instance, Gantry>
	{
		// Token: 0x06007E2D RID: 32301 RVA: 0x002E7644 File Offset: 0x002E5844
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.extended;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.retracted_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("off_pre").OnAnimQueueComplete(this.retracted);
			this.retracted.PlayAnim("off").ParamTransition<bool>(this.should_extend, this.extended_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsTrue);
			this.extended_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.extended);
			this.extended.Enter(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(false);
			}).PlayAnim("on").ParamTransition<bool>(this.should_extend, this.retracted_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsFalse).ToggleTag(GameTags.GantryExtended);
		}

		// Token: 0x0400605A RID: 24666
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted_pre;

		// Token: 0x0400605B RID: 24667
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted;

		// Token: 0x0400605C RID: 24668
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended_pre;

		// Token: 0x0400605D RID: 24669
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended;

		// Token: 0x0400605E RID: 24670
		public StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.BoolParameter should_extend;
	}

	// Token: 0x020012B0 RID: 4784
	public class Instance : GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.GameInstance
	{
		// Token: 0x06007E2F RID: 32303 RVA: 0x002E77D0 File Offset: 0x002E59D0
		public Instance(Gantry master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_extend.Set(true, base.smi, false);
		}

		// Token: 0x06007E30 RID: 32304 RVA: 0x002E7856 File Offset: 0x002E5A56
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(Gantry.PORT_ID);
		}

		// Token: 0x06007E31 RID: 32305 RVA: 0x002E7868 File Offset: 0x002E5A68
		public bool IsExtended()
		{
			if (!this.IsAutomated())
			{
				return this.manual_on;
			}
			return this.logic_on;
		}

		// Token: 0x06007E32 RID: 32306 RVA: 0x002E787F File Offset: 0x002E5A7F
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldExtend();
		}

		// Token: 0x06007E33 RID: 32307 RVA: 0x002E788E File Offset: 0x002E5A8E
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x06007E34 RID: 32308 RVA: 0x002E78A9 File Offset: 0x002E5AA9
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldExtend();
		}

		// Token: 0x06007E35 RID: 32309 RVA: 0x002E78B4 File Offset: 0x002E5AB4
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != Gantry.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldExtend();
		}

		// Token: 0x06007E36 RID: 32310 RVA: 0x002E78F4 File Offset: 0x002E5AF4
		private void UpdateShouldExtend()
		{
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_extend.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_extend.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x0400605F RID: 24671
		private Operational operational;

		// Token: 0x04006060 RID: 24672
		public LogicPorts logic;

		// Token: 0x04006061 RID: 24673
		public bool logic_on = true;

		// Token: 0x04006062 RID: 24674
		private bool manual_on;
	}
}
