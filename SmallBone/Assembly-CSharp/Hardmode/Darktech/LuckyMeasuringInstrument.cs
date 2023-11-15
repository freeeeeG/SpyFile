using System;
using System.Collections;
using Characters.Gear;
using Data;
using GameResources;
using Level;
using Level.BlackMarket;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x0200016C RID: 364
	public sealed class LuckyMeasuringInstrument : MonoBehaviour
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x00015AFD File Offset: 0x00013CFD
		public int lootCount
		{
			get
			{
				return Singleton<DarktechManager>.Instance.setting.행운계측기설정.lootableCount;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00015B13 File Offset: 0x00013D13
		public int remainLootCount
		{
			get
			{
				return this.lootCount - GameData.HardmodeProgress.luckyMeasuringInstrument.lootCount.value;
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00015B2C File Offset: 0x00013D2C
		private void Start()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 716722307 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			int value = GameData.HardmodeProgress.luckyMeasuringInstrument.lootCount.value;
			int lootableCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.lootableCount;
			this._reroll.@base = this;
			this._reroll.onInteracted += this.Load;
			this._reroll.Initialize();
			if (value >= lootableCount)
			{
				this._reroll.Deactivate();
				return;
			}
			this.Load();
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00015BDC File Offset: 0x00013DDC
		private void EvaluateRarity()
		{
			int uniquePityCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.uniquePityCount;
			int legendaryPityCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.legendaryPityCount;
			GameData.HardmodeProgress.LuckyMeasuringInstrument luckyMeasuringInstrument = GameData.HardmodeProgress.luckyMeasuringInstrument;
			IntData lastUniqueDropOrder = luckyMeasuringInstrument.lastUniqueDropOrder;
			IntData lastLegendaryDropOrder = luckyMeasuringInstrument.lastLegendaryDropOrder;
			this._rarity = Singleton<DarktechManager>.Instance.setting.행운계측기설정.weightByRarity.Evaluate(this._random);
			IntData refreshCount = luckyMeasuringInstrument.refreshCount;
			if (refreshCount.value - lastLegendaryDropOrder.value >= legendaryPityCount)
			{
				this._rarity = Rarity.Legendary;
				lastLegendaryDropOrder.value = refreshCount.value;
			}
			else if (refreshCount.value - lastUniqueDropOrder.value >= uniquePityCount && refreshCount.value - lastLegendaryDropOrder.value >= uniquePityCount)
			{
				this._rarity = Rarity.Unique;
				lastUniqueDropOrder.value = refreshCount.value;
			}
			Rarity rarity = this._rarity;
			if (rarity == Rarity.Unique)
			{
				lastUniqueDropOrder.value = refreshCount.value;
				return;
			}
			if (rarity != Rarity.Legendary)
			{
				return;
			}
			lastLegendaryDropOrder.value = refreshCount.value;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00015CDD File Offset: 0x00013EDD
		private IEnumerator CLoad()
		{
			LuckyMeasuringInstrument.<>c__DisplayClass13_0 CS$<>8__locals1 = new LuckyMeasuringInstrument.<>c__DisplayClass13_0();
			CS$<>8__locals1.<>4__this = this;
			while (!this._collector.loadCompleted)
			{
				yield return null;
			}
			SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
			CS$<>8__locals1.luckyMeasuringInstrumentData = GameData.HardmodeProgress.luckyMeasuringInstrument;
			IntData refreshCount = CS$<>8__locals1.luckyMeasuringInstrumentData.refreshCount;
			ItemReference itemReference = null;
			while (itemReference == null)
			{
				bool flag = false;
				this.EvaluateRarity();
				itemReference = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._rarity);
				for (int i = 0; i < CS$<>8__locals1.luckyMeasuringInstrumentData.items.length; i++)
				{
					string text = CS$<>8__locals1.luckyMeasuringInstrumentData.items[i];
					if (text != null && itemReference.name.Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
					}
				}
				if (flag)
				{
					itemReference = null;
				}
			}
			ItemRequest gearRequest = itemReference.LoadAsync();
			while (!gearRequest.isDone)
			{
				yield return null;
			}
			CS$<>8__locals1.gear = Singleton<Service>.Instance.levelManager.DropItem(gearRequest, this._slot.dropPoint);
			CS$<>8__locals1.destructible = CS$<>8__locals1.gear.destructible;
			CS$<>8__locals1.gear.destructible = false;
			CS$<>8__locals1.gear.dropped.onLoot += CS$<>8__locals1.<CLoad>g__OnLoot|0;
			string name = CS$<>8__locals1.gear.name;
			if (CS$<>8__locals1.luckyMeasuringInstrumentData.refreshCount.value < CS$<>8__locals1.luckyMeasuringInstrumentData.maxRefreshCount)
			{
				CS$<>8__locals1.luckyMeasuringInstrumentData.items[CS$<>8__locals1.luckyMeasuringInstrumentData.refreshCount.value] = name;
			}
			if (this._slot.droppedGear != null && this._slot.droppedGear.gear.state == Gear.State.Dropped)
			{
				UnityEngine.Object.Destroy(this._slot.droppedGear.gear.gameObject);
			}
			this._slot.droppedGear = CS$<>8__locals1.gear.dropped;
			CS$<>8__locals1.gear.dropped.dropMovement.Stop();
			CS$<>8__locals1.gear.dropped.dropMovement.Float();
			yield break;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00015CEC File Offset: 0x00013EEC
		private void Load()
		{
			base.StartCoroutine(this.CLoad());
		}

		// Token: 0x04000580 RID: 1408
		private const int _randomSeed = 716722307;

		// Token: 0x04000581 RID: 1409
		[SerializeField]
		private GameObject _body;

		// Token: 0x04000582 RID: 1410
		[SerializeField]
		private LuckyMeasuringInstrumentSlot _slot;

		// Token: 0x04000583 RID: 1411
		[SerializeField]
		private LuckyMeasuringInstrumentReroll _reroll;

		// Token: 0x04000584 RID: 1412
		[SerializeField]
		private Collector _collector;

		// Token: 0x04000585 RID: 1413
		private System.Random _random;

		// Token: 0x04000586 RID: 1414
		private Rarity _rarity;
	}
}
