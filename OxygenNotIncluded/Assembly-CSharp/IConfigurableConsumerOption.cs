using System;
using UnityEngine;

// Token: 0x02000C0F RID: 3087
public interface IConfigurableConsumerOption
{
	// Token: 0x060061C3 RID: 25027
	Tag GetID();

	// Token: 0x060061C4 RID: 25028
	string GetName();

	// Token: 0x060061C5 RID: 25029
	string GetDetailedDescription();

	// Token: 0x060061C6 RID: 25030
	string GetDescription();

	// Token: 0x060061C7 RID: 25031
	Sprite GetIcon();

	// Token: 0x060061C8 RID: 25032
	IConfigurableConsumerIngredient[] GetIngredients();
}
