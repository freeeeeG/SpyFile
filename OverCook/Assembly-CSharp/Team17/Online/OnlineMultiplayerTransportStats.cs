using System;
using System.Text;

namespace Team17.Online
{
	// Token: 0x02000969 RID: 2409
	public class OnlineMultiplayerTransportStats : IOnlineMultiplayerTransportStats
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06002F27 RID: 12071 RVA: 0x000DC7BF File Offset: 0x000DABBF
		public string Text
		{
			get
			{
				return this.BuildText();
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000DC7C8 File Offset: 0x000DABC8
		public void Update(float gameTime)
		{
			if (gameTime - this.m_gameTimeLast >= 1f)
			{
				this.m_gameTimeLast = gameTime;
				this.m_dataBytesSentLast = this.m_dataBytesSent;
				this.m_voiceBytesSentLast = this.m_voiceBytesSent;
				this.m_dataBytesReceivedLast = this.m_dataBytesReceived;
				this.m_voiceBytesReceivedLast = this.m_voiceBytesReceived;
				this.m_dataBytesSent = 0UL;
				this.m_voiceBytesSent = 0UL;
				this.m_dataBytesReceived = 0UL;
				this.m_voiceBytesReceived = 0UL;
			}
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000DC840 File Offset: 0x000DAC40
		public void Add(OnlineMultiplayerTransportStats.StatType statType, uint byteValue)
		{
			switch (statType)
			{
			case OnlineMultiplayerTransportStats.StatType.eDataSent:
				this.m_dataBytesSent += (ulong)byteValue;
				break;
			case OnlineMultiplayerTransportStats.StatType.eDataReceived:
				this.m_dataBytesReceived += (ulong)byteValue;
				break;
			case OnlineMultiplayerTransportStats.StatType.eVoiceSent:
				this.m_voiceBytesSent += (ulong)byteValue;
				break;
			case OnlineMultiplayerTransportStats.StatType.eVoiceReceived:
				this.m_voiceBytesReceived += (ulong)byteValue;
				break;
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000DC8C0 File Offset: 0x000DACC0
		public string BuildText()
		{
			try
			{
				this.m_builder.Length = 0;
				this.m_builder.Append("Network Transport : Data { ");
				this.m_builder.Append(this.m_dataBytesSentLast.ToString());
				this.m_builder.Append(" : ");
				this.m_builder.Append(this.m_dataBytesReceivedLast.ToString());
				this.m_builder.Append(" }  Voice { ");
				this.m_builder.Append(this.m_voiceBytesSentLast.ToString());
				this.m_builder.Append(" : ");
				this.m_builder.Append(this.m_voiceBytesReceivedLast.ToString());
				this.m_builder.Append(" }");
				return this.m_builder.ToString();
			}
			catch (Exception)
			{
			}
			return this.m_builderFailure;
		}

		// Token: 0x040025AD RID: 9645
		private StringBuilder m_builder = new StringBuilder(0, 150);

		// Token: 0x040025AE RID: 9646
		private string m_builderFailure = "Network Transport Error";

		// Token: 0x040025AF RID: 9647
		private float m_gameTimeLast;

		// Token: 0x040025B0 RID: 9648
		private ulong m_dataBytesSent;

		// Token: 0x040025B1 RID: 9649
		private ulong m_voiceBytesSent;

		// Token: 0x040025B2 RID: 9650
		private ulong m_dataBytesReceived;

		// Token: 0x040025B3 RID: 9651
		private ulong m_voiceBytesReceived;

		// Token: 0x040025B4 RID: 9652
		private ulong m_dataBytesSentLast;

		// Token: 0x040025B5 RID: 9653
		private ulong m_voiceBytesSentLast;

		// Token: 0x040025B6 RID: 9654
		private ulong m_dataBytesReceivedLast;

		// Token: 0x040025B7 RID: 9655
		private ulong m_voiceBytesReceivedLast;

		// Token: 0x0200096A RID: 2410
		public enum StatType
		{
			// Token: 0x040025B9 RID: 9657
			eDataSent,
			// Token: 0x040025BA RID: 9658
			eDataReceived,
			// Token: 0x040025BB RID: 9659
			eVoiceSent,
			// Token: 0x040025BC RID: 9660
			eVoiceReceived
		}
	}
}
