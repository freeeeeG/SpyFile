using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200001B RID: 27
	public class MB2_Log
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00004078 File Offset: 0x00002478
		public static void Log(MB2_LogLevel l, string msg, MB2_LogLevel currentThreshold)
		{
			if (l <= currentThreshold)
			{
				if (l == MB2_LogLevel.error)
				{
					Debug.LogError(msg);
				}
				if (l == MB2_LogLevel.warn)
				{
					Debug.LogWarning(string.Format("frm={0} WARN {1}", Time.frameCount, msg));
				}
				if (l == MB2_LogLevel.info)
				{
					Debug.Log(string.Format("frm={0} INFO {1}", Time.frameCount, msg));
				}
				if (l == MB2_LogLevel.debug)
				{
					Debug.Log(string.Format("frm={0} DEBUG {1}", Time.frameCount, msg));
				}
				if (l == MB2_LogLevel.trace)
				{
					Debug.Log(string.Format("frm={0} TRACE {1}", Time.frameCount, msg));
				}
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004120 File Offset: 0x00002520
		public static string Error(string msg, params object[] args)
		{
			string arg = string.Format(msg, args);
			string text = string.Format("f={0} ERROR {1}", Time.frameCount, arg);
			Debug.LogError(text);
			return text;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004154 File Offset: 0x00002554
		public static string Warn(string msg, params object[] args)
		{
			string arg = string.Format(msg, args);
			string text = string.Format("f={0} WARN {1}", Time.frameCount, arg);
			Debug.LogWarning(text);
			return text;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004188 File Offset: 0x00002588
		public static string Info(string msg, params object[] args)
		{
			string arg = string.Format(msg, args);
			string text = string.Format("f={0} INFO {1}", Time.frameCount, arg);
			Debug.Log(text);
			return text;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000041BC File Offset: 0x000025BC
		public static string LogDebug(string msg, params object[] args)
		{
			string arg = string.Format(msg, args);
			string text = string.Format("f={0} DEBUG {1}", Time.frameCount, arg);
			Debug.Log(text);
			return text;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000041F0 File Offset: 0x000025F0
		public static string Trace(string msg, params object[] args)
		{
			string arg = string.Format(msg, args);
			string text = string.Format("f={0} TRACE {1}", Time.frameCount, arg);
			Debug.Log(text);
			return text;
		}
	}
}
