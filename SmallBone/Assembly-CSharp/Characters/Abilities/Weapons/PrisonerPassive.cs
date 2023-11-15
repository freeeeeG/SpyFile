using System;
using System.Linq;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Weapons.Gauges;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Weapons
{
	// Token: 0x02000BEF RID: 3055
	[Serializable]
	public class PrisonerPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06003EB7 RID: 16055 RVA: 0x000B659B File Offset: 0x000B479B
		// (set) Token: 0x06003EB8 RID: 16056 RVA: 0x000B65A3 File Offset: 0x000B47A3
		public Character owner { get; set; }

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06003EBA RID: 16058 RVA: 0x00071719 File Offset: 0x0006F919
		// (set) Token: 0x06003EBB RID: 16059 RVA: 0x00002191 File Offset: 0x00000391
		public float remainTime
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06003EBC RID: 16060 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06003EBD RID: 16061 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003EBE RID: 16062 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x000B65AC File Offset: 0x000B47AC
		// (set) Token: 0x06003EC0 RID: 16064 RVA: 0x000B65B4 File Offset: 0x000B47B4
		public int iconStacks { get; protected set; }

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x000B65C0 File Offset: 0x000B47C0
		~PrisonerPassive()
		{
			this._defaultIcon = null;
			this._walk = null;
			this._walk2 = null;
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x000B65FC File Offset: 0x000B47FC
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x000B6608 File Offset: 0x000B4808
		private void AttachEasterEgg()
		{
			if (this._easterAttached)
			{
				return;
			}
			this._easterAttached = true;
			this._remainEasterEggDuration = this._easterEggDuration;
			this.owner.stat.AttachValues(this._easterEggStat);
			this._characterAnimation.SetWalk(this._walk2);
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x000B6658 File Offset: 0x000B4858
		private void DetachEasterEgg()
		{
			if (!this._easterAttached)
			{
				return;
			}
			this._easterAttached = false;
			this.owner.stat.DetachValues(this._easterEggStat);
			this._characterAnimation.SetWalk(this._walk);
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x000B6691 File Offset: 0x000B4891
		public void UpdateTime(float deltaTime)
		{
			if (!this._easterAttached)
			{
				return;
			}
			this._remainEasterEggDuration -= deltaTime;
			if (this._remainEasterEggDuration < 0f)
			{
				this.DetachEasterEgg();
			}
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x000B66C0 File Offset: 0x000B48C0
		public override void Initialize()
		{
			base.Initialize();
			this._scrolls = new PrisonerPassive.Scroll[]
			{
				this._brutality,
				this._tactics,
				this._survival,
				this._minotaurus,
				this._assassin,
				this._guardian,
				this._epic
			};
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x000B6720 File Offset: 0x000B4920
		public void Attach()
		{
			this._damageDealt = this._totalDamage;
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			this._gauge.onChanged += this.OnGaugeChanged;
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x000B6777 File Offset: 0x000B4977
		public void Detach()
		{
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			this._gauge.onChanged -= this.OnGaugeChanged;
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x000B67B8 File Offset: 0x000B49B8
		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!this._motionTypes[gaveDamage.motionType] || !this._attackTypes[gaveDamage.attackType] || !this._attributes[gaveDamage.attribute])
			{
				return;
			}
			if (target.character == null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
			{
				return;
			}
			this._damageDealt += damageDealt;
			int num = 0;
			while (this._damageDealt > this._totalDamage)
			{
				this._damageDealt -= this._totalDamage;
				if (MMMaths.PercentChance(this._possibility))
				{
					this._cellPrefab.Spawn(gaveDamage.hitPoint, this._gauge);
					num++;
					if (num >= this._maxCellCount)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x000B6894 File Offset: 0x000B4A94
		private PrisonerPassive.Scroll GetScrollToObtain()
		{
			int maxExclusive = (from scroll in this._scrolls
			select scroll.weight).Sum();
			int num = UnityEngine.Random.Range(0, maxExclusive) + 1;
			for (int i = 0; i < this._scrolls.Length; i++)
			{
				num -= this._scrolls[i].weight;
				if (num <= 0)
				{
					return this._scrolls[i];
				}
			}
			Debug.LogError("Scroll index is exceeded!");
			return this._scrolls.Random<PrisonerPassive.Scroll>();
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x000B6920 File Offset: 0x000B4B20
		private void OnGaugeChanged(float oldValue, float newValue)
		{
			if (newValue < this._gauge.maxValue)
			{
				return;
			}
			this.AttachEasterEgg();
			this._gauge.Clear();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._scrollSound, this.owner.transform.position);
			PrisonerPassive.Scroll scrollToObtain = this.GetScrollToObtain();
			scrollToObtain.effect.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
			if (scrollToObtain.brutality)
			{
				this.owner.ability.Add(this._brutalityStat.ability);
			}
			if (scrollToObtain.tactics)
			{
				this.owner.ability.Add(this._tacticsStat.ability);
			}
			if (scrollToObtain.survival)
			{
				this.owner.ability.Add(this._survivalStat.ability);
			}
		}

		// Token: 0x0400305C RID: 12380
		[Header("Walk Easteregg")]
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x0400305D RID: 12381
		[SerializeField]
		private AnimationClip _walk;

		// Token: 0x0400305E RID: 12382
		[SerializeField]
		private AnimationClip _walk2;

		// Token: 0x0400305F RID: 12383
		[SerializeField]
		private Stat.Values _easterEggStat;

		// Token: 0x04003060 RID: 12384
		[SerializeField]
		private float _easterEggDuration;

		// Token: 0x04003061 RID: 12385
		private float _remainEasterEggDuration;

		// Token: 0x04003062 RID: 12386
		private bool _easterAttached;

		// Token: 0x04003063 RID: 12387
		[Space]
		[SerializeField]
		[Range(1f, 100f)]
		private int _possibility;

		// Token: 0x04003064 RID: 12388
		[SerializeField]
		private double _totalDamage;

		// Token: 0x04003065 RID: 12389
		[SerializeField]
		private int _maxCellCount;

		// Token: 0x04003066 RID: 12390
		private double _damageDealt;

		// Token: 0x04003067 RID: 12391
		[SerializeField]
		[Space]
		private MotionTypeBoolArray _motionTypes;

		// Token: 0x04003068 RID: 12392
		[SerializeField]
		private AttackTypeBoolArray _attackTypes;

		// Token: 0x04003069 RID: 12393
		[SerializeField]
		private DamageAttributeBoolArray _attributes;

		// Token: 0x0400306A RID: 12394
		[SerializeField]
		[Space]
		private ValueGauge _gauge;

		// Token: 0x0400306B RID: 12395
		[SerializeField]
		private DroppedCell _cellPrefab;

		// Token: 0x0400306C RID: 12396
		[SerializeField]
		[Header("Buff Stats(클릭해서 이동 가능)")]
		private StackableStatBonusComponent _brutalityStat;

		// Token: 0x0400306D RID: 12397
		[SerializeField]
		private StackableStatBonusComponent _tacticsStat;

		// Token: 0x0400306E RID: 12398
		[SerializeField]
		private StackableStatBonusComponent _survivalStat;

		// Token: 0x0400306F RID: 12399
		[Header("Effects and Weights")]
		[SerializeField]
		private SoundInfo _scrollSound;

		// Token: 0x04003070 RID: 12400
		[Space]
		[SerializeField]
		private PrisonerPassive.Scroll _brutality;

		// Token: 0x04003071 RID: 12401
		[SerializeField]
		private PrisonerPassive.Scroll _tactics;

		// Token: 0x04003072 RID: 12402
		[SerializeField]
		private PrisonerPassive.Scroll _survival;

		// Token: 0x04003073 RID: 12403
		[SerializeField]
		[Space]
		private PrisonerPassive.Scroll _minotaurus;

		// Token: 0x04003074 RID: 12404
		[SerializeField]
		private PrisonerPassive.Scroll _assassin;

		// Token: 0x04003075 RID: 12405
		[SerializeField]
		private PrisonerPassive.Scroll _guardian;

		// Token: 0x04003076 RID: 12406
		[SerializeField]
		[Space]
		private PrisonerPassive.Scroll _epic;

		// Token: 0x04003077 RID: 12407
		private PrisonerPassive.Scroll[] _scrolls;

		// Token: 0x02000BF0 RID: 3056
		[Serializable]
		private class Scroll
		{
			// Token: 0x17000D48 RID: 3400
			// (get) Token: 0x06003ECF RID: 16079 RVA: 0x000B6A0B File Offset: 0x000B4C0B
			public EffectInfo effect
			{
				get
				{
					return this._effect;
				}
			}

			// Token: 0x17000D49 RID: 3401
			// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x000B6A13 File Offset: 0x000B4C13
			public int weight
			{
				get
				{
					return this._weight;
				}
			}

			// Token: 0x17000D4A RID: 3402
			// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x000B6A1B File Offset: 0x000B4C1B
			public bool brutality
			{
				get
				{
					return this._brutality;
				}
			}

			// Token: 0x17000D4B RID: 3403
			// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x000B6A23 File Offset: 0x000B4C23
			public bool tactics
			{
				get
				{
					return this._tactics;
				}
			}

			// Token: 0x17000D4C RID: 3404
			// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x000B6A2B File Offset: 0x000B4C2B
			public bool survival
			{
				get
				{
					return this._survival;
				}
			}

			// Token: 0x06003ED4 RID: 16084 RVA: 0x000B6A33 File Offset: 0x000B4C33
			public Scroll(bool brutality, bool tactics, bool survival)
			{
				this._brutality = brutality;
				this._tactics = tactics;
				this._survival = survival;
			}

			// Token: 0x0400307A RID: 12410
			[SerializeField]
			private EffectInfo _effect;

			// Token: 0x0400307B RID: 12411
			[Range(0f, 100f)]
			[SerializeField]
			private int _weight;

			// Token: 0x0400307C RID: 12412
			[SerializeField]
			private bool _brutality;

			// Token: 0x0400307D RID: 12413
			[SerializeField]
			private bool _tactics;

			// Token: 0x0400307E RID: 12414
			[SerializeField]
			private bool _survival;
		}
	}
}
