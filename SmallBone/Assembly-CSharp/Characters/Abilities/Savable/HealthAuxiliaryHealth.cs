using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B89 RID: 2953
	public sealed class HealthAuxiliaryHealth : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06003C7B RID: 15483 RVA: 0x000B17E0 File Offset: 0x000AF9E0
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06003C7C RID: 15484 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x000B17E8 File Offset: 0x000AF9E8
		// (set) Token: 0x06003C7E RID: 15486 RVA: 0x000B17F0 File Offset: 0x000AF9F0
		public float remainTime { get; set; }

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06003C7F RID: 15487 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06003C80 RID: 15488 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06003C81 RID: 15489 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06003C82 RID: 15490 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06003C83 RID: 15491 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06003C84 RID: 15492 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06003C85 RID: 15493 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06003C86 RID: 15494 RVA: 0x000B17F9 File Offset: 0x000AF9F9
		// (set) Token: 0x06003C87 RID: 15495 RVA: 0x000B1801 File Offset: 0x000AFA01
		public float duration { get; set; }

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06003C88 RID: 15496 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06003C89 RID: 15497 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x000B180A File Offset: 0x000AFA0A
		// (set) Token: 0x06003C8B RID: 15499 RVA: 0x000B1813 File Offset: 0x000AFA13
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

		// Token: 0x06003C8C RID: 15500 RVA: 0x000B181D File Offset: 0x000AFA1D
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x000B185C File Offset: 0x000AFA5C
		void IAbilityInstance.Attach()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				float num = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟[this._level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
				this._buff.values[i].value = (double)num;
			}
			this._owner.stat.AttachValues(this._buff);
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x000B18D4 File Offset: 0x000AFAD4
		void IAbilityInstance.Refresh()
		{
			for (int i = 0; i < this._buff.values.Length; i++)
			{
				float num = Singleton<DarktechManager>.Instance.setting.건강보조장치체력버프스텟[this._level] * Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량;
				this._buff.values[i].value = (double)num;
			}
			this._owner.stat.SetNeedUpdate();
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x000B1944 File Offset: 0x000AFB44
		void IAbilityInstance.Detach()
		{
			this._owner.stat.DetachValues(this._buff);
		}

		// Token: 0x04002F4A RID: 12106
		private readonly Stat.Values _buff = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 10.0)
		});

		// Token: 0x04002F4B RID: 12107
		private Character _owner;

		// Token: 0x04002F4E RID: 12110
		private int _level;
	}
}
