using System;

namespace Team17.Online
{
	// Token: 0x02000930 RID: 2352
	public interface IOnlineAvatarImageCoordinator
	{
		// Token: 0x06002E68 RID: 11880
		bool RequestAvatarImage(GamepadUser localUser, AvatarImageRequestCompletionCallback completionCallback, out ulong uniqueRequestId);

		// Token: 0x06002E69 RID: 11881
		bool RequestAvatarImage(GamepadUser primaryLocalUser, OnlineUserPlatformId remoteUser, AvatarImageRequestCompletionCallback completionCallback, out ulong uniqueRequestId);
	}
}
