using System;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public class UpgradeFX : GameStateMachine<UpgradeFX, UpgradeFX.Instance>
{
	// Token: 0x0600221B RID: 8731 RVA: 0x000BB880 File Offset: 0x000B9A80
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).ToggleReactable((UpgradeFX.Instance smi) => smi.CreateReactable()).Exit("DestroyFX", delegate(UpgradeFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001367 RID: 4967
	public StateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02001218 RID: 4632
	public new class Instance : GameStateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007BD4 RID: 31700 RVA: 0x002DEEC4 File Offset: 0x002DD0C4
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("upgrade_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06007BD5 RID: 31701 RVA: 0x002DEF26 File Offset: 0x002DD126
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x06007BD6 RID: 31702 RVA: 0x002DEF44 File Offset: 0x002DD144
		public Reactable CreateReactable()
		{
			return new EmoteReactable(base.master.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
		}
	}
}
