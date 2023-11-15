using System;

// Token: 0x020000F3 RID: 243
public class GoldKeeper : Boss
{
	// Token: 0x17000288 RID: 648
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x0001087E File Offset: 0x0000EA7E
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.GoldKeeper;
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x00010882 File Offset: 0x0000EA82
	public override int MaxAmount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x0600061C RID: 1564 RVA: 0x00010885 File Offset: 0x0000EA85
	protected override bool ShakeCam
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00010888 File Offset: 0x0000EA88
	protected override void SetStrategy()
	{
		base.DamageStrategy = new GoldKeeperStrategy(this);
	}
}
