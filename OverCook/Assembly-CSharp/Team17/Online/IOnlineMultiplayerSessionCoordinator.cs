using System;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x0200094B RID: 2379
	public interface IOnlineMultiplayerSessionCoordinator
	{
		// Token: 0x06002EA8 RID: 11944
		bool IsIdle();

		// Token: 0x06002EA9 RID: 11945
		void RegisterDisconnectionCallback(OnlineMultiplayerSessionDisconnectionCallback callback);

		// Token: 0x06002EAA RID: 11946
		void UnRegisterDisconnectionCallback(OnlineMultiplayerSessionDisconnectionCallback callback);

		// Token: 0x06002EAB RID: 11947
		void RegisterRemoteUserDisconnectionCallback(OnlineMultiplayerSessionRemoteUserDisconnectionCallback callback);

		// Token: 0x06002EAC RID: 11948
		void UnRegisterRemoteUserDisconnectionCallback(OnlineMultiplayerSessionRemoteUserDisconnectionCallback callback);

		// Token: 0x06002EAD RID: 11949
		void RegisterDataReceivedCallback(OnlineMultiplayerSessionDataReceivedCallback callback);

		// Token: 0x06002EAE RID: 11950
		void UnRegisterDataReceivedCallback(OnlineMultiplayerSessionDataReceivedCallback callback);

		// Token: 0x06002EAF RID: 11951
		bool IsHost();

		// Token: 0x06002EB0 RID: 11952
		bool Create(OnlineMultiplayerLocalUserId localUserId, List<OnlineMultiplayerSessionPropertyValue> sessionProperties, OnlineMultiplayerSessionVisibility visibility, string sessionName, OnlineMultiplayerSessionPlayTogetherHosting playtogetherHosting, OnlineMultiplayerSessionCreateCallback createCallback, OnlineMultiplayerSessionJoinDecisionCallback joinDecisionCallback, OnlineMultiplayerSessionUserJoinedCallback newUserJoinedCallback);

		// Token: 0x06002EB1 RID: 11953
		bool Join(List<OnlineMultiplayerSessionJoinLocalUserData> localUserData, OnlineMultiplayerSessionInvite sessionInvite, OnlineMultiplayerSessionJoinCallback joinCallback);

		// Token: 0x06002EB2 RID: 11954
		bool Join(List<OnlineMultiplayerSessionJoinLocalUserData> localUserData, OnlineMultiplayerSessionEnumeratedRoom enumeratedSession, OnlineMultiplayerSessionJoinCallback joinCallback);

		// Token: 0x06002EB3 RID: 11955
		OnlineMultiplayerNonPrimaryLocalUserChangeResult AddNonPrimaryLocalUser(OnlineMultiplayerLocalUserId localUserId, OnlineMultiplayerSessionAddNonPrimaryLocalUserCallback joinCallback);

		// Token: 0x06002EB4 RID: 11956
		OnlineMultiplayerNonPrimaryLocalUserChangeResult RemoveNonPrimaryLocalUser(OnlineMultiplayerLocalUserId localUserId);

		// Token: 0x06002EB5 RID: 11957
		bool Modify(List<OnlineMultiplayerSessionPropertyValue> sessionProperties, OnlineMultiplayerSessionVisibility visibility);

		// Token: 0x06002EB6 RID: 11958
		bool AutoMatchmake(OnlineMultiplayerSessionJoinLocalUserData localUserData, List<OnlineMultiplayerSessionPropertyValue> hostingSessionProperties, string hostingSessionName, OnlineMultiplayerSessionJoinDecisionCallback hostingJoinDecisionCallback, OnlineMultiplayerSessionUserJoinedCallback hostingNewUserJoinedCallback, List<OnlineMultiplayerSessionPropertySearchValue> autoMatchingFilterParameters, OnlineMultiplayerSessionJoinCallback joinCallback);

		// Token: 0x06002EB7 RID: 11959
		bool SendData(IOnlineMultiplayerSessionUserId recipientUserId, byte[] data, int dataSize, bool sendReliably);

		// Token: 0x06002EB8 RID: 11960
		void ShowSendInviteDialog(string msg);

		// Token: 0x06002EB9 RID: 11961
		void Leave();

		// Token: 0x06002EBA RID: 11962
		IOnlineMultiplayerSessionUserId[] Members();

		// Token: 0x06002EBB RID: 11963
		bool IsMemberAlready(OnlineMultiplayerSessionInvite pendingSessionInvite);
	}
}
