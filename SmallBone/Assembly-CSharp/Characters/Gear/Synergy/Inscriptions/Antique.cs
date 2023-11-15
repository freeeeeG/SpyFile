using System;
using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000865 RID: 2149
	public sealed class Antique : SimpleStatBonusKeyword
	{
		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x00088CA5 File Offset: 0x00086EA5
		protected override double[] statBonusByStep
		{
			get
			{
				return this._healthBonusByStep;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06002CD7 RID: 11479 RVA: 0x00088ACF File Offset: 0x00086CCF
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Constant;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06002CD8 RID: 11480 RVA: 0x00088CAD File Offset: 0x00086EAD
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.Health;
			}
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x00088CB4 File Offset: 0x00086EB4
		protected override void Initialize()
		{
			base.Initialize();
			this._maxStepStatBonus.Initialize();
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x00088CC8 File Offset: 0x00086EC8
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step == this._healthBonusByStep.Length - 1)
			{
				base.character.health.onChanged += this.OnHealthChanged;
				this.OnHealthChanged();
				return;
			}
			base.character.health.onChanged -= this.OnHealthChanged;
			base.character.ability.Remove(this._maxStepStatBonus);
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x00088D48 File Offset: 0x00086F48
		private void OnHealthChanged()
		{
			if (base.character.health.percent >= this._healthCondition)
			{
				if (!base.character.ability.Contains(this._maxStepStatBonus))
				{
					base.character.ability.Add(this._maxStepStatBonus);
					return;
				}
			}
			else
			{
				base.character.ability.Remove(this._maxStepStatBonus);
			}
		}

		// Token: 0x040025AD RID: 9645
		[Header("2세트 효과")]
		[SerializeField]
		private double[] _healthBonusByStep;

		// Token: 0x040025AE RID: 9646
		[SerializeField]
		[Range(0f, 1f)]
		[Header("4세트 효과")]
		private double _healthCondition;

		// Token: 0x040025AF RID: 9647
		[SerializeField]
		private Sprite _maxStepStatBonusIcon;

		// Token: 0x040025B0 RID: 9648
		[SerializeField]
		[Information("Percent", InformationAttribute.InformationType.Info, false)]
		private double _takingDamagePercent;

		// Token: 0x040025B1 RID: 9649
		[SerializeField]
		private Characters.Abilities.CharacterStat.StatBonus _maxStepStatBonus;
	}
}
