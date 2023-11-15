using System;
using UnityEngine;

namespace LayerLab
{
	// Token: 0x020000B6 RID: 182
	public class PanelSimpleCasual : MonoBehaviour
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000A048 File Offset: 0x00008248
		public void OnEnable()
		{
			for (int i = 0; i < this.otherPanels.Length; i++)
			{
				this.otherPanels[i].SetActive(true);
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A078 File Offset: 0x00008278
		public void OnDisable()
		{
			for (int i = 0; i < this.otherPanels.Length; i++)
			{
				this.otherPanels[i].SetActive(false);
			}
		}

		// Token: 0x04000212 RID: 530
		[SerializeField]
		private GameObject[] otherPanels;
	}
}
