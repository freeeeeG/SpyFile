using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters.Operations;
using Characters.Projectiles;
using GameResources;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000720 RID: 1824
	public class Stat
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x0006F051 File Offset: 0x0006D251
		public PriorityList<Stat.OnUpdatedDelegate> onUpdated
		{
			get
			{
				return this._onUpdated;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x0006F059 File Offset: 0x0006D259
		// (set) Token: 0x060024F7 RID: 9463 RVA: 0x0006F061 File Offset: 0x0006D261
		public Stat getDamageOverridingStat { get; set; }

		// Token: 0x060024F8 RID: 9464 RVA: 0x0006F06C File Offset: 0x0006D26C
		public Stat(Character owner)
		{
			this._owner = owner;
			this.SetAll(Stat.Category.Percent, 1.0);
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x0006F0DC File Offset: 0x0006D2DC
		public Stat(Character owner, Stat stat)
		{
			this._owner = owner;
			this.SetAll(Stat.Category.Percent, 1.0);
			this._values = (double[,])stat._values.Clone();
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x0006F164 File Offset: 0x0006D364
		private void Initialize()
		{
			this.SetAll(Stat.Category.Constant, 0.0);
			this.SetAll(Stat.Category.Fixed, 0.0);
			this.SetAll(Stat.Category.Percent, 1.0);
			this.SetAll(Stat.Category.PercentPoint, 0.0);
			this.SetAll(Stat.Category.Final, 0.0);
			this._values[Stat.Category.Percent.index, Stat.Kind.CriticalDamage.index] = 1.0;
			this._values[Stat.Category.PercentPoint.index, Stat.Kind.CriticalDamage.index] = 0.5;
			this._values[Stat.Category.Constant.index, Stat.Kind.EmberDamage.index] = (double)CharacterStatusSetting.instance.burn.rangeRadius;
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x0006F254 File Offset: 0x0006D454
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Apply(Stat.Values value)
		{
			for (int i = 0; i < value.values.Length; i++)
			{
				this.Apply(value.values[i]);
			}
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x0006F284 File Offset: 0x0006D484
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Apply(Stat.Value value)
		{
			if (value.categoryIndex == Stat.Category.Percent.index)
			{
				this._values[value.categoryIndex, value.kindIndex] *= value.value;
				return;
			}
			this._values[value.categoryIndex, value.kindIndex] += value.value;
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x0006F2E8 File Offset: 0x0006D4E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetAll(Stat.Category category, double value)
		{
			for (int i = 0; i < Stat.Kind.count; i++)
			{
				this._values[category.index, i] = value;
			}
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x0006F318 File Offset: 0x0006D518
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double Get(int categoryIndex, int kindIndex)
		{
			return this._values[categoryIndex, kindIndex];
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x0006F327 File Offset: 0x0006D527
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double Get(Stat.Category category, Stat.Kind kind)
		{
			return this._values[category.index, kind.index];
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x0006F340 File Offset: 0x0006D540
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetConstant(Stat.Kind kind)
		{
			return this.Get(Stat.Category.Constant, kind);
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x0006F34E File Offset: 0x0006D54E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetFixed(Stat.Kind kind)
		{
			return this.Get(Stat.Category.Fixed, kind);
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x0006F35C File Offset: 0x0006D55C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetPercent(Stat.Kind kind)
		{
			return this.Get(Stat.Category.Percent, kind);
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x0006F36A File Offset: 0x0006D56A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetPercentPoint(Stat.Kind kind)
		{
			return this.Get(Stat.Category.PercentPoint, kind);
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x0006F378 File Offset: 0x0006D578
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetFinal(Stat.Kind kind)
		{
			return this.Get(Stat.Category.Final, kind);
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x0006F386 File Offset: 0x0006D586
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetFinalPercent(Stat.Kind kind)
		{
			return this.Get(Stat.Category.Percent, kind) * (1.0 + this.Get(Stat.Category.PercentPoint, kind));
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x0006F3AB File Offset: 0x0006D5AB
		public Damage.Attribute GetAdaptiveForceAttribute()
		{
			if (this.GetFinal(Stat.Kind.PhysicalAttackDamage) >= this.GetFinal(Stat.Kind.MagicAttackDamage))
			{
				return Damage.Attribute.Physical;
			}
			return Damage.Attribute.Magic;
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x0006F3C8 File Offset: 0x0006D5C8
		public Damage GetDamage(double baseDamage, Vector2 hitPoint, HitInfo hitInfo)
		{
			Damage.Attribute attribute = hitInfo.attribute;
			if (this.IsChangeAttribute != null && this.IsChangeAttribute(hitInfo))
			{
				if (attribute == Damage.Attribute.Physical)
				{
					attribute = Damage.Attribute.Magic;
				}
				else if (attribute == Damage.Attribute.Magic)
				{
					attribute = Damage.Attribute.Physical;
				}
			}
			if (attribute != Damage.Attribute.Fixed && this.adaptiveAttribute)
			{
				attribute = this.GetAdaptiveForceAttribute();
			}
			if (this.getDamageOverridingStat != null)
			{
				return this.getDamageOverridingStat.GetDamage(baseDamage, hitPoint, hitInfo);
			}
			double num = 1.0;
			if (attribute != Damage.Attribute.Physical)
			{
				if (attribute == Damage.Attribute.Magic)
				{
					num += this.GetFinal(Stat.Kind.MagicAttackDamage) - 1.0;
				}
			}
			else
			{
				num += this.GetFinal(Stat.Kind.PhysicalAttackDamage) - 1.0;
			}
			if (hitInfo.attackType == Damage.AttackType.Projectile)
			{
				num += this.GetFinal(Stat.Kind.ProjectileAttackDamage) - 1.0;
			}
			Damage.MotionType motionType = hitInfo.motionType;
			if (motionType != Damage.MotionType.Basic)
			{
				if (motionType == Damage.MotionType.Skill)
				{
					num += this.GetFinal(Stat.Kind.SkillAttackDamage) - 1.0;
				}
			}
			else
			{
				num += this.GetFinal(Stat.Kind.BasicAttackDamage) - 1.0;
			}
			num *= this.GetFinal(Stat.Kind.AttackDamage);
			return new Damage(this._owner, baseDamage * (double)hitInfo.damageMultiplier, hitPoint, attribute, hitInfo.attackType, hitInfo.motionType, hitInfo.key, num, hitInfo.stoppingPower * (float)this.GetFinal(Stat.Kind.StoppingPower), this.GetFinal(Stat.Kind.CriticalChance) - 1.0 + (double)hitInfo.extraCriticalChance, this.GetFinal(Stat.Kind.CriticalDamage) + (double)hitInfo.extraCriticalDamage, 1.0, true, false, 0.0, this._owner.stat.GetIgnoreDamageReduction(), 0, null, 1.0);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x0006F580 File Offset: 0x0006D780
		public Damage GetProjectileDamage(IProjectile projectile, double baseDamage, Vector2 hitPoint, HitInfo hitInfo)
		{
			Damage damage = this.GetDamage(baseDamage, hitPoint, hitInfo);
			return new Damage(new Attacker(this._owner, projectile), baseDamage * (double)hitInfo.damageMultiplier, hitPoint, damage.attribute, hitInfo.attackType, hitInfo.motionType, hitInfo.key, damage.multiplier, hitInfo.stoppingPower * (float)this.GetFinal(Stat.Kind.StoppingPower), this.GetFinal(Stat.Kind.CriticalChance) - 1.0 + (double)hitInfo.extraCriticalChance, this.GetFinal(Stat.Kind.CriticalDamage) + (double)hitInfo.extraCriticalDamage, 1.0, true, false, 0.0, this._owner.stat.GetIgnoreDamageReduction(), 0, null, 1.0);
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x0006F64A File Offset: 0x0006D84A
		public float GetCooldownSpeed()
		{
			return (float)this.GetFinal(Stat.Kind.CooldownSpeed);
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x0006F658 File Offset: 0x0006D858
		public float GetCooldownSpeed(Stat.Kind kind)
		{
			return this.GetCooldownSpeed() + (float)this.GetFinal(kind) - 1f;
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x0006F66F File Offset: 0x0006D86F
		public float GetSkillCooldownSpeed()
		{
			return this.GetCooldownSpeed(Stat.Kind.SkillCooldownSpeed);
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x0006F67C File Offset: 0x0006D87C
		public float GetDashCooldownSpeed()
		{
			return this.GetCooldownSpeed(Stat.Kind.DashCooldownSpeed);
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x0006F689 File Offset: 0x0006D889
		public float GetSwapCooldownSpeed()
		{
			return this.GetCooldownSpeed(Stat.Kind.SwapCooldownSpeed);
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x0006F696 File Offset: 0x0006D896
		public float GetQuintessenceCooldownSpeed()
		{
			return this.GetCooldownSpeed(Stat.Kind.EssenceCooldownSpeed);
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x0006F6A3 File Offset: 0x0006D8A3
		public double GetIgnoreDamageReduction()
		{
			return this.GetFinal(Stat.Kind.IgnoreDamageReduction) - 1.0;
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x0006F6BC File Offset: 0x0006D8BC
		private float InterpolatedAttackSpeed(double attackSpeed)
		{
			if (attackSpeed > 1.100000023841858)
			{
				attackSpeed = 1.100000023841858 + (attackSpeed - 1.100000023841858) * 0.949999988079071;
			}
			if (attackSpeed > 1.2000000476837158)
			{
				attackSpeed = 1.2000000476837158 + (attackSpeed - 1.2000000476837158) * 0.8999999761581421;
			}
			if (attackSpeed > 1.2999999523162842)
			{
				attackSpeed = 1.2999999523162842 + (attackSpeed - 1.2999999523162842) * 0.8500000238418579;
			}
			if (attackSpeed > 1.399999976158142)
			{
				attackSpeed = 1.399999976158142 + (attackSpeed - 1.399999976158142) * 0.800000011920929;
			}
			if (attackSpeed > 1.5)
			{
				attackSpeed = 1.5 + (attackSpeed - 1.5) * 0.75;
			}
			return (float)attackSpeed;
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x0006F7AC File Offset: 0x0006D9AC
		public float GetInterpolatedBasicAttackSpeed()
		{
			return this.InterpolatedAttackSpeed(this.GetFinal(Stat.Kind.AttackSpeed) * this.GetFinal(Stat.Kind.BasicAttackSpeed));
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0006F7CB File Offset: 0x0006D9CB
		public float GetInterpolatedSkillAttackSpeed()
		{
			return this.InterpolatedAttackSpeed(this.GetFinal(Stat.Kind.AttackSpeed) * this.GetFinal(Stat.Kind.SkillAttackSpeed));
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x0006F7EA File Offset: 0x0006D9EA
		public float GetInterpolatedChargingSpeed()
		{
			return this.InterpolatedAttackSpeed(this.GetFinal(Stat.Kind.AttackSpeed) * this.GetFinal(Stat.Kind.ChargingSpeed));
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x0006F809 File Offset: 0x0006DA09
		public float GetInterpolatedBasicAttackChargingSpeed()
		{
			return this.InterpolatedAttackSpeed(this.GetFinal(Stat.Kind.AttackSpeed) * this.GetFinal(Stat.Kind.BasicAttackSpeed) * this.GetFinal(Stat.Kind.ChargingSpeed));
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x0006F834 File Offset: 0x0006DA34
		public float GetInterpolatedSkillAttackChargingSpeed()
		{
			return this.InterpolatedAttackSpeed(this.GetFinal(Stat.Kind.AttackSpeed) * this.GetFinal(Stat.Kind.SkillAttackSpeed) * this.GetFinal(Stat.Kind.ChargingSpeed));
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x0006F860 File Offset: 0x0006DA60
		public float GetInterpolatedMovementSpeed()
		{
			float num = (float)this.GetFinal(Stat.Kind.MovementSpeed);
			if (num > 6f)
			{
				num = 6f + (num - 6f) * 0.95f;
			}
			if (num > 6.5f)
			{
				num = 6.5f + (num - 6.5f) * 0.9f;
			}
			if (num > 7f)
			{
				num = 7f + (num - 7f) * 0.85f;
			}
			if (num > 7.5f)
			{
				num = 7.5f + (num - 7.5f) * 0.8f;
			}
			if (num > 8f)
			{
				num = 8f + (num - 8f) * 0.75f;
			}
			return num;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x0006F908 File Offset: 0x0006DB08
		public double GetStatusResistacneFor(CharacterStatus.Kind kind)
		{
			switch (kind)
			{
			case CharacterStatus.Kind.Stun:
				return this.GetFinal(Stat.Kind.StatusResistance) * this.GetFinal(Stat.Kind.StunResistance);
			case CharacterStatus.Kind.Freeze:
				return this.GetFinal(Stat.Kind.StatusResistance) * this.GetFinal(Stat.Kind.FreezeResistance);
			case CharacterStatus.Kind.Burn:
				return this.GetFinal(Stat.Kind.StatusResistance) * this.GetFinal(Stat.Kind.BurnResistance);
			case CharacterStatus.Kind.Wound:
				return this.GetFinal(Stat.Kind.StatusResistance) * this.GetFinal(Stat.Kind.BleedResistance);
			case CharacterStatus.Kind.Poison:
				return this.GetFinal(Stat.Kind.StatusResistance) * this.GetFinal(Stat.Kind.PoisonResistance);
			default:
				return this.GetFinal(Stat.Kind.StatusResistance);
			}
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x0006F9B4 File Offset: 0x0006DBB4
		public bool ApplyDefense(ref Damage damage)
		{
			double num = 1.0;
			num *= this.GetFinal(Stat.Kind.TakingDamage);
			Damage.Attribute attribute = damage.attribute;
			if (attribute != Damage.Attribute.Physical)
			{
				if (attribute == Damage.Attribute.Magic)
				{
					num *= this.GetFinal(Stat.Kind.TakingMagicDamage);
				}
			}
			else
			{
				num *= this.GetFinal(Stat.Kind.TakingPhysicalDamage);
			}
			damage.ignoreDamageReduction = ((damage.ignoreDamageReduction > 1.0) ? 1.0 : damage.ignoreDamageReduction);
			if (num < 1.0 && damage.ignoreDamageReduction > 0.0)
			{
				num = 1.0 - (1.0 - num) * (1.0 - damage.ignoreDamageReduction);
				if (num > 1.0)
				{
					return false;
				}
			}
			damage.multiplier *= num;
			return false;
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x0006FA90 File Offset: 0x0006DC90
		public bool Evade(ref Damage damage)
		{
			double num = this.GetFinal(Stat.Kind.EvasionChance);
			Damage.AttackType attackType = damage.attackType;
			if (attackType != Damage.AttackType.Melee)
			{
				if (attackType == Damage.AttackType.Ranged)
				{
					num *= this.GetFinal(Stat.Kind.RangedEvasionChance);
				}
			}
			else
			{
				num *= this.GetFinal(Stat.Kind.MeleeEvasionChance);
			}
			return !MMMaths.Chance(num);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x0006FAE4 File Offset: 0x0006DCE4
		public bool Contains(Stat.Values values)
		{
			if (this._bonuses.Contains(values))
			{
				return true;
			}
			for (int i = 0; i < this._bonusesWithEvent.Count; i++)
			{
				if (this._bonusesWithEvent[i].values == values)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x0006FB2E File Offset: 0x0006DD2E
		public void AttachValues(Stat.Values values)
		{
			this._bonuses.Add(values);
			this._needUpdate = true;
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x0006FB43 File Offset: 0x0006DD43
		public void AttachOrUpdateValues(Stat.Values values)
		{
			if (this._bonuses.Contains(values))
			{
				return;
			}
			this.AttachValues(values);
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x0006FB5B File Offset: 0x0006DD5B
		public void AttachValues(Stat.Values values, Stat.ValuesWithEvent.OnDetachDelegate onDetach)
		{
			this._bonusesWithEvent.Add(new Stat.ValuesWithEvent(values, onDetach));
			this._needUpdate = true;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x0006FB78 File Offset: 0x0006DD78
		public void DetachValues(Stat.Values values)
		{
			if (this._bonuses.Remove(values))
			{
				this._needUpdate = true;
				return;
			}
			for (int i = 0; i < this._bonusesWithEvent.Count; i++)
			{
				if (this._bonusesWithEvent[i].values == values)
				{
					Stat.ValuesWithEvent.OnDetachDelegate onDetach = this._bonusesWithEvent[i]._onDetach;
					if (onDetach != null)
					{
						onDetach(this);
					}
					this._bonusesWithEvent.RemoveAt(i);
					this._needUpdate = true;
					return;
				}
			}
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x0006FBF6 File Offset: 0x0006DDF6
		public void AttachTimedValues(Stat.Values values, float duration, Stat.ValuesWithEvent.OnDetachDelegate onDetach = null)
		{
			if (duration <= 0f)
			{
				return;
			}
			this._timedBonuses.Add(new Stat.TimedValues(values, duration, null));
			this._needUpdate = true;
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x0006FC1C File Offset: 0x0006DE1C
		public void DetachTimedValues(Stat.Values values)
		{
			for (int i = 0; i < this._timedBonuses.Count; i++)
			{
				if (this._timedBonuses[i].values == values)
				{
					this._timedBonuses.RemoveAt(i);
					this._needUpdate = true;
					return;
				}
			}
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x0006FC68 File Offset: 0x0006DE68
		public bool UpdateTimedValues(Stat.Values values, float duration, Stat.ValuesWithEvent.OnDetachDelegate onDetach = null)
		{
			for (int i = 0; i < this._timedBonuses.Count; i++)
			{
				if (this._timedBonuses[i].values == values)
				{
					Stat.ValuesWithEvent.OnDetachDelegate onDetach2 = this._timedBonuses[i]._onDetach;
					if (onDetach2 != null)
					{
						onDetach2(this);
					}
					this._timedBonuses[i]._onDetach = onDetach;
					this._timedBonuses[i].SetTime(duration);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x0006FCE3 File Offset: 0x0006DEE3
		public void AttachOrUpdateTimedValues(Stat.Values values, float duration, Stat.ValuesWithEvent.OnDetachDelegate onDetach = null)
		{
			if (this.UpdateTimedValues(values, duration, onDetach))
			{
				return;
			}
			this._timedBonuses.Add(new Stat.TimedValues(values, duration, onDetach));
			this._needUpdate = true;
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x0006FD0C File Offset: 0x0006DF0C
		public void TakeTime(float deltaTime)
		{
			deltaTime /= (float)this.GetFinal(Stat.Kind.BuffDuration);
			for (int i = this._timedBonuses.Count - 1; i >= 0; i--)
			{
				if (this._timedBonuses[i].TakeTime(deltaTime))
				{
					Stat.ValuesWithEvent.OnDetachDelegate onDetach = this._timedBonuses[i]._onDetach;
					if (onDetach != null)
					{
						onDetach(this);
					}
					this._timedBonuses.RemoveAt(i);
					this._needUpdate = true;
				}
			}
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x0006FD85 File Offset: 0x0006DF85
		public void SetNeedUpdate()
		{
			this._needUpdate = true;
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x0006FD8E File Offset: 0x0006DF8E
		public bool UpdateIfNecessary()
		{
			if (this._needUpdate)
			{
				this.Update();
				return true;
			}
			return false;
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x0006FDA4 File Offset: 0x0006DFA4
		public void Update()
		{
			this._needUpdate = false;
			this.Initialize();
			for (int i = 0; i < this._bonuses.Count; i++)
			{
				this.Apply(this._bonuses[i]);
			}
			for (int j = 0; j < this._bonusesWithEvent.Count; j++)
			{
				this.Apply(this._bonusesWithEvent[j].values);
			}
			for (int k = 0; k < this._timedBonuses.Count; k++)
			{
				this.Apply(this._timedBonuses[k].values);
			}
			for (int l = 0; l < Stat.Kind.count; l++)
			{
				switch (Stat.Kind.values[l].valueForm)
				{
				case Stat.Kind.ValueForm.Constant:
					this._values[Stat.Category.Final.index, l] = this._values[Stat.Category.Constant.index, l] * (this._values[Stat.Category.Percent.index, l] * (1.0 + this._values[Stat.Category.PercentPoint.index, l])) + this._values[Stat.Category.Fixed.index, l];
					break;
				case Stat.Kind.ValueForm.Percent:
					this._values[Stat.Category.Final.index, l] = this._values[Stat.Category.Percent.index, l] * (1.0 + this._values[Stat.Category.PercentPoint.index, l]);
					break;
				case Stat.Kind.ValueForm.Product:
					this._values[Stat.Category.Final.index, l] = this._values[Stat.Category.Percent.index, l];
					break;
				}
			}
			foreach (Stat.OnUpdatedDelegate onUpdatedDelegate in this._onUpdated)
			{
				double[] array = onUpdatedDelegate(this._values);
				for (int m = 0; m < Stat.Kind.count; m++)
				{
					this._values[Stat.Category.Final.index, m] = array[m];
				}
			}
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x0006FFF4 File Offset: 0x0006E1F4
		public void Clear()
		{
			this._bonuses.Clear();
			for (int i = 0; i < this._bonusesWithEvent.Count; i++)
			{
				Stat.ValuesWithEvent.OnDetachDelegate onDetach = this._bonusesWithEvent[i]._onDetach;
				if (onDetach != null)
				{
					onDetach(this);
				}
			}
			this._bonusesWithEvent.Clear();
			for (int j = 0; j < this._timedBonuses.Count; j++)
			{
				Stat.ValuesWithEvent.OnDetachDelegate onDetach2 = this._timedBonuses[j]._onDetach;
				if (onDetach2 != null)
				{
					onDetach2(this);
				}
			}
			this._timedBonuses.Clear();
			this._needUpdate = true;
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x00070090 File Offset: 0x0006E290
		public void ClearNontimedValues()
		{
			this._bonuses.Clear();
			for (int i = 0; i < this._bonusesWithEvent.Count; i++)
			{
				Stat.ValuesWithEvent.OnDetachDelegate onDetach = this._bonusesWithEvent[i]._onDetach;
				if (onDetach != null)
				{
					onDetach(this);
				}
			}
			this._bonusesWithEvent.Clear();
			this._needUpdate = true;
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000700F0 File Offset: 0x0006E2F0
		public void ClearTimedValues()
		{
			for (int i = 0; i < this._timedBonuses.Count; i++)
			{
				Stat.ValuesWithEvent.OnDetachDelegate onDetach = this._timedBonuses[i]._onDetach;
				if (onDetach != null)
				{
					onDetach(this);
				}
			}
			this._timedBonuses.Clear();
			this._needUpdate = true;
		}

		// Token: 0x04001F57 RID: 8023
		private readonly Character _owner;

		// Token: 0x04001F58 RID: 8024
		private readonly double[,] _values = new double[Stat.Category.count, Stat.Kind.count];

		// Token: 0x04001F59 RID: 8025
		private bool _needUpdate;

		// Token: 0x04001F5A RID: 8026
		private readonly List<Stat.Values> _bonuses = new List<Stat.Values>();

		// Token: 0x04001F5B RID: 8027
		private readonly List<Stat.ValuesWithEvent> _bonusesWithEvent = new List<Stat.ValuesWithEvent>();

		// Token: 0x04001F5C RID: 8028
		private readonly List<Stat.TimedValues> _timedBonuses = new List<Stat.TimedValues>();

		// Token: 0x04001F5D RID: 8029
		public bool changeAttribute;

		// Token: 0x04001F5E RID: 8030
		public bool adaptiveAttribute;

		// Token: 0x04001F5F RID: 8031
		public Func<HitInfo, bool> IsChangeAttribute;

		// Token: 0x04001F60 RID: 8032
		private PriorityList<Stat.OnUpdatedDelegate> _onUpdated = new PriorityList<Stat.OnUpdatedDelegate>();

		// Token: 0x02000721 RID: 1825
		public sealed class Category
		{
			// Token: 0x0600252A RID: 9514 RVA: 0x00070144 File Offset: 0x0006E344
			static Category()
			{
				Stat.Category._names = (from kind in Stat.Category._values
				select kind.name).ToArray<string>();
				Stat.Category.values = Array.AsReadOnly<Stat.Category>(Stat.Category._values);
				Stat.Category.names = Array.AsReadOnly<string>(Stat.Category._names);
				Stat.Category.count = Stat.Category._count;
			}

			// Token: 0x0600252B RID: 9515 RVA: 0x00070220 File Offset: 0x0006E420
			private Category(string name, string format)
			{
				this.name = name;
				this.format = format;
				this.index = Stat.Category._count++;
			}

			// Token: 0x0600252C RID: 9516 RVA: 0x00070249 File Offset: 0x0006E449
			public override string ToString()
			{
				return this.name;
			}

			// Token: 0x04001F62 RID: 8034
			public static readonly Stat.Category Constant;

			// Token: 0x04001F63 RID: 8035
			public static readonly Stat.Category Fixed;

			// Token: 0x04001F64 RID: 8036
			public static readonly Stat.Category Percent;

			// Token: 0x04001F65 RID: 8037
			public static readonly Stat.Category PercentPoint;

			// Token: 0x04001F66 RID: 8038
			public static readonly Stat.Category Final;

			// Token: 0x04001F67 RID: 8039
			public static readonly int count;

			// Token: 0x04001F68 RID: 8040
			public static readonly ReadOnlyCollection<Stat.Category> values;

			// Token: 0x04001F69 RID: 8041
			public static readonly ReadOnlyCollection<string> names;

			// Token: 0x04001F6A RID: 8042
			private static readonly string[] _names;

			// Token: 0x04001F6B RID: 8043
			private static Stat.Category[] _values = new Stat.Category[]
			{
				Stat.Category.Constant = new Stat.Category("Constant", ""),
				Stat.Category.Fixed = new Stat.Category("Fixed", ""),
				Stat.Category.Percent = new Stat.Category("Percent", "%"),
				Stat.Category.PercentPoint = new Stat.Category("PercentPoint", "%p"),
				Stat.Category.Final = new Stat.Category("Final", "")
			};

			// Token: 0x04001F6C RID: 8044
			private static int _count;

			// Token: 0x04001F6D RID: 8045
			public readonly string name;

			// Token: 0x04001F6E RID: 8046
			public readonly string format;

			// Token: 0x04001F6F RID: 8047
			public readonly int index;
		}

		// Token: 0x02000723 RID: 1827
		public sealed class Kind
		{
			// Token: 0x06002530 RID: 9520 RVA: 0x00070268 File Offset: 0x0006E468
			static Kind()
			{
				Stat.Kind._names = (from kind in Stat.Kind._values
				select kind.name).ToArray<string>();
				Stat.Kind.values = Array.AsReadOnly<Stat.Kind>(Stat.Kind._values);
				Stat.Kind.names = Array.AsReadOnly<string>(Stat.Kind._names);
				Stat.Kind.count = Stat.Kind._count;
			}

			// Token: 0x06002531 RID: 9521 RVA: 0x0007068A File Offset: 0x0006E88A
			private Kind(string name, Stat.Kind.ValueForm type)
			{
				this.name = name;
				this.valueForm = type;
				this.index = Stat.Kind._count++;
			}

			// Token: 0x06002532 RID: 9522 RVA: 0x000706B3 File Offset: 0x0006E8B3
			public override string ToString()
			{
				return this.name;
			}

			// Token: 0x04001F71 RID: 8049
			public static readonly Stat.Kind Health;

			// Token: 0x04001F72 RID: 8050
			public static readonly Stat.Kind AttackDamage;

			// Token: 0x04001F73 RID: 8051
			public static readonly Stat.Kind PhysicalAttackDamage;

			// Token: 0x04001F74 RID: 8052
			public static readonly Stat.Kind MagicAttackDamage;

			// Token: 0x04001F75 RID: 8053
			public static readonly Stat.Kind TakingDamage;

			// Token: 0x04001F76 RID: 8054
			public static readonly Stat.Kind TakingPhysicalDamage;

			// Token: 0x04001F77 RID: 8055
			public static readonly Stat.Kind TakingMagicDamage;

			// Token: 0x04001F78 RID: 8056
			public static readonly Stat.Kind AttackSpeed;

			// Token: 0x04001F79 RID: 8057
			public static readonly Stat.Kind MovementSpeed;

			// Token: 0x04001F7A RID: 8058
			public static readonly Stat.Kind CriticalChance;

			// Token: 0x04001F7B RID: 8059
			public static readonly Stat.Kind CriticalDamage;

			// Token: 0x04001F7C RID: 8060
			public static readonly Stat.Kind EvasionChance;

			// Token: 0x04001F7D RID: 8061
			public static readonly Stat.Kind MeleeEvasionChance;

			// Token: 0x04001F7E RID: 8062
			public static readonly Stat.Kind RangedEvasionChance;

			// Token: 0x04001F7F RID: 8063
			public static readonly Stat.Kind ProjectileEvasionChance;

			// Token: 0x04001F80 RID: 8064
			public static readonly Stat.Kind StoppingResistance;

			// Token: 0x04001F81 RID: 8065
			public static readonly Stat.Kind KnockbackResistance;

			// Token: 0x04001F82 RID: 8066
			public static readonly Stat.Kind StatusResistance;

			// Token: 0x04001F83 RID: 8067
			public static readonly Stat.Kind StoppingPower;

			// Token: 0x04001F84 RID: 8068
			public static readonly Stat.Kind BasicAttackDamage;

			// Token: 0x04001F85 RID: 8069
			public static readonly Stat.Kind SkillAttackDamage;

			// Token: 0x04001F86 RID: 8070
			public static readonly Stat.Kind CooldownSpeed;

			// Token: 0x04001F87 RID: 8071
			public static readonly Stat.Kind SkillCooldownSpeed;

			// Token: 0x04001F88 RID: 8072
			public static readonly Stat.Kind DashCooldownSpeed;

			// Token: 0x04001F89 RID: 8073
			public static readonly Stat.Kind SwapCooldownSpeed;

			// Token: 0x04001F8A RID: 8074
			public static readonly Stat.Kind EssenceCooldownSpeed;

			// Token: 0x04001F8B RID: 8075
			public static readonly Stat.Kind BuffDuration;

			// Token: 0x04001F8C RID: 8076
			public static readonly Stat.Kind BasicAttackSpeed;

			// Token: 0x04001F8D RID: 8077
			public static readonly Stat.Kind SkillAttackSpeed;

			// Token: 0x04001F8E RID: 8078
			public static readonly Stat.Kind CharacterSize;

			// Token: 0x04001F8F RID: 8079
			public static readonly Stat.Kind DashDistance;

			// Token: 0x04001F90 RID: 8080
			public static readonly Stat.Kind StunResistance;

			// Token: 0x04001F91 RID: 8081
			public static readonly Stat.Kind FreezeResistance;

			// Token: 0x04001F92 RID: 8082
			public static readonly Stat.Kind BurnResistance;

			// Token: 0x04001F93 RID: 8083
			public static readonly Stat.Kind BleedResistance;

			// Token: 0x04001F94 RID: 8084
			public static readonly Stat.Kind PoisonResistance;

			// Token: 0x04001F95 RID: 8085
			public static readonly Stat.Kind PoisonTickFrequency;

			// Token: 0x04001F96 RID: 8086
			public static readonly Stat.Kind BleedDamage;

			// Token: 0x04001F97 RID: 8087
			public static readonly Stat.Kind EmberDamage;

			// Token: 0x04001F98 RID: 8088
			public static readonly Stat.Kind FreezeDuration;

			// Token: 0x04001F99 RID: 8089
			public static readonly Stat.Kind StunDuration;

			// Token: 0x04001F9A RID: 8090
			public static readonly Stat.Kind SpiritAttackCooldownSpeed;

			// Token: 0x04001F9B RID: 8091
			public static readonly Stat.Kind ProjectileAttackDamage;

			// Token: 0x04001F9C RID: 8092
			public static readonly Stat.Kind TakingHealAmount;

			// Token: 0x04001F9D RID: 8093
			public static readonly Stat.Kind ChargingSpeed;

			// Token: 0x04001F9E RID: 8094
			public static readonly Stat.Kind IgnoreDamageReduction;

			// Token: 0x04001F9F RID: 8095
			public static readonly int count;

			// Token: 0x04001FA0 RID: 8096
			public static readonly ReadOnlyCollection<Stat.Kind> values;

			// Token: 0x04001FA1 RID: 8097
			public static readonly ReadOnlyCollection<string> names;

			// Token: 0x04001FA2 RID: 8098
			private static readonly string[] _names;

			// Token: 0x04001FA3 RID: 8099
			private static readonly Stat.Kind[] _values = new Stat.Kind[]
			{
				Stat.Kind.Health = new Stat.Kind("체력", Stat.Kind.ValueForm.Constant),
				Stat.Kind.AttackDamage = new Stat.Kind("공격력/모두", Stat.Kind.ValueForm.Percent),
				Stat.Kind.PhysicalAttackDamage = new Stat.Kind("공격력/물리", Stat.Kind.ValueForm.Percent),
				Stat.Kind.MagicAttackDamage = new Stat.Kind("공격력/마법", Stat.Kind.ValueForm.Percent),
				Stat.Kind.TakingDamage = new Stat.Kind("받는피해감소/모두", Stat.Kind.ValueForm.Product),
				Stat.Kind.TakingPhysicalDamage = new Stat.Kind("받는피해감소/물리", Stat.Kind.ValueForm.Product),
				Stat.Kind.TakingMagicDamage = new Stat.Kind("받는피해감소/마법", Stat.Kind.ValueForm.Product),
				Stat.Kind.AttackSpeed = new Stat.Kind("공격속도/모두", Stat.Kind.ValueForm.Percent),
				Stat.Kind.MovementSpeed = new Stat.Kind("이동속도", Stat.Kind.ValueForm.Constant),
				Stat.Kind.CriticalChance = new Stat.Kind("치명타 확률", Stat.Kind.ValueForm.Percent),
				Stat.Kind.CriticalDamage = new Stat.Kind("치명타 피해량", Stat.Kind.ValueForm.Percent),
				Stat.Kind.EvasionChance = new Stat.Kind("회피율/모두", Stat.Kind.ValueForm.Product),
				Stat.Kind.MeleeEvasionChance = new Stat.Kind("회피율/근접", Stat.Kind.ValueForm.Product),
				Stat.Kind.RangedEvasionChance = new Stat.Kind("회피율/원거리", Stat.Kind.ValueForm.Product),
				Stat.Kind.StoppingResistance = new Stat.Kind("경직저항", Stat.Kind.ValueForm.Product),
				Stat.Kind.KnockbackResistance = new Stat.Kind("넉백저항", Stat.Kind.ValueForm.Product),
				Stat.Kind.StatusResistance = new Stat.Kind("상태이상저항/모두", Stat.Kind.ValueForm.Product),
				Stat.Kind.ProjectileEvasionChance = new Stat.Kind("회피율/투사체", Stat.Kind.ValueForm.Product),
				Stat.Kind.StoppingPower = new Stat.Kind("저지력", Stat.Kind.ValueForm.Product),
				Stat.Kind.BasicAttackDamage = new Stat.Kind("공격력/기본", Stat.Kind.ValueForm.Percent),
				Stat.Kind.SkillAttackDamage = new Stat.Kind("공격력/스킬", Stat.Kind.ValueForm.Percent),
				Stat.Kind.CooldownSpeed = new Stat.Kind("쿨다운가속/모두", Stat.Kind.ValueForm.Percent),
				Stat.Kind.SkillCooldownSpeed = new Stat.Kind("쿨다운가속/스킬", Stat.Kind.ValueForm.Percent),
				Stat.Kind.DashCooldownSpeed = new Stat.Kind("쿨다운가속/대시", Stat.Kind.ValueForm.Percent),
				Stat.Kind.SwapCooldownSpeed = new Stat.Kind("쿨다운가속/교대", Stat.Kind.ValueForm.Percent),
				Stat.Kind.EssenceCooldownSpeed = new Stat.Kind("쿨다운가속/정수", Stat.Kind.ValueForm.Percent),
				Stat.Kind.BuffDuration = new Stat.Kind("버프 지속시간", Stat.Kind.ValueForm.Product),
				Stat.Kind.BasicAttackSpeed = new Stat.Kind("공격속도/기본", Stat.Kind.ValueForm.Percent),
				Stat.Kind.SkillAttackSpeed = new Stat.Kind("공격속도/스킬", Stat.Kind.ValueForm.Percent),
				Stat.Kind.CharacterSize = new Stat.Kind("크기", Stat.Kind.ValueForm.Percent),
				Stat.Kind.DashDistance = new Stat.Kind("대시거리", Stat.Kind.ValueForm.Percent),
				Stat.Kind.StunResistance = new Stat.Kind("상태이상저항/스턴", Stat.Kind.ValueForm.Product),
				Stat.Kind.FreezeResistance = new Stat.Kind("상태이상저항/빙결", Stat.Kind.ValueForm.Product),
				Stat.Kind.BurnResistance = new Stat.Kind("상태이상저항/화상", Stat.Kind.ValueForm.Product),
				Stat.Kind.BleedResistance = new Stat.Kind("상태이상저항/출혈", Stat.Kind.ValueForm.Product),
				Stat.Kind.PoisonResistance = new Stat.Kind("상태이상저항/중독", Stat.Kind.ValueForm.Product),
				Stat.Kind.PoisonTickFrequency = new Stat.Kind("상태이상/중독피해빈도감소량", Stat.Kind.ValueForm.Constant),
				Stat.Kind.BleedDamage = new Stat.Kind("상태이상/출혈데미지", Stat.Kind.ValueForm.Percent),
				Stat.Kind.EmberDamage = new Stat.Kind("상태이상/화상주변데미지", Stat.Kind.ValueForm.Percent),
				Stat.Kind.FreezeDuration = new Stat.Kind("상태이상/추가빙결지속시간", Stat.Kind.ValueForm.Constant),
				Stat.Kind.StunDuration = new Stat.Kind("상태이상/추가스턴지속시간", Stat.Kind.ValueForm.Constant),
				Stat.Kind.SpiritAttackCooldownSpeed = new Stat.Kind("정령/쿨다운가속", Stat.Kind.ValueForm.Percent),
				Stat.Kind.ProjectileAttackDamage = new Stat.Kind("공격력/투사체", Stat.Kind.ValueForm.Percent),
				Stat.Kind.TakingHealAmount = new Stat.Kind("받는 회복량", Stat.Kind.ValueForm.Percent),
				Stat.Kind.ChargingSpeed = new Stat.Kind("공격속도/차지", Stat.Kind.ValueForm.Percent),
				Stat.Kind.IgnoreDamageReduction = new Stat.Kind("피해량감소무시", Stat.Kind.ValueForm.Percent)
			};

			// Token: 0x04001FA4 RID: 8100
			private static int _count;

			// Token: 0x04001FA5 RID: 8101
			public readonly Stat.Kind.ValueForm valueForm;

			// Token: 0x04001FA6 RID: 8102
			public readonly string name;

			// Token: 0x04001FA7 RID: 8103
			public readonly int index;

			// Token: 0x02000724 RID: 1828
			public enum ValueForm
			{
				// Token: 0x04001FA9 RID: 8105
				Constant,
				// Token: 0x04001FAA RID: 8106
				Percent,
				// Token: 0x04001FAB RID: 8107
				Product
			}
		}

		// Token: 0x02000726 RID: 1830
		[Serializable]
		public sealed class Value : ICloneable
		{
			// Token: 0x170007C0 RID: 1984
			// (get) Token: 0x06002536 RID: 9526 RVA: 0x000706CF File Offset: 0x0006E8CF
			internal static string positiveString
			{
				get
				{
					return Localization.GetLocalizedString("stat_Positive");
				}
			}

			// Token: 0x170007C1 RID: 1985
			// (get) Token: 0x06002537 RID: 9527 RVA: 0x000706DB File Offset: 0x0006E8DB
			internal static string negativeString
			{
				get
				{
					return Localization.GetLocalizedString("stat_Negative");
				}
			}

			// Token: 0x06002538 RID: 9528 RVA: 0x000706E7 File Offset: 0x0006E8E7
			public Value(Stat.Category category, Stat.Kind kind, double value)
			{
				this.categoryIndex = category.index;
				this.kindIndex = kind.index;
				this.value = value;
			}

			// Token: 0x06002539 RID: 9529 RVA: 0x0007070E File Offset: 0x0006E90E
			public Value(int categoryIndex, int kindIndex, double value)
			{
				this.categoryIndex = categoryIndex;
				this.kindIndex = kindIndex;
				this.value = value;
			}

			// Token: 0x0600253A RID: 9530 RVA: 0x0007072B File Offset: 0x0006E92B
			public Stat.Value Clone()
			{
				return new Stat.Value(this.categoryIndex, this.kindIndex, this.value);
			}

			// Token: 0x0600253B RID: 9531 RVA: 0x00070744 File Offset: 0x0006E944
			object ICloneable.Clone()
			{
				return this.Clone();
			}

			// Token: 0x0600253C RID: 9532 RVA: 0x0007074C File Offset: 0x0006E94C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsCategory(Stat.Category category)
			{
				return this.categoryIndex == category.index;
			}

			// Token: 0x0600253D RID: 9533 RVA: 0x0007075C File Offset: 0x0006E95C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsCategory(int categoryIndex)
			{
				return this.categoryIndex == categoryIndex;
			}

			// Token: 0x0600253E RID: 9534 RVA: 0x00070767 File Offset: 0x0006E967
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsKind(Stat.Kind.ValueForm valueForm)
			{
				return Stat.Kind.values[this.kindIndex].valueForm == valueForm;
			}

			// Token: 0x0600253F RID: 9535 RVA: 0x00070784 File Offset: 0x0006E984
			public double GetStackedValue(double stacks)
			{
				if (this.IsKind(Stat.Kind.ValueForm.Product))
				{
					return 1.0 - (1.0 - this.value) * stacks;
				}
				if (this.IsCategory(Stat.Category.Percent))
				{
					return 1.0 + this.value * stacks;
				}
				return this.value * stacks;
			}

			// Token: 0x06002540 RID: 9536 RVA: 0x000707DF File Offset: 0x0006E9DF
			public double GetMultipliedValue(float multiplier)
			{
				if (this.IsCategory(Stat.Category.Percent))
				{
					return (this.value - 1.0) * (double)multiplier + 1.0;
				}
				return (double)multiplier * this.value;
			}

			// Token: 0x04001FAD RID: 8109
			public static readonly Stat.Value blockMovement = new Stat.Value(Stat.Category.Percent, Stat.Kind.MovementSpeed, 0.0);

			// Token: 0x04001FAE RID: 8110
			public int categoryIndex;

			// Token: 0x04001FAF RID: 8111
			public int kindIndex;

			// Token: 0x04001FB0 RID: 8112
			public double value;
		}

		// Token: 0x02000727 RID: 1831
		[Serializable]
		public class Values : ReorderableArray<Stat.Value>, ICloneable
		{
			// Token: 0x170007C2 RID: 1986
			// (get) Token: 0x06002542 RID: 9538 RVA: 0x00070834 File Offset: 0x0006EA34
			public List<string> strings
			{
				get
				{
					List<string> result;
					if ((result = this._strings) == null)
					{
						result = (this._strings = (from v in this.values
						select v.ToString()).ToList<string>());
					}
					return result;
				}
			}

			// Token: 0x06002543 RID: 9539 RVA: 0x00070883 File Offset: 0x0006EA83
			public Values(params Stat.Value[] values)
			{
				this.values = values;
			}

			// Token: 0x06002544 RID: 9540 RVA: 0x00070894 File Offset: 0x0006EA94
			public Stat.Values Clone()
			{
				Stat.Value[] array = new Stat.Value[this.values.Length];
				for (int i = 0; i < this.values.Length; i++)
				{
					array[i] = this.values[i].Clone();
				}
				return new Stat.Values(array);
			}

			// Token: 0x06002545 RID: 9541 RVA: 0x000708D8 File Offset: 0x0006EAD8
			object ICloneable.Clone()
			{
				return this.Clone();
			}

			// Token: 0x04001FB1 RID: 8113
			public static readonly Stat.Values blockMovement = new Stat.Values(new Stat.Value[]
			{
				Stat.Value.blockMovement
			});

			// Token: 0x04001FB2 RID: 8114
			private List<string> _strings;
		}

		// Token: 0x02000729 RID: 1833
		public class ValuesWithEvent
		{
			// Token: 0x0600254A RID: 9546 RVA: 0x0007090E File Offset: 0x0006EB0E
			internal ValuesWithEvent(Stat.Values values, Stat.ValuesWithEvent.OnDetachDelegate onDetach)
			{
				this.values = values;
				this._onDetach = onDetach;
			}

			// Token: 0x04001FB5 RID: 8117
			public readonly Stat.Values values;

			// Token: 0x04001FB6 RID: 8118
			internal Stat.ValuesWithEvent.OnDetachDelegate _onDetach;

			// Token: 0x0200072A RID: 1834
			// (Invoke) Token: 0x0600254C RID: 9548
			public delegate void OnDetachDelegate(Stat stat);
		}

		// Token: 0x0200072B RID: 1835
		private class TimedValues : Stat.ValuesWithEvent
		{
			// Token: 0x0600254F RID: 9551 RVA: 0x00070924 File Offset: 0x0006EB24
			public TimedValues(Stat.Values values, float duration, Stat.ValuesWithEvent.OnDetachDelegate onDetach = null) : base(values, onDetach)
			{
				this._timeToExpire = duration;
			}

			// Token: 0x06002550 RID: 9552 RVA: 0x00070935 File Offset: 0x0006EB35
			public void SetTime(float time)
			{
				this._timeToExpire = time;
			}

			// Token: 0x06002551 RID: 9553 RVA: 0x00070940 File Offset: 0x0006EB40
			public bool TakeTime(float time)
			{
				return (this._timeToExpire -= time) <= 0f;
			}

			// Token: 0x04001FB7 RID: 8119
			private float _timeToExpire;
		}

		// Token: 0x0200072C RID: 1836
		// (Invoke) Token: 0x06002553 RID: 9555
		public delegate double[] OnUpdatedDelegate(double[,] values);
	}
}
