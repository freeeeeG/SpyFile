using System;

// Token: 0x02000891 RID: 2193
public static class ServerGameSetup
{
	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06002AAF RID: 10927 RVA: 0x000C7FB8 File Offset: 0x000C63B8
	// (set) Token: 0x06002AAE RID: 10926 RVA: 0x000C7F84 File Offset: 0x000C6384
	public static GameMode Mode
	{
		get
		{
			return ServerGameSetup.m_Mode;
		}
		set
		{
			if (ServerGameSetup.m_Mode != value)
			{
				ServerGameSetup.m_Mode = value;
				if (!ServerMessenger.GameSetup(ServerGameSetup.m_Mode))
				{
				}
			}
		}
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x000C7FBF File Offset: 0x000C63BF
	public static void BecomeClient()
	{
		ServerGameSetup.m_Mode = GameMode.COUNT;
	}

	// Token: 0x040021BC RID: 8636
	private static GameMode m_Mode = GameMode.COUNT;
}
