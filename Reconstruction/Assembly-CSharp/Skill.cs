using System;

// Token: 0x020000FC RID: 252
public abstract class Skill
{
	// Token: 0x06000664 RID: 1636 RVA: 0x0001154C File Offset: 0x0000F74C
	public virtual void OnBorn()
	{
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x0001154E File Offset: 0x0000F74E
	public virtual void OnGameUpdating()
	{
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00011550 File Offset: 0x0000F750
	public virtual void OnDying()
	{
	}

	// Token: 0x040002D0 RID: 720
	protected Enemy enemy;
}
