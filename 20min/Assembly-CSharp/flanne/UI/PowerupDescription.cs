using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200021C RID: 540
	public class PowerupDescription : DataUIBinding<Powerup>
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x0002C7FC File Offset: 0x0002A9FC
		public override void Refresh()
		{
			if (this.icon != null)
			{
				this.icon.sprite = base.data.icon;
			}
			this.nameTMP.text = base.data.nameString;
			this.descriptionTMP.text = base.data.description;
			if (this.powerupTreeUI != null)
			{
				if (base.data.powerupTreeUIData != null)
				{
					this.powerupTreeUI.gameObject.SetActive(true);
					this.powerupTreeUI.data = base.data.powerupTreeUIData;
					return;
				}
				this.powerupTreeUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x04000863 RID: 2147
		[SerializeField]
		private Image icon;

		// Token: 0x04000864 RID: 2148
		[SerializeField]
		private TMP_Text nameTMP;

		// Token: 0x04000865 RID: 2149
		[SerializeField]
		private TMP_Text descriptionTMP;

		// Token: 0x04000866 RID: 2150
		[SerializeField]
		private PowerupTreeUI powerupTreeUI;
	}
}
