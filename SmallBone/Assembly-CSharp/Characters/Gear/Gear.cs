using System;
using Data;
using GameResources;
using Level;
using UnityEngine;

namespace Characters.Gear
{
	// Token: 0x0200081A RID: 2074
	public abstract class Gear : MonoBehaviour
	{
		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06002AAE RID: 10926 RVA: 0x00083890 File Offset: 0x00081A90
		// (remove) Token: 0x06002AAF RID: 10927 RVA: 0x000838C8 File Offset: 0x00081AC8
		public event Action onDropped;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06002AB0 RID: 10928 RVA: 0x00083900 File Offset: 0x00081B00
		// (remove) Token: 0x06002AB1 RID: 10929 RVA: 0x00083938 File Offset: 0x00081B38
		public event Action onEquipped;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06002AB2 RID: 10930 RVA: 0x0008396D File Offset: 0x00081B6D
		// (remove) Token: 0x06002AB3 RID: 10931 RVA: 0x00083986 File Offset: 0x00081B86
		public event Action<Gear> onDiscard
		{
			add
			{
				this._onDiscard = (Action<Gear>)Delegate.Combine(this._onDiscard, value);
			}
			remove
			{
				this._onDiscard = (Action<Gear>)Delegate.Remove(this._onDiscard, value);
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x0008399F File Offset: 0x00081B9F
		public Sprite unlockIcon
		{
			get
			{
				if (!(this._unlockIcon == null))
				{
					return this._unlockIcon;
				}
				return GearResource.instance.GetItemBuffIcon(base.name);
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000839C6 File Offset: 0x00081BC6
		public Sprite defaultUnlockIcon
		{
			get
			{
				return this._unlockIcon;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002AB6 RID: 10934
		public abstract Gear.Type type { get; }

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x000839CE File Offset: 0x00081BCE
		public Rarity rarity
		{
			get
			{
				return this._rarity;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002AB8 RID: 10936 RVA: 0x000839D6 File Offset: 0x00081BD6
		public Gear.Tag gearTag
		{
			get
			{
				return this._gearTag;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002AB9 RID: 10937 RVA: 0x000839DE File Offset: 0x00081BDE
		public Stat.Values stat
		{
			get
			{
				return this._stat;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002ABA RID: 10938 RVA: 0x000839E6 File Offset: 0x00081BE6
		public DroppedGear dropped
		{
			get
			{
				return this._dropped;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x000839EE File Offset: 0x00081BEE
		public GameObject equipped
		{
			get
			{
				return this._equipped;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002ABC RID: 10940 RVA: 0x000839F6 File Offset: 0x00081BF6
		// (set) Token: 0x06002ABD RID: 10941 RVA: 0x000839FE File Offset: 0x00081BFE
		public bool lootable { get; set; } = true;

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002ABE RID: 10942 RVA: 0x00083A07 File Offset: 0x00081C07
		public string[] setItemKeys
		{
			get
			{
				return this._setItemKeys;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x00083A0F File Offset: 0x00081C0F
		public Sprite setItemImage
		{
			get
			{
				return this._setItemImage;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002AC0 RID: 10944 RVA: 0x00083A17 File Offset: 0x00081C17
		public RuntimeAnimatorController setItemAnimator
		{
			get
			{
				return this._setItemAnimator;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x00083A1F File Offset: 0x00081C1F
		public string[] groupItemKeys
		{
			get
			{
				return this._groupItemKeys;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002AC2 RID: 10946
		protected abstract string _prefix { get; }

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x00083A27 File Offset: 0x00081C27
		protected string _keyBase
		{
			get
			{
				return this._prefix + "/" + base.name;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002AC4 RID: 10948 RVA: 0x00083A3F File Offset: 0x00081C3F
		public string displayNameKey
		{
			get
			{
				return this._keyBase + "/name";
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x00083A51 File Offset: 0x00081C51
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/name");
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002AC6 RID: 10950 RVA: 0x00083A68 File Offset: 0x00081C68
		public string description
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/desc");
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x00083A7F File Offset: 0x00081C7F
		public string flavor
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/flavor");
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002AC8 RID: 10952 RVA: 0x00083A96 File Offset: 0x00081C96
		public string typeDisplayName
		{
			get
			{
				return Localization.GetLocalizedString("label/" + this._prefix + "/name");
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x00083AB2 File Offset: 0x00081CB2
		public bool hasFlavor
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.flavor);
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002ACA RID: 10954 RVA: 0x00083AC2 File Offset: 0x00081CC2
		public Sprite icon
		{
			get
			{
				return this.dropped.spriteRenderer.sprite;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x00083AD4 File Offset: 0x00081CD4
		public virtual Sprite thumbnail
		{
			get
			{
				return GearResource.instance.GetGearThumbnail(base.name) ?? this.icon;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x00018EC5 File Offset: 0x000170C5
		public virtual GameData.Currency.Type currencyTypeByDiscard
		{
			get
			{
				return GameData.Currency.Type.Gold;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002ACD RID: 10957 RVA: 0x00018EC5 File Offset: 0x000170C5
		public virtual int currencyByDiscard
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002ACE RID: 10958 RVA: 0x00083AF0 File Offset: 0x00081CF0
		// (set) Token: 0x06002ACF RID: 10959 RVA: 0x00083AF8 File Offset: 0x00081CF8
		public Gear.State state
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state == value)
				{
					return;
				}
				this._state = value;
				Gear.State state = this._state;
				if (state == Gear.State.Dropped)
				{
					this.OnDropped();
					return;
				}
				if (state != Gear.State.Equipped)
				{
					return;
				}
				this.OnEquipped();
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x00083B32 File Offset: 0x00081D32
		// (set) Token: 0x06002AD1 RID: 10961 RVA: 0x00083B3A File Offset: 0x00081D3A
		public Character owner { get; protected set; }

		// Token: 0x06002AD2 RID: 10962 RVA: 0x00083B43 File Offset: 0x00081D43
		protected virtual void Awake()
		{
			if (this._dropped != null)
			{
				this._dropped.onLoot += this.OnLoot;
			}
			this.OnDropped();
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x00083B71 File Offset: 0x00081D71
		public virtual void Initialize()
		{
			DroppedGear dropped = this._dropped;
			if (dropped == null)
			{
				return;
			}
			dropped.Initialize(this);
		}

		// Token: 0x06002AD4 RID: 10964
		protected abstract void OnLoot(Character character);

		// Token: 0x06002AD5 RID: 10965 RVA: 0x00083B84 File Offset: 0x00081D84
		protected virtual void OnDropped()
		{
			base.transform.parent = Map.Instance.transform;
			base.transform.localScale = Vector3.one;
			if (this._equipped != null)
			{
				this._equipped.SetActive(false);
			}
			if (this._dropped != null)
			{
				this._dropped.gameObject.SetActive(true);
			}
			Action action = this.onDropped;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x00083C00 File Offset: 0x00081E00
		protected virtual void OnEquipped()
		{
			if (this._dropped != null)
			{
				this._dropped.gameObject.SetActive(false);
			}
			if (this._equipped != null)
			{
				this._equipped.SetActive(true);
			}
			Action action = this.onEquipped;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04002458 RID: 9304
		protected Action<Gear> _onDiscard;

		// Token: 0x04002459 RID: 9305
		[Space]
		[Tooltip("입수가능")]
		public bool obtainable = true;

		// Token: 0x0400245A RID: 9306
		[Tooltip("파괴가능")]
		public bool destructible = true;

		// Token: 0x0400245B RID: 9307
		[Tooltip("해금해야 드랍되는지, obtainable이 false이면 어쨌든 입수 불가능이므로 주의")]
		public bool needUnlock;

		// Token: 0x0400245C RID: 9308
		[SerializeField]
		private Sprite _unlockIcon;

		// Token: 0x0400245D RID: 9309
		[SerializeField]
		[Space]
		private Rarity _rarity;

		// Token: 0x0400245E RID: 9310
		[EnumFlag]
		[SerializeField]
		private Gear.Tag _gearTag;

		// Token: 0x0400245F RID: 9311
		[SerializeField]
		[Space]
		protected Stat.Values _stat;

		// Token: 0x04002460 RID: 9312
		[SerializeField]
		private DroppedGear _dropped;

		// Token: 0x04002461 RID: 9313
		[SerializeField]
		private GameObject _equipped;

		// Token: 0x04002462 RID: 9314
		[Space]
		[SerializeField]
		private string[] _setItemKeys;

		// Token: 0x04002463 RID: 9315
		[SerializeField]
		private Sprite _setItemImage;

		// Token: 0x04002464 RID: 9316
		[SerializeField]
		private RuntimeAnimatorController _setItemAnimator;

		// Token: 0x04002465 RID: 9317
		[Tooltip("이 아이템을 소지하고 있을 경우 드랍될 수 없는 아이템")]
		[SerializeField]
		[Space]
		private string[] _groupItemKeys;

		// Token: 0x04002466 RID: 9318
		private Gear.State _state;

		// Token: 0x0200081B RID: 2075
		public enum Type
		{
			// Token: 0x0400246A RID: 9322
			Weapon,
			// Token: 0x0400246B RID: 9323
			Item,
			// Token: 0x0400246C RID: 9324
			Quintessence,
			// Token: 0x0400246D RID: 9325
			Upgrade
		}

		// Token: 0x0200081C RID: 2076
		public enum State
		{
			// Token: 0x0400246F RID: 9327
			Dropped,
			// Token: 0x04002470 RID: 9328
			Equipped
		}

		// Token: 0x0200081D RID: 2077
		[Flags]
		public enum Tag
		{
			// Token: 0x04002472 RID: 9330
			Carleon = 1,
			// Token: 0x04002473 RID: 9331
			Skeleton = 2,
			// Token: 0x04002474 RID: 9332
			Spirit = 4,
			// Token: 0x04002475 RID: 9333
			Omen = 8,
			// Token: 0x04002476 RID: 9334
			UpgradedOmen = 16
		}
	}
}
