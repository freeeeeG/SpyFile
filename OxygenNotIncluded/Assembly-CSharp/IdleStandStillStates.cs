using System;
using STRINGS;

// Token: 0x020000CF RID: 207
public class IdleStandStillStates : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>
{
	// Token: 0x060003B4 RID: 948 RVA: 0x0001CBD8 File Offset: 0x0001ADD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.IDLE.NAME, CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).ToggleTag(GameTags.Idle);
		this.loop.Enter(new StateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State.Callback(this.PlayIdle));
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001CC58 File Offset: 0x0001AE58
	public void PlayIdle(IdleStandStillStates.Instance smi)
	{
		KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					component.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		component.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x04000274 RID: 628
	private GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State loop;

	// Token: 0x02000EF2 RID: 3826
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400548D RID: 21645
		public IdleStandStillStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02001FAA RID: 8106
		// (Invoke) Token: 0x0600A319 RID: 41753
		public delegate HashedString IdleAnimCallback(IdleStandStillStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x02000EF3 RID: 3827
	public new class Instance : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.GameInstance
	{
		// Token: 0x0600709D RID: 28829 RVA: 0x002BB13A File Offset: 0x002B933A
		public Instance(Chore<IdleStandStillStates.Instance> chore, IdleStandStillStates.Def def) : base(chore, def)
		{
		}
	}
}
