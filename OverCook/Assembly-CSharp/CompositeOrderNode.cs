using System;

// Token: 0x020009BA RID: 2490
[Serializable]
public class CompositeOrderNode : OrderDefinitionNode
{
	// Token: 0x060030C2 RID: 12482 RVA: 0x000E506C File Offset: 0x000E346C
	public override AssembledDefinitionNode Convert()
	{
		CompositeAssembledNode compositeAssembledNode = new CompositeAssembledNode();
		compositeAssembledNode.m_composition = this.m_composition.ConvertAll((OrderDefinitionNode x) => x.Convert());
		compositeAssembledNode.m_optional = this.m_optional.ConvertAll((OrderDefinitionNode x) => x.Convert());
		return compositeAssembledNode;
	}

	// Token: 0x04002731 RID: 10033
	public OrderDefinitionNode[] m_composition = new OrderDefinitionNode[0];

	// Token: 0x04002732 RID: 10034
	public OrderDefinitionNode[] m_optional = new OrderDefinitionNode[0];
}
