using System;
using UnityEngine;

// Token: 0x02000881 RID: 2177
public class IncapacitationMonitor : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
	// Token: 0x06003F68 RID: 16232 RVA: 0x00162364 File Offset: 0x00160564
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.healthy.TagTransition(GameTags.CaloriesDepleted, this.incapacitated, false).TagTransition(GameTags.HitPointsDepleted, this.incapacitated, false).TagTransition(GameTags.HitByHighEnergyParticle, this.incapacitated, false).TagTransition(GameTags.RadiationSicknessIncapacitation, this.incapacitated, false).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.RecoverStamina(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.start_recovery.TagTransition(new Tag[]
		{
			GameTags.CaloriesDepleted,
			GameTags.HitPointsDepleted
		}, this.healthy, true);
		this.incapacitated.EventTransition(GameHashes.IncapacitationRecovery, this.start_recovery, null).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((IncapacitationMonitor.Instance smi) => new BeIncapacitatedChore(smi.master), null).ParamTransition<float>(this.bleedOutStamina, this.die, GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ToggleUrge(Db.Get().Urges.BeIncapacitated).Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.Trigger(-1506500077, null);
		}).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.Bleed(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.die.Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<DeathMonitor.Instance>().Kill(smi.GetCauseOfIncapacitation());
		});
	}

	// Token: 0x04002927 RID: 10535
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04002928 RID: 10536
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State start_recovery;

	// Token: 0x04002929 RID: 10537
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x0400292A RID: 10538
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;

	// Token: 0x0400292B RID: 10539
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x0400292C RID: 10540
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x0400292D RID: 10541
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x0400292E RID: 10542
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x02001685 RID: 5765
	public new class Instance : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B3A RID: 35642 RVA: 0x003160C0 File Offset: 0x003142C0
		public Instance(IStateMachineTarget master) : base(master)
		{
			Health component = master.GetComponent<Health>();
			if (component)
			{
				component.CanBeIncapacitated = true;
			}
		}

		// Token: 0x06008B3B RID: 35643 RVA: 0x003160EA File Offset: 0x003142EA
		public void Bleed(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
		}

		// Token: 0x06008B3C RID: 35644 RVA: 0x00316114 File Offset: 0x00314314
		public void RecoverStamina(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
		}

		// Token: 0x06008B3D RID: 35645 RVA: 0x0031616E File Offset: 0x0031436E
		public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
		{
			return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
		}

		// Token: 0x06008B3E RID: 35646 RVA: 0x00316198 File Offset: 0x00314398
		public Death GetCauseOfIncapacitation()
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (component.HasTag(GameTags.HitByHighEnergyParticle))
			{
				return Db.Get().Deaths.HitByHighEnergyParticle;
			}
			if (component.HasTag(GameTags.RadiationSicknessIncapacitation))
			{
				return Db.Get().Deaths.Radiation;
			}
			if (component.HasTag(GameTags.CaloriesDepleted))
			{
				return Db.Get().Deaths.Starvation;
			}
			if (component.HasTag(GameTags.HitPointsDepleted))
			{
				return Db.Get().Deaths.Slain;
			}
			return Db.Get().Deaths.Generic;
		}
	}
}
