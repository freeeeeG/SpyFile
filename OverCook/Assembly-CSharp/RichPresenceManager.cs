using System;

// Token: 0x02000738 RID: 1848
public class RichPresenceManager : RichPresenceMangerBase
{
	// Token: 0x06002367 RID: 9063 RVA: 0x000AACC3 File Offset: 0x000A90C3
	public static void SetGameMode(GameMode mode)
	{
		if (RichPresenceMangerBase.m_gameMode != mode)
		{
			RichPresenceMangerBase.m_gameMode = mode;
			if (RichPresenceMangerBase.OnGameModeSet != null)
			{
				RichPresenceMangerBase.OnGameModeSet();
			}
		}
	}
}
