using System;
using System.Text;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200001C RID: 28
	public class ObjectLog
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00004222 File Offset: 0x00002622
		public ObjectLog(short bufferSize)
		{
			this.logMessages = new string[(int)bufferSize];
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004238 File Offset: 0x00002638
		private void _CacheLogMessage(string msg)
		{
			if (this.logMessages.Length == 0)
			{
				return;
			}
			this.logMessages[this.pos] = msg;
			this.pos++;
			if (this.pos >= this.logMessages.Length)
			{
				this.pos = 0;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004289 File Offset: 0x00002689
		public void Log(MB2_LogLevel l, string msg, MB2_LogLevel currentThreshold)
		{
			MB2_Log.Log(l, msg, currentThreshold);
			this._CacheLogMessage(msg);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000429A File Offset: 0x0000269A
		public void Error(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Error(msg, args));
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000042A9 File Offset: 0x000026A9
		public void Warn(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Warn(msg, args));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000042B8 File Offset: 0x000026B8
		public void Info(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Info(msg, args));
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000042C7 File Offset: 0x000026C7
		public void LogDebug(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.LogDebug(msg, args));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000042D6 File Offset: 0x000026D6
		public void Trace(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Trace(msg, args));
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000042E8 File Offset: 0x000026E8
		public string Dump()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			if (this.logMessages[this.logMessages.Length - 1] != null)
			{
				num = this.pos;
			}
			for (int i = 0; i < this.logMessages.Length; i++)
			{
				int num2 = (num + i) % this.logMessages.Length;
				if (this.logMessages[num2] == null)
				{
					break;
				}
				stringBuilder.AppendLine(this.logMessages[num2]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000061 RID: 97
		private int pos;

		// Token: 0x04000062 RID: 98
		private string[] logMessages;
	}
}
