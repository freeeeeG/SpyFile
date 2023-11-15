using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200010F RID: 271
public static class DebugLog
{
	// Token: 0x060006B8 RID: 1720 RVA: 0x00012657 File Offset: 0x00010857
	[Conditional("EnableDebug")]
	public static void Logger(string msg)
	{
		Debug.Log("<color=yellow>" + msg + "</color>");
	}
}
