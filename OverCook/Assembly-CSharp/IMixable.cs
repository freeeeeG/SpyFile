using System;

// Token: 0x020004C8 RID: 1224
public interface IMixable
{
	// Token: 0x0600169B RID: 5787
	MixedCompositeOrderNode.MixingProgress GetMixedOrderState();

	// Token: 0x0600169C RID: 5788
	float GetMixingProgress();

	// Token: 0x0600169D RID: 5789
	bool IsMixed();

	// Token: 0x0600169E RID: 5790
	bool IsOverMixed();

	// Token: 0x0600169F RID: 5791
	bool Mix(float _mixingDeltaTime);
}
