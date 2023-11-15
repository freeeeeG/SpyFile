using System;
using System.Collections.Generic;

// Token: 0x020000D2 RID: 210
public class Cheetah : Boss
{
	// Token: 0x17000252 RID: 594
	// (get) Token: 0x0600054D RID: 1357 RVA: 0x0000E988 File Offset: 0x0000CB88
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Cheetah;
		}
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0000E98C File Offset: 0x0000CB8C
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		base.ShowBossText(1f);
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0000E9A8 File Offset: 0x0000CBA8
	protected override void SetStrategy()
	{
		base.DamageStrategy = new CheetahStrateygy(this);
	}
}
