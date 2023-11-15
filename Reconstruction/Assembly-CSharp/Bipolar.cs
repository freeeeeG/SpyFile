using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class Bipolar : Boss
{
	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x0000E5CB File Offset: 0x0000C7CB
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Bipolar;
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0000E5D0 File Offset: 0x0000C7D0
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		if (Bipolar.FirstBipolar == null)
		{
			Bipolar.FirstBipolar = this;
			Bipolar bipolar = Singleton<GameManager>.Instance.SpawnEnemy(this.EnemyType, 0, intensify, dmgResist, BoardSystem.GetReversePath()) as Bipolar;
			bipolar.pathTiles = bipolar.pathTiles.ToList<BasicTile>();
			bipolar.pathTiles.Reverse();
			this.brother = bipolar;
			bipolar.brother = this;
		}
		base.ShowBossText(1f);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0000E654 File Offset: 0x0000C854
	public override bool GameUpdate()
	{
		if (this.brother != null)
		{
			this.synCounter += Time.deltaTime;
			if (this.synCounter >= this.synInterval)
			{
				float currentHealth = Mathf.Max(this.brother.DamageStrategy.CurrentHealth, base.DamageStrategy.CurrentHealth);
				base.DamageStrategy.CurrentHealth = currentHealth;
				this.synCounter = 0f;
			}
		}
		return base.GameUpdate();
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0000E6CD File Offset: 0x0000C8CD
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		if (this.brother == null)
		{
			Bipolar.FirstBipolar = null;
			return;
		}
		this.brother.brother = null;
		this.brother = null;
	}

	// Token: 0x04000221 RID: 545
	public static Bipolar FirstBipolar;

	// Token: 0x04000222 RID: 546
	private Bipolar brother;

	// Token: 0x04000223 RID: 547
	private float synCounter;

	// Token: 0x04000224 RID: 548
	private float synInterval = 8f;
}
