using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000878 RID: 2168
	public sealed class Chase : SimpleStatBonusKeyword
	{
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06002D8E RID: 11662 RVA: 0x0008A2AC File Offset: 0x000884AC
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06002D8F RID: 11663 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06002D90 RID: 11664 RVA: 0x0008A2B4 File Offset: 0x000884B4
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.DashCooldownSpeed;
			}
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x0008A2BB File Offset: 0x000884BB
		protected override void Initialize()
		{
			base.Initialize();
			this._maxStepAbilityComponent.Initialize();
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x0008A2D0 File Offset: 0x000884D0
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step == this._statBonusByStep.Length - 1)
			{
				if (!base.character.ability.Contains(this._maxStepAbilityComponent.ability))
				{
					base.character.ability.Add(this._maxStepAbilityComponent.ability);
					return;
				}
			}
			else
			{
				base.character.ability.Remove(this._maxStepAbilityComponent.ability);
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x0008A350 File Offset: 0x00088550
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._maxStepAbilityComponent.ability);
		}

		// Token: 0x0400261B RID: 9755
		[SerializeField]
		[Header("2세트 효과")]
		private double[] _statBonusByStep = new double[]
		{
			0.0,
			0.20000000298023224,
			0.20000000298023224
		};

		// Token: 0x0400261C RID: 9756
		[SerializeField]
		[Header("4세트 효과")]
		private AbilityComponent _maxStepAbilityComponent;
	}
}
