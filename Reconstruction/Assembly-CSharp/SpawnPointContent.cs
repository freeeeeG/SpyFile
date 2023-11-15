using System;

// Token: 0x020001D7 RID: 471
public class SpawnPointContent : TrapContent
{
	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0001F80A File Offset: 0x0001DA0A
	public override GameTileContentType ContentType
	{
		get
		{
			return GameTileContentType.SpawnPoint;
		}
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x0001F80D File Offset: 0x0001DA0D
	protected override void Awake()
	{
		base.IsReveal = true;
		this.Important = true;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x0001F81D File Offset: 0x0001DA1D
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
	}
}
