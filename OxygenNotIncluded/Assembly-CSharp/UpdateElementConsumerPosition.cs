using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
[AddComponentMenu("KMonoBehaviour/scripts/UpdateElementConsumerPosition")]
public class UpdateElementConsumerPosition : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600054D RID: 1357 RVA: 0x000244B9 File Offset: 0x000226B9
	public void Sim200ms(float dt)
	{
		base.GetComponent<ElementConsumer>().RefreshConsumptionRate();
	}
}
