using System;

// Token: 0x020004B5 RID: 1205
public interface IBaseCookable
{
	// Token: 0x06001672 RID: 5746
	bool IsBurning();

	// Token: 0x06001673 RID: 5747
	float GetCookingProgress();

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06001674 RID: 5748
	float AccessCookingTime { get; }

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06001675 RID: 5749
	CookingStepData AccessCookingType { get; }

	// Token: 0x06001676 RID: 5750
	CookedCompositeOrderNode.CookingProgress GetCookedOrderState();

	// Token: 0x06001677 RID: 5751
	CookingStationType GetRequiredStationType();
}
