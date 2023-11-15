using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000242 RID: 578
public class TurretBillboard : MonoBehaviour
{
	// Token: 0x06000EDA RID: 3802 RVA: 0x00027360 File Offset: 0x00025560
	public void SetBillBoard()
	{
		this.turrets.Clear();
		this.HighestTurret = null;
		foreach (IGameBehavior gameBehavior in Singleton<GameManager>.Instance.elementTurrets.behaviors)
		{
			TurretItem turretItem = Object.Instantiate<TurretItem>(this.turretItemPrefab, this.contentParent);
			turretItem.SetItemData(gameBehavior as TurretContent);
			this.turrets.Add(turretItem);
		}
		foreach (IGameBehavior gameBehavior2 in Singleton<GameManager>.Instance.refactorTurrets.behaviors)
		{
			TurretItem turretItem = Object.Instantiate<TurretItem>(this.turretItemPrefab, this.contentParent);
			turretItem.SetItemData(gameBehavior2 as TurretContent);
			this.turrets.Add(turretItem);
		}
		this.SortBillBoard();
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x00027468 File Offset: 0x00025668
	private void SortBillBoard()
	{
		for (int i = 0; i < this.turrets.Count - 1; i++)
		{
			for (int j = 0; j < this.turrets.Count - 1 - i; j++)
			{
				if (this.turrets[j].TotalDamage > this.turrets[j + 1].TotalDamage)
				{
					TurretItem value = this.turrets[j];
					this.turrets[j] = this.turrets[j + 1];
					this.turrets[j + 1] = value;
				}
			}
		}
		for (int k = 0; k < this.turrets.Count; k++)
		{
			this.turrets[k].transform.SetAsFirstSibling();
			this.turrets[k].SetRank(this.turrets.Count - k - 1);
		}
		if (this.turrets.Count > 0)
		{
			this.HighestTurret = this.turrets[this.turrets.Count - 1].m_Turret;
		}
	}

	// Token: 0x0400074B RID: 1867
	[SerializeField]
	private TurretItem turretItemPrefab;

	// Token: 0x0400074C RID: 1868
	[SerializeField]
	private Transform contentParent;

	// Token: 0x0400074D RID: 1869
	private List<TurretItem> turrets = new List<TurretItem>();

	// Token: 0x0400074E RID: 1870
	public TurretContent HighestTurret;
}
