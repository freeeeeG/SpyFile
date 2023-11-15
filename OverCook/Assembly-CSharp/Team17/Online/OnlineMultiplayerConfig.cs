using System;

namespace Team17.Online
{
	// Token: 0x02000954 RID: 2388
	public static class OnlineMultiplayerConfig
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06002ECF RID: 11983 RVA: 0x000DBAF1 File Offset: 0x000D9EF1
		public static uint MaxPlayers
		{
			get
			{
				return 4U;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x000DBAF4 File Offset: 0x000D9EF4
		public static uint MaxTransportMessageSize
		{
			get
			{
				return 1024U;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06002ED1 RID: 11985 RVA: 0x000DBAFB File Offset: 0x000D9EFB
		public static uint CodeVersion
		{
			get
			{
				return 17U;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x000DBAFF File Offset: 0x000D9EFF
		public static uint MaxSocketIterationsPerUpdate
		{
			get
			{
				return 100U;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x000DBB03 File Offset: 0x000D9F03
		public static ushort MaxBrowsedSessionsToEnumerate
		{
			get
			{
				return 10;
			}
		}
	}
}
