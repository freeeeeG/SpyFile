using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Operations;
using Data;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI.GearPopup;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x020004C9 RID: 1225
	public sealed class DroppedRandomInscriptionItemBox : DroppedPurchasableReward
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x0004AB62 File Offset: 0x00048D62
		public override string displayName
		{
			get
			{
				return string.Format(Localization.GetLocalizedString(base._keyBase + "/name"), Inscription.GetName(this._key));
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x0004AB89 File Offset: 0x00048D89
		public override string description
		{
			get
			{
				return string.Format(Localization.GetLocalizedString(base._keyBase + "/desc"), Inscription.GetName(this._key));
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x0004ABB0 File Offset: 0x00048DB0
		public string interaction
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/search");
			}
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0004ABBC File Offset: 0x00048DBC
		private void Start()
		{
			DroppedRandomInscriptionItemBox.ExtraSeed extraSeed;
			if (!Map.Instance.TryGetComponent<DroppedRandomInscriptionItemBox.ExtraSeed>(out extraSeed))
			{
				extraSeed = Map.Instance.gameObject.AddComponent<DroppedRandomInscriptionItemBox.ExtraSeed>();
			}
			DroppedRandomInscriptionItemBox.ExtraSeed extraSeed2 = extraSeed;
			extraSeed2.value += 1;
			this._onDeactivate.Initialize();
			this._onChangedKey.Initialize();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 716722307 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex + (int)extraSeed.value));
			this._keywords = Inscription.keys.ToList<Inscription.Key>();
			this._keywords.Remove(Inscription.Key.None);
			this._keywords.Remove(Inscription.Key.SunAndMoon);
			this._keywords.Remove(Inscription.Key.Omen);
			this._keywords.Remove(Inscription.Key.Sin);
			this.LoadItemWithKey();
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged += this.HandleOnItemInstanceChanged;
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0004ACB8 File Offset: 0x00048EB8
		private void HandleOnItemInstanceChanged()
		{
			if (!Singleton<Service>.Instance.gearManager.CanDrop(this._key))
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				player.StartCoroutine(this._onChangedKey.CRun(player));
				this.LoadItemWithKey();
			}
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0004AD08 File Offset: 0x00048F08
		private void LoadItemWithKey()
		{
			Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
			do
			{
				this._key = this._keywords.Random(this._random);
			}
			while (synergy.inscriptions[this._key].isMaxStep);
			this.LoadItem();
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0004AD6C File Offset: 0x00048F6C
		private void LoadItem()
		{
			do
			{
				Rarity rarity = this._rarityPossibilities.Evaluate(this._random);
				this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemByKeyword(this._random, rarity, this._key);
			}
			while (this._itemToDrop == null);
			this._itemRequest = this._itemToDrop.LoadAsync();
			this._inscriptionDisplay.sprite = Inscription.GetActiveIcon(this._key);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0004ADDC File Offset: 0x00048FDC
		private void OnDestroy()
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.HandleOnItemInstanceChanged;
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest == null)
			{
				return;
			}
			itemRequest.Release();
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0004AE0C File Offset: 0x0004900C
		public override void OpenPopupBy(Character character)
		{
			base.OpenPopupBy(character);
			Vector3 position = base.transform.position;
			Vector3 position2 = character.transform.position;
			position.x = position2.x + ((position.x > position2.x) ? InteractiveObject._popupUIOffset.x : (-InteractiveObject._popupUIOffset.x));
			position.y += InteractiveObject._popupUIOffset.y;
			GearPopupCanvas gearPopupCanvas = Scene<GameBase>.instance.uiManager.gearPopupCanvas;
			gearPopupCanvas.gearPopup.Set(this.displayName, this.description);
			gearPopupCanvas.gearPopup.SetInteractionLabelAsPurchase(this.interaction, base.priceCurrency, base.price);
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Open(position);
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0004AED8 File Offset: 0x000490D8
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			if (base.price != 0)
			{
				return;
			}
			base.StartCoroutine(this.<InteractWith>g__CDelayedDrop|24_0());
			this.ClosePopup();
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.StartCoroutine(this._onDeactivate.CRun(player));
			base.Deactivate();
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x0004AF31 File Offset: 0x00049131
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0004AF44 File Offset: 0x00049144
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CDelayedDrop|24_0()
		{
			while (!this._itemRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropItem(this._itemRequest, this._dropPoint.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, this._dropPoint.position);
			yield break;
		}

		// Token: 0x040014C4 RID: 5316
		private const int _randomSeed = 716722307;

		// Token: 0x040014C5 RID: 5317
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x040014C6 RID: 5318
		[SerializeField]
		private SpriteRenderer _inscriptionDisplay;

		// Token: 0x040014C7 RID: 5319
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x040014C8 RID: 5320
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onDeactivate;

		// Token: 0x040014C9 RID: 5321
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onChangedKey;

		// Token: 0x040014CA RID: 5322
		private Inscription.Key _key;

		// Token: 0x040014CB RID: 5323
		private ItemReference _itemToDrop;

		// Token: 0x040014CC RID: 5324
		private ItemRequest _itemRequest;

		// Token: 0x040014CD RID: 5325
		private List<Inscription.Key> _keywords;

		// Token: 0x040014CE RID: 5326
		private System.Random _random;

		// Token: 0x020004CA RID: 1226
		private sealed class ExtraSeed : MonoBehaviour
		{
			// Token: 0x040014CF RID: 5327
			public short value;
		}
	}
}
