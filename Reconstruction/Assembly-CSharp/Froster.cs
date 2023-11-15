using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class Froster : Restorer
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0000F93D File Offset: 0x0000DB3D
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Froster;
		}
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0000F940 File Offset: 0x0000DB40
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		this.freezeTime = Mathf.Min(10f, 1f + (float)((GameRes.CurrentWave + 1) / 20));
		this.freezeRange = Mathf.Min(6f, 1f + (float)((GameRes.CurrentWave + 1) / 20));
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x0000F99E File Offset: 0x0000DB9E
	public override void OnDie()
	{
		base.OnDie();
		Singleton<StaticData>.Instance.FrostTurretEffect(this.model.position, this.freezeRange, this.freezeTime);
	}

	// Token: 0x04000272 RID: 626
	private float freezeRange;

	// Token: 0x04000273 RID: 627
	private float freezeTime;
}
