using System;
using STRINGS;

// Token: 0x02000871 RID: 2161
public class DeathMonitor : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>
{
	// Token: 0x06003F2A RID: 16170 RVA: 0x001606A4 File Offset: 0x0015E8A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.alive.ParamTransition<Death>(this.death, this.dying_duplicant, (DeathMonitor.Instance smi, Death p) => p != null && smi.IsDuplicant).ParamTransition<Death>(this.death, this.dying_creature, (DeathMonitor.Instance smi, Death p) => p != null && !smi.IsDuplicant);
		this.dying_duplicant.ToggleAnims("anim_emotes_default_kanim", 0f, "").ToggleTag(GameTags.Dying).ToggleChore((DeathMonitor.Instance smi) => new DieChore(smi.master, this.death.Get(smi)), this.die);
		this.dying_creature.ToggleBehaviour(GameTags.Creatures.Die, (DeathMonitor.Instance smi) => true, delegate(DeathMonitor.Instance smi)
		{
			smi.GoTo(this.dead_creature);
		});
		this.die.ToggleTag(GameTags.Dying).Enter("Die", delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.PreventChoreInterruption);
			Death death = this.death.Get(smi);
			if (smi.IsDuplicant)
			{
				DeathMessage message = new DeathMessage(smi.gameObject, death);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_localized", false), smi.master.transform.GetPosition(), 1f);
				KFMOD.PlayUISound(GlobalAssets.GetSound("Death_Notification_ST", false));
				Messenger.Instance.QueueMessage(message);
			}
		}).TriggerOnExit(GameHashes.Died, null).GoTo(this.dead);
		this.dead.ToggleAnims("anim_emotes_default_kanim", 0f, "").DefaultState(this.dead.ground).ToggleTag(GameTags.Dead).Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.ApplyDeath();
			Game.Instance.Trigger(282337316, smi.gameObject);
		});
		this.dead.ground.Enter(delegate(DeathMonitor.Instance smi)
		{
			Death death = this.death.Get(smi);
			if (death == null)
			{
				death = Db.Get().Deaths.Generic;
			}
			if (smi.IsDuplicant)
			{
				smi.GetComponent<KAnimControllerBase>().Play(death.loopAnim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}).EventTransition(GameHashes.OnStore, this.dead.carried, (DeathMonitor.Instance smi) => smi.IsDuplicant && smi.HasTag(GameTags.Stored));
		this.dead.carried.ToggleAnims("anim_dead_carried_kanim", 0f, "").PlayAnim("idle_default", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.dead.ground, (DeathMonitor.Instance smi) => !smi.HasTag(GameTags.Stored));
		this.dead_creature.Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
		}).PlayAnim("idle_dead", KAnim.PlayMode.Loop);
	}

	// Token: 0x040028E9 RID: 10473
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State alive;

	// Token: 0x040028EA RID: 10474
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_duplicant;

	// Token: 0x040028EB RID: 10475
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_creature;

	// Token: 0x040028EC RID: 10476
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State die;

	// Token: 0x040028ED RID: 10477
	public DeathMonitor.Dead dead;

	// Token: 0x040028EE RID: 10478
	public DeathMonitor.Dead dead_creature;

	// Token: 0x040028EF RID: 10479
	public StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.ResourceParameter<Death> death;

	// Token: 0x0200165B RID: 5723
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200165C RID: 5724
	public class Dead : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State
	{
		// Token: 0x04006B5D RID: 27485
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State ground;

		// Token: 0x04006B5E RID: 27486
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State carried;
	}

	// Token: 0x0200165D RID: 5725
	public new class Instance : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.GameInstance
	{
		// Token: 0x06008A79 RID: 35449 RVA: 0x00313D64 File Offset: 0x00311F64
		public Instance(IStateMachineTarget master, DeathMonitor.Def def) : base(master, def)
		{
			this.isDuplicant = base.GetComponent<MinionIdentity>();
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06008A7A RID: 35450 RVA: 0x00313D7F File Offset: 0x00311F7F
		public bool IsDuplicant
		{
			get
			{
				return this.isDuplicant;
			}
		}

		// Token: 0x06008A7B RID: 35451 RVA: 0x00313D87 File Offset: 0x00311F87
		public void Kill(Death death)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x06008A7C RID: 35452 RVA: 0x00313DA2 File Offset: 0x00311FA2
		public void PickedUp(object data = null)
		{
			if (data is Storage || (data != null && (bool)data))
			{
				base.smi.GoTo(base.sm.dead.carried);
			}
		}

		// Token: 0x06008A7D RID: 35453 RVA: 0x00313DD8 File Offset: 0x00311FD8
		public bool IsDead()
		{
			return base.smi.IsInsideState(base.smi.sm.dead);
		}

		// Token: 0x06008A7E RID: 35454 RVA: 0x00313DF8 File Offset: 0x00311FF8
		public void ApplyDeath()
		{
			if (this.isDuplicant)
			{
				Game.Instance.assignmentManager.RemoveFromAllGroups(base.GetComponent<MinionIdentity>().assignableProxy.Get());
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Dead, base.smi.sm.death.Get(base.smi));
				float value = 600f - GameClock.Instance.GetTimeSinceStartOfReport();
				ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, value, string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.IS_DEAD_TASK), base.smi.master.gameObject.GetProperName());
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null)
				{
					component.RegisterListeners();
				}
			}
			base.GetComponent<KPrefabID>().AddTag(GameTags.Corpse, false);
		}

		// Token: 0x04006B5F RID: 27487
		private bool isDuplicant;
	}
}
