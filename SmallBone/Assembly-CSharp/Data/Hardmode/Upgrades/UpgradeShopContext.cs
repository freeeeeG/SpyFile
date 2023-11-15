using System;
using Characters.Gear.Upgrades;

namespace Data.Hardmode.Upgrades
{
	// Token: 0x020002B5 RID: 693
	public sealed class UpgradeShopContext
	{
		// Token: 0x06000E24 RID: 3620 RVA: 0x0002CC2E File Offset: 0x0002AE2E
		public UpgradeShopContext()
		{
			this._settings = UpgradeShopSettings.instance;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0002CC41 File Offset: 0x0002AE41
		public int GetRemoveCost(UpgradeObject.Type type)
		{
			return this._settings.GetRemoveCost(type);
		}

		// Token: 0x04000BC5 RID: 3013
		private UpgradeShopSettings _settings;
	}
}
