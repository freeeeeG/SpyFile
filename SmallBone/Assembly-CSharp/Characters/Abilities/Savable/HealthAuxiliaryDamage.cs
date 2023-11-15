using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B88 RID: 2952
	public sealed class HealthAuxiliaryDamage : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x000B163D File Offset: 0x000AF83D
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06003C63 RID: 15459 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x000B1645 File Offset: 0x000AF845
		// (set) Token: 0x06003C65 RID: 15461 RVA: 0x000B164D File Offset: 0x000AF84D
		public float remainTime { get; set; }

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06003C66 RID: 15462 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06003C67 RID: 15463 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06003C68 RID: 15464 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06003C69 RID: 15465 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06003C6A RID: 15466 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06003C6B RID: 15467 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06003C6C RID: 15468 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06003C6D RID: 15469 RVA: 0x000B1656 File Offset: 0x000AF856
		// (set) Token: 0x06003C6E RID: 15470 RVA: 0x000B165E File Offset: 0x000AF85E
		public float duration { get; set; }

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06003C6F RID: 15471 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06003C70 RID: 15472 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06003C71 RID: 15473 RVA: 0x000B1667 File Offset: 0x000AF867
		// (set) Token: 0x06003C72 RID: 15474 RVA: 0x000B1670 File Offset: 0x000AF870
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

		// Token: 0x06003C73 RID: 15475 RVA: 0x000B167A File Offset: 0x000AF87A
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x000B16E0 File Offset: 0x000AF8E0
		void IAbilityInstance.Attach()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				float num = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프스텟[this._level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
				this._buff.values[i].value = (double)num;
			}
			this._owner.stat.AttachValues(this._buff);
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x000B1758 File Offset: 0x000AF958
		void IAbilityInstance.Refresh()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				float num = Singleton<DarktechManager>.Instance.setting.건강보조장치공격력버프스텟[this._level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
				this._buff.values[i].value = (double)num;
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x000B17C8 File Offset: 0x000AF9C8
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F45 RID: 12101
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 1.3),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 1.3)
		});

		// Token: 0x04002F46 RID: 12102
		private Character _owner;

		// Token: 0x04002F49 RID: 12105
		private int _level;
	}
}
