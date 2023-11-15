using System;

// Token: 0x02000107 RID: 263
public struct TurretBuffInfo
{
	// Token: 0x06000690 RID: 1680 RVA: 0x00011CA8 File Offset: 0x0000FEA8
	public TurretBuffInfo(TurretBuffName buffName, int stacks, float duration)
	{
		this.BuffName = buffName;
		this.Stacks = stacks;
		this.Duration = duration;
	}

	// Token: 0x04000309 RID: 777
	public TurretBuffName BuffName;

	// Token: 0x0400030A RID: 778
	public int Stacks;

	// Token: 0x0400030B RID: 779
	public float Duration;
}
