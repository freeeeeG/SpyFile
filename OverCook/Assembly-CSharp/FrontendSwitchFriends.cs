using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AC5 RID: 2757
public class FrontendSwitchFriends : FrontendMenuBehaviour
{
	// Token: 0x06003767 RID: 14183 RVA: 0x0010511C File Offset: 0x0010351C
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_friendsCoordinator = onlinePlatformManager.OnlineFriendsCoordinator();
		this.m_hideInvoker = hideInvoker;
		this.Open(currentGamer);
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		return true;
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x00105178 File Offset: 0x00103578
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		this.Clear();
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		this.m_scrollView.Hide(true, false);
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x06003769 RID: 14185 RVA: 0x001051B1 File Offset: 0x001035B1
	public void OnCancel()
	{
		this.Close();
		if (T17FrontendFlow.Instance != null && T17FrontendFlow.Instance.m_PlayerLobby != null)
		{
			T17FrontendFlow.Instance.m_PlayerLobby.ShowPlayWithFriendsMenu();
		}
	}

	// Token: 0x0600376A RID: 14186 RVA: 0x001051ED File Offset: 0x001035ED
	public override void Close()
	{
		base.Close();
	}

	// Token: 0x0600376B RID: 14187 RVA: 0x001051F5 File Offset: 0x001035F5
	public void Clear()
	{
		this.m_scrollView.ClearContents();
		this.m_entrys.Clear();
		if (this.m_noEntriesMessage != null)
		{
			this.m_noEntriesMessage.SetActive(false);
		}
	}

	// Token: 0x0600376C RID: 14188 RVA: 0x0010522A File Offset: 0x0010362A
	public void Open(GamepadUser currentGamer)
	{
		this.Enumerate();
		this.UpdateResultsList();
	}

	// Token: 0x0600376D RID: 14189 RVA: 0x00105238 File Offset: 0x00103638
	public void Refresh()
	{
		this.Clear();
		this.Enumerate();
		this.UpdateResultsList();
	}

	// Token: 0x0600376E RID: 14190 RVA: 0x0010524C File Offset: 0x0010364C
	private void Enumerate()
	{
		List<OnlineFriend> list = this.DEBUG_GetFriends(base.CurrentGamepadUser);
		list.Sort(delegate(OnlineFriend x, OnlineFriend y)
		{
			if (x.m_status == y.m_status)
			{
				return x.m_displayName.CompareTo(y.m_displayName);
			}
			if ((x.m_status == OnlineFriend.FriendStatus.eOnlineInSameApplication && y.m_status == OnlineFriend.FriendStatus.eOnline) || (x.m_status == OnlineFriend.FriendStatus.eOnline && y.m_status == OnlineFriend.FriendStatus.eOnlineInSameApplication))
			{
				return x.m_displayName.CompareTo(y.m_displayName);
			}
			return x.m_status.CompareTo(y.m_status) * -1;
		});
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_entryPrefab, this.m_containerObject.transform);
			this.m_entrys.Add(gameObject);
			this.m_scrollView.AddNewObject(gameObject);
			if (gameObject != null && gameObject.transform != null)
			{
				FrontendFriendsJoin component = gameObject.GetComponent<FrontendFriendsJoin>();
				if (component != null)
				{
					component.SetLocalUser(base.CurrentGamepadUser);
					component.SetSelectedFriend(list[i]);
					component.onFriendSelected = new FrontendFriendsJoin.OnSelectedDelegate(this.OnFriendSelected);
					component.SetName(list[i].m_displayName);
					T17Button t17Button = gameObject.RequestComponent<T17Button>();
					Navigation navigation = t17Button.navigation;
					navigation.selectOnLeft = null;
					navigation.selectOnRight = null;
					if (i == 0)
					{
						navigation.selectOnUp = null;
					}
					t17Button.navigation = navigation;
					component.SetStatus(list[i].m_status);
					if (list[i].m_status == OnlineFriend.FriendStatus.eOnlineInSameApplicationAndJoinable)
					{
						if (t17Button != null)
						{
							t17Button.SubmitAudioTag = GameOneShotAudioTag.UISelect;
						}
					}
					else if (t17Button != null)
					{
						t17Button.SubmitAudioTag = GameOneShotAudioTag.UIBack;
					}
				}
			}
		}
	}

	// Token: 0x0600376F RID: 14191 RVA: 0x001053CC File Offset: 0x001037CC
	private void UpdateResultsList()
	{
		if (this.m_entrys.Count > 0)
		{
			this.m_scrollView.Show(base.CurrentGamepadUser, this.m_Parent, this.m_ObjectThatInvokedShow, this.m_hideInvoker);
		}
		else
		{
			this.m_scrollView.Hide(true, false);
			if (this.m_CachedEventSystem != null && this.m_BorderSelectables.selectOnRight != null)
			{
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnRight.gameObject);
			}
		}
		if (this.m_noEntriesMessage != null)
		{
			this.m_noEntriesMessage.SetActive(this.m_entrys.Count == 0);
		}
	}

	// Token: 0x06003770 RID: 14192 RVA: 0x0010548D File Offset: 0x0010388D
	private void OnFriendSelected(OnlineFriend friend)
	{
		this.Close();
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x00105498 File Offset: 0x00103898
	public List<OnlineFriend> DEBUG_GetFriends(GamepadUser localUser)
	{
		if (null != localUser)
		{
			try
			{
				int num = 100;
				List<OnlineFriend> list = new List<OnlineFriend>(num);
				for (int i = 0; i < num; i++)
				{
					OnlineFriend onlineFriend = new OnlineFriend();
					onlineFriend.m_displayName = "test" + UnityEngine.Random.Range(5f, 32f).ToString();
					if (i <= 3)
					{
						onlineFriend.m_status = OnlineFriend.FriendStatus.eOnlineInSameApplicationAndJoinable;
					}
					else if (i <= 6)
					{
						onlineFriend.m_status = OnlineFriend.FriendStatus.eOnlineInSameApplication;
					}
					else if (i <= 9)
					{
						onlineFriend.m_status = OnlineFriend.FriendStatus.eOnline;
					}
					else
					{
						onlineFriend.m_status = OnlineFriend.FriendStatus.eOffline;
					}
					list.Add(onlineFriend);
				}
				return list;
			}
			catch (Exception)
			{
			}
		}
		return null;
	}

	// Token: 0x04002C7B RID: 11387
	private IOnlineFriendsCoordinator m_friendsCoordinator;

	// Token: 0x04002C7C RID: 11388
	[SerializeField]
	private GameObject m_containerObject;

	// Token: 0x04002C7D RID: 11389
	[SerializeField]
	private GameObject m_entryPrefab;

	// Token: 0x04002C7E RID: 11390
	[SerializeField]
	private GameObject m_noEntriesMessage;

	// Token: 0x04002C7F RID: 11391
	[SerializeField]
	private T17ScrollView m_scrollView;

	// Token: 0x04002C80 RID: 11392
	private List<GameObject> m_entrys = new List<GameObject>();

	// Token: 0x04002C81 RID: 11393
	private bool m_hideInvoker;
}
