using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200023F RID: 575
	public class PowerupWidget : Widget<PowerupProperties>
	{
		// Token: 0x06000C9D RID: 3229 RVA: 0x0002E0B0 File Offset: 0x0002C2B0
		public override void SetProperties(PowerupProperties properties)
		{
			Powerup powerup = properties.powerup;
			if (this.icon != null)
			{
				this.icon.sprite = powerup.icon;
			}
			if (this.nameTMP != null)
			{
				this.nameTMP.text = powerup.nameString;
			}
			if (this.descriptionTMP != null)
			{
				this.descriptionTMP.text = powerup.description;
			}
		}

		// Token: 0x040008D3 RID: 2259
		[SerializeField]
		private Image icon;

		// Token: 0x040008D4 RID: 2260
		[SerializeField]
		private TMP_Text nameTMP;

		// Token: 0x040008D5 RID: 2261
		[SerializeField]
		private TMP_Text descriptionTMP;
	}
}
