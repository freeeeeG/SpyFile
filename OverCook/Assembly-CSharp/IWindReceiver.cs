using System;

// Token: 0x0200058F RID: 1423
public interface IWindReceiver
{
	// Token: 0x06001B01 RID: 6913
	void AddWindSource(IWindSource _volume);

	// Token: 0x06001B02 RID: 6914
	void RemoveWindSource(IWindSource _volume);
}
