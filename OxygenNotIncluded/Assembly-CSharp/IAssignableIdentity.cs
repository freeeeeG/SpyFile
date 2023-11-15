using System;
using System.Collections.Generic;

// Token: 0x020004B9 RID: 1209
public interface IAssignableIdentity
{
	// Token: 0x06001B70 RID: 7024
	string GetProperName();

	// Token: 0x06001B71 RID: 7025
	List<Ownables> GetOwners();

	// Token: 0x06001B72 RID: 7026
	Ownables GetSoleOwner();

	// Token: 0x06001B73 RID: 7027
	bool IsNull();

	// Token: 0x06001B74 RID: 7028
	bool HasOwner(Assignables owner);

	// Token: 0x06001B75 RID: 7029
	int NumOwners();
}
