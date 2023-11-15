using System;
using Team17.Online;

// Token: 0x02000899 RID: 2201
public interface InviteHandler
{
	// Token: 0x06002AED RID: 10989
	void Start();

	// Token: 0x06002AEE RID: 10990
	void Stop();

	// Token: 0x06002AEF RID: 10991
	void Update();

	// Token: 0x06002AF0 RID: 10992
	void HandleAcceptedInvite(AcceptInviteData invite);

	// Token: 0x06002AF1 RID: 10993
	void HandlePlayTogetherHost(OnlineMultiplayerSessionPlayTogetherHosting host);

	// Token: 0x06002AF2 RID: 10994
	bool IsBusy();

	// Token: 0x06002AF3 RID: 10995
	bool IsAwaitingUserInput();
}
