using System;
using UnityEngine;

// Token: 0x02000834 RID: 2100
public class JetSuitMonitor : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance>
{
	// Token: 0x06003D0E RID: 15630 RVA: 0x00153104 File Offset: 0x00151304
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.Target(this.owner);
		this.off.EventTransition(GameHashes.PathAdvanced, this.flying, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStartFlying));
		this.flying.Enter(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StartFlying)).Exit(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StopFlying)).EventTransition(GameHashes.PathAdvanced, this.off, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStopFlying)).Update(new Action<JetSuitMonitor.Instance, float>(JetSuitMonitor.Emit), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06003D0F RID: 15631 RVA: 0x001531A0 File Offset: 0x001513A0
	public static bool ShouldStartFlying(JetSuitMonitor.Instance smi)
	{
		return smi.navigator && smi.navigator.CurrentNavType == NavType.Hover;
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x001531BF File Offset: 0x001513BF
	public static bool ShouldStopFlying(JetSuitMonitor.Instance smi)
	{
		return !smi.navigator || smi.navigator.CurrentNavType != NavType.Hover;
	}

	// Token: 0x06003D11 RID: 15633 RVA: 0x001531E1 File Offset: 0x001513E1
	public static void StartFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x001531E3 File Offset: 0x001513E3
	public static void StopFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x001531E8 File Offset: 0x001513E8
	public static void Emit(JetSuitMonitor.Instance smi, float dt)
	{
		if (!smi.navigator)
		{
			return;
		}
		GameObject gameObject = smi.sm.owner.Get(smi);
		if (!gameObject)
		{
			return;
		}
		int gameCell = Grid.PosToCell(gameObject.transform.GetPosition());
		float num = 0.1f * dt;
		num = Mathf.Min(num, smi.jet_suit_tank.amount);
		smi.jet_suit_tank.amount -= num;
		float num2 = num * 3f;
		if (num2 > 1E-45f)
		{
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementConsumerSimUpdate, num2, 473.15f, byte.MaxValue, 0, true, -1);
		}
		if (smi.jet_suit_tank.amount == 0f)
		{
			smi.navigator.AddTag(GameTags.JetSuitOutOfFuel);
			smi.navigator.SetCurrentNavType(NavType.Floor);
		}
	}

	// Token: 0x040027DC RID: 10204
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040027DD RID: 10205
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying;

	// Token: 0x040027DE RID: 10206
	public StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

	// Token: 0x02001600 RID: 5632
	public new class Instance : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600890E RID: 35086 RVA: 0x00310762 File Offset: 0x0030E962
		public Instance(IStateMachineTarget master, GameObject owner) : base(master)
		{
			base.sm.owner.Set(owner, base.smi, false);
			this.navigator = owner.GetComponent<Navigator>();
			this.jet_suit_tank = master.GetComponent<JetSuitTank>();
		}

		// Token: 0x04006A2C RID: 27180
		public Navigator navigator;

		// Token: 0x04006A2D RID: 27181
		public JetSuitTank jet_suit_tank;
	}
}
