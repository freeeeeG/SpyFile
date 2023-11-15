using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Characters.Operations;
using Data;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008BE RID: 2238
	public sealed class TreasureChest : InteractiveObject, ILootable
	{
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06002FA8 RID: 12200 RVA: 0x0008EFC0 File Offset: 0x0008D1C0
		// (remove) Token: 0x06002FA9 RID: 12201 RVA: 0x0008EFF8 File Offset: 0x0008D1F8
		public event Action onLoot;

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06002FAA RID: 12202 RVA: 0x0008F02D File Offset: 0x0008D22D
		// (set) Token: 0x06002FAB RID: 12203 RVA: 0x0008F035 File Offset: 0x0008D235
		public bool looted { get; private set; }

		// Token: 0x06002FAC RID: 12204 RVA: 0x0008F040 File Offset: 0x0008D240
		protected override void Awake()
		{
			base.Awake();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._onDrop.Initialize();
			this._onLegendary.Initialize();
			if (this._dropMovement == null)
			{
				base.Activate();
				return;
			}
			this._dropMovement.onGround += this.Activate;
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x0008F0E4 File Offset: 0x0008D2E4
		private void Start()
		{
			if (this._level == TreasureChest.Level.Set2)
			{
				return;
			}
			this.EvaluateGearRarity();
			this.Load();
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x0008F0FB File Offset: 0x0008D2FB
		public override void OnActivate()
		{
			base.OnActivate();
			base.StartCoroutine(this._onDrop.CRun(Singleton<Service>.Instance.levelManager.player));
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x0008F124 File Offset: 0x0008D324
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x0008F13C File Offset: 0x0008D33C
		public override void InteractWithByPressing(Character character)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this.Drop();
			base.Deactivate();
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x0008F168 File Offset: 0x0008D368
		private T Evalualte<T>(System.Random random, int[] possibilities, IList<T> values)
		{
			int maxValue = possibilities.Sum();
			int num = random.Next(0, maxValue) + 1;
			for (int i = 0; i < possibilities.Length; i++)
			{
				num -= possibilities[i];
				if (num <= 0)
				{
					return values[i];
				}
			}
			return values[0];
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x0008F1B0 File Offset: 0x0008D3B0
		private void Load()
		{
			Treasure.StageInfo treasureInfo = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.treasureInfo;
			int[] possibilities = new int[]
			{
				treasureInfo.goldChestWeight,
				treasureInfo.currencyBagChestWeight,
				treasureInfo.itemChechWeight
			};
			this._rewardType = this.Evalualte<TreasureChest.RewardType>(this._random, possibilities, TreasureChest.rewardValues);
			if (this._rewardType == TreasureChest.RewardType.Items)
			{
				this.LoadItem();
			}
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x0008F220 File Offset: 0x0008D420
		private void LoadItem()
		{
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest != null)
			{
				itemRequest.Release();
			}
			do
			{
				this.EvaluateGearRarity();
				this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._gearRarity);
			}
			while (this._itemToDrop == null);
			this._itemRequest = this._itemToDrop.LoadAsync();
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x0008F27E File Offset: 0x0008D47E
		private void EvaluateGearRarity()
		{
			this._gearRarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.treasureInfo.itemRarityPossibilities.Evaluate(this._random);
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x0008F2B0 File Offset: 0x0008D4B0
		private void Drop()
		{
			TreasureChest.<>c__DisplayClass33_0 CS$<>8__locals1 = new TreasureChest.<>c__DisplayClass33_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.currentStage = Singleton<Service>.Instance.levelManager.currentChapter.currentStage;
			CS$<>8__locals1.levelManager = Singleton<Service>.Instance.levelManager;
			if (this._level == TreasureChest.Level.Set2)
			{
				base.StartCoroutine(CS$<>8__locals1.<Drop>g__CDelayedGoldDrop|0());
				return;
			}
			TreasureChest.<>c__DisplayClass33_1 CS$<>8__locals2 = new TreasureChest.<>c__DisplayClass33_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			switch (this._rewardType)
			{
			case TreasureChest.RewardType.Gold:
				base.StartCoroutine(CS$<>8__locals2.CS$<>8__locals1.<Drop>g__CDelayedGoldDrop|1());
				return;
			case TreasureChest.RewardType.CurrencyBag:
				CS$<>8__locals2.rarity = CS$<>8__locals2.CS$<>8__locals1.currentStage.treasureInfo.currencyRarityPossibilities.Evaluate(this._random);
				CS$<>8__locals2.type = GameData.Currency.Type.Bone;
				CS$<>8__locals2.amountInBag = CS$<>8__locals2.CS$<>8__locals1.currentStage.treasureInfo.boneRangeByRarity.Evaluate(CS$<>8__locals2.rarity);
				base.StartCoroutine(CS$<>8__locals2.<Drop>g__CDelayedCurrencyBagDrop|2());
				return;
			case TreasureChest.RewardType.Items:
				this.Load();
				base.StartCoroutine(CS$<>8__locals2.CS$<>8__locals1.<Drop>g__CDelayedDrop|3());
				if (this._gearRarity == Rarity.Legendary)
				{
					base.StartCoroutine(this._onLegendary.CRun(Singleton<Service>.Instance.levelManager.player));
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x0008F3E7 File Offset: 0x0008D5E7
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest == null)
			{
				return;
			}
			itemRequest.Release();
		}

		// Token: 0x0400275C RID: 10076
		public static readonly ReadOnlyCollection<TreasureChest.RewardType> rewardValues = EnumValues<TreasureChest.RewardType>.Values;

		// Token: 0x0400275D RID: 10077
		private const int _randomSeed = 2028506624;

		// Token: 0x0400275E RID: 10078
		private const float _delayToDrop = 0.5f;

		// Token: 0x0400275F RID: 10079
		[SerializeField]
		private TreasureChest.Level _level;

		// Token: 0x04002760 RID: 10080
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x04002761 RID: 10081
		[SerializeField]
		private Animator _animator;

		// Token: 0x04002762 RID: 10082
		[SerializeField]
		private DropMovement _dropMovement;

		// Token: 0x04002763 RID: 10083
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onDrop;

		// Token: 0x04002764 RID: 10084
		[Header("레전더리 효과")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onLegendary;

		// Token: 0x04002765 RID: 10085
		private ItemReference _itemToDrop;

		// Token: 0x04002766 RID: 10086
		private ItemRequest _itemRequest;

		// Token: 0x04002767 RID: 10087
		private System.Random _random;

		// Token: 0x04002768 RID: 10088
		private TreasureChest.RewardType _rewardType;

		// Token: 0x04002769 RID: 10089
		private Rarity _gearRarity;

		// Token: 0x020008BF RID: 2239
		public enum Level
		{
			// Token: 0x0400276D RID: 10093
			Set2,
			// Token: 0x0400276E RID: 10094
			Set4
		}

		// Token: 0x020008C0 RID: 2240
		public enum RewardType
		{
			// Token: 0x04002770 RID: 10096
			Gold,
			// Token: 0x04002771 RID: 10097
			CurrencyBag,
			// Token: 0x04002772 RID: 10098
			Items
		}

		// Token: 0x020008C1 RID: 2241
		[Serializable]
		public class RewardTypeBoolArray : EnumArray<TreasureChest.RewardType, bool>
		{
			// Token: 0x06002FB9 RID: 12217 RVA: 0x0008F428 File Offset: 0x0008D628
			public RewardTypeBoolArray()
			{
			}

			// Token: 0x06002FBA RID: 12218 RVA: 0x0008F430 File Offset: 0x0008D630
			public RewardTypeBoolArray(params bool[] values)
			{
				int num = Math.Min(base.Array.Length, values.Length);
				for (int i = 0; i < num; i++)
				{
					base.Array[i] = values[i];
				}
			}
		}
	}
}
