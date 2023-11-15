using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200009D RID: 157
	public class DisableAction : MonoBehaviour
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x000195FC File Offset: 0x000177FC
		public void Disable()
		{
			base.gameObject.SetActive(false);
		}
	}
}
