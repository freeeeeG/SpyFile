using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000A0 RID: 160
	[AddComponentMenu("Modular Options/Misc/Disable On Start")]
	public class DisableOnStart : MonoBehaviour
	{
		// Token: 0x0600022D RID: 557 RVA: 0x00008FBD File Offset: 0x000071BD
		private void Start()
		{
			base.gameObject.SetActive(false);
		}
	}
}
