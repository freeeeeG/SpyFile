using System;

namespace Team17.Online
{
	// Token: 0x0200094F RID: 2383
	public interface IOnlineMultiplayerSessionPropertyCoordinator
	{
		// Token: 0x06002EC3 RID: 11971
		bool IsInitialized();

		// Token: 0x06002EC4 RID: 11972
		IOnlineMultiplayerSessionProperty FindProperty(OnlineMultiplayerSessionPropertyId id);
	}
}
