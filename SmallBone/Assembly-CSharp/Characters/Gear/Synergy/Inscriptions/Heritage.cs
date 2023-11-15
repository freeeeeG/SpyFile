using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000893 RID: 2195
	public sealed class Heritage : SimpleStatBonusKeyword
	{
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06002E67 RID: 11879 RVA: 0x0008BF28 File Offset: 0x0008A128
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06002E68 RID: 11880 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06002E69 RID: 11881 RVA: 0x0008BF30 File Offset: 0x0008A130
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.EssenceCooldownSpeed;
			}
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x0008BF37 File Offset: 0x0008A137
		protected override void Initialize()
		{
			base.Initialize();
			this._step2ability.Initialize();
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x0008BF4A File Offset: 0x0008A14A
		public override void Attach()
		{
			base.Attach();
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x0008BF52 File Offset: 0x0008A152
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._step2ability.ability);
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x0008BF78 File Offset: 0x0008A178
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.isMaxStep)
			{
				base.character.ability.Add(this._step2ability.ability);
				return;
			}
			base.character.ability.Remove(this._step2ability.ability);
		}

		// Token: 0x0400268A RID: 9866
		[Header("1세트 효과")]
		[SerializeField]
		private double[] _statBonusByStep = new double[]
		{
			0.0,
			0.4000000059604645,
			0.4000000059604645
		};

		// Token: 0x0400268B RID: 9867
		[Header("3세트 효과 (Percent)")]
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _step2ability;
	}
}
