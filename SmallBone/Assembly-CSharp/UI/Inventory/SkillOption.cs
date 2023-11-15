using System;
using Characters.Gear.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x02000445 RID: 1093
	public class SkillOption : MonoBehaviour
	{
		// Token: 0x060014C6 RID: 5318 RVA: 0x000408F4 File Offset: 0x0003EAF4
		public void Set(SkillInfo skillDescInfo)
		{
			this._name.text = skillDescInfo.displayName;
			this._icon.sprite = skillDescInfo.cachedIcon;
			this._icon.SetNativeSize();
			if (this._description != null)
			{
				this._description.text = skillDescInfo.description;
			}
			if (this._cooldown != null)
			{
				this._cooldown.Set(skillDescInfo.action.cooldown);
			}
		}

		// Token: 0x040011E0 RID: 4576
		[SerializeField]
		private TextMeshProUGUI _name;

		// Token: 0x040011E1 RID: 4577
		[SerializeField]
		private Image _icon;

		// Token: 0x040011E2 RID: 4578
		[SerializeField]
		private TextMeshProUGUI _description;

		// Token: 0x040011E3 RID: 4579
		[SerializeField]
		private Cooldown _cooldown;
	}
}
