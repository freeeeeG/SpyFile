using System;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public class FliesFX : GameStateMachine<FliesFX, FliesFX.Instance>
{
	// Token: 0x0600220D RID: 8717 RVA: 0x000BAFB8 File Offset: 0x000B91B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("swarm_pre").QueueAnim("swarm_loop", true, null).Exit("DestroyFX", delegate(FliesFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001354 RID: 4948
	public StateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x0200120D RID: 4621
	public new class Instance : GameStateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007BAF RID: 31663 RVA: 0x002DE82C File Offset: 0x002DCA2C
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("fly_swarm_kanim", base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06007BB0 RID: 31664 RVA: 0x002DE898 File Offset: 0x002DCA98
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
