using System;
using Characters.Abilities;
using Data;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x02000614 RID: 1556
	public class Chef : Npc
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x0005E978 File Offset: 0x0005CB78
		public string submitLine
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/chef/submit/line").Random<string>();
			}
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0005E98C File Offset: 0x0005CB8C
		private void Start()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1177618293 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			if (MMMaths.PercentChance(this._random, currentChapter.currentStage.marketSettings.masterPossibility))
			{
				base.Activate();
				return;
			}
			base.Deactivate();
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0005EA00 File Offset: 0x0005CC00
		protected override void OnActivate()
		{
			SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
			GlobalSettings marketSettings2 = Settings.instance.marketSettings;
			Rarity rarity = marketSettings.masterDishPossibilities.Evaluate(this._random);
			AbilityBuff abilityBuff = this._foodList.Take(this._random, rarity);
			float num = (float)marketSettings2.masterDishPrices[rarity] * marketSettings.masterDishPriceMultiplier;
			num *= UnityEngine.Random.Range(0.95f, 1.05f);
			this._price = (int)(num / 10f) * 10;
			this._foodInstance = UnityEngine.Object.Instantiate<AbilityBuff>(abilityBuff, this._slot);
			this._foodInstance.name = abilityBuff.name;
			this._foodInstance.price = this._price;
			this._foodInstance.onSold += delegate()
			{
				this._price = 0;
				this._priceDisplay.text = "---";
				this._lineText.Run(this.submitLine);
			};
			this._foodInstance.Initialize();
			this._priceDisplay.text = this._price.ToString();
			this._lineText.gameObject.SetActive(true);
			this._talk.SetActive(true);
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0005EB16 File Offset: 0x0005CD16
		protected override void OnDeactivate()
		{
			this._lineText.gameObject.SetActive(false);
			this._priceDisplay.text = string.Empty;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0005EB39 File Offset: 0x0005CD39
		public void Update()
		{
			this._priceDisplay.color = (GameData.Currency.gold.Has(this._price) ? Color.white : Color.red);
		}

		// Token: 0x04001A51 RID: 6737
		private const int _randomSeed = 1177618293;

		// Token: 0x04001A52 RID: 6738
		[SerializeField]
		private TMP_Text _priceDisplay;

		// Token: 0x04001A53 RID: 6739
		[SerializeField]
		private Transform _slot;

		// Token: 0x04001A54 RID: 6740
		[SerializeField]
		private AbilityBuffList _foodList;

		// Token: 0x04001A55 RID: 6741
		[SerializeField]
		private NpcLineText _lineText;

		// Token: 0x04001A56 RID: 6742
		[SerializeField]
		private GameObject _talk;

		// Token: 0x04001A57 RID: 6743
		private AbilityBuff _foodInstance;

		// Token: 0x04001A58 RID: 6744
		private int _price;

		// Token: 0x04001A59 RID: 6745
		private System.Random _random;
	}
}
