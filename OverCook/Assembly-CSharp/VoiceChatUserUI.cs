using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000B81 RID: 2945
internal class VoiceChatUserUI : MonoBehaviour
{
	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06003C1F RID: 15391 RVA: 0x0011FB8E File Offset: 0x0011DF8E
	// (set) Token: 0x06003C20 RID: 15392 RVA: 0x0011FB96 File Offset: 0x0011DF96
	public EngagementSlot PlayerSlot
	{
		get
		{
			return this.m_PlayerSlot;
		}
		set
		{
			this.m_PlayerSlot = value;
			this.UpdateTalkingUser();
		}
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x0011FBA5 File Offset: 0x0011DFA5
	private void Awake()
	{
		this.DisableAllHUD();
		this.UpdateTalkingUser();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.UpdateTalkingUser));
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x0011FBD3 File Offset: 0x0011DFD3
	private void OnDestroy()
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.UpdateTalkingUser));
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x0011FBF8 File Offset: 0x0011DFF8
	private void Update()
	{
		if (this.m_TalkingUser != null && this.m_TalkingUser.SessionId != null)
		{
			if (!this.m_TalkingUser.SessionId.IsLocal && this.m_TalkingUser.SessionId.IsLocallyMuted)
			{
				if (this.m_GamerName != null)
				{
					this.m_GamerName.gameObject.SetActive(true);
					this.m_GamerName.SetNonLocalizedText(this.m_TalkingUser.DisplayName);
				}
				this.m_TalkingIcon.SetActive(false);
				this.m_MuteIcon.SetActive(true);
			}
			else if (this.m_TalkingUser.SessionId.IsSpeaking)
			{
				if (this.m_GamerName != null)
				{
					this.m_GamerName.gameObject.SetActive(true);
					this.m_GamerName.SetNonLocalizedText(this.m_TalkingUser.DisplayName);
				}
				this.m_TalkingIcon.SetActive(true);
				this.m_MuteIcon.SetActive(false);
			}
			else
			{
				this.DisableAllHUD();
			}
		}
		else
		{
			this.DisableAllHUD();
		}
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x0011FD1C File Offset: 0x0011E11C
	private void UpdateTalkingUser()
	{
		FastList<User> users = ClientUserSystem.m_Users;
		if (this.m_PlayerSlot < (EngagementSlot)users.Count)
		{
			this.m_TalkingUser = users._items[(int)this.m_PlayerSlot];
		}
		else
		{
			this.m_TalkingUser = null;
		}
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x0011FD5F File Offset: 0x0011E15F
	private void DisableAllHUD()
	{
		if (this.m_GamerName != null)
		{
			this.m_GamerName.gameObject.SetActive(false);
		}
		this.m_TalkingIcon.SetActive(false);
		this.m_MuteIcon.SetActive(false);
	}

	// Token: 0x040030D6 RID: 12502
	[SerializeField]
	private EngagementSlot m_PlayerSlot;

	// Token: 0x040030D7 RID: 12503
	[SerializeField]
	private T17Text m_GamerName;

	// Token: 0x040030D8 RID: 12504
	[SerializeField]
	private GameObject m_TalkingIcon;

	// Token: 0x040030D9 RID: 12505
	[SerializeField]
	private GameObject m_MuteIcon;

	// Token: 0x040030DA RID: 12506
	private User m_TalkingUser;
}
