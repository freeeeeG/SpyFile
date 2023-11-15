using System;
using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000864 RID: 2148
	public class AbsoluteZero : SimpleStatBonusKeyword
	{
		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06002CCD RID: 11469 RVA: 0x00088AC7 File Offset: 0x00086CC7
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x00088ACF File Offset: 0x00086CCF
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Constant;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x00088AD6 File Offset: 0x00086CD6
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.FreezeDuration;
			}
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x00088ADD File Offset: 0x00086CDD
		protected override void Initialize()
		{
			base.Initialize();
			this._step1Ability.Initialize();
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x00088AF0 File Offset: 0x00086CF0
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step >= 1)
			{
				if (!base.character.ability.Contains(this._step1Ability.ability) && base.character.ability.GetInstance<KettleOfSwampWitch>() == null)
				{
					base.character.ability.Add(this._step1Ability.ability);
				}
			}
			else
			{
				base.character.ability.Remove(this._step1Ability.ability);
			}
			if (this.keyword.isMaxStep)
			{
				base.character.status.freezeMaxHitStack = 3;
				return;
			}
			base.character.status.freezeMaxHitStack = 1;
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x00088BAA File Offset: 0x00086DAA
		private void AttachAbility(Character attacker, Character target)
		{
			if (this.keyword.isMaxStep)
			{
				return;
			}
			target.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x00088BD1 File Offset: 0x00086DD1
		public override void Attach()
		{
			base.character.status.Register(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Release, new CharacterStatus.OnTimeDelegate(this.AttachAbility));
			base.character.status.Register(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Refresh, new CharacterStatus.OnTimeDelegate(this.AttachAbility));
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x00088C10 File Offset: 0x00086E10
		public override void Detach()
		{
			base.character.ability.Remove(this._statBonus);
			base.character.ability.Remove(this._step1Ability.ability);
			base.character.status.freezeMaxHitStack = 1;
			base.character.status.Unregister(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Release, new CharacterStatus.OnTimeDelegate(this.AttachAbility));
			base.character.status.Unregister(CharacterStatus.Kind.Freeze, CharacterStatus.Timing.Refresh, new CharacterStatus.OnTimeDelegate(this.AttachAbility));
		}

		// Token: 0x040025AA RID: 9642
		[SerializeField]
		private double[] _statBonusByStep;

		// Token: 0x040025AB RID: 9643
		[Header("1세트 효과")]
		[SerializeField]
		private OperationByTriggerComponent _step1Ability;

		// Token: 0x040025AC RID: 9644
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		[Header("3세트 효과")]
		private AbilityComponent _abilityComponent;
	}
}
