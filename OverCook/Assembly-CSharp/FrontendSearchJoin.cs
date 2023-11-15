using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AC2 RID: 2754
public class FrontendSearchJoin : MonoBehaviour
{
	// Token: 0x0600375C RID: 14172 RVA: 0x00104FC1 File Offset: 0x001033C1
	private void Start()
	{
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x00104FC3 File Offset: 0x001033C3
	public void SetSessionName(string sessionName)
	{
		if (this.m_name != null)
		{
			this.m_name.text = sessionName;
		}
	}

	// Token: 0x0600375E RID: 14174 RVA: 0x00104FE2 File Offset: 0x001033E2
	public void SetSelectedSession(OnlineMultiplayerSessionEnumeratedRoom session)
	{
		this.m_selectedSession = session;
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x00104FEB File Offset: 0x001033EB
	public void SetLocalUser(GamepadUser localUser)
	{
		this.m_localUser = localUser;
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x00104FF4 File Offset: 0x001033F4
	public void Join()
	{
		JoinEnumeratedRoomOptions joinEnumeratedRoomOptions = new JoinEnumeratedRoomOptions();
		joinEnumeratedRoomOptions.Room = this.m_selectedSession;
		joinEnumeratedRoomOptions.User = this.m_localUser;
		string progressText = "Missing User";
		User user = null;
		FastList<User> users = ClientUserSystem.m_Users;
		for (int i = 0; i < users.Count; i++)
		{
			User user2 = users._items[i];
			if (user2.Engagement == EngagementSlot.One)
			{
				if (user2 != null && user2.GamepadUser != null)
				{
					user = user2;
				}
				break;
			}
		}
		if (user != null)
		{
			progressText = Localization.Get(this.c_InProgressText, new LocToken[]
			{
				new LocToken("[NAME]", user.DisplayName)
			});
		}
		if (T17FrontendFlow.Instance != null && T17FrontendFlow.Instance.m_PlayerLobby != null)
		{
			T17FrontendFlow.Instance.m_PlayerLobby.JoinAdhocGame(joinEnumeratedRoomOptions, progressText);
		}
		if (this.onComplete != null)
		{
			this.onComplete();
		}
	}

	// Token: 0x04002C73 RID: 11379
	[SerializeField]
	private T17Text m_name;

	// Token: 0x04002C74 RID: 11380
	private OnlineMultiplayerSessionEnumeratedRoom m_selectedSession;

	// Token: 0x04002C75 RID: 11381
	private GamepadUser m_localUser;

	// Token: 0x04002C76 RID: 11382
	public FrontendSearchJoin.onCompleteDelegate onComplete;

	// Token: 0x04002C77 RID: 11383
	private readonly string c_TitleText = "Text.PleaseWait";

	// Token: 0x04002C78 RID: 11384
	private readonly string c_InProgressText = "Online.ConnectionMode.JoinSession.InProgress";

	// Token: 0x04002C79 RID: 11385
	private readonly string c_SuccessText = "Online.ConnectionMode.JoinSession.Result.eSuccess";

	// Token: 0x04002C7A RID: 11386
	private readonly string c_FailedText = "Online.ConnectionMode.JoinSession.Result.eGenericFailure";

	// Token: 0x02000AC3 RID: 2755
	// (Invoke) Token: 0x06003762 RID: 14178
	public delegate void onCompleteDelegate();
}
