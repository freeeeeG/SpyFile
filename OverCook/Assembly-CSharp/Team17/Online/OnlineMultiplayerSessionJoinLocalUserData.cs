using System;
using System.ComponentModel;

namespace Team17.Online
{
	// Token: 0x0200095A RID: 2394
	public class OnlineMultiplayerSessionJoinLocalUserData
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06002EDE RID: 11998 RVA: 0x000DBBC3 File Offset: 0x000D9FC3
		// (set) Token: 0x06002EDF RID: 11999 RVA: 0x000DBBCB File Offset: 0x000D9FCB
		[DefaultValue(null)]
		public OnlineMultiplayerLocalUserId Id { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x000DBBD4 File Offset: 0x000D9FD4
		// (set) Token: 0x06002EE1 RID: 12001 RVA: 0x000DBBDC File Offset: 0x000D9FDC
		[DefaultValue(null)]
		public byte[] GameData { get; set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x000DBBE5 File Offset: 0x000D9FE5
		// (set) Token: 0x06002EE3 RID: 12003 RVA: 0x000DBBED File Offset: 0x000D9FED
		[DefaultValue(0L)]
		public uint GameDataSize { get; set; }
	}
}
