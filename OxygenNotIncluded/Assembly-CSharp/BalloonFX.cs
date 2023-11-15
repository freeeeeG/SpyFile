using System;
using Database;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
	// Token: 0x06002206 RID: 8710 RVA: 0x000BABCC File Offset: 0x000B8DCC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.Exit("DestroyFX", delegate(BalloonFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x0400134A RID: 4938
	public StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x0400134B RID: 4939
	public KAnimFile defaultAnim = Assets.GetAnim("balloon_anim_kanim");

	// Token: 0x0400134C RID: 4940
	private KAnimFile defaultBalloon = Assets.GetAnim("balloon_basic_red_kanim");

	// Token: 0x0400134D RID: 4941
	private const string defaultAnimName = "ballon_anim_kanim";

	// Token: 0x0400134E RID: 4942
	private const string balloonAnimName = "balloon_basic_red_kanim";

	// Token: 0x0400134F RID: 4943
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04001350 RID: 4944
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x02001208 RID: 4616
	public new class Instance : GameStateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007B98 RID: 31640 RVA: 0x002DE444 File Offset: 0x002DC644
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.balloonAnimController = FXHelpers.CreateEffectOverride(new string[]
			{
				"ballon_anim_kanim",
				"balloon_basic_red_kanim"
			}, master.gameObject.transform.GetPosition() + new Vector3(0f, 0.3f, 1f), master.transform, true, Grid.SceneLayer.Creatures, false);
			base.sm.fx.Set(this.balloonAnimController.gameObject, base.smi, false);
			this.balloonAnimController.defaultAnim = "idle_default";
			master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add(this.balloonAnimController.GetComponent<KBatchedAnimController>());
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x002DE4FC File Offset: 0x002DC6FC
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverride)
		{
			KAnimFile kanimFile = balloonOverride.animFile.IsSome() ? balloonOverride.animFile.Unwrap() : base.smi.sm.defaultBalloon;
			this.balloonAnimController.SwapAnims(new KAnimFile[]
			{
				base.smi.sm.defaultAnim,
				kanimFile
			});
			SymbolOverrideController component = this.balloonAnimController.GetComponent<SymbolOverrideController>();
			if (this.currentBodyOverrideSymbol.IsSome())
			{
				component.RemoveSymbolOverride("body", 0);
			}
			if (balloonOverride.symbol.IsNone())
			{
				if (this.currentBodyOverrideSymbol.IsSome())
				{
					component.AddSymbolOverride("body", base.smi.sm.defaultAnim.GetData().build.GetSymbol("body"), 0);
				}
				this.balloonAnimController.SetBatchGroupOverride(HashedString.Invalid);
			}
			else
			{
				component.AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
				this.balloonAnimController.SetBatchGroupOverride(kanimFile.batchTag);
			}
			this.currentBodyOverrideSymbol = balloonOverride;
		}

		// Token: 0x06007B9A RID: 31642 RVA: 0x002DE62C File Offset: 0x002DC82C
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x04005E44 RID: 24132
		private KBatchedAnimController balloonAnimController;

		// Token: 0x04005E45 RID: 24133
		private Option<BalloonOverrideSymbol> currentBodyOverrideSymbol;
	}
}
