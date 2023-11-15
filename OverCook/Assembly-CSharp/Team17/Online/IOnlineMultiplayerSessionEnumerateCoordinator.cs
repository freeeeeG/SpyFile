using System;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x0200094D RID: 2381
	public interface IOnlineMultiplayerSessionEnumerateCoordinator
	{
		// Token: 0x06002EC0 RID: 11968
		bool IsIdle();

		// Token: 0x06002EC1 RID: 11969
		bool Start(OnlineMultiplayerLocalUserId localUserId, List<OnlineMultiplayerSessionPropertySearchValue> filterParameters, ushort maxResults, OnlineMultiplayerSessionEnumerateCallback enumerateCallback);

		// Token: 0x06002EC2 RID: 11970
		void Cancel();
	}
}
