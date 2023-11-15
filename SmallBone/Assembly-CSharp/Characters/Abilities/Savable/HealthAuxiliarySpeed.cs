using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B8A RID: 2954
	public sealed class HealthAuxiliarySpeed : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06003C94 RID: 15508 RVA: 0x000B195C File Offset: 0x000AFB5C
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06003C95 RID: 15509 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06003C96 RID: 15510 RVA: 0x000B1964 File Offset: 0x000AFB64
		// (set) Token: 0x06003C97 RID: 15511 RVA: 0x000B196C File Offset: 0x000AFB6C
		public float remainTime { get; set; }

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06003C98 RID: 15512 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06003C99 RID: 15513 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06003C9A RID: 15514 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06003C9B RID: 15515 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06003C9C RID: 15516 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06003C9D RID: 15517 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06003C9E RID: 15518 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06003C9F RID: 15519 RVA: 0x000B1975 File Offset: 0x000AFB75
		// (set) Token: 0x06003CA0 RID: 15520 RVA: 0x000B197D File Offset: 0x000AFB7D
		public float duration { get; set; }

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x000B1986 File Offset: 0x000AFB86
		// (set) Token: 0x06003CA4 RID: 15524 RVA: 0x000B198F File Offset: 0x000AFB8F
		public float stack
		{
			get
			{
				return (float)this._level;
			}
			set
			{
				this._level = (int)value;
			}
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x000B1999 File Offset: 0x000AFB99
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x000B1A1C File Offset: 0x000AFC1C
		void IAbilityInstance.Attach()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				float num = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟[this._level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
				this._buff.values[i].value = (double)num;
			}
			this._owner.stat.AttachValues(this._buff);
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x000B1A94 File Offset: 0x000AFC94
		void IAbilityInstance.Refresh()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				float num = Singleton<DarktechManager>.Instance.setting.건강보조장치속도버프스텟[this._level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
				this._buff.values[i].value = (double)num;
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x000B1B04 File Offset: 0x000AFD04
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F4F RID: 12111
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillAttackSpeed, 1.5),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.BasicAttackSpeed, 1.5),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.SkillCooldownSpeed, 1.5)
		});

		// Token: 0x04002F50 RID: 12112
		private Character _owner;

		// Token: 0x04002F53 RID: 12115
		private int _level;
	}
}
