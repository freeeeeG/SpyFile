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
	// Token: 0x0200061E RID: 1566
	public class QuintessenceMaster : Npc
	{
		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001F61 RID: 8033 RVA: 0x0005F7A2 File Offset: 0x0005D9A2
		public string submitLine
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/QuintessenceMeister/submit/line").Random<string>();
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0005F7B4 File Offset: 0x0005D9B4
		private void Start()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1569003183 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			if (MMMaths.PercentChance(this._random, currentChapter.currentStage.marketSettings.quintessencePossibility))
			{
				base.Activate();
				return;
			}
			base.Deactivate();
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0005F827 File Offset: 0x0005DA27
		private IEnumerator CDropGear()
		{
			QuintessenceMaster.<>c__DisplayClass10_0 CS$<>8__locals1 = new QuintessenceMaster.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			SettingsByStage settingsByStage = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
			GlobalSettings globalSetting = Settings.instance.marketSettings;
			Rarity rarity = settingsByStage.quintessenceMeisterPossibilities.Evaluate(this._random);
			EssenceReference quintessenceToTake = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(this._random, rarity);
			EssenceRequest request = quintessenceToTake.LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			CS$<>8__locals1.droppedGear = levelManager.DropQuintessence(request, this._slot.position);
			float num = (float)globalSetting.quintessenceMeisterPrices[rarity] * settingsByStage.quintessenceMeisterPriceMultiplier;
			num *= UnityEngine.Random.Range(0.95f, 1.05f);
			this._price = (int)(num / 10f) * 10;
			CS$<>8__locals1.droppedGear.dropped.price = this._price;
			CS$<>8__locals1.destructible = CS$<>8__locals1.droppedGear.destructible;
			CS$<>8__locals1.droppedGear.destructible = false;
			CS$<>8__locals1.droppedGear.dropped.dropMovement.Stop();
			this._priceDisplay.text = this._price.ToString();
			CS$<>8__locals1.droppedGear.dropped.onLoot += CS$<>8__locals1.<CDropGear>g__OnLoot|0;
			yield break;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0005F836 File Offset: 0x0005DA36
		protected override void OnActivate()
		{
			this._lineText.gameObject.SetActive(true);
			this._talk.SetActive(true);
			base.StartCoroutine(this.CDropGear());
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0005F862 File Offset: 0x0005DA62
		protected override void OnDeactivate()
		{
			this._lineText.gameObject.SetActive(false);
			this._priceDisplay.text = string.Empty;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0005F885 File Offset: 0x0005DA85
		public void Update()
		{
			this._priceDisplay.color = (GameData.Currency.gold.Has(this._price) ? Color.white : Color.red);
		}

		// Token: 0x04001A89 RID: 6793
		private const int _randomSeed = 1569003183;

		// Token: 0x04001A8A RID: 6794
		[SerializeField]
		private TMP_Text _priceDisplay;

		// Token: 0x04001A8B RID: 6795
		[SerializeField]
		private Transform _slot;

		// Token: 0x04001A8C RID: 6796
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x04001A8D RID: 6797
		[SerializeField]
		private GameObject _talk;

		// Token: 0x04001A8E RID: 6798
		private int _price;

		// Token: 0x04001A8F RID: 6799
		private System.Random _random;
	}
}
