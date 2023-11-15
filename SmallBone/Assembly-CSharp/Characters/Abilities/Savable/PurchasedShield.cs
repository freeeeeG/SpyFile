using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Savable
{
	// Token: 0x02000B8D RID: 2957
	public sealed class PurchasedShield : IAbility, IAbilityInstance, ISavableAbility
	{
		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x000B1E0B File Offset: 0x000B000B
		Character IAbilityInstance.owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x000B1E13 File Offset: 0x000B0013
		// (set) Token: 0x06003CE2 RID: 15586 RVA: 0x000B1E1B File Offset: 0x000B001B
		public float remainTime { get; set; }

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003CE4 RID: 15588 RVA: 0x000B1E24 File Offset: 0x000B0024
		public Sprite icon
		{
			get
			{
				if (this.remainTime >= 1f)
				{
					return Singleton<DarktechManager>.Instance.resource.bigCloverIcon;
				}
				return Singleton<DarktechManager>.Instance.resource.smallCloverIcon;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillInversed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003CE7 RID: 15591 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool iconFillFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003CE8 RID: 15592 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06003CE9 RID: 15593 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06003CEA RID: 15594 RVA: 0x000B1E52 File Offset: 0x000B0052
		// (set) Token: 0x06003CEB RID: 15595 RVA: 0x000B1E5A File Offset: 0x000B005A
		public float duration { get; set; }

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06003CED RID: 15597 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool removeOnSwapWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06003CEE RID: 15598 RVA: 0x000B1E63 File Offset: 0x000B0063
		// (set) Token: 0x06003CEF RID: 15599 RVA: 0x000B1E6A File Offset: 0x000B006A
		public float stack
		{
			get
			{
				return PurchasedShield._currentValue;
			}
			set
			{
				PurchasedShield._currentValue = value;
			}
		}

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x06003CF0 RID: 15600 RVA: 0x000B1E74 File Offset: 0x000B0074
		// (remove) Token: 0x06003CF1 RID: 15601 RVA: 0x000B1EAC File Offset: 0x000B00AC
		public event Action<Shield.Instance> onBroke;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06003CF2 RID: 15602 RVA: 0x000B1EE4 File Offset: 0x000B00E4
		// (remove) Token: 0x06003CF3 RID: 15603 RVA: 0x000B1F1C File Offset: 0x000B011C
		public event Action<Shield.Instance> onDetach;

		// Token: 0x06003CF4 RID: 15604 RVA: 0x000B1F51 File Offset: 0x000B0151
		public IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return this;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x000B1F5B File Offset: 0x000B015B
		public void UpdateTime(float deltaTime)
		{
			PurchasedShield._currentValue = (float)this._shieldInstance.amount;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x000B1F6E File Offset: 0x000B016E
		void IAbilityInstance.Attach()
		{
			this._shieldInstance = this._owner.health.shield.Add(this.ability, PurchasedShield._currentValue, new Action(this.OnShieldBroke));
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x000B1FA2 File Offset: 0x000B01A2
		void IAbilityInstance.Refresh()
		{
			if (this._shieldInstance != null)
			{
				this._shieldInstance.amount = (double)PurchasedShield._currentValue;
			}
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x000B1FBD File Offset: 0x000B01BD
		void IAbilityInstance.Detach()
		{
			Action<Shield.Instance> action = this.onDetach;
			if (action != null)
			{
				action(this._shieldInstance);
			}
			if (this._owner.health.shield.Remove(this.ability))
			{
				this._shieldInstance = null;
			}
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x000B1FFA File Offset: 0x000B01FA
		private void OnShieldBroke()
		{
			Action<Shield.Instance> action = this.onBroke;
			if (action != null)
			{
				action(this._shieldInstance);
			}
			this._shieldInstance = null;
			PurchasedShield._currentValue = 0f;
			this._owner.ability.Remove(this);
		}

		// Token: 0x04002F60 RID: 12128
		private Character _owner;

		// Token: 0x04002F65 RID: 12133
		private Shield.Instance _shieldInstance;

		// Token: 0x04002F66 RID: 12134
		private static float _currentValue;
	}
}
