using System;

// Token: 0x02000037 RID: 55
public class AirborneBuff : TimeBuff
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600014A RID: 330 RVA: 0x0000696F File Offset: 0x00004B6F
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.RuleAirborneBuff;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600014B RID: 331 RVA: 0x00006973 File Offset: 0x00004B73
	public override float BasicDuration
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600014C RID: 332 RVA: 0x0000697A File Offset: 0x00004B7A
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600014D RID: 333 RVA: 0x0000697D File Offset: 0x00004B7D
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00006980 File Offset: 0x00004B80
	public override void Affect(int stacks)
	{
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00006984 File Offset: 0x00004B84
	public override void End()
	{
		if (GameRes.EnemyRemain >= this.Target.MaxAmount)
		{
			return;
		}
		Singleton<GameManager>.Instance.SpawnEnemy(this.Target.EnemyType, (this.Target.PathPoints.Count - this.Target.PointIndex) / 2 + this.Target.PointIndex, this.Target.Intensify * 0.25f, this.Target.DmgResist, BoardSystem.shortestPoints).Buffable.RemoveBuff(EnemyBuffName.RuleAirborneBuff);
	}
}
