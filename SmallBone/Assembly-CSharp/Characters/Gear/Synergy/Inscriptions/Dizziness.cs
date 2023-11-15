using System;
using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200087B RID: 2171
	public class Dizziness : SimpleStatBonusKeyword
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x0008A581 File Offset: 0x00088781
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06002D9F RID: 11679 RVA: 0x00088ACF File Offset: 0x00086CCF
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Constant;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x0008A589 File Offset: 0x00088789
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.StunDuration;
			}
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x0008A590 File Offset: 0x00088790
		protected override void Initialize()
		{
			base.Initialize();
			this._step1Ability.Initialize();
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x0008A5A4 File Offset: 0x000887A4
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step >= 1)
			{
				if (!base.character.ability.Contains(this._step1Ability.ability) && base.character.ability.GetInstance<KettleOfSwampWitch>() == null)
				{
					base.character.ability.Add(this._step1Ability.ability);
					return;
				}
			}
			else
			{
				base.character.ability.Remove(this._step1Ability.ability);
			}
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x0008A630 File Offset: 0x00088830
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (this.keyword.step < this.keyword.steps.Count - 1)
			{
				return false;
			}
			if (target.character == null)
			{
				return false;
			}
			if (target.character.status == null)
			{
				return false;
			}
			if (!target.character.status.stuned)
			{
				return false;
			}
			damage.percentMultiplier *= (double)this._attackDamageMultiplier;
			return false;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x0008A6A9 File Offset: 0x000888A9
		public override void Attach()
		{
			base.Attach();
			base.character.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x0008A6D4 File Offset: 0x000888D4
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._statBonus);
			base.character.ability.Remove(this._step1Ability.ability);
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x04002626 RID: 9766
		[SerializeField]
		private double[] _statBonusByStep = new double[]
		{
			0.0,
			0.0,
			1.0,
			1.0
		};

		// Token: 0x04002627 RID: 9767
		[SerializeField]
		[Header("1 Step 효과")]
		private OperationByTriggerComponent _step1Ability;

		// Token: 0x04002628 RID: 9768
		[SerializeField]
		[Tooltip("Percent 효과")]
		[Header("3 Step 효과")]
		private float _attackDamageMultiplier = 0.3f;
	}
}
