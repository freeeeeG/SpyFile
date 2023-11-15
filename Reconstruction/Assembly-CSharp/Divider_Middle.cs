using System;

// Token: 0x020000D4 RID: 212
public class Divider_Middle : Divider
{
	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000558 RID: 1368 RVA: 0x0000EAA8 File Offset: 0x0000CCA8
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Divider_Middle;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000559 RID: 1369 RVA: 0x0000EAAC File Offset: 0x0000CCAC
	protected override bool ShakeCam
	{
		get
		{
			return false;
		}
	}
}
