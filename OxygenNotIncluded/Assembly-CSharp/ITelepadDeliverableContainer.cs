using System;
using UnityEngine;

// Token: 0x02000802 RID: 2050
public interface ITelepadDeliverableContainer
{
	// Token: 0x06003A75 RID: 14965
	void SelectDeliverable();

	// Token: 0x06003A76 RID: 14966
	void DeselectDeliverable();

	// Token: 0x06003A77 RID: 14967
	GameObject GetGameObject();
}
