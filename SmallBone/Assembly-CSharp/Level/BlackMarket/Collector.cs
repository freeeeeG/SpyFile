using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Gear;
using Characters.Gear.Items;
using Data;
using FX;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x02000615 RID: 1557
	public class Collector : Npc
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x0005EB96 File Offset: 0x0005CD96
		public string submitLine
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/collector/submit/line").Random<string>();
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001F2E RID: 7982 RVA: 0x0005EBA7 File Offset: 0x0005CDA7
		// (set) Token: 0x06001F2F RID: 7983 RVA: 0x0005EBAF File Offset: 0x0005CDAF
		public bool loadCompleted { get; set; }

		// Token: 0x06001F30 RID: 7984 RVA: 0x0005EBB8 File Offset: 0x0005CDB8
		private void Awake()
		{
			this._reroll.onInteracted += this.Reroll;
			this._keywordRandomItemRerollCount = new Dictionary<string, int>();
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0005EBDC File Offset: 0x0005CDDC
		private void Start()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 716722307 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			if (MMMaths.PercentChance(this._random, currentChapter.currentStage.marketSettings.collectorPossibility))
			{
				base.Activate();
				return;
			}
			base.Deactivate();
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0005EC4F File Offset: 0x0005CE4F
		protected override void OnActivate()
		{
			this._lineText.gameObject.SetActive(true);
			this._talk.SetActive(true);
			base.StartCoroutine(this.CDisplayItems(false));
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0005EC7C File Offset: 0x0005CE7C
		protected override void OnDeactivate()
		{
			this._lineText.gameObject.SetActive(false);
			CollectorGearSlot[] slots = this._slots;
			for (int i = 0; i < slots.Length; i++)
			{
				slots[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0005ECBD File Offset: 0x0005CEBD
		private void Reroll()
		{
			base.StartCoroutine(this.CDisplayItems(true));
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0005ECCD File Offset: 0x0005CECD
		private IEnumerator CDisplayItems(bool reroll = false)
		{
			Collector.<>c__DisplayClass19_0 CS$<>8__locals1 = new Collector.<>c__DisplayClass19_0();
			CS$<>8__locals1.<>4__this = this;
			this.loadCompleted = false;
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			CS$<>8__locals1.gearInfosToDrop = new GearReference[this._slots.Length];
			GearRequest[] gearRequests = new GearRequest[this._slots.Length];
			for (int j = 0; j < this._slots.Length; j++)
			{
				this._slots[j].gameObject.SetActive(true);
				ItemReference itemToTake;
				do
				{
					Rarity rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings.collectorItemPossibilities.Evaluate(this._random);
					itemToTake = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, rarity);
				}
				while (CS$<>8__locals1.<CDisplayItems>g__Duplicated|0(itemToTake.name));
				CS$<>8__locals1.gearInfosToDrop[j] = itemToTake;
			}
			for (int k = 0; k < this._slots.Length; k++)
			{
				gearRequests[k] = CS$<>8__locals1.gearInfosToDrop[k].LoadAsync();
			}
			int num;
			for (int i = 0; i < this._slots.Length; i = num + 1)
			{
				Collector.<>c__DisplayClass19_1 CS$<>8__locals2 = new Collector.<>c__DisplayClass19_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				while (!gearRequests[i].isDone)
				{
					yield return null;
				}
				CS$<>8__locals2.gear = Singleton<Service>.Instance.levelManager.DropGear(gearRequests[i], this._slots[i].itemPosition);
				SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
				GlobalSettings marketSettings2 = Settings.instance.marketSettings;
				int price = (int)((float)marketSettings2.collectorItemPrices[CS$<>8__locals2.gear.rarity] * marketSettings.collectorItemPriceMultiplier * marketSettings2.collectorItemPriceMultiplier * UnityEngine.Random.Range(0.95f, 1.05f) / 10f) * 10;
				CS$<>8__locals2.gear.dropped.price = price;
				CS$<>8__locals2.destructible = CS$<>8__locals2.gear.destructible;
				CS$<>8__locals2.gear.destructible = false;
				CS$<>8__locals2.gear.dropped.onLoot += CS$<>8__locals2.<CDisplayItems>g__OnLoot|1;
				CollectorGearSlot collectorGearSlot = this._slots[i];
				if (collectorGearSlot.droppedGear != null && collectorGearSlot.droppedGear.price > 0 && collectorGearSlot.droppedGear.gear.state == Gear.State.Dropped)
				{
					UnityEngine.Object.Destroy(collectorGearSlot.droppedGear.gear.gameObject);
				}
				this._slots[i].droppedGear = CS$<>8__locals2.gear.dropped;
				if (reroll)
				{
					KeywordRandomizer component = CS$<>8__locals2.gear.GetComponent<KeywordRandomizer>();
					if (component != null)
					{
						if (!this._keywordRandomItemRerollCount.ContainsKey(CS$<>8__locals2.gear.name))
						{
							this._keywordRandomItemRerollCount.Add(CS$<>8__locals2.gear.name, 0);
						}
						component.UpdateKeword(this._keywordRandomItemRerollCount[CS$<>8__locals2.gear.name]);
						Dictionary<string, int> keywordRandomItemRerollCount = this._keywordRandomItemRerollCount;
						string name = CS$<>8__locals2.gear.name;
						keywordRandomItemRerollCount[name]++;
					}
				}
				CS$<>8__locals2.gear.dropped.dropMovement.Stop();
				CS$<>8__locals2.gear.dropped.dropMovement.Float();
				CS$<>8__locals2 = null;
				num = i;
			}
			this.loadCompleted = true;
			yield break;
		}

		// Token: 0x04001A5A RID: 6746
		private const int _randomSeed = 716722307;

		// Token: 0x04001A5B RID: 6747
		[SerializeField]
		protected SoundInfo _buySound;

		// Token: 0x04001A5C RID: 6748
		[SerializeField]
		private CollectorReroll _reroll;

		// Token: 0x04001A5D RID: 6749
		[SerializeField]
		private CollectorGearSlot[] _slots;

		// Token: 0x04001A5E RID: 6750
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x04001A5F RID: 6751
		[SerializeField]
		private GameObject _talk;

		// Token: 0x04001A60 RID: 6752
		private System.Random _random;

		// Token: 0x04001A61 RID: 6753
		private Dictionary<string, int> _keywordRandomItemRerollCount;
	}
}
