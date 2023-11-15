using System;
using System.Collections.Generic;

// Token: 0x020000CD RID: 205
public class Binary : Boss
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000534 RID: 1332 RVA: 0x0000E48F File Offset: 0x0000C68F
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Binary;
		}
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0000E494 File Offset: 0x0000C694
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		if (Binary.FirstBinary == null)
		{
			this.SpeedIntensify = 0f;
			Binary.FirstBinary = this;
			this.m_brother = (Singleton<GameManager>.Instance.SpawnEnemy(this.EnemyType, this.PointIndex, intensify, dmgResist, BoardSystem.shortestPoints) as Binary);
			this.m_brother.m_brother = this;
			this.m_brother.SpeedIntensify = 0f;
		}
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0000E514 File Offset: 0x0000C714
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		if (this.m_brother != null)
		{
			this.m_brother.SpeedIntensify += 1f;
			this.m_brother.ProgressFactor = this.m_brother.Speed * this.m_brother.Adjust;
			this.m_brother.ShowBossText(1f);
			this.m_brother.m_brother = null;
			this.m_brother = null;
			return;
		}
		Binary.FirstBinary = null;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0000E598 File Offset: 0x0000C798
	public override void OnDie()
	{
		base.OnDie();
		if (this.m_brother == null)
		{
			Binary.FirstBinary = null;
			Singleton<LevelManager>.Instance.SetAchievement("ACH_BINARY");
		}
	}

	// Token: 0x0400021F RID: 543
	public static Binary FirstBinary;

	// Token: 0x04000220 RID: 544
	private Binary m_brother;
}
