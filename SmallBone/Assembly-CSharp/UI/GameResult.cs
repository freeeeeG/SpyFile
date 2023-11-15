using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using GameResources;
using Hardmode;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x0200039E RID: 926
	public class GameResult : MonoBehaviour
	{
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x00032422 File Offset: 0x00030622
		// (set) Token: 0x06001105 RID: 4357 RVA: 0x0003242A File Offset: 0x0003062A
		public bool animationFinished { get; private set; }

		// Token: 0x06001106 RID: 4358 RVA: 0x00032434 File Offset: 0x00030634
		private void OnEnable()
		{
			PlayerInput.blocked.Attach(this);
			Chronometer.global.AttachTimeScale(this, 0f);
			base.StartCoroutine(this.CAnimate());
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._panel.sprite = this._hardmodePanel;
				this._yourEnemy.sprite = this._yourEnemyInHard;
			}
			else
			{
				this._panel.sprite = this._normalmodePanel;
				this._yourEnemy.sprite = this._yourEnemyInNormal;
			}
			this._playTime.text = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
			this._deaths.text = GameData.Progress.deaths.ToString();
			this._kills.text = GameData.Progress.kills.ToString();
			this._darkQuartz.text = GameData.Currency.darkQuartz.income.ToString();
			this._gold.text = GameData.Currency.gold.income.ToString();
			this._bone.text = GameData.Currency.bone.income.ToString();
			this._totalDamage.text = GameData.Progress.totalDamage.ToString();
			this._totalTakingDamage.text = GameData.Progress.totalTakingDamage.ToString();
			this._bestDamage.text = GameData.Progress.bestDamage.ToString();
			this._totalHeal.text = GameData.Progress.totalHeal.ToString();
			Inventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory;
			int num = GameData.Progress.encounterWeaponCount;
			WeaponInventory weapon = inventory.weapon;
			num += weapon.weapons.Count((Weapon element) => element != null);
			this._encounterWeapon.text = num.ToString();
			num = GameData.Progress.encounterItemCount;
			ItemInventory item = inventory.item;
			num += item.items.Count((Item element) => element != null);
			this._encounterItem.text = num.ToString();
			num = GameData.Progress.encounterItemCount;
			QuintessenceInventory quintessence = inventory.quintessence;
			num += quintessence.items.Count((Quintessence element) => element != null);
			this._encounterEssence.text = num.ToString();
			this.UpdateGearList();
			this._deathCam.texture = CommonResource.instance.deathCamRenderTexture;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x000326EC File Offset: 0x000308EC
		private void OnDisable()
		{
			PlayerInput.blocked.Detach(this);
			Chronometer.global.DetachTimeScale(this);
			this._deathCam.texture = null;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00032714 File Offset: 0x00030914
		private void UpdateGearList()
		{
			this._gearListContainer.Empty();
			this._upgradeListListContainer.Empty();
			Inventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory;
			WeaponInventory weapon = inventory.weapon;
			QuintessenceInventory quintessence = inventory.quintessence;
			ItemInventory item = inventory.item;
			for (int i = 0; i < weapon.weapons.Length; i++)
			{
				Weapon weapon2 = weapon.weapons[i];
				if (weapon2 != null)
				{
					GearImageContainer gearImageContainer = UnityEngine.Object.Instantiate<GearImageContainer>(this._gearContainerPrefab, this._gearListContainer);
					gearImageContainer.image.sprite = weapon2.icon;
					gearImageContainer.image.SetNativeSize();
				}
			}
			for (int j = 0; j < quintessence.items.Count; j++)
			{
				Quintessence quintessence2 = quintessence.items[j];
				if (quintessence2 != null)
				{
					GearImageContainer gearImageContainer2 = UnityEngine.Object.Instantiate<GearImageContainer>(this._gearContainerPrefab, this._gearListContainer);
					gearImageContainer2.image.sprite = quintessence2.icon;
					gearImageContainer2.image.SetNativeSize();
				}
			}
			for (int k = 0; k < item.items.Count; k++)
			{
				Item item2 = item.items[k];
				if (item2 != null)
				{
					GearImageContainer gearImageContainer3 = UnityEngine.Object.Instantiate<GearImageContainer>(this._gearContainerPrefab, this._gearListContainer);
					gearImageContainer3.image.sprite = item2.icon;
					gearImageContainer3.image.SetNativeSize();
				}
			}
			if (!Singleton<HardmodeManager>.Instance.hardmode)
			{
				return;
			}
			UpgradeInventory upgrade = inventory.upgrade;
			for (int l = 0; l < upgrade.upgrades.Count; l++)
			{
				UpgradeObject upgradeObject = upgrade.upgrades[l];
				if (upgradeObject != null)
				{
					GearImageContainer gearImageContainer4 = UnityEngine.Object.Instantiate<GearImageContainer>(this._upgradeContainerPrefab, this._upgradeListListContainer);
					gearImageContainer4.image.sprite = upgradeObject.icon;
					gearImageContainer4.image.SetNativeSize();
				}
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x000328F6 File Offset: 0x00030AF6
		private IEnumerator CAnimate()
		{
			this.animationFinished = false;
			float time = 0f;
			Vector3 targetPosition = this._container.transform.position;
			Vector3 position = targetPosition;
			position.y += 200f;
			while (time < 1f)
			{
				this._container.transform.position = Vector3.LerpUnclamped(position, targetPosition, this._curve.Evaluate(time));
				yield return null;
				time += Time.unscaledDeltaTime;
			}
			this._container.transform.position = targetPosition;
			this.animationFinished = true;
			yield break;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0001913A File Offset: 0x0001733A
		public void ShowEndResult()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00032908 File Offset: 0x00030B08
		public void Show()
		{
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				string localizedString = Localization.GetLocalizedString(this._hardSubTitleKey);
				this._subTitle.text = string.Format(localizedString, Singleton<HardmodeManager>.Instance.currentLevel);
			}
			else
			{
				this._subTitle.text = Localization.GetLocalizedString(this._normalSubTitleKey);
			}
			this._title.text = Localization.GetLocalizedString(this._titleKey);
			this._stageName.text = Singleton<Service>.Instance.levelManager.currentChapter.stageName;
			this._yourEnemy.gameObject.SetActive(true);
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000329B8 File Offset: 0x00030BB8
		public void ShowEndingResult()
		{
			HardmodeManager instance = Singleton<HardmodeManager>.Instance;
			this._title.text = Localization.GetLocalizedString(this._endingTitleKey);
			if (instance.hardmode)
			{
				if (instance.currentLevel > instance.clearedLevel)
				{
					this._subTitle.text = string.Format(Localization.GetLocalizedString(this._hardmodeEndingFirstSubTitleKey), instance.currentLevel);
				}
				else
				{
					this._subTitle.text = string.Format(Localization.GetLocalizedString(this._hardmodeEndingSubTitleKey), instance.currentLevel);
				}
			}
			else
			{
				this._subTitle.text = Localization.GetLocalizedString(this._endingSubTitleKey);
			}
			this._stageName.text = Localization.GetLocalizedString(this._endingStageNameKey);
			this._yourEnemy.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00032A90 File Offset: 0x00030C90
		public void ShowEndingPortrait()
		{
			this._deathCam.gameObject.SetActive(false);
			this._endingPortrait.SetActive(true);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00032AAF File Offset: 0x00030CAF
		public void HideEndingPortrait()
		{
			this._deathCam.gameObject.SetActive(true);
			this._endingPortrait.SetActive(false);
		}

		// Token: 0x04000DE7 RID: 3559
		[SerializeField]
		private GameObject _container;

		// Token: 0x04000DE8 RID: 3560
		[SerializeField]
		private Image _panel;

		// Token: 0x04000DE9 RID: 3561
		[SerializeField]
		private Sprite _hardmodePanel;

		// Token: 0x04000DEA RID: 3562
		[SerializeField]
		private Sprite _normalmodePanel;

		// Token: 0x04000DEB RID: 3563
		[SerializeField]
		private AnimationCurve _curve;

		// Token: 0x04000DEC RID: 3564
		[Header("Left Info")]
		[SerializeField]
		private TextMeshProUGUI _playTime;

		// Token: 0x04000DED RID: 3565
		[SerializeField]
		private TextMeshProUGUI _deaths;

		// Token: 0x04000DEE RID: 3566
		[SerializeField]
		private TextMeshProUGUI _kills;

		// Token: 0x04000DEF RID: 3567
		[SerializeField]
		private TextMeshProUGUI _darkQuartz;

		// Token: 0x04000DF0 RID: 3568
		[SerializeField]
		private TextMeshProUGUI _gold;

		// Token: 0x04000DF1 RID: 3569
		[SerializeField]
		private TextMeshProUGUI _bone;

		// Token: 0x04000DF2 RID: 3570
		[SerializeField]
		private TextMeshProUGUI _totalDamage;

		// Token: 0x04000DF3 RID: 3571
		[SerializeField]
		private TextMeshProUGUI _totalTakingDamage;

		// Token: 0x04000DF4 RID: 3572
		[SerializeField]
		private TextMeshProUGUI _bestDamage;

		// Token: 0x04000DF5 RID: 3573
		[SerializeField]
		private TextMeshProUGUI _totalHeal;

		// Token: 0x04000DF6 RID: 3574
		[SerializeField]
		private TextMeshProUGUI _encounterWeapon;

		// Token: 0x04000DF7 RID: 3575
		[SerializeField]
		private TextMeshProUGUI _encounterItem;

		// Token: 0x04000DF8 RID: 3576
		[SerializeField]
		private TextMeshProUGUI _encounterEssence;

		// Token: 0x04000DF9 RID: 3577
		[Header("Right Info")]
		[SerializeField]
		private TextMeshProUGUI _title;

		// Token: 0x04000DFA RID: 3578
		[SerializeField]
		private TextMeshProUGUI _subTitle;

		// Token: 0x04000DFB RID: 3579
		[SerializeField]
		private Image _yourEnemy;

		// Token: 0x04000DFC RID: 3580
		[SerializeField]
		private Sprite _yourEnemyInNormal;

		// Token: 0x04000DFD RID: 3581
		[SerializeField]
		private Sprite _yourEnemyInHard;

		// Token: 0x04000DFE RID: 3582
		[SerializeField]
		private TextMeshProUGUI _stageName;

		// Token: 0x04000DFF RID: 3583
		[SerializeField]
		private RawImage _deathCam;

		// Token: 0x04000E00 RID: 3584
		[SerializeField]
		private GameObject _endingPortrait;

		// Token: 0x04000E01 RID: 3585
		[SerializeField]
		private Transform _gearListContainer;

		// Token: 0x04000E02 RID: 3586
		[SerializeField]
		private GearImageContainer _gearContainerPrefab;

		// Token: 0x04000E03 RID: 3587
		[SerializeField]
		private Transform _upgradeListListContainer;

		// Token: 0x04000E04 RID: 3588
		[SerializeField]
		private GearImageContainer _upgradeContainerPrefab;

		// Token: 0x04000E06 RID: 3590
		private readonly string _titleKey = "label/gameResult/title";

		// Token: 0x04000E07 RID: 3591
		private readonly string _normalSubTitleKey = "label/gameResult/subTitle";

		// Token: 0x04000E08 RID: 3592
		private readonly string _hardSubTitleKey = "label/gameResult/hardmode/subTitle";

		// Token: 0x04000E09 RID: 3593
		private readonly string _endingTitleKey = "label/gameResult/ending/title";

		// Token: 0x04000E0A RID: 3594
		private readonly string _endingSubTitleKey = "label/gameResult/ending/subTitle";

		// Token: 0x04000E0B RID: 3595
		private readonly string _endingStageNameKey = "label/gameResult/ending/stageName";

		// Token: 0x04000E0C RID: 3596
		private readonly string _hardmodeEndingFirstSubTitleKey = "label/gameResult/ending/hardmode/firstSubTitle";

		// Token: 0x04000E0D RID: 3597
		private readonly string _hardmodeEndingSubTitleKey = "label/gameResult/ending/hardmode/subTitle";
	}
}
