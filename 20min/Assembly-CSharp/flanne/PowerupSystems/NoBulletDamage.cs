using System;
using UnityEngine;

namespace flanne.PowerupSystems
{
	// Token: 0x020001DD RID: 477
	public class NoBulletDamage : MonoBehaviour
	{
		// Token: 0x06000A77 RID: 2679 RVA: 0x00028CA6 File Offset: 0x00026EA6
		private void Start()
		{
			StatsHolder stats = base.transform.GetComponentInParent<PlayerController>().stats;
			stats[StatType.BulletDamage].AddMultiplierReduction(0f);
			Debug.Log(stats[StatType.BulletDamage].Modify(1f));
		}
	}
}
