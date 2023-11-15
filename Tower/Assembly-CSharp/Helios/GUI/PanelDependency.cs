using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000DF RID: 223
	public class PanelDependency : MonoBehaviour
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0000E814 File Offset: 0x0000CA14
		public void OnEnable()
		{
			foreach (GameObject gameObject in this.otherPanels)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000E84C File Offset: 0x0000CA4C
		public void OnDisable()
		{
			foreach (GameObject gameObject in this.otherPanels)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04000314 RID: 788
		[SerializeField]
		private GameObject[] otherPanels;
	}
}
