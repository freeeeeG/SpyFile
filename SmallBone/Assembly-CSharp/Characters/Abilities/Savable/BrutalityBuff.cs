using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B8F RID: 2959
	public sealed class BrutalityBuff : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06003D16 RID: 15638 RVA: 0x000B218B File Offset: 0x000B038B
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06003D17 RID: 15639 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x000B2193 File Offset: 0x000B0393
		// (set) Token: 0x06003D19 RID: 15641 RVA: 0x000B219B File Offset: 0x000B039B
		public float remainTime { get; set; }

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003D1A RID: 15642 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003D1B RID: 15643 RVA: 0x000B21A4 File Offset: 0x000B03A4
		public Sprite icon
		{
			get
			{
				return Singleton<DarktechManager>.Instance.resource.brutalityBuffIcon;
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003D1C RID: 15644 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003D1D RID: 15645 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003D1E RID: 15646 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003D1F RID: 15647 RVA: 0x000B21B5 File Offset: 0x000B03B5
		public int iconStacks
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003D20 RID: 15648 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003D21 RID: 15649 RVA: 0x000B21BD File Offset: 0x000B03BD
		// (set) Token: 0x06003D22 RID: 15650 RVA: 0x000B21C5 File Offset: 0x000B03C5
		public float duration { get; set; }

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003D24 RID: 15652 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x000B21CE File Offset: 0x000B03CE
		// (set) Token: 0x06003D26 RID: 15654 RVA: 0x000B21D7 File Offset: 0x000B03D7
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

		// Token: 0x06003D27 RID: 15655 RVA: 0x000B21E1 File Offset: 0x000B03E1
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x000B2250 File Offset: 0x000B0450
		void IAbilityInstance.Attach()
		{
			this._attached = this._buff.Clone();
			this.remainTime = (float)Singleton<DarktechManager>.Instance.setting.품목순환장치버프맵카운트;
			this._owner.stat.AttachValues(this._attached);
			for (int i = 0; i < this._attached.values.Length; i++)
			{
				this._attached.values[i].value = this._buff.values[i].GetStackedValue((double)this.stack);
			}
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x000B22E0 File Offset: 0x000B04E0
		void IAbilityInstance.Refresh()
		{
			this.remainTime = (float)Singleton<DarktechManager>.Instance.setting.품목순환장치버프맵카운트;
			float stack = this.stack;
			this.stack = stack + 1f;
			for (int i = 0; i < this._attached.values.Length; i++)
			{
				this._attached.values[i].value = this._buff.values[i].GetStackedValue((double)this.stack);
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x000B236A File Offset: 0x000B056A
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F6C RID: 12140
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.10000000149011612),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.10000000149011612)
		});

		// Token: 0x04002F6D RID: 12141
		private Stat.Values _attached;

		// Token: 0x04002F6E RID: 12142
		private Character _owner;

		// Token: 0x04002F71 RID: 12145
		private int _level = 1;
	}
}
