using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class ArmorHolder : MonoBehaviour
{
	// Token: 0x0600052C RID: 1324 RVA: 0x0000E38C File Offset: 0x0000C58C
	public void Initialize(Enemy enemy, float maxHealth)
	{
		this.enemyParent = enemy;
		for (int i = 0; i < this.m_Armors.Length; i++)
		{
			this.m_Armors[i].Initialize(enemy, maxHealth, this);
			this.armorRemain++;
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0000E3D1 File Offset: 0x0000C5D1
	public void RemoveArmor(int value)
	{
		this.armorRemain -= value;
	}

	// Token: 0x04000219 RID: 537
	[SerializeField]
	private Armor[] m_Armors;

	// Token: 0x0400021A RID: 538
	private int armorRemain;

	// Token: 0x0400021B RID: 539
	private Enemy enemyParent;
}
