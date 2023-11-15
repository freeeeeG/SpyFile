using System;
using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200087F RID: 2175
	public class ExcessiveBleeding : SimpleStatBonusKeyword
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06002DCE RID: 11726 RVA: 0x0008AC72 File Offset: 0x00088E72
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByLevel;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06002DCF RID: 11727 RVA: 0x00089044 File Offset: 0x00087244
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Percent;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06002DD0 RID: 11728 RVA: 0x0008AC7A File Offset: 0x00088E7A
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.BleedDamage;
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x0008AC81 File Offset: 0x00088E81
		protected override void Initialize()
		{
			base.Initialize();
			this._step1Ability.Initialize();
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x0008AC94 File Offset: 0x00088E94
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
				base.character.status.canBleedCritical = true;
				return;
			}
			base.character.status.canBleedCritical = false;
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x0008AD4E File Offset: 0x00088F4E
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._step1Ability.ability);
			base.character.status.wound.critical = false;
		}

		// Token: 0x0400263F RID: 9791
		[SerializeField]
		private double[] _statBonusByLevel;

		// Token: 0x04002640 RID: 9792
		[Header("1세트 효과")]
		[SerializeField]
		private OperationByTriggerComponent _step1Ability;
	}
}
