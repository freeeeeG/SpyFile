using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200028D RID: 653
public class EnemyInfoTips : TileTips
{
	// Token: 0x06001022 RID: 4130 RVA: 0x0002B370 File Offset: 0x00029570
	public void ReadEnemyAtt(List<EnemyAttribute> atts)
	{
		for (int i = 0; i < this.grids.Count; i++)
		{
			if (atts.Count > i)
			{
				this.grids[i].SetEnemyInfo(atts[i]);
				this.grids[i].gameObject.SetActive(true);
			}
			else
			{
				this.grids[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private List<EnemyGrid> grids;
}
