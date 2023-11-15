using System;
using System.Collections.Generic;

// Token: 0x020000C3 RID: 195
public class AircraftCarrier2 : AircraftCarrier
{
	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000505 RID: 1285 RVA: 0x0000DDCD File Offset: 0x0000BFCD
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.AircraftCarrier2;
		}
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		BuffInfo buffInfo = new BuffInfo(EnemyBuffName.Invisible, 1, false);
		base.DamageStrategy.ApplyBuff(buffInfo);
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0000DE06 File Offset: 0x0000C006
	public override void OnDie()
	{
		base.OnDie();
		if (!Singleton<LevelManager>.Instance.LevelEnd)
		{
			Singleton<LevelManager>.Instance.SetAchievement("ACH_DRAGON");
		}
	}
}
