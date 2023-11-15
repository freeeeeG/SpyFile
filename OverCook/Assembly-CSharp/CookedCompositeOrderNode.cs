using System;

// Token: 0x020009BC RID: 2492
[Serializable]
public class CookedCompositeOrderNode : CompositeOrderNode
{
	// Token: 0x060030D4 RID: 12500 RVA: 0x000E5B98 File Offset: 0x000E3F98
	public override AssembledDefinitionNode Convert()
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = new CookedCompositeAssembledNode();
		cookedCompositeAssembledNode.m_composition = this.m_composition.ConvertAll((OrderDefinitionNode x) => x.Convert());
		cookedCompositeAssembledNode.m_optional = this.m_optional.ConvertAll((OrderDefinitionNode x) => x.Convert());
		cookedCompositeAssembledNode.m_cookingStep = this.m_cookingStep;
		cookedCompositeAssembledNode.m_progress = this.m_progress;
		return cookedCompositeAssembledNode;
	}

	// Token: 0x04002738 RID: 10040
	public CookingStepData m_cookingStep;

	// Token: 0x04002739 RID: 10041
	public CookedCompositeOrderNode.CookingProgress m_progress = CookedCompositeOrderNode.CookingProgress.Cooked;

	// Token: 0x020009BD RID: 2493
	public enum CookingProgress
	{
		// Token: 0x0400273D RID: 10045
		Raw,
		// Token: 0x0400273E RID: 10046
		Cooked,
		// Token: 0x0400273F RID: 10047
		Burnt
	}
}
