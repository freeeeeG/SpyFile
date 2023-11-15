using System;
using flanne.RuneSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200021F RID: 543
	public class RuneDescription : DataUIBinding<RuneData>
	{
		// Token: 0x06000C0C RID: 3084 RVA: 0x0002C968 File Offset: 0x0002AB68
		public override void Refresh()
		{
			this.iconImage.sprite = base.data.icon;
			this.costTMP.text = base.data.costPerLevel.ToString();
			this.nameTMP.text = base.data.nameString;
			this.descriptionTMP.text = base.data.description;
		}

		// Token: 0x0400086D RID: 2157
		[SerializeField]
		private Image iconImage;

		// Token: 0x0400086E RID: 2158
		[SerializeField]
		private TMP_Text costTMP;

		// Token: 0x0400086F RID: 2159
		[SerializeField]
		private TMP_Text nameTMP;

		// Token: 0x04000870 RID: 2160
		[SerializeField]
		private TMP_Text descriptionTMP;
	}
}
