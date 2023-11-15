using System;

// Token: 0x020004C9 RID: 1225
public interface IClientMixable
{
	// Token: 0x060016A0 RID: 5792
	MixedCompositeOrderNode.MixingProgress GetMixedOrderState();

	// Token: 0x060016A1 RID: 5793
	float GetMixingProgress();

	// Token: 0x060016A2 RID: 5794
	bool IsMixed();

	// Token: 0x060016A3 RID: 5795
	bool IsOverMixed();

	// Token: 0x060016A4 RID: 5796
	GameLoopingAudioTag GetMixingSoundTag();
}
