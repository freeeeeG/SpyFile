using System;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200089A RID: 2202
	public sealed class ManaCycle : SimpleStatBonusKeyword
	{
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x0008C78F File Offset: 0x0008A98F
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x0008C797 File Offset: 0x0008A997
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.SkillCooldownSpeed;
			}
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x0008C79E File Offset: 0x0008A99E
		public override void Attach()
		{
			base.Attach();
			base.character.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x0008C7C7 File Offset: 0x0008A9C7
		public override void Detach()
		{
			base.Detach();
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x0008C7EC File Offset: 0x0008A9EC
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x0008C7F4 File Offset: 0x0008A9F4
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!this.keyword.isMaxStep)
			{
				return false;
			}
			if (damage.motionType != Damage.MotionType.Skill)
			{
				return false;
			}
			damage.percentMultiplier *= (double)this._skillAttackMultiplier;
			return false;
		}

		// Token: 0x040026CD RID: 9933
		[SerializeField]
		[Header("2세트 효과")]
		private double[] _statBonusByStep = new double[]
		{
			0.0,
			0.4000000059604645,
			0.4000000059604645
		};

		// Token: 0x040026CE RID: 9934
		[Header("4세트 효과 (Percent)")]
		[SerializeField]
		private float _skillAttackMultiplier = 1.3f;
	}
}
