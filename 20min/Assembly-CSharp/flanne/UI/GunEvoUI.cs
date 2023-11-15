using System;
using flanne.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000217 RID: 535
	public class GunEvoUI : DataUI<GunEvolution>
	{
		// Token: 0x06000BFC RID: 3068 RVA: 0x0002C734 File Offset: 0x0002A934
		protected override void SetProperties()
		{
			this.icon.sprite = base.data.icon;
			this.nameTMP.text = base.data.nameString;
			this.descriptionTMP.text = base.data.description;
		}

		// Token: 0x0400085D RID: 2141
		[SerializeField]
		private Image icon;

		// Token: 0x0400085E RID: 2142
		[SerializeField]
		private TMP_Text nameTMP;

		// Token: 0x0400085F RID: 2143
		[SerializeField]
		private TMP_Text descriptionTMP;
	}
}
