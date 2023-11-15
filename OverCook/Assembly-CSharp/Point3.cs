using System;

// Token: 0x02000268 RID: 616
[Serializable]
public class Point3
{
	// Token: 0x06000B5E RID: 2910 RVA: 0x0003CE57 File Offset: 0x0003B257
	public Point3(int _x, int _y, int _z)
	{
		this.X = _x;
		this.Y = _y;
		this.Z = _z;
	}

	// Token: 0x040008D7 RID: 2263
	public int X;

	// Token: 0x040008D8 RID: 2264
	public int Y;

	// Token: 0x040008D9 RID: 2265
	public int Z;
}
