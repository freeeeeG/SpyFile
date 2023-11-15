using System;

// Token: 0x020000D5 RID: 213
public class Divider_Small : Divider
{
	// Token: 0x17000257 RID: 599
	// (get) Token: 0x0600055B RID: 1371 RVA: 0x0000EAB7 File Offset: 0x0000CCB7
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Divider_Small;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x0600055C RID: 1372 RVA: 0x0000EABB File Offset: 0x0000CCBB
	protected override bool ShakeCam
	{
		get
		{
			return false;
		}
	}
}
