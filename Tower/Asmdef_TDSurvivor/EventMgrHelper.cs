using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020000EB RID: 235
public static class EventMgrHelper
{
	// Token: 0x060005E4 RID: 1508 RVA: 0x000171DC File Offset: 0x000153DC
	public static T SafeCast<T>(Enum key, IEventBase obj) where T : IEventBase
	{
		T result = default(T);
		try
		{
			result = (T)((object)obj);
		}
		catch (InvalidCastException ex)
		{
			Type typeFromHandle = typeof(T);
			EventMgrHelper.LogError(string.Format("型別錯誤: key:<color=red>{0}.{1}</color>, {2}, {3}", new object[]
			{
				key.GetType(),
				key,
				typeFromHandle.Name,
				ex.Message
			}));
		}
		catch (Exception e)
		{
			EventMgrHelper.LogError(e);
		}
		return result;
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x00017260 File Offset: 0x00015460
	[Conditional("EventMgrDebug")]
	public static void Log(string message)
	{
		Debug.Log("<color=#00FF00>[EventMgr]</color> " + message);
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x00017272 File Offset: 0x00015472
	public static void LogError(string message)
	{
		Debug.LogError("<color=#FF0000>[EventMgr]</color> " + message);
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x00017284 File Offset: 0x00015484
	public static void LogError(Exception e)
	{
		Debug.LogError("<color=#FF0000>[EventMgr]</color> " + e.Message + "\n" + e.StackTrace);
	}
}
