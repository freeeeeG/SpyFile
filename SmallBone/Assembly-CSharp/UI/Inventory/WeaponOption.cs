using System;
using Characters.Gear.Weapons;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.Inventory
{
	// Token: 0x0200044A RID: 1098
	public class WeaponOption : MonoBehaviour
	{
		// Token: 0x060014DD RID: 5341 RVA: 0x00040FD8 File Offset: 0x0003F1D8
		public void Set(Weapon weapon)
		{
			this._thumnailIcon.enabled = true;
			this._thumnailIcon.sprite = weapon.thumbnail;
			this._thumnailIcon.transform.localScale = Vector3.one * 3f;
			this._thumnailIcon.SetNativeSize();
			this._name.text = weapon.displayName;
			this._rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", weapon.rarity));
			this._category.text = weapon.categoryDisplayName;
			this._flavor.text = (weapon.hasFlavor ? weapon.flavor : string.Empty);
			this._passive.text = weapon.description;
			this._category.gameObject.SetActive(true);
			if (weapon.currentSkills.Count == 1)
			{
				this._skillContainer.gameObject.SetActive(true);
				this._skill2Container.gameObject.SetActive(false);
				this._skillDetailContainer.gameObject.SetActive(true);
				this._skill2DetailContainer.gameObject.SetActive(false);
				this._skillSwapKey.SetActive(false);
				this._skill.Set(weapon.currentSkills[0]);
				this._skillDetail.Set(weapon.currentSkills[0]);
			}
			else
			{
				this._skillContainer.gameObject.SetActive(false);
				this._skill2Container.gameObject.SetActive(true);
				this._skillDetailContainer.gameObject.SetActive(false);
				this._skill2DetailContainer.gameObject.SetActive(true);
				this._skillSwapKey.SetActive(true);
				this._skill1.Set(weapon.currentSkills[0]);
				this._skill2.Set(weapon.currentSkills[1]);
				this._skill1Detail.Set(weapon.currentSkills[0]);
				this._skill2Detail.Set(weapon.currentSkills[1]);
			}
			this._swapSkillNameSimple.text = weapon.activeName;
			this._swapSkillNameDetail.text = weapon.activeName;
			this._swapSkillDescription.text = weapon.activeDescription;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00041234 File Offset: 0x0003F434
		private void Update()
		{
			if (this._detailContainer.activeSelf && !KeyMapper.Map.UiInteraction3.IsPressed)
			{
				this._simpleContainer.SetActive(true);
				this._detailContainer.SetActive(false);
				return;
			}
			if (!this._detailContainer.activeSelf && KeyMapper.Map.UiInteraction3.IsPressed)
			{
				if (KeyMapper.Map.UiInteraction2.IsPressed)
				{
					return;
				}
				this._simpleContainer.SetActive(false);
				this._detailContainer.SetActive(true);
			}
		}

		// Token: 0x0400120B RID: 4619
		[SerializeField]
		private Image _thumnailIcon;

		// Token: 0x0400120C RID: 4620
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400120D RID: 4621
		[SerializeField]
		private TMP_Text _rarity;

		// Token: 0x0400120E RID: 4622
		[SerializeField]
		private TMP_Text _category;

		// Token: 0x0400120F RID: 4623
		[Space]
		[SerializeField]
		private GameObject _simpleContainer;

		// Token: 0x04001210 RID: 4624
		[SerializeField]
		private GameObject _detailContainer;

		// Token: 0x04001211 RID: 4625
		[Space]
		[SerializeField]
		private TMP_Text _flavor;

		// Token: 0x04001212 RID: 4626
		[SerializeField]
		private TMP_Text _passive;

		// Token: 0x04001213 RID: 4627
		[Space]
		[SerializeField]
		private TMP_Text _swapSkillNameSimple;

		// Token: 0x04001214 RID: 4628
		[SerializeField]
		private TMP_Text _swapSkillNameDetail;

		// Token: 0x04001215 RID: 4629
		[SerializeField]
		private TMP_Text _swapSkillDescription;

		// Token: 0x04001216 RID: 4630
		[SerializeField]
		[Space]
		private GameObject _skillSwapKey;

		// Token: 0x04001217 RID: 4631
		[Space]
		[SerializeField]
		private GameObject _skillContainer;

		// Token: 0x04001218 RID: 4632
		[SerializeField]
		private SkillOption _skill;

		// Token: 0x04001219 RID: 4633
		[SerializeField]
		[Space]
		private GameObject _skill2Container;

		// Token: 0x0400121A RID: 4634
		[SerializeField]
		private SkillOption _skill1;

		// Token: 0x0400121B RID: 4635
		[SerializeField]
		private SkillOption _skill2;

		// Token: 0x0400121C RID: 4636
		[Space]
		[SerializeField]
		private GameObject _skillDetailContainer;

		// Token: 0x0400121D RID: 4637
		[SerializeField]
		private SkillOption _skillDetail;

		// Token: 0x0400121E RID: 4638
		[Space]
		[SerializeField]
		private GameObject _skill2DetailContainer;

		// Token: 0x0400121F RID: 4639
		[SerializeField]
		private SkillOption _skill1Detail;

		// Token: 0x04001220 RID: 4640
		[SerializeField]
		private SkillOption _skill2Detail;
	}
}
