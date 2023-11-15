using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class Transfer : Runner
{
	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600057D RID: 1405 RVA: 0x0000F012 File Offset: 0x0000D212
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Transfer;
		}
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0000F015 File Offset: 0x0000D215
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		this.blink = 1;
		this.blinkDistance = Mathf.Min(8, 1 + (GameRes.CurrentWave + 1) / 20);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0000F048 File Offset: 0x0000D248
	protected override void OnEnemyUpdate()
	{
		base.OnEnemyUpdate();
		if (this.isOutTroing)
		{
			return;
		}
		if (this.blink >= 1 && base.DamageStrategy.CurrentHealth / base.DamageStrategy.MaxHealth < 0.5f)
		{
			this.blink--;
			base.Flash(-this.blinkDistance);
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0000F0A6 File Offset: 0x0000D2A6
	protected override void PrepareNextState()
	{
		base.PrepareNextState();
		this.enemyCol.transform.localPosition = Vector3.zero;
	}

	// Token: 0x04000249 RID: 585
	private int blink = 1;

	// Token: 0x0400024A RID: 586
	private int blinkDistance;
}
