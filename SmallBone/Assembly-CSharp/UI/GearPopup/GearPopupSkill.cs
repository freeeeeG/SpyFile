using System;
using Characters.Gear.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup
{
	// Token: 0x02000451 RID: 1105
	public class GearPopupSkill : MonoBehaviour
	{
		// Token: 0x0600150D RID: 5389 RVA: 0x0004217C File Offset: 0x0004037C
		public void Set(SkillInfo skillInfo)
		{
			string cooldown = string.Empty;
			skillInfo.action.cooldown.Serialize();
			if (skillInfo.action.cooldown.time != null)
			{
				cooldown = skillInfo.action.cooldown.time.cooldownTime.ToString("0");
			}
			this.Set(skillInfo.GetIcon(), skillInfo.displayName, cooldown, skillInfo.description);
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x000421ED File Offset: 0x000403ED
		public void Set(Sprite icon, string name, string cooldown, string description)
		{
			this._name.text = name;
			this._description.text = description;
			this._cooldown.text = cooldown;
			this._icon.sprite = icon;
		}

		// Token: 0x0400125D RID: 4701
		[SerializeField]
		private Image _icon;

		// Token: 0x0400125E RID: 4702
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400125F RID: 4703
		[SerializeField]
		private TMP_Text _cooldown;

		// Token: 0x04001260 RID: 4704
		[SerializeField]
		private TMP_Text _description;
	}
}
