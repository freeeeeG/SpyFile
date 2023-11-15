using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200009E RID: 158
	public class DisableOnInvisible : MonoBehaviour
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x000195FC File Offset: 0x000177FC
		private void OnBecameInvisible()
		{
			base.gameObject.SetActive(false);
		}
	}
}
