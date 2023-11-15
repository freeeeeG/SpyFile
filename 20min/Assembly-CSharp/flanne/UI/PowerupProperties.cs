using System;

namespace flanne.UI
{
	// Token: 0x0200023E RID: 574
	public class PowerupProperties : IUIProperties
	{
		// Token: 0x06000C9C RID: 3228 RVA: 0x0002E09F File Offset: 0x0002C29F
		public PowerupProperties(Powerup p)
		{
			this.powerup = p;
		}

		// Token: 0x040008D2 RID: 2258
		public Powerup powerup;
	}
}
