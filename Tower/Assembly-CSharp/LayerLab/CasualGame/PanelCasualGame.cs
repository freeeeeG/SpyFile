using System;
using UnityEngine;

namespace LayerLab.CasualGame
{
	// Token: 0x020000B9 RID: 185
	public class PanelCasualGame : MonoBehaviour
	{
		// Token: 0x06000289 RID: 649 RVA: 0x0000A3A4 File Offset: 0x000085A4
		public void OnEnable()
		{
			for (int i = 0; i < this.otherPanels.Length; i++)
			{
				this.otherPanels[i].SetActive(true);
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A3D4 File Offset: 0x000085D4
		public void OnDisable()
		{
			for (int i = 0; i < this.otherPanels.Length; i++)
			{
				this.otherPanels[i].SetActive(false);
			}
		}

		// Token: 0x0400021B RID: 539
		[SerializeField]
		private GameObject[] otherPanels;
	}
}
