using System;
using flanne.RuneSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000220 RID: 544
	public class RuneIcon : DataUIBinding<RuneData>
	{
		// Token: 0x06000C0E RID: 3086 RVA: 0x0002C9DA File Offset: 0x0002ABDA
		public override void Refresh()
		{
			this.iconImage.sprite = base.data.icon;
			this.levelTMP.text = base.data.level.ToString();
		}

		// Token: 0x04000871 RID: 2161
		[SerializeField]
		private Image iconImage;

		// Token: 0x04000872 RID: 2162
		[SerializeField]
		private TMP_Text levelTMP;
	}
}
