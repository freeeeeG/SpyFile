using System;
using System.Collections.Generic;

// Token: 0x0200086C RID: 2156
public class ColonyRationMonitor : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance>
{
	// Token: 0x06003F1C RID: 16156 RVA: 0x001602A8 File Offset: 0x0015E4A8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Update("UpdateOutOfRations", delegate(ColonyRationMonitor.Instance smi, float dt)
		{
			smi.UpdateIsOutOfRations();
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.ParamTransition<bool>(this.isOutOfRations, this.outofrations, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsTrue).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
		this.outofrations.ParamTransition<bool>(this.isOutOfRations, this.satisfied, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsFalse).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
	}

	// Token: 0x040028DA RID: 10458
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040028DB RID: 10459
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State outofrations;

	// Token: 0x040028DC RID: 10460
	private StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isOutOfRations = new StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x0200164F RID: 5711
	public new class Instance : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008A51 RID: 35409 RVA: 0x00313825 File Offset: 0x00311A25
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.UpdateIsOutOfRations();
		}

		// Token: 0x06008A52 RID: 35410 RVA: 0x00313834 File Offset: 0x00311A34
		public void UpdateIsOutOfRations()
		{
			bool value = true;
			using (List<Edible>.Enumerator enumerator = Components.Edibles.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent<Pickupable>().UnreservedAmount > 0f)
					{
						value = false;
						break;
					}
				}
			}
			base.smi.sm.isOutOfRations.Set(value, base.smi, false);
		}

		// Token: 0x06008A53 RID: 35411 RVA: 0x003138B8 File Offset: 0x00311AB8
		public bool IsOutOfRations()
		{
			return base.smi.sm.isOutOfRations.Get(base.smi);
		}
	}
}
