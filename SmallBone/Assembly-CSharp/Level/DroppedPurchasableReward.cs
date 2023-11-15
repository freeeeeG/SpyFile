using System;
using Characters;
using Data;
using GameResources;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004D4 RID: 1236
	public abstract class DroppedPurchasableReward : InteractiveObject, IPurchasable
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600180E RID: 6158 RVA: 0x0004B900 File Offset: 0x00049B00
		// (remove) Token: 0x0600180F RID: 6159 RVA: 0x0004B938 File Offset: 0x00049B38
		public event Action<Character> onLoot;

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x0004B96D File Offset: 0x00049B6D
		// (set) Token: 0x06001811 RID: 6161 RVA: 0x0004B975 File Offset: 0x00049B75
		public int price { get; set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x0004B97E File Offset: 0x00049B7E
		// (set) Token: 0x06001813 RID: 6163 RVA: 0x0004B986 File Offset: 0x00049B86
		public GameData.Currency.Type priceCurrency { get; set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x0004B98F File Offset: 0x00049B8F
		protected string _keyBase
		{
			get
			{
				return "DroppedPurchasableReward/" + base.name;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x0004B9A1 File Offset: 0x00049BA1
		public virtual string displayName
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/name");
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x0004B9B8 File Offset: 0x00049BB8
		public virtual string description
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/desc");
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001817 RID: 6167 RVA: 0x0004B9CF File Offset: 0x00049BCF
		public DropMovement dropMovement
		{
			get
			{
				return this._dropMovement;
			}
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0004B9D7 File Offset: 0x00049BD7
		protected override void Awake()
		{
			base.Awake();
			if (this._dropMovement == null)
			{
				base.Activate();
				return;
			}
			this._dropMovement.onGround += this.Activate;
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0004BA0C File Offset: 0x00049C0C
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			GameData.Currency currency = GameData.Currency.currencies[this.priceCurrency];
			if (!currency.Has(this.price))
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactFailSound, base.transform.position);
				return;
			}
			if (!currency.Consume(this.price))
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactFailSound, base.transform.position);
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this.price = 0;
			Action<Character> action = this.onLoot;
			if (action == null)
			{
				return;
			}
			action(character);
		}

		// Token: 0x040014FD RID: 5373
		[SerializeField]
		private DropMovement _dropMovement;

		// Token: 0x040014FE RID: 5374
		[SerializeField]
		private DroppedEffect _droppedEffect;

		// Token: 0x040014FF RID: 5375
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001503 RID: 5379
		protected const string _prefix = "DroppedPurchasableReward";
	}
}
