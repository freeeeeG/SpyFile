using System;

namespace Team17.Online
{
	// Token: 0x02000956 RID: 2390
	public class OnlineMultiplayerReturnCode<ReturnCodeType> : SteamOnlineMultiplayerReturnCode
	{
		// Token: 0x06002ED6 RID: 11990 RVA: 0x000DBB46 File Offset: 0x000D9F46
		public bool DisplayPlatformSpecificError(bool blockAllUnityThreads = false)
		{
			if (this.m_usePlatformError)
			{
				base.ShowErrorDialog(blockAllUnityThreads);
				return true;
			}
			return false;
		}

		// Token: 0x04002576 RID: 9590
		public bool m_usePlatformError;

		// Token: 0x04002577 RID: 9591
		public ReturnCodeType m_returnCode = default(ReturnCodeType);
	}
}
