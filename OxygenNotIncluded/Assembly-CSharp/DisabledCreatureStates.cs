using System;
using STRINGS;

// Token: 0x020000BB RID: 187
public class DisabledCreatureStates : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>
{
	// Token: 0x0600035C RID: 860 RVA: 0x0001A9A0 File Offset: 0x00018BA0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.disableCreature;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.DISABLED.NAME, CREATURES.STATUSITEMS.DISABLED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Creatures.Behaviours.DisableCreature, this.behaviourcomplete, true);
		this.disableCreature.PlayAnim((DisabledCreatureStates.Instance smi) => smi.def.disabledAnim, KAnim.PlayMode.Once);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.DisableCreature, false);
	}

	// Token: 0x04000242 RID: 578
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State disableCreature;

	// Token: 0x04000243 RID: 579
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State behaviourcomplete;

	// Token: 0x02000EB6 RID: 3766
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600701D RID: 28701 RVA: 0x002BA4D4 File Offset: 0x002B86D4
		public Def(string anim)
		{
			this.disabledAnim = anim;
		}

		// Token: 0x0400542A RID: 21546
		public string disabledAnim = "off";
	}

	// Token: 0x02000EB7 RID: 3767
	public new class Instance : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.GameInstance
	{
		// Token: 0x0600701E RID: 28702 RVA: 0x002BA4EE File Offset: 0x002B86EE
		public Instance(Chore<DisabledCreatureStates.Instance> chore, DisabledCreatureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Behaviours.DisableCreature);
		}
	}
}
