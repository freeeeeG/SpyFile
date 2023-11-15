using System;
using System.ComponentModel;

namespace Team17.Online
{
	// Token: 0x0200095B RID: 2395
	public class OnlineMultiplayerSessionJoinRemoteUserData
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06002EE5 RID: 12005 RVA: 0x000DBBFE File Offset: 0x000D9FFE
		// (set) Token: 0x06002EE6 RID: 12006 RVA: 0x000DBC06 File Offset: 0x000DA006
		[DefaultValue(null)]
		public byte[] GameData { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06002EE7 RID: 12007 RVA: 0x000DBC0F File Offset: 0x000DA00F
		// (set) Token: 0x06002EE8 RID: 12008 RVA: 0x000DBC17 File Offset: 0x000DA017
		[DefaultValue(0L)]
		public uint GameDataSize { get; set; }
	}
}
