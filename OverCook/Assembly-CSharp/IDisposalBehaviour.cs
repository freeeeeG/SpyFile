using System;

// Token: 0x0200060D RID: 1549
public interface IDisposalBehaviour
{
	// Token: 0x06001D55 RID: 7509
	void AddToDisposer(ICarrier _carrier, IDisposer _iDisposer);

	// Token: 0x06001D56 RID: 7510
	void AddToDisposer(IDisposer _iDisposer);

	// Token: 0x06001D57 RID: 7511
	void Destroying(IDisposer _iDisposer);

	// Token: 0x06001D58 RID: 7512
	bool WillBeDestroyed();
}
