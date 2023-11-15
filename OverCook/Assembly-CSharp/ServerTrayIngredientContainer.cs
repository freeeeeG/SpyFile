using System;

// Token: 0x020005DC RID: 1500
public class ServerTrayIngredientContainer : ServerIngredientContainer
{
	// Token: 0x06001CAA RID: 7338 RVA: 0x0008BF3C File Offset: 0x0008A33C
	public override void AddIngredient(AssembledDefinitionNode _orderData)
	{
		CompositeAssembledNode compositeAssembledNode = _orderData as CompositeAssembledNode;
		if (compositeAssembledNode != null && compositeAssembledNode.m_permittedEntries.Count > 0)
		{
			for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
			{
				base.AddIngredient(compositeAssembledNode.m_composition[i]);
			}
			return;
		}
		base.AddIngredient(_orderData);
	}
}
