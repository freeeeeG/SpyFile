using System;
using UnityEngine;

// Token: 0x02000903 RID: 2307
public class ServerTime
{
	// Token: 0x06002D14 RID: 11540 RVA: 0x000D4E04 File Offset: 0x000D3204
	public static void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = realtimeSinceStartup - ServerTime.m_fLastTime;
		ServerTime.m_fServerTime += num;
		ServerTime.m_fLastTime = ServerTime.m_fServerTime;
		if (ServerTime.m_fServerTime > ServerTime.m_fNextSyncTime)
		{
			ServerTime.m_fNextSyncTime = 3f + Time.realtimeSinceStartup;
			ServerMessenger.TimeSync(ServerTime.m_fServerTime);
		}
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x000D4E5F File Offset: 0x000D325F
	public static void StartTime()
	{
		ServerTime.m_fServerTime = 0f;
		ServerTime.m_fLastTime = 0f;
	}

	// Token: 0x04002432 RID: 9266
	public const float kTimeSyncFrequency = 3f;

	// Token: 0x04002433 RID: 9267
	private static float m_fNextSyncTime;

	// Token: 0x04002434 RID: 9268
	private static float m_fServerTime;

	// Token: 0x04002435 RID: 9269
	private static float m_fLastTime;
}
