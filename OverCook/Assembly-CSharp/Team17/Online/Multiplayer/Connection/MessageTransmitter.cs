using System;

namespace Team17.Online.Multiplayer.Connection
{
	// Token: 0x02000881 RID: 2177
	public class MessageTransmitter
	{
		// Token: 0x06002A36 RID: 10806 RVA: 0x000C5417 File Offset: 0x000C3817
		public virtual void Initialise(Generic<bool, byte[], int, bool> sendData)
		{
			this.m_SendData = sendData;
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000C5420 File Offset: 0x000C3820
		public virtual bool Transmit(byte[] data, int size, bool bReliable)
		{
			return this.m_SendData(data, size, bReliable);
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000C5430 File Offset: 0x000C3830
		public virtual void Update()
		{
		}

		// Token: 0x04002141 RID: 8513
		protected Generic<bool, byte[], int, bool> m_SendData;
	}
}
