using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class Divider : Boss
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000551 RID: 1361 RVA: 0x0000E9BE File Offset: 0x0000CBBE
	protected override bool ShakeCam
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000552 RID: 1362 RVA: 0x0000E9C1 File Offset: 0x0000CBC1
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Divider;
		}
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0000E9C5 File Offset: 0x0000CBC5
	public override void OnDie()
	{
		if (!this.isOutTroing)
		{
			this.GetSprings();
		}
		base.OnDie();
		Singleton<LevelManager>.Instance.SetAchievement("ACH_DIVIDER");
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0000E9EC File Offset: 0x0000CBEC
	protected void GetSprings()
	{
		for (int i = 0; i < this.springs; i++)
		{
			this.SpawnEnemy();
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0000EA10 File Offset: 0x0000CC10
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		base.ShowBossText(0.5f);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0000EA2C File Offset: 0x0000CC2C
	private void SpawnEnemy()
	{
		if (this.SpawnEnemyType == EnemyType.None)
		{
			return;
		}
		(Singleton<GameManager>.Instance.SpawnEnemy(this.SpawnEnemyType, this.PointIndex, this.Intensify / 2f, this.DmgResist, BoardSystem.shortestPoints) as Divider).Progress = Mathf.Clamp(base.Progress + Random.Range(-0.2f, 0.2f), 0f, 1f);
	}

	// Token: 0x0400022E RID: 558
	[SerializeField]
	protected int springs;

	// Token: 0x0400022F RID: 559
	[SerializeField]
	protected EnemyType SpawnEnemyType;
}
