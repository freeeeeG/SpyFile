using System;

// Token: 0x020004C7 RID: 1223
public interface IIngredientContents
{
	// Token: 0x06001692 RID: 5778
	bool CanAddIngredient(AssembledDefinitionNode _orderData);

	// Token: 0x06001693 RID: 5779
	void AddIngredient(AssembledDefinitionNode _orderData);

	// Token: 0x06001694 RID: 5780
	AssembledDefinitionNode RemoveIngredient(int i);

	// Token: 0x06001695 RID: 5781
	AssembledDefinitionNode GetContentsElement(int i);

	// Token: 0x06001696 RID: 5782
	AssembledDefinitionNode[] GetContents();

	// Token: 0x06001697 RID: 5783
	int GetContentsCount();

	// Token: 0x06001698 RID: 5784
	bool CanTakeContents(AssembledDefinitionNode[] _contents);

	// Token: 0x06001699 RID: 5785
	void Empty();

	// Token: 0x0600169A RID: 5786
	bool HasContents();
}
