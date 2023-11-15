using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000ED RID: 237
	public class StartHalfHP : MonoBehaviour
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x0001E984 File Offset: 0x0001CB84
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			componentInParent.playerHealth.hp -= componentInParent.playerHealth.maxHP / 2;
		}
	}
}
