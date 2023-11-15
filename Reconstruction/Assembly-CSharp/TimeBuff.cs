using System;

// Token: 0x02000029 RID: 41
public abstract class TimeBuff : EnemyBuff
{
	// Token: 0x060000D3 RID: 211 RVA: 0x00006230 File Offset: 0x00004430
	public override void ApplyBuff(Enemy target, int stacks, bool isAbnomral = false)
	{
		base.ApplyBuff(target, stacks, isAbnomral);
		this.Duration = this.BasicDuration;
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

	// Token: 0x060000D4 RID: 212 RVA: 0x000062A4 File Offset: 0x000044A4
	public override void Tick(float delta)
	{
		this.Duration -= delta;
		if (this.Duration <= 0f)
		{
			this.End();
			base.IsFinished = true;
		}
	}

	// Token: 0x0400010F RID: 271
	private int addStacks;
}
