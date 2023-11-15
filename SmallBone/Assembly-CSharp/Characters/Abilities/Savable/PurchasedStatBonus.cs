using System;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B8E RID: 2958
	public sealed class PurchasedStatBonus : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06003CFD RID: 15613 RVA: 0x000B2036 File Offset: 0x000B0236
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06003CFE RID: 15614 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06003CFF RID: 15615 RVA: 0x000B203E File Offset: 0x000B023E
		// (set) Token: 0x06003D00 RID: 15616 RVA: 0x000B2046 File Offset: 0x000B0246
		public float remainTime { get; set; }

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06003D01 RID: 15617 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06003D02 RID: 15618 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06003D03 RID: 15619 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003D04 RID: 15620 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003D05 RID: 15621 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003D06 RID: 15622 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06003D07 RID: 15623 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003D08 RID: 15624 RVA: 0x000B204F File Offset: 0x000B024F
		// (set) Token: 0x06003D09 RID: 15625 RVA: 0x000B2057 File Offset: 0x000B0257
		public float duration { get; set; }

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x000B2060 File Offset: 0x000B0260
		// (set) Token: 0x06003D0D RID: 15629 RVA: 0x000B2067 File Offset: 0x000B0267
		public float stack
		{
			get
			{
				return PurchasedStatBonus._currentValue;
			}
			set
			{
				if (PurchasedStatBonus._currentValue == 0f)
				{
					PurchasedStatBonus._currentValue = value;
					return;
				}
				PurchasedStatBonus._currentValue += value;
			}
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x000B2088 File Offset: 0x000B0288
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x000B20C8 File Offset: 0x000B02C8
		void IAbilityInstance.Attach()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._buff.values[i].value = (double)PurchasedStatBonus._currentValue;
			}
			this._owner.stat.AttachValues(this._buff);
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x000B211C File Offset: 0x000B031C
		void IAbilityInstance.Refresh()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				this._buff.values[i].value = (double)PurchasedStatBonus._currentValue;
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x000B2169 File Offset: 0x000B0369
		void IAbilityInstance.Detach()
		{
			PurchasedStatBonus._currentValue = 0f;
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F67 RID: 12135
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 0.0)
		});

		// Token: 0x04002F68 RID: 12136
		private Character _owner;

		// Token: 0x04002F6B RID: 12139
		private static float _currentValue;
	}
}
