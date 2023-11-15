using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class ItemDrone : Drone
{
	// Token: 0x060002FE RID: 766 RVA: 0x000127B0 File Offset: 0x000109B0
	private void Awake()
	{
		this.targetPos = Random.insideUnitCircle * 12f;
	}
}
