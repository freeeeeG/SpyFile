using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000129 RID: 297
	public class SetSporeFlowerScaleWithBulletDamageOnStart : MonoBehaviour
	{
		// Token: 0x0600080B RID: 2059 RVA: 0x000220A1 File Offset: 0x000202A1
		private void Start()
		{
			PlayerController.Instance.GetComponentInChildren<SporeFlower>().multiplyByBulletDamage = true;
		}
	}
}
