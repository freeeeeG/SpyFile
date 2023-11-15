using System;
using Characters;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI.GearPopup;
using UnityEngine;

namespace Level
{
	// Token: 0x020004A3 RID: 1187
	public sealed class CurrencyBag : InteractiveObject, IPurchasable
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x00047D04 File Offset: 0x00045F04
		private string _keyBase
		{
			get
			{
				if (!string.IsNullOrEmpty(this._overrideNameKey))
				{
					return "currencyBag/" + this._overrideNameKey;
				}
				return "currencyBag/" + base.name;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x00047D34 File Offset: 0x00045F34
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/name");
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x00047D4B File Offset: 0x00045F4B
		public string description
		{
			get
			{
				return Localization.GetLocalizedString(this._keyBase + "/desc");
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00047D62 File Offset: 0x00045F62
		// (set) Token: 0x060016C1 RID: 5825 RVA: 0x00047D6A File Offset: 0x00045F6A
		public int amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				this._amount = value;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x00047D73 File Offset: 0x00045F73
		// (set) Token: 0x060016C3 RID: 5827 RVA: 0x00047D7B File Offset: 0x00045F7B
		public int count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x00047D84 File Offset: 0x00045F84
		public Rarity rarity
		{
			get
			{
				return this._rarity;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00047D8C File Offset: 0x00045F8C
		// (set) Token: 0x060016C6 RID: 5830 RVA: 0x00047D94 File Offset: 0x00045F94
		public int price { get; set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060016C7 RID: 5831 RVA: 0x00047D9D File Offset: 0x00045F9D
		// (set) Token: 0x060016C8 RID: 5832 RVA: 0x00047DA5 File Offset: 0x00045FA5
		public GameData.Currency.Type priceCurrency { get; set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x00047DAE File Offset: 0x00045FAE
		// (set) Token: 0x060016CA RID: 5834 RVA: 0x00047DB6 File Offset: 0x00045FB6
		public bool released { get; set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x00047DBF File Offset: 0x00045FBF
		public DropMovement dropMovement
		{
			get
			{
				return this._dropMovement;
			}
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00047DC7 File Offset: 0x00045FC7
		protected override void Awake()
		{
			base.Awake();
			this._dropMovement.onGround += this.Activate;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00047DE7 File Offset: 0x00045FE7
		public override void OnActivate()
		{
			base.OnActivate();
			if (this.rarity == Rarity.Legendary && this._droppedEffect != null)
			{
				this._droppedEffect.SpawnLegendaryEffect();
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00047E14 File Offset: 0x00046014
		public override void OpenPopupBy(Character character)
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			gearPopupCanvas.gearPopup.Set(this.displayName, this.description);
			string localizedString = Localization.GetLocalizedString("label/interaction/loot");
			string colorCode = GameData.Currency.currencies[this._type].colorCode;
			gearPopupCanvas.gearPopup.SetInteractionLabel(string.Format("{0} (<color=#{1}>{2}</color>)", localizedString, colorCode, this._amount));
			gearPopupCanvas.Open(position);
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00047F0C File Offset: 0x0004610C
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			this.released = true;
			this.ClosePopup();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00047F30 File Offset: 0x00046130
		private void OnDestroy()
		{
			if (!this.released)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.DropCurrency(this._type, this._amount, this._count, (this._dropPoint != null) ? this._dropPoint.position : base.transform.position);
		}

		// Token: 0x040013F7 RID: 5111
		private const string _prefix = "currencyBag";

		// Token: 0x040013F8 RID: 5112
		[SerializeField]
		private DropMovement _dropMovement;

		// Token: 0x040013F9 RID: 5113
		[SerializeField]
		private DroppedEffect _droppedEffect;

		// Token: 0x040013FA RID: 5114
		[SerializeField]
		private Rarity _rarity;

		// Token: 0x040013FB RID: 5115
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x040013FC RID: 5116
		[SerializeField]
		private int _amount;

		// Token: 0x040013FD RID: 5117
		[SerializeField]
		private int _count;

		// Token: 0x040013FE RID: 5118
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x040013FF RID: 5119
		[SerializeField]
		private string _overrideNameKey;
	}
}
