using System;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x0200098A RID: 2442
	public abstract class SteamOnlineMultiplayerSessionPropertyCoordinator
	{
		// Token: 0x06002FAA RID: 12202 RVA: 0x000DBC58 File Offset: 0x000DA058
		protected bool Open(List<OnlineMultiplayerSessionProperty> properties)
		{
			if (!this.m_isBaseInitialized && properties != null)
			{
				this.m_isBaseInitialized = true;
			}
			return this.m_isBaseInitialized;
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000DBC78 File Offset: 0x000DA078
		protected void Close()
		{
			if (this.m_isBaseInitialized)
			{
				this.m_isBaseInitialized = false;
			}
		}

		// Token: 0x04002640 RID: 9792
		private bool m_isBaseInitialized;
	}
}
