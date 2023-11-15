using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000B80 RID: 2944
public class UIFocusBlocker : MonoBehaviour
{
	// Token: 0x06003C1B RID: 15387 RVA: 0x0011FAC8 File Offset: 0x0011DEC8
	private void OnEnable()
	{
		FastList<User> users = ClientUserSystem.m_Users;
		if (T17EventSystemsManager.Instance != null)
		{
			for (int i = 0; i < users.Count; i++)
			{
				User user = users._items[i];
				if (user.Engagement == EngagementSlot.One)
				{
					if (user != null && user.GamepadUser != null)
					{
						T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user.GamepadUser);
						if (eventSystemForGamepadUser != null)
						{
							this.m_Supressor = eventSystemForGamepadUser.Disable(this);
						}
					}
					break;
				}
			}
		}
	}

	// Token: 0x06003C1C RID: 15388 RVA: 0x0011FB56 File Offset: 0x0011DF56
	private void OnDisable()
	{
		if (this.m_Supressor != null)
		{
			this.m_Supressor.Release();
		}
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x0011FB6E File Offset: 0x0011DF6E
	private void OnDestroy()
	{
		if (this.m_Supressor != null)
		{
			this.m_Supressor.Release();
		}
	}

	// Token: 0x040030D5 RID: 12501
	private Suppressor m_Supressor;
}
