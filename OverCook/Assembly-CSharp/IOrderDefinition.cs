using System;

// Token: 0x020007FF RID: 2047
public interface IOrderDefinition
{
	// Token: 0x06002735 RID: 10037
	AssembledDefinitionNode GetOrderComposition();

	// Token: 0x06002736 RID: 10038
	void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback);

	// Token: 0x06002737 RID: 10039
	void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback);
}
