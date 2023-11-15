using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters;
using Data;
using FX;
using Hardmode.Darktech;
using Platforms;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x02000613 RID: 1555
	public sealed class CollectorSpecialSlot : MonoBehaviour
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x0005E4B0 File Offset: 0x0005C6B0
		public Vector3 itemPosition
		{
			get
			{
				return this._itemPosition.position;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001F1C RID: 7964 RVA: 0x0005E4BD File Offset: 0x0005C6BD
		// (set) Token: 0x06001F1D RID: 7965 RVA: 0x0005E4C8 File Offset: 0x0005C6C8
		public DroppedPurchasableReward dropped
		{
			get
			{
				return this._dropped;
			}
			set
			{
				this._dropped = value;
				this._text.text = this._dropped.price.ToString();
			}
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0005E4FA File Offset: 0x0005C6FA
		private void Awake()
		{
			this._reroll.onInteracted += this.DisplayItem;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0005E514 File Offset: 0x0005C714
		private void OnEnable()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 716722307 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			this._darkTechManager = Singleton<DarktechManager>.Instance;
			this._remainList = this._darkTechManager.setting.품목순화장치가중치.ToList<DarktechSetting.ItemRotationEquipmentInfo>();
			this._cycle = new HashSet<DroppedPurchasableReward>();
			this._itemWeights = new List<ValueTuple<DroppedPurchasableReward, float>>();
			this._priceMultiplierByStage = this._darkTechManager.setting.품목순환장치상품별가격Dict[new ValueTuple<Chapter.Type, int>(currentChapter.type, currentChapter.stageIndex)];
			this.DisplayItem();
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0005E5CC File Offset: 0x0005C7CC
		private void DisplayItem()
		{
			if (this._remainList.Count == 0)
			{
				return;
			}
			DroppedPurchasableReward item = this.TakeOne();
			this.DropItem(item);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0005E5F8 File Offset: 0x0005C7F8
		private DroppedPurchasableReward TakeOne()
		{
			if (this._cycle.Count == this._remainList.Count)
			{
				this._cycle.Clear();
			}
			this._itemWeights.Clear();
			for (int i = 0; i < this._remainList.Count; i++)
			{
				if (!this._cycle.Contains(this._remainList[i].item))
				{
					this._itemWeights.Add(new ValueTuple<DroppedPurchasableReward, float>(this._remainList[i].item, (float)this._remainList[i].weight));
				}
			}
			WeightedRandomizer<DroppedPurchasableReward> weightedRandomizer = new WeightedRandomizer<DroppedPurchasableReward>(this._itemWeights);
			DroppedPurchasableReward droppedPurchasableReward;
			if (this._itemWeights.Count > 1)
			{
				do
				{
					droppedPurchasableReward = weightedRandomizer.TakeOne(this._random);
					if (!(this.dropped != null))
					{
						break;
					}
				}
				while (droppedPurchasableReward.name.Equals(this.dropped.name, StringComparison.OrdinalIgnoreCase));
			}
			else
			{
				droppedPurchasableReward = weightedRandomizer.TakeOne(this._random);
			}
			this._cycle.Add(droppedPurchasableReward);
			return droppedPurchasableReward;
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0005E708 File Offset: 0x0005C908
		private void DropItem(DroppedPurchasableReward item)
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
			GlobalSettings marketSettings2 = Settings.instance.marketSettings;
			DroppedPurchasableReward droppedPurchasableReward = UnityEngine.Object.Instantiate<DroppedPurchasableReward>(item, this._itemPosition);
			droppedPurchasableReward.name = item.name;
			int price = 1;
			for (int i = 0; i < this._darkTechManager.setting.품목순화장치가중치.Length; i++)
			{
				DarktechSetting.ItemRotationEquipmentInfo itemRotationEquipmentInfo = this._darkTechManager.setting.품목순화장치가중치[i];
				if (itemRotationEquipmentInfo.item.name.Equals(droppedPurchasableReward.name, StringComparison.OrdinalIgnoreCase))
				{
					price = (int)((float)itemRotationEquipmentInfo.basePrice * this._priceMultiplierByStage);
				}
			}
			droppedPurchasableReward.price = price;
			droppedPurchasableReward.onLoot += this.<DropItem>g__OnLoot|21_0;
			if (this.dropped != null)
			{
				UnityEngine.Object.Destroy(this.dropped.gameObject);
			}
			this.dropped = droppedPurchasableReward;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0005E7FC File Offset: 0x0005C9FC
		private void Update()
		{
			if (this._dropped == null || !this._dropped.gameObject.activeInHierarchy)
			{
				this._text.text = "---";
				this._text.color = Color.white;
				return;
			}
			if (this._dropped.price > 0)
			{
				this._text.color = (GameData.Currency.gold.Has(this._dropped.price) ? Color.white : Color.red);
				return;
			}
			this._text.text = "---";
			this._text.color = Color.white;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0005E8A8 File Offset: 0x0005CAA8
		[CompilerGenerated]
		private void <DropItem>g__OnLoot|21_0(Character character)
		{
			Achievement.Type.SpecialGuest.Set();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._buySound, base.transform.position);
			this.dropped.onLoot -= this.<DropItem>g__OnLoot|21_0;
			for (int i = this._remainList.Count - 1; i >= 0; i--)
			{
				if (this.dropped.name.Equals(this._remainList[i].item.name, StringComparison.OrdinalIgnoreCase))
				{
					if (this._cycle.Contains(this._remainList[i].item))
					{
						this._cycle.Remove(this._remainList[i].item);
					}
					this._remainList.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04001A45 RID: 6725
		private const int _randomSeed = 716722307;

		// Token: 0x04001A46 RID: 6726
		[SerializeField]
		private CollectorReroll _reroll;

		// Token: 0x04001A47 RID: 6727
		[SerializeField]
		private Transform _itemPosition;

		// Token: 0x04001A48 RID: 6728
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04001A49 RID: 6729
		[SerializeField]
		private SoundInfo _buySound;

		// Token: 0x04001A4A RID: 6730
		private System.Random _random;

		// Token: 0x04001A4B RID: 6731
		private DroppedPurchasableReward _dropped;

		// Token: 0x04001A4C RID: 6732
		private List<DarktechSetting.ItemRotationEquipmentInfo> _remainList;

		// Token: 0x04001A4D RID: 6733
		private HashSet<DroppedPurchasableReward> _cycle;

		// Token: 0x04001A4E RID: 6734
		private List<ValueTuple<DroppedPurchasableReward, float>> _itemWeights;

		// Token: 0x04001A4F RID: 6735
		private DarktechManager _darkTechManager;

		// Token: 0x04001A50 RID: 6736
		private float _priceMultiplierByStage;
	}
}
