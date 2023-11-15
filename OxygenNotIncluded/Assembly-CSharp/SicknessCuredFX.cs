using System;
using UnityEngine;

// Token: 0x0200057D RID: 1405
public class SicknessCuredFX : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance>
{
	// Token: 0x06002217 RID: 8727 RVA: 0x000BB71C File Offset: 0x000B991C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).Exit("DestroyFX", delegate(SicknessCuredFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x0400135F RID: 4959
	public StateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02001214 RID: 4628
	public new class Instance : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007BC9 RID: 31689 RVA: 0x002DED64 File Offset: 0x002DCF64
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("recentlyhealed_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06007BCA RID: 31690 RVA: 0x002DEDC6 File Offset: 0x002DCFC6
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
