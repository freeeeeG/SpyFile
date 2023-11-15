using System;

// Token: 0x020004B6 RID: 1206
public interface ICookable : IBaseCookable
{
	// Token: 0x06001678 RID: 5752
	bool IsCooked();

	// Token: 0x06001679 RID: 5753
	bool Cook(float _cookingDeltatTime);
}
