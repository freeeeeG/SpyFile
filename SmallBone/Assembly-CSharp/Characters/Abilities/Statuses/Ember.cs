using System;
using System.Collections.Generic;
using FX.SpriteEffects;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B6E RID: 2926
	public sealed class Ember : IAbility, IAbilityInstance
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06003ACD RID: 15053 RVA: 0x000AD8DB File Offset: 0x000ABADB
		// (set) Token: 0x06003ACE RID: 15054 RVA: 0x000AD8E3 File Offset: 0x000ABAE3
		public Character owner { get; private set; }

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06003ACF RID: 15055 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x000AD8EC File Offset: 0x000ABAEC
		// (set) Token: 0x06003AD1 RID: 15057 RVA: 0x000AD8F4 File Offset: 0x000ABAF4
		public float remainTime { get; set; }

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06003AD3 RID: 15059 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x000AD8FD File Offset: 0x000ABAFD
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / this.duration;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x000AD90C File Offset: 0x000ABB0C
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x000AD91E File Offset: 0x000ABB1E
		// (set) Token: 0x06003ADA RID: 15066 RVA: 0x000AD926 File Offset: 0x000ABB26
		public float duration { get; set; }

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x000AD92F File Offset: 0x000ABB2F
		public Ember(Character owner)
		{
			this.owner = owner;
			this._damageMultiplier = 1.0;
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x000AD970 File Offset: 0x000ABB70
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
			this._remainTimeToNextTick -= deltaTime;
			if (this._remainTimeToNextTick <= 0f)
			{
				this._remainTimeToNextTick += 0.33f;
				this.GiveDamage();
			}
			bool flag = false;
			for (int i = this._remainTimes.Count - 1; i >= 0; i--)
			{
				List<float> remainTimes = this._remainTimes;
				int index = i;
				if ((remainTimes[index] -= deltaTime) <= 0f)
				{
					this._damages.RemoveAt(i);
					this._remainTimes.RemoveAt(i);
					flag = true;
				}
			}
			if (flag)
			{
				this.UpdateDamage();
			}
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x000ADA20 File Offset: 0x000ABC20
		private void GiveDamage()
		{
			Character character = this._attackers[this._attackers.Count - 1];
			if (character == null)
			{
				return;
			}
			Damage damage = new Damage(character, this._currentDamage * this._damageMultiplier, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), Damage.Attribute.Magic, Damage.AttackType.Additional, Damage.MotionType.Status, 1.0, 0f, 0.0, 1.0, 1.0, true, false, 0.0, 0.0, 0, null, 1.0);
			character.Attack(this.owner, ref damage);
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x000ADAD8 File Offset: 0x000ABCD8
		private void UpdateDamage()
		{
			this._currentDamage = 0.0;
			for (int i = 0; i < this._damages.Count; i++)
			{
				double num = this._damages[i];
				if (this._currentDamage < num)
				{
					this._currentDamage = num;
				}
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x000ADB28 File Offset: 0x000ABD28
		public void Add(Character attacker, float duration)
		{
			if (this.remainTime < duration)
			{
				this.remainTime = duration;
				this.duration = duration;
			}
			this._attackers.Add(attacker);
			this._damages.Add(3.3000001907348633);
			this._remainTimes.Add(duration);
			this.UpdateDamage();
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x000ADB80 File Offset: 0x000ABD80
		public void Attach()
		{
			this.remainTime = this.duration;
			this._remainTimeToNextTick = 0f;
			this.owner.spriteEffectStack.Add(Ember._colorBlend);
			this.owner.stat.AttachValues(Ember._stat);
			this.SpawnFloatingText();
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x000ADBD4 File Offset: 0x000ABDD4
		public void Detach()
		{
			this.owner.spriteEffectStack.Remove(Ember._colorBlend);
			this.owner.stat.DetachValues(Ember._stat);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x000ADC04 File Offset: 0x000ABE04
		private void SpawnFloatingText()
		{
			Vector2 v = MMMaths.RandomPointWithinBounds(this.owner.collider.bounds);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnStatus(Localization.GetLocalizedString("floating/status/burn"), v, "#DD4900");
		}

		// Token: 0x04002E99 RID: 11929
		private const float _tickInterval = 0.33f;

		// Token: 0x04002E9A RID: 11930
		private static readonly ColorBlend _colorBlend = new ColorBlend(100, new Color(0.53333336f, 0.13333334f, 0f, 1f), 0f);

		// Token: 0x04002E9B RID: 11931
		private const string _floatingTextKey = "floating/status/burn";

		// Token: 0x04002E9C RID: 11932
		private const string _floatingTextColor = "#DD4900";

		// Token: 0x04002E9D RID: 11933
		private const Damage.Attribute _damageAttribute = Damage.Attribute.Magic;

		// Token: 0x04002E9E RID: 11934
		private static Stat.Values _stat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.100000023841858)
		});

		// Token: 0x04002EA2 RID: 11938
		private readonly List<Character> _attackers = new List<Character>();

		// Token: 0x04002EA3 RID: 11939
		private readonly List<double> _damages = new List<double>();

		// Token: 0x04002EA4 RID: 11940
		private readonly List<float> _remainTimes = new List<float>();

		// Token: 0x04002EA5 RID: 11941
		private readonly double _damageMultiplier;

		// Token: 0x04002EA6 RID: 11942
		private float _remainTimeToNextTick;

		// Token: 0x04002EA7 RID: 11943
		private double _currentDamage;
	}
}
