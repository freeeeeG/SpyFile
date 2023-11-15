using System;
using System.Collections.Generic;

// Token: 0x020000E8 RID: 232
public class Rhinoceros : Boss
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x060005AF RID: 1455 RVA: 0x0000F8F1 File Offset: 0x0000DAF1
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Rhinoceros;
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		BuffInfo buffInfo = new BuffInfo(EnemyBuffName.TileDamageResistBuff, 1, false);
		base.DamageStrategy.ApplyBuff(buffInfo);
		base.ShowBossText(1f);
	}
}
