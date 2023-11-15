using System;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class ItemCarrierAdapter : ICarrierPlacement
{
	// Token: 0x0600196B RID: 6507 RVA: 0x0007FFC3 File Offset: 0x0007E3C3
	public ItemCarrierAdapter(GameObject _item)
	{
		this.m_item = _item;
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x0007FFD2 File Offset: 0x0007E3D2
	public GameObject InspectCarriedItem()
	{
		return this.m_item;
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x0007FFDA File Offset: 0x0007E3DA
	public void DestroyCarriedItem()
	{
		this.m_item = null;
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0007FFE4 File Offset: 0x0007E3E4
	public GameObject TakeItem()
	{
		GameObject item = this.m_item;
		this.m_item = null;
		return item;
	}

	// Token: 0x0400143D RID: 5181
	private GameObject m_item;
}
