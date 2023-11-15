using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000889 RID: 2185
internal class UserSystemDebugDisplay : DebugDisplay
{
	// Token: 0x06002A67 RID: 10855 RVA: 0x000C68D1 File Offset: 0x000C4CD1
	public override void OnSetUp()
	{
		this.m_OnlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x000C68DE File Offset: 0x000C4CDE
	public override void OnUpdate()
	{
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x000C68E0 File Offset: 0x000C4CE0
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		FastList<User> users = ServerUserSystem.m_Users;
		int count = users.Count;
		if (count > 0)
		{
			if (ServerUserSystem.GetEngagementPrivilegeCheckStatus().GetProgress() == eConnectionModeSwitchProgress.InProgress)
			{
				base.DrawText(ref rect, style, ServerUserSystem.GetEngagementPrivilegeCheckStatus().GetLocalisedProgressDescription());
			}
			base.DrawText(ref rect, style, "Server users");
			for (int i = 0; i < count; i++)
			{
				User user = users._items[i];
				base.DrawText(ref rect, style, string.Concat(new string[]
				{
					"nm:",
					user.DisplayName,
					" mchn:",
					user.Machine.ToString(),
					" ents:",
					user.EntityID.ToString(),
					",",
					user.Entity2ID.ToString(),
					" slt:",
					user.Engagement.ToString(),
					" ste:",
					user.GameState.ToString(),
					" tm:",
					user.Team.ToString(),
					" party: ",
					user.PartyPersist.ToString(),
					" col:",
					user.Colour.ToString()
				}));
			}
		}
		users = ClientUserSystem.m_Users;
		count = users.Count;
		if (count > 0)
		{
			base.DrawText(ref rect, style, "Client users");
			for (int j = 0; j < count; j++)
			{
				User user2 = users._items[j];
				base.DrawText(ref rect, style, string.Concat(new string[]
				{
					"nm:",
					user2.DisplayName,
					" mchn:",
					user2.Machine.ToString(),
					" ents:",
					user2.EntityID.ToString(),
					",",
					user2.Entity2ID.ToString(),
					" slt:",
					user2.Engagement.ToString(),
					" ste:",
					user2.GameState.ToString(),
					" tm:",
					user2.Team.ToString(),
					" party: ",
					user2.PartyPersist.ToString(),
					" col:",
					user2.Colour.ToString()
				}));
			}
		}
	}

	// Token: 0x04002172 RID: 8562
	private IOnlinePlatformManager m_OnlinePlatformManager;
}
