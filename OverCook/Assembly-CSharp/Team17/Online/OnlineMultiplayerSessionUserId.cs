using System;
using System.IO;
using BitStream;

namespace Team17.Online
{
	// Token: 0x02000968 RID: 2408
	public class OnlineMultiplayerSessionUserId : SteamOnlineMultiplayerSessionUserId, IOnlineMultiplayerSessionUserId
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06002F1A RID: 12058 RVA: 0x000DC5B0 File Offset: 0x000DA9B0
		public string DisplayName
		{
			get
			{
				return this.m_displayName;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06002F1B RID: 12059 RVA: 0x000DC5B8 File Offset: 0x000DA9B8
		public bool IsHost
		{
			get
			{
				return this.m_isHost;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06002F1C RID: 12060 RVA: 0x000DC5C0 File Offset: 0x000DA9C0
		public bool IsLocal
		{
			get
			{
				return this.m_isLocal;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06002F1D RID: 12061 RVA: 0x000DC5C8 File Offset: 0x000DA9C8
		public byte UniqueId
		{
			get
			{
				return this.m_uniqueId;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06002F1E RID: 12062 RVA: 0x000DC5D0 File Offset: 0x000DA9D0
		// (set) Token: 0x06002F1F RID: 12063 RVA: 0x000DC5D8 File Offset: 0x000DA9D8
		public bool IsLocallyMuted
		{
			get
			{
				return this.m_isLocallyMuted;
			}
			set
			{
				if (!this.IsLocal)
				{
					this.m_isLocallyMuted = value;
					this.m_muteStatusChanged = true;
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000DC5F3 File Offset: 0x000DA9F3
		public bool IsSpeaking
		{
			get
			{
				return this.m_isLocallySpeaking;
			}
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000DC5FC File Offset: 0x000DA9FC
		public void Serialize(BitStreamWriter stream)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(this.m_displayName);
					binaryWriter.Write(this.m_isHost);
					binaryWriter.Write(this.m_uniqueId);
					base.Write(binaryWriter);
				}
				byte[] array = memoryStream.ToArray();
				stream.Write((uint)array.Length, 16);
				stream.Write(array, array.Length * 8);
			}
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000DC6A0 File Offset: 0x000DAAA0
		public void Deserialize(BitStreamReader stream)
		{
			uint num = stream.ReadUInt32(16);
			byte[] array = new byte[num];
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				array[num2] = stream.ReadByte(8);
				num2++;
			}
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					this.m_displayName = binaryReader.ReadString();
					this.m_isHost = binaryReader.ReadBoolean();
					this.m_uniqueId = binaryReader.ReadByte();
					base.Read(binaryReader);
				}
			}
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000DC75C File Offset: 0x000DAB5C
		public void SetLocallySpeaking(float gameTimeAtStartOfFrame, float smoothTime = 2f)
		{
			this.m_locallySpeakingStopGameTime = gameTimeAtStartOfFrame + smoothTime;
			this.m_isLocallySpeaking = true;
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000DC76E File Offset: 0x000DAB6E
		public void UpdateLocallySpeaking(float gameTimeAtStartOfFrame)
		{
			if (this.m_isLocallySpeaking && gameTimeAtStartOfFrame > this.m_locallySpeakingStopGameTime)
			{
				this.m_isLocallySpeaking = false;
				this.m_locallySpeakingStopGameTime = 0f;
			}
		}

		// Token: 0x040025A4 RID: 9636
		public static readonly byte c_InvalidUniqueId;

		// Token: 0x040025A5 RID: 9637
		public string m_displayName = string.Empty;

		// Token: 0x040025A6 RID: 9638
		public bool m_isHost;

		// Token: 0x040025A7 RID: 9639
		public byte m_uniqueId = OnlineMultiplayerSessionUserId.c_InvalidUniqueId;

		// Token: 0x040025A8 RID: 9640
		public bool m_isLocal;

		// Token: 0x040025A9 RID: 9641
		public bool m_isLocallyMuted;

		// Token: 0x040025AA RID: 9642
		public bool m_muteStatusChanged;

		// Token: 0x040025AB RID: 9643
		private bool m_isLocallySpeaking;

		// Token: 0x040025AC RID: 9644
		private float m_locallySpeakingStopGameTime;
	}
}
