using System;

// Token: 0x020003EE RID: 1006
public struct NavOffset
{
	// Token: 0x06001530 RID: 5424 RVA: 0x000704DC File Offset: 0x0006E6DC
	public NavOffset(NavType nav_type, int x, int y)
	{
		this.navType = nav_type;
		this.offset.x = x;
		this.offset.y = y;
	}

	// Token: 0x04000B8D RID: 2957
	public NavType navType;

	// Token: 0x04000B8E RID: 2958
	public CellOffset offset;
}
