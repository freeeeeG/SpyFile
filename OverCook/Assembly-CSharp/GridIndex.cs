using System;
using System.Collections.Generic;

// Token: 0x02000505 RID: 1285
public struct GridIndex : IEqualityComparer<GridIndex>
{
	// Token: 0x060017F1 RID: 6129 RVA: 0x0007A13B File Offset: 0x0007853B
	public GridIndex(int _x, int _y, int _z)
	{
		this.m_x = _x;
		this.m_y = _y;
		this.m_z = _z;
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x0007A152 File Offset: 0x00078552
	public static bool operator ==(GridIndex _token1, GridIndex _token2)
	{
		return _token1.X == _token2.X && _token1.Y == _token2.Y && _token1.Z == _token2.Z;
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x0007A18D File Offset: 0x0007858D
	public static bool operator !=(GridIndex _token1, GridIndex _token2)
	{
		return !(_token1 == _token2);
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x0007A199 File Offset: 0x00078599
	public static GridIndex operator +(GridIndex _token1, GridIndex _token2)
	{
		return new GridIndex(_token1.X + _token2.X, _token1.Y + _token2.Y, _token1.Z + _token2.Z);
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x0007A1D0 File Offset: 0x000785D0
	public override bool Equals(object obj)
	{
		if (obj.GetType() == base.GetType())
		{
			GridIndex gridIndex = (GridIndex)obj;
			return this.X == gridIndex.X && this.Y == this.Y && this.Z == gridIndex.Z;
		}
		return false;
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x0007A236 File Offset: 0x00078636
	public override int GetHashCode()
	{
		return this.m_x << 16 ^ this.m_y << 8 ^ this.m_z;
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x0007A251 File Offset: 0x00078651
	public bool Equals(GridIndex x, GridIndex y)
	{
		return x.GetHashCode() == y.GetHashCode();
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x0007A26F File Offset: 0x0007866F
	public int GetHashCode(GridIndex obj)
	{
		return obj.m_x << 16 ^ obj.m_y << 8 ^ this.m_z;
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060017F9 RID: 6137 RVA: 0x0007A28C File Offset: 0x0007868C
	public int X
	{
		get
		{
			return this.m_x;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060017FA RID: 6138 RVA: 0x0007A294 File Offset: 0x00078694
	public int Y
	{
		get
		{
			return this.m_y;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060017FB RID: 6139 RVA: 0x0007A29C File Offset: 0x0007869C
	public int Z
	{
		get
		{
			return this.m_z;
		}
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x0007A2A4 File Offset: 0x000786A4
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"(",
			this.m_x.ToString(),
			", ",
			this.m_y.ToString(),
			", ",
			this.m_z.ToString(),
			")"
		});
	}

	// Token: 0x04001359 RID: 4953
	private int m_x;

	// Token: 0x0400135A RID: 4954
	private int m_y;

	// Token: 0x0400135B RID: 4955
	private int m_z;
}
