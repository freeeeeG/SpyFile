using System;
using System.Collections.Generic;

// Token: 0x020000F4 RID: 244
public class Leader : Tanker
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001089E File Offset: 0x0000EA9E
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Leader;
		}
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x000108A1 File Offset: 0x0000EAA1
	protected override void SetStrategy()
	{
		base.DamageStrategy = new LeaderStrategy(this);
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x000108B0 File Offset: 0x0000EAB0
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		BuffInfo buffInfo = new BuffInfo(EnemyBuffName.Invisible, 1, false);
		base.DamageStrategy.ApplyBuff(buffInfo);
	}
}
