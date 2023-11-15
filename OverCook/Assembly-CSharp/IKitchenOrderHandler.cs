using System;

// Token: 0x020006C2 RID: 1730
public interface IKitchenOrderHandler
{
	// Token: 0x060020BB RID: 8379
	void FoodDelivered(AssembledDefinitionNode _definition, PlatingStepData _plateType, ServerPlateStation _station);
}
