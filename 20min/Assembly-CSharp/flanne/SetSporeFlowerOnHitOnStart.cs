using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000128 RID: 296
	public class SetSporeFlowerOnHitOnStart : MonoBehaviour
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x0002208F File Offset: 0x0002028F
		private void Start()
		{
			PlayerController.Instance.GetComponentInChildren<SporeFlower>().procsOnHit = true;
		}
	}
}
