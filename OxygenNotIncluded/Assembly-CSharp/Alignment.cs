using System;
using UnityEngine;

// Token: 0x02000B4B RID: 2891
public readonly struct Alignment
{
	// Token: 0x0600591D RID: 22813 RVA: 0x00209DC1 File Offset: 0x00207FC1
	public Alignment(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x0600591E RID: 22814 RVA: 0x00209DD1 File Offset: 0x00207FD1
	public static Alignment Custom(float x, float y)
	{
		return new Alignment(x, y);
	}

	// Token: 0x0600591F RID: 22815 RVA: 0x00209DDA File Offset: 0x00207FDA
	public static Alignment TopLeft()
	{
		return new Alignment(0f, 1f);
	}

	// Token: 0x06005920 RID: 22816 RVA: 0x00209DEB File Offset: 0x00207FEB
	public static Alignment Top()
	{
		return new Alignment(0.5f, 1f);
	}

	// Token: 0x06005921 RID: 22817 RVA: 0x00209DFC File Offset: 0x00207FFC
	public static Alignment TopRight()
	{
		return new Alignment(1f, 1f);
	}

	// Token: 0x06005922 RID: 22818 RVA: 0x00209E0D File Offset: 0x0020800D
	public static Alignment Left()
	{
		return new Alignment(0f, 0.5f);
	}

	// Token: 0x06005923 RID: 22819 RVA: 0x00209E1E File Offset: 0x0020801E
	public static Alignment Center()
	{
		return new Alignment(0.5f, 0.5f);
	}

	// Token: 0x06005924 RID: 22820 RVA: 0x00209E2F File Offset: 0x0020802F
	public static Alignment Right()
	{
		return new Alignment(1f, 0.5f);
	}

	// Token: 0x06005925 RID: 22821 RVA: 0x00209E40 File Offset: 0x00208040
	public static Alignment BottomLeft()
	{
		return new Alignment(0f, 0f);
	}

	// Token: 0x06005926 RID: 22822 RVA: 0x00209E51 File Offset: 0x00208051
	public static Alignment Bottom()
	{
		return new Alignment(0.5f, 0f);
	}

	// Token: 0x06005927 RID: 22823 RVA: 0x00209E62 File Offset: 0x00208062
	public static Alignment BottomRight()
	{
		return new Alignment(1f, 0f);
	}

	// Token: 0x06005928 RID: 22824 RVA: 0x00209E73 File Offset: 0x00208073
	public static implicit operator Vector2(Alignment a)
	{
		return new Vector2(a.x, a.y);
	}

	// Token: 0x06005929 RID: 22825 RVA: 0x00209E86 File Offset: 0x00208086
	public static implicit operator Alignment(Vector2 v)
	{
		return new Alignment(v.x, v.y);
	}

	// Token: 0x04003C4B RID: 15435
	public readonly float x;

	// Token: 0x04003C4C RID: 15436
	public readonly float y;
}
