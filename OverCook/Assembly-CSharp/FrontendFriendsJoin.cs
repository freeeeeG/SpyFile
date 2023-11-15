using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AB6 RID: 2742
public class FrontendFriendsJoin : MonoBehaviour
{
	// Token: 0x06003691 RID: 13969 RVA: 0x000FFF18 File Offset: 0x000FE318
	private void Start()
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_friendsCoordinator = onlinePlatformManager.OnlineFriendsCoordinator();
	}

	// Token: 0x06003692 RID: 13970 RVA: 0x000FFF37 File Offset: 0x000FE337
	public void SetName(string name)
	{
		if (this.m_name != null)
		{
			this.m_name.text = name;
		}
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x000FFF58 File Offset: 0x000FE358
	public void SetStatus(OnlineFriend.FriendStatus status)
	{
		if (this.m_StatusText != null)
		{
			switch (status)
			{
			case OnlineFriend.FriendStatus.eOffline:
				this.m_StatusText.SetLocalisedTextCatchAll(FrontendFriendsJoin.s_OfflineText);
				break;
			case OnlineFriend.FriendStatus.eOnline:
			case OnlineFriend.FriendStatus.eOnlineInSameApplication:
				this.m_StatusText.SetLocalisedTextCatchAll(FrontendFriendsJoin.s_OnlineText);
				break;
			case OnlineFriend.FriendStatus.eOnlineInSameApplicationAndJoinable:
				this.m_StatusText.SetLocalisedTextCatchAll(FrontendFriendsJoin.s_JoinableText);
				break;
			}
		}
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x000FFFD5 File Offset: 0x000FE3D5
	public void SetSelectedFriend(OnlineFriend friend)
	{
		this.m_selectedFriend = friend;
	}

	// Token: 0x06003695 RID: 13973 RVA: 0x000FFFDE File Offset: 0x000FE3DE
	public void SetLocalUser(GamepadUser localUser)
	{
		this.m_localUser = localUser;
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x000FFFE8 File Offset: 0x000FE3E8
	public void Join()
	{
		bool flag = this.m_friendsCoordinator.Join(this.m_localUser, this.m_selectedFriend);
		if (flag && this.onFriendSelected != null)
		{
			this.onFriendSelected(this.m_selectedFriend);
		}
	}

	// Token: 0x04002BD7 RID: 11223
	private IOnlineFriendsCoordinator m_friendsCoordinator;

	// Token: 0x04002BD8 RID: 11224
	[SerializeField]
	private T17Text m_name;

	// Token: 0x04002BD9 RID: 11225
	[SerializeField]
	private T17Text m_StatusText;

	// Token: 0x04002BDA RID: 11226
	private OnlineFriend m_selectedFriend;

	// Token: 0x04002BDB RID: 11227
	private GamepadUser m_localUser;

	// Token: 0x04002BDC RID: 11228
	public FrontendFriendsJoin.OnSelectedDelegate onFriendSelected;

	// Token: 0x04002BDD RID: 11229
	private static readonly string s_OnlineText = "LobbyScreen.Friends.Online";

	// Token: 0x04002BDE RID: 11230
	private static readonly string s_OfflineText = "LobbyScreen.Friends.Offline";

	// Token: 0x04002BDF RID: 11231
	private static readonly string s_JoinableText = "LobbyScreen.Friends.Joinable";

	// Token: 0x02000AB7 RID: 2743
	// (Invoke) Token: 0x06003699 RID: 13977
	public delegate void OnSelectedDelegate(OnlineFriend friend);
}
