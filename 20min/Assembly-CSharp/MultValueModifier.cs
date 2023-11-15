using System;

// Token: 0x02000032 RID: 50
public class MultValueModifier : ValueModifier
{
	// Token: 0x060003BF RID: 959 RVA: 0x00014A11 File Offset: 0x00012C11
	public MultValueModifier(int sortOrder, float toMultiply) : base(sortOrder)
	{
		this.toMultiply = toMultiply;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00014A21 File Offset: 0x00012C21
	public override float Modify(float fromValue, float toValue)
	{
		return fromValue * this.toMultiply - fromValue + toValue;
	}

	// Token: 0x040001D0 RID: 464
	public readonly float toMultiply;
}
