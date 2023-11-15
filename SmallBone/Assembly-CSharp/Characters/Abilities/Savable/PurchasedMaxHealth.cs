using System;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B8C RID: 2956
	public sealed class PurchasedMaxHealth : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x000B1CD0 File Offset: 0x000AFED0
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06003CC8 RID: 15560 RVA: 0x000B1CD8 File Offset: 0x000AFED8
		// (set) Token: 0x06003CC9 RID: 15561 RVA: 0x000B1CE0 File Offset: 0x000AFEE0
		public float remainTime { get; set; }

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06003CCA RID: 15562 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003CCC RID: 15564 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06003CCD RID: 15565 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06003CCE RID: 15566 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06003CCF RID: 15567 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06003CD0 RID: 15568 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x000B1CE9 File Offset: 0x000AFEE9
		// (set) Token: 0x06003CD2 RID: 15570 RVA: 0x000B1CF1 File Offset: 0x000AFEF1
		public float duration { get; set; }

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06003CD3 RID: 15571 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06003CD4 RID: 15572 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x000B1CFA File Offset: 0x000AFEFA
		// (set) Token: 0x06003CD6 RID: 15574 RVA: 0x000B1D01 File Offset: 0x000AFF01
		public float stack
		{
			get
			{
				return PurchasedMaxHealth._currentValue;
			}
			set
			{
				PurchasedMaxHealth._currentValue = value;
			}
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x000B1D09 File Offset: 0x000AFF09
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x000B1D48 File Offset: 0x000AFF48
		void IAbilityInstance.Attach()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._buff.values[i].value = (double)PurchasedMaxHealth._currentValue;
			}
			this._owner.stat.AttachValues(this._buff);
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x000B1D9C File Offset: 0x000AFF9C
		void IAbilityInstance.Refresh()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._buff.values[i].value = (double)PurchasedMaxHealth._currentValue;
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x000B1DE9 File Offset: 0x000AFFE9
		void IAbilityInstance.Detach()
		{
			PurchasedMaxHealth._currentValue = 0f;
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F5B RID: 12123
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 0.0)
		});

		// Token: 0x04002F5C RID: 12124
		private Character _owner;

		// Token: 0x04002F5F RID: 12127
		private static float _currentValue;
	}
}
