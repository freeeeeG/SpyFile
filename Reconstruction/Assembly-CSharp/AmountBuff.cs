using System;

// Token: 0x02000036 RID: 54
public class AmountBuff : TimeBuff
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000140 RID: 320 RVA: 0x000068A0 File Offset: 0x00004AA0
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.RuleAmountBuff;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000141 RID: 321 RVA: 0x000068A4 File Offset: 0x00004AA4
	public override float KeyValue
	{
		get
		{
			return 25f;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000142 RID: 322 RVA: 0x000068AB File Offset: 0x00004AAB
	public override float BasicDuration
	{
		get
		{
			return 9999f;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000143 RID: 323 RVA: 0x000068B2 File Offset: 0x00004AB2
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000144 RID: 324 RVA: 0x000068B5 File Offset: 0x00004AB5
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x000068B8 File Offset: 0x00004AB8
	public override void Tick(float delta)
	{
		base.Tick(delta);
		this.timeCounter += delta;
		if (this.timeCounter > this.KeyValue)
		{
			this.timeCounter = 0f;
			this.Duplicate();
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000068F0 File Offset: 0x00004AF0
	private void Duplicate()
	{
		if (GameRes.EnemyRemain >= this.Target.MaxAmount)
		{
			return;
		}
		Singleton<GameManager>.Instance.SpawnEnemy(this.Target.EnemyType, this.Target.PointIndex, this.Target.Intensify * 0.5f * this.Target.DamageStrategy.HealthPercent, this.Target.DmgResist, BoardSystem.shortestPoints);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00006963 File Offset: 0x00004B63
	public override void Affect(int stacks)
	{
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00006965 File Offset: 0x00004B65
	public override void End()
	{
	}

	// Token: 0x04000117 RID: 279
	private float timeCounter;
}
