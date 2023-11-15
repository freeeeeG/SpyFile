using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006E2 RID: 1762
public class Attack
{
	// Token: 0x06003055 RID: 12373 RVA: 0x000FFAC6 File Offset: 0x000FDCC6
	public Attack(AttackProperties properties, GameObject[] targets)
	{
		this.properties = properties;
		this.targets = targets;
		this.RollHits();
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x000FFAE4 File Offset: 0x000FDCE4
	private void RollHits()
	{
		int num = 0;
		while (num < this.targets.Length && num <= this.properties.maxHits - 1)
		{
			if (this.targets[num] != null)
			{
				new Hit(this.properties, this.targets[num]);
			}
			num++;
		}
	}

	// Token: 0x04001C7F RID: 7295
	private AttackProperties properties;

	// Token: 0x04001C80 RID: 7296
	private GameObject[] targets;

	// Token: 0x04001C81 RID: 7297
	public List<Hit> Hits;
}
