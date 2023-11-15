using System;

// Token: 0x02000800 RID: 2048
public interface IClientOrderDefinition
{
	// Token: 0x06002738 RID: 10040
	AssembledDefinitionNode GetOrderComposition();

	// Token: 0x06002739 RID: 10041
	void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback);

	// Token: 0x0600273A RID: 10042
	void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback);
}
