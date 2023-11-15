using System;

// Token: 0x0200047E RID: 1150
public interface IHandleOrderModification
{
	// Token: 0x06001573 RID: 5491
	bool CanAddOrderContents(AssembledDefinitionNode[] _contents);

	// Token: 0x06001574 RID: 5492
	void AddOrderContents(AssembledDefinitionNode[] _contents);
}
