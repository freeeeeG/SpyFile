using System;
using Characters.Gear.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x02000449 RID: 1097
	public sealed class UpgradeOption : MonoBehaviour
	{
		// Token: 0x060014DB RID: 5339 RVA: 0x00040F14 File Offset: 0x0003F114
		public void Set(UpgradeObject upgrade)
		{
			this._thumnailIcon.enabled = true;
			this._thumnailIcon.sprite = upgrade.thumbnail;
			this._thumnailIcon.transform.localScale = Vector3.one * 3f;
			this._thumnailIcon.SetNativeSize();
			this._name.text = upgrade.displayName;
			if (upgrade.type == UpgradeObject.Type.Cursed)
			{
				this._type.text = UpgradeResource.Reference.curseText;
			}
			else
			{
				this._type.text = string.Empty;
			}
			this._flavor.text = upgrade.flavor;
			this._description.text = upgrade.reference.GetCurrentDescription("755754", "B2977B");
		}

		// Token: 0x04001204 RID: 4612
		[SerializeField]
		private Image _thumnailIcon;

		// Token: 0x04001205 RID: 4613
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04001206 RID: 4614
		[SerializeField]
		private TMP_Text _type;

		// Token: 0x04001207 RID: 4615
		[SerializeField]
		private TMP_Text _flavor;

		// Token: 0x04001208 RID: 4616
		[SerializeField]
		private TMP_Text _description;

		// Token: 0x04001209 RID: 4617
		private const string activateColorcode = "755754";

		// Token: 0x0400120A RID: 4618
		private const string deactivateColorcode = "B2977B";
	}
}
