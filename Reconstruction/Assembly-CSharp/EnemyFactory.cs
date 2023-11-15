using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000100 RID: 256
[CreateAssetMenu(menuName = "Factory/EnemyFactory", fileName = "EnemyFactory")]
public class EnemyFactory : ScriptableObject
{
	// Token: 0x06000673 RID: 1651 RVA: 0x000116A0 File Offset: 0x0000F8A0
	public void InitializeFactory()
	{
		this.EnemyDIC = new Dictionary<EnemyType, EnemyAttribute>();
		foreach (EnemyAttribute enemyAttribute in this.enemies)
		{
			this.EnemyDIC.Add(enemyAttribute.EnemyType, enemyAttribute);
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0001170C File Offset: 0x0000F90C
	public EnemyAttribute Get(EnemyType type)
	{
		if (this.EnemyDIC.ContainsKey(type))
		{
			return this.EnemyDIC[type];
		}
		Debug.Log("使用了未定义的敌人类型");
		return null;
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00011734 File Offset: 0x0000F934
	public EnemyAttribute GetRandomBoss(int level)
	{
		switch (level)
		{
		case 1:
			return this.boss1[Random.Range(0, this.boss1.Count)];
		case 2:
			return this.boss2[Random.Range(0, this.boss2.Count)];
		case 3:
			return this.boss3[Random.Range(0, this.boss3.Count)];
		case 4:
			return this.boss4[Random.Range(0, this.boss4.Count)];
		default:
			return null;
		}
	}

	// Token: 0x040002F2 RID: 754
	[SerializeField]
	private List<EnemyAttribute> enemies = new List<EnemyAttribute>();

	// Token: 0x040002F3 RID: 755
	[SerializeField]
	private List<EnemyAttribute> boss1;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	private List<EnemyAttribute> boss2;

	// Token: 0x040002F5 RID: 757
	[SerializeField]
	private List<EnemyAttribute> boss3;

	// Token: 0x040002F6 RID: 758
	[SerializeField]
	private List<EnemyAttribute> boss4;

	// Token: 0x040002F7 RID: 759
	private Dictionary<EnemyType, EnemyAttribute> EnemyDIC;
}
