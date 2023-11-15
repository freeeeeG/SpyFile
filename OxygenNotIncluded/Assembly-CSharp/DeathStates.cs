using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class DeathStates : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>
{
	// Token: 0x06000350 RID: 848 RVA: 0x0001A538 File Offset: 0x00018738
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter("EnableGravity", delegate(DeathStates.Instance smi)
		{
			smi.EnableGravityIfNecessary();
		}).Enter("Play Death Animations", delegate(DeathStates.Instance smi)
		{
			smi.PlayDeathAnimations();
		}).OnAnimQueueComplete(this.pst).ScheduleGoTo((DeathStates.Instance smi) => smi.def.DIE_ANIMATION_EXPIRATION_TIME, this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, null).Enter("Butcher", delegate(DeathStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Butcherable>() != null)
			{
				smi.GetComponent<Butcherable>().OnButcherComplete();
			}
		}).Enter("Destroy", delegate(DeathStates.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
			smi.gameObject.DeleteObject();
		}).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x04000238 RID: 568
	private GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop;

	// Token: 0x04000239 RID: 569
	public GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State pst;

	// Token: 0x02000EAA RID: 3754
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400541D RID: 21533
		public float DIE_ANIMATION_EXPIRATION_TIME = 4f;
	}

	// Token: 0x02000EAB RID: 3755
	public new class Instance : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.GameInstance
	{
		// Token: 0x06007004 RID: 28676 RVA: 0x002BA26B File Offset: 0x002B846B
		public Instance(Chore<DeathStates.Instance> chore, DeathStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}

		// Token: 0x06007005 RID: 28677 RVA: 0x002BA290 File Offset: 0x002B8490
		public void EnableGravityIfNecessary()
		{
			if (base.HasTag(GameTags.Creatures.Flyer) && !base.HasTag(GameTags.Stored))
			{
				GameComps.Gravities.Add(base.smi.gameObject, Vector2.zero, delegate()
				{
					base.smi.DisableGravity();
				});
			}
		}

		// Token: 0x06007006 RID: 28678 RVA: 0x002BA2DE File Offset: 0x002B84DE
		public void DisableGravity()
		{
			if (GameComps.Gravities.Has(base.smi.gameObject))
			{
				GameComps.Gravities.Remove(base.smi.gameObject);
			}
		}

		// Token: 0x06007007 RID: 28679 RVA: 0x002BA30C File Offset: 0x002B850C
		public void PlayDeathAnimations()
		{
			if (base.gameObject.HasTag(GameTags.PreventDeadAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("Death", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
