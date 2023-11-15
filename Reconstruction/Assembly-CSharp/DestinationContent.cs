using System;

// Token: 0x020001D3 RID: 467
public class DestinationContent : TrapContent
{
	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
	public override int DieProtect
	{
		get
		{
			return GameRes.DieProtect;
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0001F4DF File Offset: 0x0001D6DF
	public override GameTileContentType ContentType
	{
		get
		{
			return GameTileContentType.Destination;
		}
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x0001F4E2 File Offset: 0x0001D6E2
	protected override void Awake()
	{
		base.IsReveal = true;
		this.Important = true;
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x0001F4F2 File Offset: 0x0001D6F2
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
	}
}
