using System;

// Token: 0x02000399 RID: 921
public class CoalBucketCosmeticDecisions : OverlapModelsMealDecisions
{
	// Token: 0x0600115B RID: 4443 RVA: 0x000639CF File Offset: 0x00061DCF
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}
}
