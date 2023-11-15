using System;

// Token: 0x0200038E RID: 910
public readonly struct Padding
{
	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060012DB RID: 4827 RVA: 0x000646FD File Offset: 0x000628FD
	public float Width
	{
		get
		{
			return this.left + this.right;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060012DC RID: 4828 RVA: 0x0006470C File Offset: 0x0006290C
	public float Height
	{
		get
		{
			return this.top + this.bottom;
		}
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x0006471B File Offset: 0x0006291B
	public Padding(float left, float right, float top, float bottom)
	{
		this.top = top;
		this.bottom = bottom;
		this.left = left;
		this.right = right;
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x0006473A File Offset: 0x0006293A
	public static Padding All(float padding)
	{
		return new Padding(padding, padding, padding, padding);
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00064745 File Offset: 0x00062945
	public static Padding Symmetric(float horizontal, float vertical)
	{
		return new Padding(horizontal, horizontal, vertical, vertical);
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x00064750 File Offset: 0x00062950
	public static Padding Only(float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
	{
		return new Padding(left, right, top, bottom);
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x0006475B File Offset: 0x0006295B
	public static Padding Vertical(float vertical)
	{
		return new Padding(0f, 0f, vertical, vertical);
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x0006476E File Offset: 0x0006296E
	public static Padding Horizontal(float horizontal)
	{
		return new Padding(horizontal, horizontal, 0f, 0f);
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x00064781 File Offset: 0x00062981
	public static Padding Top(float amount)
	{
		return new Padding(0f, 0f, amount, 0f);
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x00064798 File Offset: 0x00062998
	public static Padding Left(float amount)
	{
		return new Padding(amount, 0f, 0f, 0f);
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x000647AF File Offset: 0x000629AF
	public static Padding Bottom(float amount)
	{
		return new Padding(0f, 0f, 0f, amount);
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x000647C6 File Offset: 0x000629C6
	public static Padding Right(float amount)
	{
		return new Padding(0f, amount, 0f, 0f);
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x000647DD File Offset: 0x000629DD
	public static Padding operator +(Padding a, Padding b)
	{
		return new Padding(a.left + b.left, a.right + b.right, a.top + b.top, a.bottom + b.bottom);
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x00064818 File Offset: 0x00062A18
	public static Padding operator -(Padding a, Padding b)
	{
		return new Padding(a.left - b.left, a.right - b.right, a.top - b.top, a.bottom - b.bottom);
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x00064853 File Offset: 0x00062A53
	public static Padding operator *(float f, Padding p)
	{
		return p * f;
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x0006485C File Offset: 0x00062A5C
	public static Padding operator *(Padding p, float f)
	{
		return new Padding(p.left * f, p.right * f, p.top * f, p.bottom * f);
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x00064883 File Offset: 0x00062A83
	public static Padding operator /(Padding p, float f)
	{
		return new Padding(p.left / f, p.right / f, p.top / f, p.bottom / f);
	}

	// Token: 0x04000A3F RID: 2623
	public readonly float top;

	// Token: 0x04000A40 RID: 2624
	public readonly float bottom;

	// Token: 0x04000A41 RID: 2625
	public readonly float left;

	// Token: 0x04000A42 RID: 2626
	public readonly float right;
}
