using System;
using UnityEngine;

namespace LayerLab.FantasyRPG
{
	// Token: 0x020000B8 RID: 184
	public class PanelFantasyRPG : MonoBehaviour
	{
		// Token: 0x06000286 RID: 646 RVA: 0x0000A33C File Offset: 0x0000853C
		public void OnEnable()
		{
			for (int i = 0; i < this.otherPanels.Length; i++)
			{
				this.otherPanels[i].SetActive(true);
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000A36C File Offset: 0x0000856C
		public void OnDisable()
		{
			for (int i = 0; i < this.otherPanels.Length; i++)
			{
				this.otherPanels[i].SetActive(false);
			}
		}

		// Token: 0x0400021A RID: 538
		[SerializeField]
		private GameObject[] otherPanels;
	}
}
