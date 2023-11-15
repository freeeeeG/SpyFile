using System;
using System.Diagnostics;

namespace InControl
{
	// Token: 0x020002BA RID: 698
	public class Logger
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000D68 RID: 3432 RVA: 0x000433AC File Offset: 0x000417AC
		// (remove) Token: 0x06000D69 RID: 3433 RVA: 0x000433E0 File Offset: 0x000417E0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Logger.LogMessageHandler OnLogMessage;

		// Token: 0x06000D6A RID: 3434 RVA: 0x00043414 File Offset: 0x00041814
		public static void LogInfo(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage message = new LogMessage
				{
					text = text,
					type = LogMessageType.Info
				};
				Logger.OnLogMessage(message);
			}
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00043450 File Offset: 0x00041850
		public static void LogWarning(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage message = new LogMessage
				{
					text = text,
					type = LogMessageType.Warning
				};
				Logger.OnLogMessage(message);
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0004348C File Offset: 0x0004188C
		public static void LogError(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage message = new LogMessage
				{
					text = text,
					type = LogMessageType.Error
				};
				Logger.OnLogMessage(message);
			}
		}

		// Token: 0x020002BB RID: 699
		// (Invoke) Token: 0x06000D6E RID: 3438
		public delegate void LogMessageHandler(LogMessage message);
	}
}
