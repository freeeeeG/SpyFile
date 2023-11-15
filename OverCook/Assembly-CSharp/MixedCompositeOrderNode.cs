using System;

// Token: 0x020009C3 RID: 2499
[Serializable]
public class MixedCompositeOrderNode : CompositeOrderNode
{
	// Token: 0x060030F4 RID: 12532 RVA: 0x000E6274 File Offset: 0x000E4674
	public override AssembledDefinitionNode Convert()
	{
		MixedCompositeAssembledNode mixedCompositeAssembledNode = new MixedCompositeAssembledNode();
		mixedCompositeAssembledNode.m_composition = this.m_composition.ConvertAll((OrderDefinitionNode x) => x.Convert());
		mixedCompositeAssembledNode.m_optional = this.m_optional.ConvertAll((OrderDefinitionNode x) => x.Convert());
		mixedCompositeAssembledNode.m_progress = this.m_progress;
		return mixedCompositeAssembledNode;
	}

	// Token: 0x0400274C RID: 10060
	public MixedCompositeOrderNode.MixingProgress m_progress = MixedCompositeOrderNode.MixingProgress.Mixed;

	// Token: 0x020009C4 RID: 2500
	public enum MixingProgress
	{
		// Token: 0x04002750 RID: 10064
		Unmixed,
		// Token: 0x04002751 RID: 10065
		Mixed,
		// Token: 0x04002752 RID: 10066
		OverMixed
	}
}
