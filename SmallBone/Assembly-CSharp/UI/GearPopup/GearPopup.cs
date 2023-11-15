using System;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Weapons;
using Data;
using GameResources;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.GearPopup
{
	// Token: 0x0200044C RID: 1100
	public class GearPopup : MonoBehaviour
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x000412EE File Offset: 0x0003F4EE
		public RectTransform rectTransform
		{
			get
			{
				return this._rectTransform;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x000412F6 File Offset: 0x0003F4F6
		private static string _interactionLootLabel
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/loot");
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x00041302 File Offset: 0x0003F502
		private static string _interactionPurcaseLabel
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/purchase");
			}
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00041310 File Offset: 0x0003F510
		private void Update()
		{
			if (this._interactiveObject == null)
			{
				return;
			}
			this.ProcessDetailView();
			if (this._pressToDestroy == null)
			{
				return;
			}
			if (this._interactiveObject.pressingPercent == 0f)
			{
				this._pressToDestroy.StopPressing();
				return;
			}
			this._pressToDestroy.PlayPressingSound();
			this._pressToDestroy.SetPercent(this._interactiveObject.pressingPercent);
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00041380 File Offset: 0x0003F580
		private void ProcessDetailView()
		{
			if (!this._detailContainer.activeSelf && KeyMapper.Map.Down.Value > 0.5f)
			{
				this._detailContainer.SetActive(true);
				return;
			}
			if (this._detailContainer.activeSelf && KeyMapper.Map.Down.Value < 0.5f)
			{
				this._detailContainer.SetActive(false);
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x000413EC File Offset: 0x0003F5EC
		private void OnDisable()
		{
			if (this._pressToDestroy == null)
			{
				return;
			}
			this._pressToDestroy.StopPressing();
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00041408 File Offset: 0x0003F608
		private void SetBasic(Gear gear)
		{
			this._interactiveObject = gear.dropped;
			this._name.text = gear.displayName;
			if (gear.gearTag.HasFlag(Gear.Tag.Omen))
			{
				this._rarity.text = Localization.GetLocalizedString("synergy/key/Omen/name");
			}
			else
			{
				this._rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", gear.rarity));
			}
			this._description.text = gear.description;
			this._rarityAndCategory.gameObject.SetActive(true);
			this.SetInteractionLabel(gear.dropped);
			this.SetDestructible(gear);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x000414C8 File Offset: 0x0003F6C8
		private void SetDestructible(Gear gear)
		{
			if (this._pressToDestroy == null)
			{
				return;
			}
			this._pressToDestroy.gameObject.SetActive(gear.destructible);
			if (!gear.destructible)
			{
				return;
			}
			this._pressToDestroy.gameObject.SetActive(true);
			this._pressToDestroy.description = Localization.GetLocalizedString("label/inventory/discardItem");
			GameData.Currency currency = GameData.Currency.currencies[gear.currencyTypeByDiscard];
			if (gear.currencyByDiscard > 0)
			{
				if (gear.type == Gear.Type.Quintessence)
				{
					this._pressToDestroy.description = string.Concat(new string[]
					{
						this._pressToDestroy.description,
						"( ",
						Quintessence.currencySpriteKey,
						" ",
						string.Format(" <color=#{0}>{1}</color> )", Quintessence.currencyTextColorCode, gear.currencyByDiscard)
					});
					return;
				}
				this._pressToDestroy.description = string.Concat(new string[]
				{
					this._pressToDestroy.description,
					"( ",
					currency.spriteTMPKey,
					" ",
					string.Format(" <color=#{0}>{1}</color> )", currency.colorCode, (int)((double)gear.currencyByDiscard * currency.multiplier.total))
				});
			}
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00041612 File Offset: 0x0003F812
		private void DisableSetDestructible()
		{
			if (this._pressToDestroy == null)
			{
				return;
			}
			this._pressToDestroy.gameObject.SetActive(false);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00041634 File Offset: 0x0003F834
		private void SetEssenceActive(string name, string description)
		{
			GameObject[] essenceObjects = this._essenceObjects;
			for (int i = 0; i < essenceObjects.Length; i++)
			{
				essenceObjects[i].SetActive(true);
			}
			this._essenceActiveName.text = name;
			this._essenceActiveDesc.text = description;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00041678 File Offset: 0x0003F878
		private void DisableEssenceActive()
		{
			GameObject[] essenceObjects = this._essenceObjects;
			for (int i = 0; i < essenceObjects.Length; i++)
			{
				essenceObjects[i].SetActive(false);
			}
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x000416A4 File Offset: 0x0003F8A4
		private void DisableExtraOptions()
		{
			this._extraOptionContainer.SetActive(false);
			this._extraOptionContainer2.SetActive(false);
			this._viewDetailContainer.SetActive(false);
			this._skill.gameObject.SetActive(false);
			this._skill1.gameObject.SetActive(false);
			this._skill2.gameObject.SetActive(false);
			this._keywordDetail1.gameObject.SetActive(false);
			this._keywordDetail2.gameObject.SetActive(false);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0004172C File Offset: 0x0003F92C
		public void Set(Gear gear)
		{
			Weapon weapon = gear as Weapon;
			if (weapon != null)
			{
				this.Set(weapon);
				return;
			}
			Item item = gear as Item;
			if (item != null)
			{
				this.Set(item);
				return;
			}
			Quintessence quintessence = gear as Quintessence;
			if (quintessence == null)
			{
				return;
			}
			this.Set(quintessence);
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0004176F File Offset: 0x0003F96F
		public void Set(Weapon weapon)
		{
			this.SetBasic(weapon);
			this._cooldownIcon.SetActive(false);
			this._categoryOrCooldown.text = weapon.categoryDisplayName;
			this.SetSkills(weapon);
			this.DisableEssenceActive();
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x000417A4 File Offset: 0x0003F9A4
		private void SetSkills(Weapon weapon)
		{
			this._viewDetailContainer.SetActive(true);
			this._keywordDetail1.gameObject.SetActive(false);
			this._keywordDetail2.gameObject.SetActive(false);
			SkillInfo skillInfo = weapon.currentSkills[0];
			Sprite icon = skillInfo.GetIcon();
			string displayName = skillInfo.displayName;
			this._extraOption.sprite = icon;
			this._extraOptionText.text = displayName;
			this._extraOption1.sprite = icon;
			this._extraOption1Text.text = displayName;
			if (weapon.currentSkills.Count < 2)
			{
				this._skill.Set(skillInfo);
				this._skill.gameObject.SetActive(true);
				this._skill1.gameObject.SetActive(false);
				this._skill2.gameObject.SetActive(false);
				this._extraOptionContainer.SetActive(true);
				this._extraOptionContainer2.SetActive(false);
				return;
			}
			this._extraOptionContainer.SetActive(false);
			this._extraOptionContainer2.SetActive(true);
			SkillInfo skillInfo2 = weapon.currentSkills[1];
			this._extraOption2.sprite = skillInfo2.GetIcon();
			this._extraOption2Text.text = skillInfo2.displayName;
			this._skill1.Set(skillInfo);
			this._skill2.Set(skillInfo2);
			this._skill1.gameObject.SetActive(true);
			this._skill2.gameObject.SetActive(true);
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x00041911 File Offset: 0x0003FB11
		public void Set(Item item)
		{
			this.SetBasic(item);
			this._cooldownIcon.SetActive(false);
			this._categoryOrCooldown.text = string.Empty;
			this.SetKeywords(item);
			this.DisableEssenceActive();
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00041944 File Offset: 0x0003FB44
		private void SetKeywords(Item item)
		{
			this._viewDetailContainer.SetActive(true);
			this._extraOptionContainer.SetActive(false);
			this._extraOptionContainer2.SetActive(true);
			this._extraOption1.sprite = Inscription.GetActiveIcon(item.keyword1);
			this._extraOption1Text.text = Inscription.GetName(item.keyword1);
			this._extraOption2.sprite = Inscription.GetActiveIcon(item.keyword2);
			this._extraOption2Text.text = Inscription.GetName(item.keyword2);
			this._skill.gameObject.SetActive(false);
			this._skill1.gameObject.SetActive(false);
			this._skill2.gameObject.SetActive(false);
			this._keywordDetail1.Set(item.keyword1);
			this._keywordDetail2.Set(item.keyword2);
			this._keywordDetail1.gameObject.SetActive(true);
			this._keywordDetail2.gameObject.SetActive(true);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00041A44 File Offset: 0x0003FC44
		public void Set(Quintessence quintessence)
		{
			this.SetBasic(quintessence);
			if (quintessence.cooldown.time == null)
			{
				this._cooldownIcon.SetActive(false);
				this._categoryOrCooldown.text = string.Empty;
			}
			else
			{
				this._cooldownIcon.SetActive(true);
				this._categoryOrCooldown.text = quintessence.cooldown.time.cooldownTime.ToString();
			}
			this.SetEssenceActive(quintessence.activeName, quintessence.activeDescription);
			this.DisableExtraOptions();
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00041ACC File Offset: 0x0003FCCC
		public void Set(string name, string description, string rarity)
		{
			this._name.text = name;
			this._description.text = description;
			this._rarityAndCategory.gameObject.SetActive(true);
			this._rarity.text = rarity;
			this._cooldownIcon.SetActive(false);
			this._categoryOrCooldown.text = string.Empty;
			this.DisableEssenceActive();
			this.DisableExtraOptions();
			this.DisableSetDestructible();
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00041B3C File Offset: 0x0003FD3C
		public void Set(string name, string description)
		{
			this.Set(name, description, string.Empty);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00041B4B File Offset: 0x0003FD4B
		public void Set(string name, string description, Rarity rarity)
		{
			this.Set(name, description, Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", rarity)));
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00041B74 File Offset: 0x0003FD74
		public void SetInteractionLabel(string interactionLabel)
		{
			this._interactiveObject = null;
			if (this._pressToDestroy != null)
			{
				this._pressToDestroy.gameObject.SetActive(false);
			}
			if (this._interactionDescription == null)
			{
				return;
			}
			this._interactionGuide.SetActive(true);
			this._interactionDescription.text = interactionLabel;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00041BD0 File Offset: 0x0003FDD0
		public void SetInteractionLabel(InteractiveObject interactiveObject, string interactionLabel, string pressingInteractionLabel)
		{
			this._interactiveObject = interactiveObject;
			if (this._interactionDescription == null)
			{
				return;
			}
			this._interactionDescription.text = interactionLabel;
			if (this._pressToDestroy == null)
			{
				return;
			}
			this._pressToDestroy.gameObject.SetActive(true);
			this._pressToDestroy.description = pressingInteractionLabel;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00041C2C File Offset: 0x0003FE2C
		public void SetInteractionLabel(DroppedGear dropped)
		{
			if (this._interactionDescription == null)
			{
				return;
			}
			if (dropped.gear != null && !dropped.gear.lootable)
			{
				this._interactionGuide.SetActive(false);
				return;
			}
			this._interactionGuide.SetActive(true);
			if (dropped.price > 0)
			{
				this.SetInteractionLabelAsPurchase(dropped.priceCurrency, dropped.price);
				return;
			}
			this.SetInteractionLabelAsLoot();
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00041C9E File Offset: 0x0003FE9E
		public void SetInteractionLabelAsLoot()
		{
			this._interactionDescription.text = GearPopup._interactionLootLabel;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00041CB0 File Offset: 0x0003FEB0
		public void SetInteractionLabelAsPurchase(GameData.Currency.Type currencyType, int price)
		{
			GameData.Currency currency = GameData.Currency.currencies[currencyType];
			string spriteTMPKey = currency.spriteTMPKey;
			string arg = currency.colorCode;
			string format = GearPopup._interactionPurcaseLabel;
			if (price == 0)
			{
				format = GearPopup._interactionLootLabel;
			}
			if (!currency.Has(price))
			{
				arg = GameData.Currency.noMoneyColorCode;
			}
			string arg2 = string.Format(" {0}  <color=#{1}>{2}</color> ", spriteTMPKey, arg, price);
			this._interactionDescription.text = string.Format(format, arg2);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00041D18 File Offset: 0x0003FF18
		public void SetInteractionLabelAsPurchase(string label, GameData.Currency.Type currencyType, int price)
		{
			GameData.Currency currency = GameData.Currency.currencies[currencyType];
			string spriteTMPKey = currency.spriteTMPKey;
			string arg = currency.colorCode;
			if (!currency.Has(price))
			{
				arg = GameData.Currency.noMoneyColorCode;
			}
			string arg2 = string.Format(" {0}  <color=#{1}>{2}</color> ", spriteTMPKey, arg, price);
			this._interactionDescription.text = string.Format("{0}({1})", label, arg2);
		}

		// Token: 0x04001223 RID: 4643
		private const float _detailViewThreshold = 0.5f;

		// Token: 0x04001224 RID: 4644
		[SerializeField]
		private Image _image;

		// Token: 0x04001225 RID: 4645
		[SerializeField]
		private Sprite _frame;

		// Token: 0x04001226 RID: 4646
		[SerializeField]
		private Sprite _frameWithKeywords;

		// Token: 0x04001227 RID: 4647
		[SerializeField]
		[Space]
		private RectTransform _rectTransform;

		// Token: 0x04001228 RID: 4648
		[SerializeField]
		[Space]
		private GameObject _interactionGuide;

		// Token: 0x04001229 RID: 4649
		[SerializeField]
		private TMP_Text _interactionDescription;

		// Token: 0x0400122A RID: 4650
		[Space]
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400122B RID: 4651
		[Space]
		[SerializeField]
		private GameObject _rarityAndCategory;

		// Token: 0x0400122C RID: 4652
		[SerializeField]
		private TMP_Text _rarity;

		// Token: 0x0400122D RID: 4653
		[SerializeField]
		private GameObject _cooldownIcon;

		// Token: 0x0400122E RID: 4654
		[SerializeField]
		private TMP_Text _categoryOrCooldown;

		// Token: 0x0400122F RID: 4655
		[SerializeField]
		[Space]
		private TMP_Text _description;

		// Token: 0x04001230 RID: 4656
		[Space]
		[SerializeField]
		private GameObject[] _essenceObjects;

		// Token: 0x04001231 RID: 4657
		[SerializeField]
		private TMP_Text _essenceActiveName;

		// Token: 0x04001232 RID: 4658
		[SerializeField]
		private TMP_Text _essenceActiveDesc;

		// Token: 0x04001233 RID: 4659
		[Space]
		[SerializeField]
		private GameObject _extraOptionContainer;

		// Token: 0x04001234 RID: 4660
		[SerializeField]
		private Image _extraOption;

		// Token: 0x04001235 RID: 4661
		[SerializeField]
		private TMP_Text _extraOptionText;

		// Token: 0x04001236 RID: 4662
		[SerializeField]
		[Space]
		private GameObject _extraOptionContainer2;

		// Token: 0x04001237 RID: 4663
		[SerializeField]
		private Image _extraOption1;

		// Token: 0x04001238 RID: 4664
		[SerializeField]
		private TMP_Text _extraOption1Text;

		// Token: 0x04001239 RID: 4665
		[SerializeField]
		private Image _extraOption2;

		// Token: 0x0400123A RID: 4666
		[SerializeField]
		private TMP_Text _extraOption2Text;

		// Token: 0x0400123B RID: 4667
		[SerializeField]
		private GameObject _viewDetailContainer;

		// Token: 0x0400123C RID: 4668
		[SerializeField]
		[Space]
		private GearPopupSkill _skill;

		// Token: 0x0400123D RID: 4669
		[SerializeField]
		private GearPopupSkill _skill1;

		// Token: 0x0400123E RID: 4670
		[SerializeField]
		private GearPopupSkill _skill2;

		// Token: 0x0400123F RID: 4671
		[Space]
		[SerializeField]
		private GearPopupKeywordDetail _keywordDetail1;

		// Token: 0x04001240 RID: 4672
		[SerializeField]
		private GearPopupKeywordDetail _keywordDetail2;

		// Token: 0x04001241 RID: 4673
		[Space]
		[SerializeField]
		private PressingButton _pressToDestroy;

		// Token: 0x04001242 RID: 4674
		[SerializeField]
		[Space]
		private GameObject _detailContainer;

		// Token: 0x04001243 RID: 4675
		private InteractiveObject _interactiveObject;

		// Token: 0x04001244 RID: 4676
		private const string _omenKey = "synergy/key/Omen/name";
	}
}
