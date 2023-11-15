using System;

// Token: 0x02000033 RID: 51
public abstract class ValueModifier : Modifier
{
	// Token: 0x060003C1 RID: 961 RVA: 0x00014A2F File Offset: 0x00012C2F
	public ValueModifier(int sortOrder) : base(sortOrder)
	{
	}

	// Token: 0x060003C2 RID: 962
	public abstract float Modify(float fromValue, float toValue);
}
