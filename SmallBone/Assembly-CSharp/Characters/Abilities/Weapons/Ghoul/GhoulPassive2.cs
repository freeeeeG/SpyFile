using System;
using System.Linq;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Weapons.Gauges;
using Level;
using UnityEngine;

namespace Characters.Abilities.Weapons.Ghoul
{
	// Token: 0x02000C22 RID: 3106
	[Serializable]
	public sealed class GhoulPassive2 : Ability, IAbilityInstance
	{
		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003FDF RID: 16351 RVA: 0x000B968D File Offset: 0x000B788D
		// (set) Token: 0x06003FE0 RID: 16352 RVA: 0x000B9695 File Offset: 0x000B7895
		public Character owner { get; set; }

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x000B969E File Offset: 0x000B789E
		// (set) Token: 0x06003FE3 RID: 16355 RVA: 0x000B96A6 File Offset: 0x000B78A6
		public float remainTime { get; set; }

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06003FE5 RID: 16357 RVA: 0x000B96AF File Offset: 0x000B78AF
		public Sprite icon
		{
			get
			{
				if (this._stacks <= 0)
				{
					return null;
				}
				return this._defaultIcon;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x000B96C2 File Offset: 0x000B78C2
		public float iconFillAmount
		{
			get
			{
				return 1f - this.remainTime / base.duration;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06003FE7 RID: 16359 RVA: 0x000B96D7 File Offset: 0x000B78D7
		public int iconStacks
		{
			get
			{
				return (int)((float)this._stacks * this._iconStacksPerStack);
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x000B96E8 File Offset: 0x000B78E8
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			Vector3 size = this._fleshPrefab.GetComponent<Collider2D>().bounds.size;
			this._spawnOffset.y = size.y * 0.6f;
			this._damageMultiperByStack.duration = (float)this._maxStack;
			if (this._used)
			{
				this._grayHealthPassive = new GhoulPassive2.GreyHealthPassive(owner.health.grayHealth, this._grayHealthDecreaseSpeed, this._grayHealthLifeTime);
			}
			return this;
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x000B976C File Offset: 0x000B796C
		public void Attach()
		{
			this.remainTime = base.duration;
			this._stat = this._statPerStack.Clone();
			this._stacks = 0;
			this.owner.stat.AttachValues(this._stat);
			this.UpdateStack();
			this.owner.health.onTookDamage += new TookDamageDelegate(this.AddGrayHealth);
			this.owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			Character owner2 = this.owner;
			owner2.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner2.onKilled, new Character.OnKilledDelegate(this.OnOwnerKill));
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x000B9844 File Offset: 0x000B7A44
		private void AddGrayHealth(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!this._used)
			{
				return;
			}
			if (tookDamage.attackType != Damage.AttackType.None)
			{
				Damage damage = tookDamage;
				if (damage.amount != 0.0)
				{
					if (!this.owner.health.shield.hasAny)
					{
						GrayHealth grayHealth = this.owner.health.grayHealth;
						double maximum = grayHealth.maximum;
						damage = tookDamage;
						grayHealth.maximum = maximum + damage.amount;
					}
					this._grayHealthPassive.Refresh();
					return;
				}
			}
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x000B98C8 File Offset: 0x000B7AC8
		private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
		{
			if (target.character == null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
			{
				return false;
			}
			string damageKey = damage.key;
			if (this._damageMultiplierByStackKey.Any((string key) => damageKey.Equals(key, StringComparison.OrdinalIgnoreCase)))
			{
				damage.multiplier *= (double)this._cachedDamageMultiplierByStack;
			}
			return false;
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x000B9940 File Offset: 0x000B7B40
		private void OnOwnerKill(ITarget target, ref Damage damage)
		{
			if (target.character == null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
			{
				return;
			}
			string damageKey = damage.key;
			if (this._killConsumeKey.Any((string key) => damageKey.Equals(key, StringComparison.OrdinalIgnoreCase)))
			{
				this.AddStack();
			}
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x000B99AC File Offset: 0x000B7BAC
		public void Detach()
		{
			this.DestroyGrayHealth();
			this.owner.stat.DetachValues(this._stat);
			this.owner.health.onTookDamage -= new TookDamageDelegate(this.AddGrayHealth);
			this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x000B9A38 File Offset: 0x000B7C38
		public void UpdateTime(float deltaTime)
		{
			this.UpdateStack();
			if (this._stacks > 0)
			{
				this.remainTime -= deltaTime;
				if (this.remainTime <= 0f)
				{
					this._stacks = 0;
				}
				if (this._stacks >= this._minStack)
				{
					this._gaugeAnimationTime += deltaTime * 2f;
					if (this._gaugeAnimationTime > 2f)
					{
						this._gaugeAnimationTime = 0f;
					}
					this._gauge.defaultBarGaugeColor.baseColor = Color.LerpUnclamped(this._defaultColor, this._canUseColor, (this._gaugeAnimationTime < 1f) ? this._gaugeAnimationTime : (2f - this._gaugeAnimationTime));
				}
				else
				{
					this._gauge.defaultBarGaugeColor.baseColor = this._defaultColor;
				}
			}
			if (this.owner.health.grayHealth.maximum <= 0.0)
			{
				return;
			}
			if (this._used)
			{
				this._grayHealthPassive.Update(deltaTime);
				this.UpdateCanHealAmount();
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x000B9B48 File Offset: 0x000B7D48
		public void Recover()
		{
			if (this._used && this.owner.health.grayHealth.canHeal > 0.0)
			{
				this.owner.health.GrayHeal();
			}
			this._cachedDamageMultiplierByStack = 1f + this._damageMultiperByStack.Evaluate((float)this._stacks);
			this._stacks = 0;
			this.UpdateStack();
			if (this._used)
			{
				this._grayHealthPassive.Refresh();
			}
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x000B9BCC File Offset: 0x000B7DCC
		private void UpdateCanHealAmount()
		{
			double maximum = this.owner.health.grayHealth.maximum;
			float time = (float)this._stacks / (float)this._maxStack;
			this.owner.health.grayHealth.canHeal = maximum * (double)this._recoveryCurve.Evaluate(time);
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x000B9C23 File Offset: 0x000B7E23
		private void DestroyGrayHealth()
		{
			if (this._used)
			{
				this.owner.health.grayHealth.maximum = 0.0;
			}
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x000B9C4B File Offset: 0x000B7E4B
		public void AddStack()
		{
			this.remainTime = base.duration;
			if (this._stacks < this._maxStack)
			{
				this._stacks++;
			}
			this.UpdateStack();
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x000B9C7C File Offset: 0x000B7E7C
		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (target.character == null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
			{
				return;
			}
			if (!this._attackTypeFilter[gaveDamage.attackType])
			{
				return;
			}
			if (!this._motionTypeFilter[gaveDamage.motionType])
			{
				return;
			}
			string damageKey = gaveDamage.key;
			if (this._consumeKey.Any((string key) => damageKey.Equals(key, StringComparison.OrdinalIgnoreCase)))
			{
				this.AddStack();
				return;
			}
			if (!MMMaths.PercentChance(this._dropPossibility))
			{
				return;
			}
			this._fleshPrefab.Spawn(gaveDamage.hitPoint + this._spawnOffset, this);
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x000B9D40 File Offset: 0x000B7F40
		private void UpdateStack()
		{
			for (int i = 0; i < this._stat.values.Length; i++)
			{
				Stat.Value value = this._stat.values[i];
				if (value.kindIndex == Stat.Kind.PhysicalAttackDamage.index && this._consumeAbility != null && this.owner.ability.Contains(this._consumeAbility.ability))
				{
					value.value = this._statPerStack.values[i].GetStackedValue((double)(this._stacks + (int)this._consumeAbility.stack * this._consumeAbilityStackMultiplier));
				}
				else
				{
					value.value = this._statPerStack.values[i].GetStackedValue((double)this._stacks);
				}
			}
			this.owner.stat.SetNeedUpdate();
			this._gauge.Set((float)this._stacks);
			if (this._minStack >= this._stacks)
			{
				this._gauge.defaultBarGaugeColor.baseColor = this._defaultColor;
				return;
			}
			this._gauge.defaultBarGaugeColor.baseColor = this._canUseColor;
		}

		// Token: 0x04003128 RID: 12584
		[Header("회색 체력")]
		[SerializeField]
		private bool _used = true;

		// Token: 0x04003129 RID: 12585
		[SerializeField]
		private float _grayHealthLifeTime;

		// Token: 0x0400312A RID: 12586
		[Tooltip("초당 줄어드는 양")]
		[SerializeField]
		private float _grayHealthDecreaseSpeed = 1f;

		// Token: 0x0400312B RID: 12587
		[SerializeField]
		private Curve _recoveryCurve;

		// Token: 0x0400312C RID: 12588
		private GhoulPassive2.GreyHealthPassive _grayHealthPassive;

		// Token: 0x0400312D RID: 12589
		[Header("살덩이")]
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x0400312E RID: 12590
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x0400312F RID: 12591
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04003130 RID: 12592
		[SerializeField]
		private Color _defaultColor;

		// Token: 0x04003131 RID: 12593
		[SerializeField]
		private Color _canUseColor;

		// Token: 0x04003132 RID: 12594
		[SerializeField]
		private DroppedGhoulFlesh _fleshPrefab;

		// Token: 0x04003133 RID: 12595
		[Range(1f, 100f)]
		[SerializeField]
		private int _dropPossibility;

		// Token: 0x04003134 RID: 12596
		private float _gaugeAnimationTime;

		// Token: 0x04003135 RID: 12597
		private Vector2 _spawnOffset;

		// Token: 0x04003136 RID: 12598
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private float _iconStacksPerStack = 1f;

		// Token: 0x04003137 RID: 12599
		[SerializeField]
		private int _minStack;

		// Token: 0x04003138 RID: 12600
		[SerializeField]
		private int _maxStack;

		// Token: 0x04003139 RID: 12601
		private int _stacks;

		// Token: 0x0400313A RID: 12602
		[SerializeField]
		[Header("살덩이 스텟")]
		private Stat.Values _statPerStack;

		// Token: 0x0400313B RID: 12603
		[SerializeField]
		private Curve _damageMultiperByStack;

		// Token: 0x0400313C RID: 12604
		private float _cachedDamageMultiplierByStack;

		// Token: 0x0400313D RID: 12605
		private Stat.Values _stat;

		// Token: 0x0400313E RID: 12606
		[Header("컨슘")]
		[SerializeField]
		private string[] _consumeKey = new string[]
		{
			"consume"
		};

		// Token: 0x0400313F RID: 12607
		[SerializeField]
		private string[] _killConsumeKey = new string[]
		{
			"kill"
		};

		// Token: 0x04003140 RID: 12608
		[SerializeField]
		private string[] _damageMultiplierByStackKey = new string[]
		{
			"gas"
		};

		// Token: 0x04003141 RID: 12609
		[SerializeField]
		private int _consumeAbilityStackMultiplier = 2;

		// Token: 0x04003142 RID: 12610
		[SerializeField]
		private StackableStatBonusComponent _consumeAbility;

		// Token: 0x02000C23 RID: 3107
		private class GreyHealthPassive
		{
			// Token: 0x06003FF8 RID: 16376 RVA: 0x000B9EDB File Offset: 0x000B80DB
			internal GreyHealthPassive(GrayHealth grayHealth, float decreasingSpeed, float freezeTime)
			{
				this._freezeTime = freezeTime;
				this._decreasingSpeed = decreasingSpeed;
				this._elapsed = 0f;
				this._grayHealth = grayHealth;
			}

			// Token: 0x06003FF9 RID: 16377 RVA: 0x000B9F04 File Offset: 0x000B8104
			internal void Update(float deltaTime)
			{
				if (this._grayHealth.maximum <= 0.0)
				{
					return;
				}
				this._elapsed += deltaTime;
				if (this._elapsed > this._freezeTime)
				{
					this._decreasing = true;
				}
				else
				{
					this._decreasing = false;
				}
				if (this._decreasing)
				{
					this._grayHealth.maximum -= (double)(this._decreasingSpeed * Chronometer.global.deltaTime);
				}
			}

			// Token: 0x06003FFA RID: 16378 RVA: 0x000B9F80 File Offset: 0x000B8180
			internal void Refresh()
			{
				this._elapsed = 0f;
			}

			// Token: 0x04003143 RID: 12611
			private float _freezeTime;

			// Token: 0x04003144 RID: 12612
			private bool _decreasing;

			// Token: 0x04003145 RID: 12613
			private float _decreasingSpeed;

			// Token: 0x04003146 RID: 12614
			private float _elapsed;

			// Token: 0x04003147 RID: 12615
			private GrayHealth _grayHealth;
		}
	}
}
