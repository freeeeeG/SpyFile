using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
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
	// Token: 0x020004CE RID: 1230
	public sealed class DroppedRandomWeaponBox : DroppedPurchasableReward
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x0004ABB0 File Offset: 0x00048DB0
		public string interaction
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/search");
			}
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0004B340 File Offset: 0x00049540
		private void Start()
		{
			this._onDeactivate.Initialize();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 716722307 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex + (int)this._category));
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0004B39C File Offset: 0x0004959C
		private void Load()
		{
			do
			{
				Rarity rarity = this._rarityPossibilities.Evaluate(this._random);
				this._weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponByCategory(this._random, rarity, this._category);
			}
			while (this._weaponToDrop == null);
			this._weaponRequest = this._weaponToDrop.LoadAsync();
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0004B3F6 File Offset: 0x000495F6
		private void OnDestroy()
		{
			WeaponRequest weaponRequest = this._weaponRequest;
			if (weaponRequest == null)
			{
				return;
			}
			weaponRequest.Release();
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0004B408 File Offset: 0x00049608
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

		// Token: 0x060017EF RID: 6127 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0004B4D3 File Offset: 0x000496D3
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			if (base.price != 0)
			{
				return;
			}
			character.StartCoroutine(this.CDelayedDrop(character));
			this.ClosePopup();
			base.Deactivate();
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0004B4FF File Offset: 0x000496FF
		private IEnumerator CDelayedDrop(Character character)
		{
			this.Load();
			while (!this._weaponRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropWeapon(this._weaponRequest, this._dropPoint.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, this._dropPoint.position);
			this._spawned = true;
			base.StartCoroutine(this._onDeactivate.CRun(character));
			yield break;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0004B515 File Offset: 0x00049715
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			Map.Instance.StartCoroutine(this.CDestroy());
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0004B52E File Offset: 0x0004972E
		private IEnumerator CDestroy()
		{
			while (!this._spawned)
			{
				yield return null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x040014E3 RID: 5347
		private const int _randomSeed = 716722307;

		// Token: 0x040014E4 RID: 5348
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x040014E5 RID: 5349
		[SerializeField]
		private Weapon.Category _category;

		// Token: 0x040014E6 RID: 5350
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x040014E7 RID: 5351
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onDeactivate;

		// Token: 0x040014E8 RID: 5352
		private WeaponReference _weaponToDrop;

		// Token: 0x040014E9 RID: 5353
		private WeaponRequest _weaponRequest;

		// Token: 0x040014EA RID: 5354
		private System.Random _random;

		// Token: 0x040014EB RID: 5355
		private bool _spawned;
	}
}
