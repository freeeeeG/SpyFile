using System;

namespace Team17.Online
{
	// Token: 0x02000950 RID: 2384
	public interface IOnlineMultiplayerSessionUserId
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06002EC5 RID: 11973
		string DisplayName { get; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06002EC6 RID: 11974
		bool IsHost { get; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06002EC7 RID: 11975
		bool IsLocal { get; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06002EC8 RID: 11976
		byte UniqueId { get; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06002EC9 RID: 11977
		// (set) Token: 0x06002ECA RID: 11978
		bool IsLocallyMuted { get; set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06002ECB RID: 11979
		bool IsSpeaking { get; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06002ECC RID: 11980
		OnlineUserPlatformId PlatformUserId { get; }
	}
}
