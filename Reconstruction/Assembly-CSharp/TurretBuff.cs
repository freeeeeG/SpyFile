using System;

// Token: 0x020000B2 RID: 178
public abstract class TurretBuff
{
	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000461 RID: 1121
	public abstract TurretBuffName TBuffName { get; }

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000462 RID: 1122 RVA: 0x0000BF6C File Offset: 0x0000A16C
	// (set) Token: 0x06000463 RID: 1123 RVA: 0x0000BF74 File Offset: 0x0000A174
	public bool IsFinished { get; set; }

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000BF7D File Offset: 0x0000A17D
	public virtual float KeyValue { get; }

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000465 RID: 1125 RVA: 0x0000BF85 File Offset: 0x0000A185
	public virtual float KeyValue2 { get; }

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000BF8D File Offset: 0x0000A18D
	public virtual int MaxStacks
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0000BF90 File Offset: 0x0000A190
	public void ApplyBuff(StrategyBase strategy, int stacks, float duration)
	{
		this.TStrategy = strategy;
		this.Duration = duration;
		this.addStacks = 0;
		if (this.CurrentStack + stacks > this.MaxStacks)
		{
			this.addStacks = this.MaxStacks - this.CurrentStack;
		}
		else
		{
			this.addStacks = stacks;
		}
		this.CurrentStack += this.addStacks;
		this.Affect(this.addStacks);
	}

	// Token: 0x06000468 RID: 1128
	public abstract void Affect(int stacks);

	// Token: 0x06000469 RID: 1129 RVA: 0x0000BFFD File Offset: 0x0000A1FD
	public virtual void Tick(float delta)
	{
		this.Duration -= delta;
		if (this.Duration <= 0f)
		{
			this.End();
			this.IsFinished = true;
		}
	}

	// Token: 0x0600046A RID: 1130
	public abstract void End();

	// Token: 0x040001A9 RID: 425
	public int CurrentStack;

	// Token: 0x040001AA RID: 426
	public float Duration;

	// Token: 0x040001AB RID: 427
	public StrategyBase TStrategy;

	// Token: 0x040001AC RID: 428
	private int addStacks;
}
