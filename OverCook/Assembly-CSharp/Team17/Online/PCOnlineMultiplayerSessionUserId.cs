using System;
using System.IO;

namespace Team17.Online
{
	// Token: 0x02000972 RID: 2418
	public abstract class PCOnlineMultiplayerSessionUserId
	{
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06002F38 RID: 12088 RVA: 0x000DCAA4 File Offset: 0x000DAEA4
		public OnlineUserPlatformId PlatformUserId
		{
			get
			{
				return new OnlineUserPlatformId();
			}
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000DCAB8 File Offset: 0x000DAEB8
		protected void Write(BinaryWriter writer)
		{
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000DCABA File Offset: 0x000DAEBA
		protected void Read(BinaryReader reader)
		{
		}
	}
}
