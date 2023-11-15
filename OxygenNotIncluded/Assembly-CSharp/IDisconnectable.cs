using System;

// Token: 0x020006BC RID: 1724
public interface IDisconnectable
{
	// Token: 0x06002EEC RID: 12012
	bool Connect();

	// Token: 0x06002EED RID: 12013
	void Disconnect();

	// Token: 0x06002EEE RID: 12014
	bool IsDisconnected();
}
