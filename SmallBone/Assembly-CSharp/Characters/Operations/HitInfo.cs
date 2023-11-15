using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations
{
	// Token: 0x02000E1C RID: 3612
	[Serializable]
	public class HitInfo
	{
		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06004815 RID: 18453 RVA: 0x000D1D1B File Offset: 0x000CFF1B
		public Damage.Attribute attribute
		{
			get
			{
				return this._attribute;
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06004816 RID: 18454 RVA: 0x000D1D23 File Offset: 0x000CFF23
		public Damage.AttackType attackType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06004817 RID: 18455 RVA: 0x000D1D2B File Offset: 0x000CFF2B
		public Damage.MotionType motionType
		{
			get
			{
				return this._motionType;
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06004818 RID: 18456 RVA: 0x000D1D33 File Offset: 0x000CFF33
		// (set) Token: 0x06004819 RID: 18457 RVA: 0x000D1D3B File Offset: 0x000CFF3B
		public float damageMultiplier
		{
			get
			{
				return this._damageMultiplier;
			}
			set
			{
				this._damageMultiplier = value;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x0600481A RID: 18458 RVA: 0x000D1D44 File Offset: 0x000CFF44
		public short priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x0600481B RID: 18459 RVA: 0x000D1D4C File Offset: 0x000CFF4C
		public float stoppingPower
		{
			get
			{
				return this._stoppingPower;
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x0600481C RID: 18460 RVA: 0x000D1D54 File Offset: 0x000CFF54
		public float extraCriticalChance
		{
			get
			{
				return this._extraCriticalChance;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x0600481D RID: 18461 RVA: 0x000D1D5C File Offset: 0x000CFF5C
		public float extraCriticalDamage
		{
			get
			{
				return this._extraCriticalDamage;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x0600481E RID: 18462 RVA: 0x000D1D64 File Offset: 0x000CFF64
		public string key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x000D1D6C File Offset: 0x000CFF6C
		public HitInfo(Damage.AttackType type)
		{
			this._type = type;
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x000D1D86 File Offset: 0x000CFF86
		public void ChangeAdaptiveDamageAttribute(Character owner)
		{
			this._attribute = owner.stat.GetAdaptiveForceAttribute();
		}

		// Token: 0x0400372D RID: 14125
		[SerializeField]
		private Damage.Attribute _attribute;

		// Token: 0x0400372E RID: 14126
		[SerializeField]
		private Damage.AttackType _type;

		// Token: 0x0400372F RID: 14127
		[FormerlySerializedAs("_attackType")]
		[SerializeField]
		private Damage.MotionType _motionType;

		// Token: 0x04003730 RID: 14128
		[SerializeField]
		private float _damageMultiplier = 1f;

		// Token: 0x04003731 RID: 14129
		[SerializeField]
		private short _priority;

		// Token: 0x04003732 RID: 14130
		[SerializeField]
		private float _stoppingPower;

		// Token: 0x04003733 RID: 14131
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04003734 RID: 14132
		[SerializeField]
		private float _extraCriticalDamage;

		// Token: 0x04003735 RID: 14133
		[SerializeField]
		private string _key;
	}
}
