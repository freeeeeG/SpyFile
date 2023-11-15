using System;
using UnityEngine;

namespace Team17.Online
{
	// Token: 0x0200097C RID: 2428
	public class SteamOnlineMultiplayerNetworkLinkCoordinator
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06002F59 RID: 12121 RVA: 0x000DD3CA File Offset: 0x000DB7CA
		public SteamOnlineMultiplayerNetworkLinkCoordinator.LinkStatus Status
		{
			get
			{
				return this.GetStatus();
			}
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000DD3D2 File Offset: 0x000DB7D2
		public void Initialize()
		{
			if (!this.m_isInitialized)
			{
				this.m_isInitialized = true;
			}
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000DD3E6 File Offset: 0x000DB7E6
		public void Shutdown()
		{
			if (this.m_isInitialized)
			{
				this.m_isInitialized = false;
			}
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000DD3FA File Offset: 0x000DB7FA
		public void Update()
		{
			if (this.m_isInitialized)
			{
			}
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000DD408 File Offset: 0x000DB808
		private SteamOnlineMultiplayerNetworkLinkCoordinator.LinkStatus GetStatus()
		{
			SteamOnlineMultiplayerNetworkLinkCoordinator.LinkStatus result = SteamOnlineMultiplayerNetworkLinkCoordinator.LinkStatus.eUnknown;
			if (this.m_isInitialized)
			{
				switch (Application.internetReachability)
				{
				case NetworkReachability.NotReachable:
					result = SteamOnlineMultiplayerNetworkLinkCoordinator.LinkStatus.eNotAvailable;
					break;
				case NetworkReachability.ReachableViaCarrierDataNetwork:
				case NetworkReachability.ReachableViaLocalAreaNetwork:
					result = SteamOnlineMultiplayerNetworkLinkCoordinator.LinkStatus.eAvailable;
					break;
				}
			}
			return result;
		}

		// Token: 0x040025D7 RID: 9687
		private bool m_isInitialized;

		// Token: 0x0200097D RID: 2429
		public enum LinkStatus
		{
			// Token: 0x040025D9 RID: 9689
			eAvailable,
			// Token: 0x040025DA RID: 9690
			eNotAvailable,
			// Token: 0x040025DB RID: 9691
			eUnknown
		}
	}
}
