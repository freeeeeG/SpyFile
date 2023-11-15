using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006FA RID: 1786
	[Serializable]
	public class CharacterSynchronization
	{
		// Token: 0x06002402 RID: 9218 RVA: 0x0006C53C File Offset: 0x0006A73C
		public void Synchronize(Character source, Character target)
		{
			if (this._lookingDirection)
			{
				source.ForceToLookAt(target.lookingDirection);
			}
			if (this._posistion)
			{
				source.transform.position = target.transform.position;
			}
			if (this._overrideDamageStat)
			{
				source.stat.getDamageOverridingStat = target.stat;
			}
			if (this._attachedStats != null)
			{
				source.stat.DetachValues(this._attachedStats);
			}
			source.stat.adaptiveAttribute = target.stat.adaptiveAttribute;
			List<Stat.Value> list;
			if (this._statsToCopy.values.Length != 0)
			{
				list = this._statsToCopy.values.ToList<Stat.Value>();
			}
			else
			{
				list = new List<Stat.Value>();
			}
			if (this._copyDamageStat)
			{
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.AttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.BasicAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.SkillAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.PhysicalAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.MagicAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.ProjectileAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.ProjectileAttackDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.CriticalDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalDamage, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.CriticalChance, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.CriticalChance, 0.0));
			}
			if (this._copyAttackSpeedStat)
			{
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackSpeed, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.AttackSpeed, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.BasicAttackSpeed, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 0.0));
				list.Add(new Stat.Value(Stat.Category.Percent, Stat.Kind.SkillAttackSpeed, 0.0));
				list.Add(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 0.0));
			}
			this._attachedStats = new Stat.Values(list.ToArray());
			if (this._attachedStats.values.Length != 0)
			{
				for (int i = 0; i < this._attachedStats.values.Length; i++)
				{
					Stat.Value value = this._attachedStats.values[i];
					value.value = target.stat.Get(value.categoryIndex, value.kindIndex);
				}
				source.stat.AttachOrUpdateValues(this._attachedStats);
				source.stat.SetNeedUpdate();
			}
		}

		// Token: 0x04001EC8 RID: 7880
		[SerializeField]
		private bool _posistion = true;

		// Token: 0x04001EC9 RID: 7881
		[SerializeField]
		private bool _lookingDirection = true;

		// Token: 0x04001ECA RID: 7882
		[SerializeField]
		private bool _overrideDamageStat;

		// Token: 0x04001ECB RID: 7883
		[SerializeField]
		private bool _copyDamageStat = true;

		// Token: 0x04001ECC RID: 7884
		[SerializeField]
		private bool _copyAttackSpeedStat;

		// Token: 0x04001ECD RID: 7885
		[SerializeField]
		private Stat.Values _statsToCopy;

		// Token: 0x04001ECE RID: 7886
		private Stat.Values _attachedStats;
	}
}
