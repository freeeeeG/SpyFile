using System;

// Token: 0x02000CA5 RID: 3237
public static class WorldGenLogger
{
	// Token: 0x06006711 RID: 26385 RVA: 0x00267D62 File Offset: 0x00265F62
	public static void LogException(string message, string stack)
	{
		Debug.LogError(message + "\n" + stack);
	}
}
