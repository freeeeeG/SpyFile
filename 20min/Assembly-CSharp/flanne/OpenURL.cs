using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200013B RID: 315
	public class OpenURL : MonoBehaviour
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x00022FF1 File Offset: 0x000211F1
		public void GoToURL()
		{
			Application.OpenURL(this.url);
		}

		// Token: 0x0400061E RID: 1566
		[SerializeField]
		private string url;
	}
}
