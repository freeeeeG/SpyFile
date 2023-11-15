using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x0200084C RID: 2124
public static class PrivilegeCheckCache
{
	// Token: 0x060028E6 RID: 10470 RVA: 0x000C00BD File Offset: 0x000BE4BD
	public static void Clear()
	{
		PrivilegeCheckCache.m_AllowedUsers.Clear();
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x000C00C9 File Offset: 0x000BE4C9
	public static void AddAllowedUser(GamepadUser gamepadUser, OnlineMultiplayerLocalUserId onlineUser)
	{
		if (PrivilegeCheckCache.m_AllowedUsers.ContainsKey(gamepadUser))
		{
			PrivilegeCheckCache.m_AllowedUsers[gamepadUser] = onlineUser;
		}
		else
		{
			PrivilegeCheckCache.m_AllowedUsers.Add(gamepadUser, onlineUser);
		}
	}

	// Token: 0x060028E8 RID: 10472 RVA: 0x000C00F8 File Offset: 0x000BE4F8
	public static void RemoveAllowedUser(GamepadUser gamepadUser)
	{
		if (PrivilegeCheckCache.m_AllowedUsers.ContainsKey(gamepadUser))
		{
			PrivilegeCheckCache.m_AllowedUsers.Remove(gamepadUser);
		}
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x000C0116 File Offset: 0x000BE516
	public static OnlineMultiplayerLocalUserId GetAllowedUser(GamepadUser gamepadUser)
	{
		if (PrivilegeCheckCache.m_AllowedUsers.ContainsKey(gamepadUser))
		{
			return PrivilegeCheckCache.m_AllowedUsers[gamepadUser];
		}
		return null;
	}

	// Token: 0x0400206A RID: 8298
	private static Dictionary<GamepadUser, OnlineMultiplayerLocalUserId> m_AllowedUsers = new Dictionary<GamepadUser, OnlineMultiplayerLocalUserId>(4);
}
