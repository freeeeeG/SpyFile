using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000FF RID: 255
	[CreateAssetMenu(fileName = "PowerupTreeUIData", menuName = "PowerupTreeUIData")]
	public class PowerupTreeUIData : ScriptableObject
	{
		// Token: 0x04000524 RID: 1316
		public Powerup startingPowerup;

		// Token: 0x04000525 RID: 1317
		public Powerup leftPowerup;

		// Token: 0x04000526 RID: 1318
		public Powerup rightPowerup;

		// Token: 0x04000527 RID: 1319
		public Powerup finalPowerup;
	}
}
