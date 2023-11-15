using System;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x02000931 RID: 2353
	public interface IOnlineFriendsCoordinator
	{
		// Token: 0x06002E6A RID: 11882
		List<OnlineFriend> DEBUG_GetFriends(GamepadUser localUser);

		// Token: 0x06002E6B RID: 11883
		List<OnlineFriend> GetFriends(GamepadUser localUser);

		// Token: 0x06002E6C RID: 11884
		int GetProfileImage(OnlineFriend onlineFriend, IntPtr unmanagedWorkBuffer, int unmanagedWorkBufferSize);

		// Token: 0x06002E6D RID: 11885
		bool Join(GamepadUser localUser, OnlineFriend friendToJoin);
	}
}
