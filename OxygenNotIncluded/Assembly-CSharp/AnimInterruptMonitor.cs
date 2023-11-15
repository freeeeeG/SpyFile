using System;

// Token: 0x02000707 RID: 1799
public class AnimInterruptMonitor : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
	// Token: 0x06003180 RID: 12672 RVA: 0x0010755E File Offset: 0x0010575E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.PlayInterruptAnim, new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim), new Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim));
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x00107591 File Offset: 0x00105791
	private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi)
	{
		return smi.anims != null;
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x0010759C File Offset: 0x0010579C
	private static void ClearAnim(AnimInterruptMonitor.Instance smi)
	{
		smi.anims = null;
	}

	// Token: 0x0200145B RID: 5211
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200145C RID: 5212
	public new class Instance : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance
	{
		// Token: 0x06008469 RID: 33897 RVA: 0x00302757 File Offset: 0x00300957
		public Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600846A RID: 33898 RVA: 0x00302761 File Offset: 0x00300961
		public void PlayAnim(HashedString anim)
		{
			this.PlayAnimSequence(new HashedString[]
			{
				anim
			});
		}

		// Token: 0x0600846B RID: 33899 RVA: 0x00302777 File Offset: 0x00300977
		public void PlayAnimSequence(HashedString[] anims)
		{
			this.anims = anims;
			base.GetComponent<CreatureBrain>().UpdateBrain();
		}

		// Token: 0x04006543 RID: 25923
		public HashedString[] anims;
	}
}
