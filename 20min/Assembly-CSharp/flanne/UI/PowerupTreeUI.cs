using System;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x0200021E RID: 542
	public class PowerupTreeUI : DataUIBinding<PowerupTreeUIData>
	{
		// Token: 0x06000C0A RID: 3082 RVA: 0x0002C8F8 File Offset: 0x0002AAF8
		public override void Refresh()
		{
			this.startingPowerup.data = base.data.startingPowerup;
			this.leftPowerup.data = base.data.leftPowerup;
			this.rightPowerup.data = base.data.rightPowerup;
			this.finalPowerup.data = base.data.finalPowerup;
		}

		// Token: 0x04000869 RID: 2153
		[SerializeField]
		private PowerupIcon startingPowerup;

		// Token: 0x0400086A RID: 2154
		[SerializeField]
		private PowerupIcon leftPowerup;

		// Token: 0x0400086B RID: 2155
		[SerializeField]
		private PowerupIcon rightPowerup;

		// Token: 0x0400086C RID: 2156
		[SerializeField]
		private PowerupIcon finalPowerup;
	}
}
