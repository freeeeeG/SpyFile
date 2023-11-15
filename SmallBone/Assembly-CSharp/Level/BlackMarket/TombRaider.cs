using System;
using System.Collections;
using Data;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x02000622 RID: 1570
	public class TombRaider : Npc
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x0005FBD2 File Offset: 0x0005DDD2
		public string submitLine
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/TombRaider/submit/line").Random<string>();
			}
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0005FBE4 File Offset: 0x0005DDE4
		private void Start()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			SettingsByStage marketSettings = currentChapter.currentStage.marketSettings;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1485841739 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			Rarity rarity = marketSettings.tombRaiderGearPossibilities.Evaluate(this._random);
			this._gearToUnlock = Singleton<Service>.Instance.gearManager.GetGearToUnlock(this._random, rarity);
			bool flag = MMMaths.PercentChance(this._random, currentChapter.currentStage.marketSettings.tombRaiderPossibility);
			if (this._gearToUnlock == null || !flag)
			{
				base.Deactivate();
				return;
			}
			this._unlockPrice = marketSettings.tombRaiderUnlockPrices[this._gearToUnlock.rarity];
			base.Activate();
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0005FCB7 File Offset: 0x0005DEB7
		private IEnumerator CDropGear()
		{
			TombRaider.<>c__DisplayClass11_0 CS$<>8__locals1 = new TombRaider.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			GearRequest request = this._gearToUnlock.LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			CS$<>8__locals1.droppedGear = levelManager.DropGear(request, this._slot.position);
			CS$<>8__locals1.droppedGear.dropped.price = this._unlockPrice;
			CS$<>8__locals1.droppedGear.dropped.priceCurrency = GameData.Currency.Type.DarkQuartz;
			CS$<>8__locals1.destructible = CS$<>8__locals1.droppedGear.destructible;
			CS$<>8__locals1.droppedGear.destructible = false;
			this._priceDisplay.text = CS$<>8__locals1.droppedGear.dropped.price.ToString();
			this._priceDisplay.color = (GameData.Currency.darkQuartz.Has(this._unlockPrice) ? Color.white : Color.red);
			CS$<>8__locals1.droppedGear.dropped.onLoot += CS$<>8__locals1.<CDropGear>g__OnLoot|0;
			yield break;
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0005FCC6 File Offset: 0x0005DEC6
		protected override void OnActivate()
		{
			this._lineText.gameObject.SetActive(true);
			this._talk.SetActive(true);
			base.StartCoroutine(this.CDropGear());
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0005FCF2 File Offset: 0x0005DEF2
		protected override void OnDeactivate()
		{
			this._lineText.gameObject.SetActive(false);
			this._priceDisplay.text = string.Empty;
		}

		// Token: 0x04001AA9 RID: 6825
		private const int _randomSeed = 1485841739;

		// Token: 0x04001AAA RID: 6826
		[SerializeField]
		private TMP_Text _priceDisplay;

		// Token: 0x04001AAB RID: 6827
		[SerializeField]
		private Transform _slot;

		// Token: 0x04001AAC RID: 6828
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x04001AAD RID: 6829
		[SerializeField]
		private GameObject _talk;

		// Token: 0x04001AAE RID: 6830
		private int _unlockPrice;

		// Token: 0x04001AAF RID: 6831
		private GearReference _gearToUnlock;

		// Token: 0x04001AB0 RID: 6832
		private System.Random _random;
	}
}
