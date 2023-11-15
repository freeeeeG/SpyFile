using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class EnemyDetector : MonoBehaviour
{
	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000575 RID: 1397 RVA: 0x0000EF02 File Offset: 0x0000D102
	// (set) Token: 0x06000576 RID: 1398 RVA: 0x0000EF0A File Offset: 0x0000D10A
	public Enemy Enemy
	{
		get
		{
			return this.enemy;
		}
		set
		{
			this.enemy = value;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000577 RID: 1399 RVA: 0x0000EF13 File Offset: 0x0000D113
	// (set) Token: 0x06000578 RID: 1400 RVA: 0x0000EF1B File Offset: 0x0000D11B
	public List<Enemy> Enemies
	{
		get
		{
			return this.enemies;
		}
		set
		{
			this.enemies = value;
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0000EF24 File Offset: 0x0000D124
	private void Awake()
	{
		this.Enemy = base.transform.root.GetComponent<Enemy>();
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0000EF3C File Offset: 0x0000D13C
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Enemy enemy = (Enemy)collision.GetComponent<TargetPoint>().Enemy;
		if (enemy != this.Enemy)
		{
			this.Enemies.Add(enemy);
			enemy.AffectHealerCount++;
			enemy.ProgressFactor = enemy.Speed * enemy.Adjust;
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0000EF98 File Offset: 0x0000D198
	private void OnTriggerExit2D(Collider2D collision)
	{
		Enemy enemy = (Enemy)collision.GetComponent<TargetPoint>().Enemy;
		if (enemy != this.Enemy && enemy.gameObject.activeSelf)
		{
			this.Enemies.Remove(enemy);
			enemy.AffectHealerCount--;
			enemy.ProgressFactor = enemy.Speed * enemy.Adjust;
		}
	}

	// Token: 0x04000247 RID: 583
	private Enemy enemy;

	// Token: 0x04000248 RID: 584
	private List<Enemy> enemies = new List<Enemy>();
}
