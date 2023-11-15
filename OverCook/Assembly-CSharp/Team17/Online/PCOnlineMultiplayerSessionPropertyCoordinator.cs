using System;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x02000971 RID: 2417
	public abstract class PCOnlineMultiplayerSessionPropertyCoordinator
	{
		// Token: 0x06002F35 RID: 12085 RVA: 0x000DCA65 File Offset: 0x000DAE65
		protected bool Open(List<OnlineMultiplayerSessionProperty> properties)
		{
			if (!this.m_isBaseInitialized && properties != null)
			{
				this.m_isBaseInitialized = true;
			}
			return this.m_isBaseInitialized;
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000DCA85 File Offset: 0x000DAE85
		protected void Close()
		{
			if (this.m_isBaseInitialized)
			{
				this.m_isBaseInitialized = false;
			}
		}

		// Token: 0x040025BD RID: 9661
		private bool m_isBaseInitialized;
	}
}
