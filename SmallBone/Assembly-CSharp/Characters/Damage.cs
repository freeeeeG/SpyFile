using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006E5 RID: 1765
	public struct Damage
	{
		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060023BF RID: 9151 RVA: 0x0006B18C File Offset: 0x0006938C
		public double amount
		{
			get
			{
				if (this.attackType == Damage.AttackType.None)
				{
					return 0.0;
				}
				if (this.attribute == Damage.Attribute.Fixed)
				{
					return Math.Ceiling(this.@base);
				}
				double num = this.@base * this.multiplier * this.percentMultiplier;
				if (this.critical)
				{
					double num2 = this.criticalDamagePercentMultiplier * this.criticalDamageMultiplier;
					num *= num2;
				}
				num += this.extraFixedDamage;
				return Math.Ceiling(num);
			}
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0006B200 File Offset: 0x00069400
		public Damage(Attacker attacker, double @base, Vector2 hitPoint, Damage.Attribute attribute, Damage.AttackType attackType, Damage.MotionType motionType, double multiplier = 1.0, float stoppingPower = 0f, double criticalChance = 0.0, double criticalDamageMultiplier = 1.0, double criticalDamagePercentMultiplier = 1.0, bool canCritical = true, bool @null = false, double extraFixedDamage = 0.0, double ignoreDamageReduction = 0.0, short priority = 0, PriorityList<bool> guaranteedCritical = null, double percentMultiplier = 1.0)
		{
			this.attacker = attacker;
			this.@base = @base;
			this.multiplier = multiplier;
			this.attribute = attribute;
			this.attackType = attackType;
			this.motionType = motionType;
			this.key = string.Empty;
			this.hitPoint = hitPoint;
			this.critical = false;
			this.stoppingPower = stoppingPower;
			this.criticalChance = criticalChance;
			this.criticalDamageMultiplier = criticalDamageMultiplier;
			this.criticalDamagePercentMultiplier = criticalDamagePercentMultiplier;
			this.canCritical = canCritical;
			this.@null = @null;
			this.extraFixedDamage = extraFixedDamage;
			this.ignoreDamageReduction = ignoreDamageReduction;
			this.priority = priority;
			this.guaranteedCritical = guaranteedCritical;
			this.percentMultiplier = percentMultiplier;
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x0006B2AC File Offset: 0x000694AC
		public Damage(Attacker attacker, double @base, Vector2 hitPoint, Damage.Attribute attribute, Damage.AttackType attackType, Damage.MotionType motionType, string key, double multiplier = 1.0, float stoppingPower = 0f, double criticalChance = 0.0, double criticalDamageMultiplier = 1.0, double criticalDamagePercentMultiplier = 1.0, bool canCritical = true, bool @null = false, double extraFixedDamage = 0.0, double ignoreDamageReduction = 0.0, short priority = 0, PriorityList<bool> guaranteedCritical = null, double percentMultiplier = 1.0)
		{
			this.attacker = attacker;
			this.@base = @base;
			this.multiplier = multiplier;
			this.attribute = attribute;
			this.attackType = attackType;
			this.motionType = motionType;
			this.key = key;
			this.hitPoint = hitPoint;
			this.critical = false;
			this.stoppingPower = stoppingPower;
			this.criticalChance = criticalChance;
			this.criticalDamageMultiplier = criticalDamageMultiplier;
			this.criticalDamagePercentMultiplier = criticalDamagePercentMultiplier;
			this.canCritical = canCritical;
			this.@null = @null;
			this.extraFixedDamage = extraFixedDamage;
			this.ignoreDamageReduction = ignoreDamageReduction;
			this.priority = priority;
			this.guaranteedCritical = guaranteedCritical;
			this.percentMultiplier = percentMultiplier;
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0006B355 File Offset: 0x00069555
		public void SetGuaranteedCritical(int priority, bool critical)
		{
			if (this.guaranteedCritical == null)
			{
				this.guaranteedCritical = new PriorityList<bool>();
			}
			this.guaranteedCritical.Add(priority, critical);
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x0006B378 File Offset: 0x00069578
		public void Evaluate(bool immuneToCritical)
		{
			if (immuneToCritical || this.motionType == Damage.MotionType.Item || this.motionType == Damage.MotionType.Quintessence || !this.canCritical || this.@null)
			{
				return;
			}
			if (this.guaranteedCritical != null && this.guaranteedCritical.Count > 0)
			{
				this.critical = this.guaranteedCritical[0];
				return;
			}
			this.critical = MMMaths.Chance(this.criticalChance);
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0006B3E8 File Offset: 0x000695E8
		public override string ToString()
		{
			return this.amount.ToString("0");
		}

		// Token: 0x04001E4A RID: 7754
		public static int evasionPriority = 0;

		// Token: 0x04001E4B RID: 7755
		public static int invulnerablePriority = 0;

		// Token: 0x04001E4C RID: 7756
		public static int cinematicPriority = int.MaxValue;

		// Token: 0x04001E4D RID: 7757
		public readonly Attacker attacker;

		// Token: 0x04001E4E RID: 7758
		public double @base;

		// Token: 0x04001E4F RID: 7759
		public Damage.Attribute attribute;

		// Token: 0x04001E50 RID: 7760
		public readonly Damage.AttackType attackType;

		// Token: 0x04001E51 RID: 7761
		public readonly Damage.MotionType motionType;

		// Token: 0x04001E52 RID: 7762
		public readonly string key;

		// Token: 0x04001E53 RID: 7763
		public bool @null;

		// Token: 0x04001E54 RID: 7764
		public bool canCritical;

		// Token: 0x04001E55 RID: 7765
		public bool critical;

		// Token: 0x04001E56 RID: 7766
		public double percentMultiplier;

		// Token: 0x04001E57 RID: 7767
		public double multiplier;

		// Token: 0x04001E58 RID: 7768
		public float stoppingPower;

		// Token: 0x04001E59 RID: 7769
		public PriorityList<bool> guaranteedCritical;

		// Token: 0x04001E5A RID: 7770
		public double criticalChance;

		// Token: 0x04001E5B RID: 7771
		public double criticalDamageMultiplier;

		// Token: 0x04001E5C RID: 7772
		public double criticalDamagePercentMultiplier;

		// Token: 0x04001E5D RID: 7773
		public double extraFixedDamage;

		// Token: 0x04001E5E RID: 7774
		public double ignoreDamageReduction;

		// Token: 0x04001E5F RID: 7775
		public short priority;

		// Token: 0x04001E60 RID: 7776
		public readonly Vector2 hitPoint;

		// Token: 0x020006E6 RID: 1766
		public enum AttackType
		{
			// Token: 0x04001E62 RID: 7778
			None,
			// Token: 0x04001E63 RID: 7779
			Melee,
			// Token: 0x04001E64 RID: 7780
			Ranged,
			// Token: 0x04001E65 RID: 7781
			Projectile,
			// Token: 0x04001E66 RID: 7782
			Additional
		}

		// Token: 0x020006E7 RID: 1767
		public enum MotionType
		{
			// Token: 0x04001E68 RID: 7784
			Basic,
			// Token: 0x04001E69 RID: 7785
			Skill,
			// Token: 0x04001E6A RID: 7786
			Item,
			// Token: 0x04001E6B RID: 7787
			Quintessence,
			// Token: 0x04001E6C RID: 7788
			Status,
			// Token: 0x04001E6D RID: 7789
			Dash,
			// Token: 0x04001E6E RID: 7790
			Swap,
			// Token: 0x04001E6F RID: 7791
			DarkAbility,
			// Token: 0x04001E70 RID: 7792
			None
		}

		// Token: 0x020006E8 RID: 1768
		public enum Attribute
		{
			// Token: 0x04001E72 RID: 7794
			Physical,
			// Token: 0x04001E73 RID: 7795
			Magic,
			// Token: 0x04001E74 RID: 7796
			Fixed
		}
	}
}
