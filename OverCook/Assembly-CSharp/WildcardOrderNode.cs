using System;

// Token: 0x020009C9 RID: 2505
[Serializable]
public class WildcardOrderNode : OrderDefinitionNode
{
	// Token: 0x06003119 RID: 12569 RVA: 0x000E667F File Offset: 0x000E4A7F
	public override AssembledDefinitionNode Convert()
	{
		return AssembledDefinitionNode.NullNode;
	}

	// Token: 0x0400275B RID: 10075
	public CookingStepData m_cookingStep;
}
