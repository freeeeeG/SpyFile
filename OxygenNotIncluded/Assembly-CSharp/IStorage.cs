using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000800 RID: 2048
public interface IStorage
{
	// Token: 0x06003A69 RID: 14953
	bool ShouldShowInUI();

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06003A6A RID: 14954
	// (set) Token: 0x06003A6B RID: 14955
	bool allowUIItemRemoval { get; set; }

	// Token: 0x06003A6C RID: 14956
	GameObject Drop(GameObject go, bool do_disease_transfer = true);

	// Token: 0x06003A6D RID: 14957
	List<GameObject> GetItems();

	// Token: 0x06003A6E RID: 14958
	bool IsFull();

	// Token: 0x06003A6F RID: 14959
	bool IsEmpty();

	// Token: 0x06003A70 RID: 14960
	float Capacity();

	// Token: 0x06003A71 RID: 14961
	float RemainingCapacity();

	// Token: 0x06003A72 RID: 14962
	float GetAmountAvailable(Tag tag);

	// Token: 0x06003A73 RID: 14963
	void ConsumeIgnoringDisease(Tag tag, float amount);
}
