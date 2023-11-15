using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000221 RID: 545
	public class SynergyDescription : DataUIBinding<Powerup>
	{
		// Token: 0x06000C10 RID: 3088 RVA: 0x0002CA10 File Offset: 0x0002AC10
		public override void Refresh()
		{
			if (this.icon != null)
			{
				this.icon.sprite = base.data.icon;
			}
			this.nameTMP.text = base.data.nameString;
			this.descriptionTMP.text = base.data.description;
			string text = "";
			for (int i = 0; i < base.data.prereqs.Count; i++)
			{
				text += base.data.prereqs[i].nameString;
				if (i + 1 < base.data.prereqs.Count)
				{
					text += " + ";
				}
			}
			this.prereqsTMP.text = text;
		}

		// Token: 0x04000873 RID: 2163
		[SerializeField]
		private Image icon;

		// Token: 0x04000874 RID: 2164
		[SerializeField]
		private TMP_Text nameTMP;

		// Token: 0x04000875 RID: 2165
		[SerializeField]
		private TMP_Text descriptionTMP;

		// Token: 0x04000876 RID: 2166
		[SerializeField]
		private TMP_Text prereqsTMP;
	}
}
