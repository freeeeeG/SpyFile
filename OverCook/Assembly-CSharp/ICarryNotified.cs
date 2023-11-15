using System;

// Token: 0x020007F4 RID: 2036
public interface ICarryNotified
{
	// Token: 0x0600271B RID: 10011
	void OnCarryBegun(ICarrier _carrier);

	// Token: 0x0600271C RID: 10012
	void OnCarryEnded(ICarrier _carrier);
}
