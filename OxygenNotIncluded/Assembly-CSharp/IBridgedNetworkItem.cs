using System;
using System.Collections.Generic;

// Token: 0x020005D4 RID: 1492
public interface IBridgedNetworkItem
{
	// Token: 0x06002504 RID: 9476
	void AddNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06002505 RID: 9477
	bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06002506 RID: 9478
	int GetNetworkCell();
}
