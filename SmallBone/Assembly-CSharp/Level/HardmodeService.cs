using System;
using Characters.Gear.Upgrades;
using UnityEngine;

namespace Level
{
	// Token: 0x020004EE RID: 1262
	public sealed class HardmodeService : MonoBehaviour
	{
		// Token: 0x060018C8 RID: 6344 RVA: 0x0004DAFF File Offset: 0x0004BCFF
		private void Awake()
		{
			base.transform.parent = null;
		}

		// Token: 0x0400159D RID: 5533
		[SerializeField]
		private UpgradeShop _upgradeShop;
	}
}
