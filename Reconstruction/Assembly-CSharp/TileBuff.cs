using System;

// Token: 0x0200002A RID: 42
public abstract class TileBuff : EnemyBuff
{
	// Token: 0x060000D6 RID: 214 RVA: 0x000062D6 File Offset: 0x000044D6
	public override void ApplyBuff(Enemy target, int stacks, bool isAbnomral = false)
	{
		base.ApplyBuff(target, stacks, isAbnomral);
		this.TileCount = (int)this.BasicDuration;
		this.PrivateStacks = stacks;
		this.Affect(stacks);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x000062FC File Offset: 0x000044FC
	public override void Tick(float delta)
	{
		this.TileCount -= (int)delta;
		if (this.TileCount <= 0)
		{
			this.End();
			base.IsFinished = true;
			return;
		}
	}

	// Token: 0x04000110 RID: 272
	public int TileCount;

	// Token: 0x04000111 RID: 273
	public int PrivateStacks;
}
