using System;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B8B RID: 2955
	public sealed class LifeChange : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06003CAD RID: 15533 RVA: 0x000B1B1C File Offset: 0x000AFD1C
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06003CAE RID: 15534 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06003CAF RID: 15535 RVA: 0x000B1B24 File Offset: 0x000AFD24
		// (set) Token: 0x06003CB0 RID: 15536 RVA: 0x000B1B2C File Offset: 0x000AFD2C
		public float remainTime { get; set; }

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x000B1B35 File Offset: 0x000AFD35
		public Sprite icon
		{
			get
			{
				return SavableAbilityResource.instance.fogWolfBuffIcons[0];
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x000B1B43 File Offset: 0x000AFD43
		public int iconStacks
		{
			get
			{
				return (int)this.stack;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x000B1B4C File Offset: 0x000AFD4C
		// (set) Token: 0x06003CB9 RID: 15545 RVA: 0x000B1B54 File Offset: 0x000AFD54
		public float duration { get; set; }

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003CBB RID: 15547 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06003CBC RID: 15548 RVA: 0x000B1B5D File Offset: 0x000AFD5D
		// (set) Token: 0x06003CBD RID: 15549 RVA: 0x000B1B66 File Offset: 0x000AFD66
		public float stack
		{
			get
			{
				return (float)this._stack;
			}
			set
			{
				this._stack = Mathf.Min((int)value, 75);
			}
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x000B1B77 File Offset: 0x000AFD77
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x000B1BE0 File Offset: 0x000AFDE0
		void IAbilityInstance.Attach()
		{
			this._buffClone = this._buff.Clone();
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._buffClone.values[i].value = this._buff.values[i].GetStackedValue((double)this.stack);
			}
			this._owner.stat.AttachValues(this._buffClone);
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x000B1C58 File Offset: 0x000AFE58
		void IAbilityInstance.Refresh()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._buffClone.values[i].value = this._buff.values[i].GetStackedValue((double)this.stack);
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x000B1CB8 File Offset: 0x000AFEB8
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F54 RID: 12116
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.PhysicalAttackDamage, 0.03999999910593033),
			new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MagicAttackDamage, 0.03999999910593033)
		});

		// Token: 0x04002F55 RID: 12117
		private Character _owner;

		// Token: 0x04002F58 RID: 12120
		private const int maxstack = 75;

		// Token: 0x04002F59 RID: 12121
		private int _stack;

		// Token: 0x04002F5A RID: 12122
		private Stat.Values _buffClone;
	}
}
