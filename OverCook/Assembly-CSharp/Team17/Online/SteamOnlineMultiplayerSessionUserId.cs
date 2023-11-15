using System;
using System.IO;
using Steamworks;

namespace Team17.Online
{
	// Token: 0x02000990 RID: 2448
	public abstract class SteamOnlineMultiplayerSessionUserId
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x000DC518 File Offset: 0x000DA918
		public OnlineUserPlatformId PlatformUserId
		{
			get
			{
				return new OnlineUserPlatformId
				{
					m_steamId = this.m_steamId
				};
			}
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000DC538 File Offset: 0x000DA938
		protected void Write(BinaryWriter writer)
		{
			writer.Write(this.m_steamId.m_SteamID);
			writer.Write(this.m_steamUserRestrictions);
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x000DC557 File Offset: 0x000DA957
		protected void Read(BinaryReader reader)
		{
			this.m_steamId = new CSteamID(reader.ReadUInt64());
			this.m_steamUserRestrictions = reader.ReadUInt32();
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x000DC576 File Offset: 0x000DA976
		public bool HasDirectTransportConnection()
		{
			return this.m_steamLocalTransportConnectionStatus == SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus.eConnectionActive && this.m_steamId.IsValid();
		}

		// Token: 0x04002651 RID: 9809
		public SteamOnlineMultiplayerSessionUserId.TransportConnectionStatus m_steamLocalTransportConnectionStatus;

		// Token: 0x04002652 RID: 9810
		public float m_steamLocalKeepaliveLastSendTime;

		// Token: 0x04002653 RID: 9811
		public float m_steamLocalKeepaliveLastReceiveTime;

		// Token: 0x04002654 RID: 9812
		public CSteamID m_steamId;

		// Token: 0x04002655 RID: 9813
		public uint m_steamUserRestrictions;

		// Token: 0x02000991 RID: 2449
		public enum TransportConnectionStatus : byte
		{
			// Token: 0x04002657 RID: 9815
			eNotApplicable,
			// Token: 0x04002658 RID: 9816
			eWaitingForJoinApprovalFromHost,
			// Token: 0x04002659 RID: 9817
			eWaitingToStartClientConnection,
			// Token: 0x0400265A RID: 9818
			eOpeningTransportConnectionToClientStartDelay,
			// Token: 0x0400265B RID: 9819
			eOpeningTransportConnectionToClient,
			// Token: 0x0400265C RID: 9820
			eConnectionOpenSendClientHello,
			// Token: 0x0400265D RID: 9821
			eConnectionActive,
			// Token: 0x0400265E RID: 9822
			eConnectionDead
		}
	}
}
