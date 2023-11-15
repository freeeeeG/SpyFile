using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x0200084D RID: 2125
public static class LocalDroppedInUserCache
{
	// Token: 0x060028EB RID: 10475 RVA: 0x000C0142 File Offset: 0x000BE542
	public static void Clear()
	{
		LocalDroppedInUserCache.m_DroppedInUsers.Clear();
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x000C014E File Offset: 0x000BE54E
	public static void AddDroppedInUser(OnlineMultiplayerLocalUserId user)
	{
		if (!LocalDroppedInUserCache.m_DroppedInUsers.Contains(user))
		{
			LocalDroppedInUserCache.m_DroppedInUsers.Add(user);
		}
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x000C016B File Offset: 0x000BE56B
	public static void RemoveDroppedInUser(OnlineMultiplayerLocalUserId user)
	{
		if (LocalDroppedInUserCache.m_DroppedInUsers.Contains(user))
		{
			LocalDroppedInUserCache.m_DroppedInUsers.Remove(user);
		}
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x000C0189 File Offset: 0x000BE589
	public static bool HasBeenDroppedIn(OnlineMultiplayerLocalUserId user)
	{
		return LocalDroppedInUserCache.m_DroppedInUsers.Contains(user);
	}

	// Token: 0x0400206B RID: 8299
	private static List<OnlineMultiplayerLocalUserId> m_DroppedInUsers = new List<OnlineMultiplayerLocalUserId>();
}
