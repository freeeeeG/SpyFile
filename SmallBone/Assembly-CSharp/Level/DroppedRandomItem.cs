using System;
using System.Collections;
using Characters;
using Characters.Operations;
using Characters.Operations.Fx;
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
	// Token: 0x020004CC RID: 1228
	public sealed class DroppedRandomItem : DroppedPurchasableReward
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x0004ABB0 File Offset: 0x00048DB0
		public string interaction
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/search");
			}
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0004AFFC File Offset: 0x000491FC
		protected override void Awake()
		{
			base.Awake();
			this._onDeactivate.Initialize();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0004B068 File Offset: 0x00049268
		private void Start()
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged += this.Load;
			this.Load();
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0004B08B File Offset: 0x0004928B
		private void OnDestroy()
		{
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest == null)
			{
				return;
			}
			itemRequest.Release();
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0004B09D File Offset: 0x0004929D
		private void EvaluateGearRarity()
		{
			this._gearRarity = Settings.instance.containerPossibilities[this._rarity].Evaluate(this._random);
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0004B0C8 File Offset: 0x000492C8
		private void Load()
		{
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest != null)
			{
				itemRequest.Release();
			}
			do
			{
				this._rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(this._random);
				this.EvaluateGearRarity();
				this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._gearRarity);
			}
			while (this._itemToDrop == null);
			this._itemRequest = this._itemToDrop.LoadAsync();
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0004B150 File Offset: 0x00049350
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			if (base.price != 0)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, this._dropPoint.position);
			this._spawn.Run(character);
			base.StartCoroutine(this.CDrop());
			this.ClosePopup();
			base.Deactivate();
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0004B1AE File Offset: 0x000493AE
		private IEnumerator CDrop()
		{
			while (!this._itemRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			Singleton<Service>.Instance.levelManager.DropItem(this._itemRequest, this._dropPoint.position);
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0004B1C0 File Offset: 0x000493C0
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

		// Token: 0x060017E1 RID: 6113 RVA: 0x00047EF0 File Offset: 0x000460F0
		public override void ClosePopup()
		{
			base.ClosePopup();
			Scene<GameBase>.instance.uiManager.gearPopupCanvas.Close();
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00002191 File Offset: 0x00000391
		public void Initialize()
		{
		}

		// Token: 0x040014D3 RID: 5331
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x040014D4 RID: 5332
		[SerializeField]
		private int priority;

		// Token: 0x040014D5 RID: 5333
		[SerializeField]
		private Color _startColor;

		// Token: 0x040014D6 RID: 5334
		[SerializeField]
		private Color _endColor;

		// Token: 0x040014D7 RID: 5335
		[SerializeField]
		private Curve _curve;

		// Token: 0x040014D8 RID: 5336
		[SerializeField]
		[Subcomponent(typeof(SpawnEffect))]
		private SpawnEffect _spawn;

		// Token: 0x040014D9 RID: 5337
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onDeactivate;

		// Token: 0x040014DA RID: 5338
		private const int _randomSeed = 2028506624;

		// Token: 0x040014DB RID: 5339
		private System.Random _random;

		// Token: 0x040014DC RID: 5340
		private Rarity _rarity;

		// Token: 0x040014DD RID: 5341
		private Rarity _gearRarity;

		// Token: 0x040014DE RID: 5342
		private ItemReference _itemToDrop;

		// Token: 0x040014DF RID: 5343
		private ItemRequest _itemRequest;
	}
}
