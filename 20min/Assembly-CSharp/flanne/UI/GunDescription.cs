using System;
using flanne.UIExtensions;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000216 RID: 534
	public class GunDescription : MenuEntryDescription<GunMenu, GunData>
	{
		// Token: 0x06000BFA RID: 3066 RVA: 0x0002C66C File Offset: 0x0002A86C
		public override void SetProperties(GunData data)
		{
			this.nameTMP.text = data.nameString;
			this.descriptionTMP.text = data.description;
			this.damageTMP.text = data.damage.ToString("00");
			this.fireRateTMP.text = (1f / data.shotCooldown).ToString("0.0");
			this.projectilesTMP.text = data.numOfProjectiles.ToString("00");
			this.ammoTMP.text = data.maxAmmo.ToString("00");
			this.reloadTimeTMP.text = data.reloadDuration.ToString("0.0");
		}

		// Token: 0x04000856 RID: 2134
		[SerializeField]
		private TMP_Text nameTMP;

		// Token: 0x04000857 RID: 2135
		[SerializeField]
		private TMP_Text descriptionTMP;

		// Token: 0x04000858 RID: 2136
		[SerializeField]
		private TMP_Text damageTMP;

		// Token: 0x04000859 RID: 2137
		[SerializeField]
		private TMP_Text fireRateTMP;

		// Token: 0x0400085A RID: 2138
		[SerializeField]
		private TMP_Text projectilesTMP;

		// Token: 0x0400085B RID: 2139
		[SerializeField]
		private TMP_Text ammoTMP;

		// Token: 0x0400085C RID: 2140
		[SerializeField]
		private TMP_Text reloadTimeTMP;
	}
}
