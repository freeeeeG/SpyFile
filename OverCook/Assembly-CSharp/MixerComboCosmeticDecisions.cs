using System;

// Token: 0x020003D7 RID: 983
public class MixerComboCosmeticDecisions : ComboCosmeticDecisions
{
	// Token: 0x0600122B RID: 4651 RVA: 0x00066F7B File Offset: 0x0006537B
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}
}
