using System;

// Token: 0x0200002F RID: 47
public class AddValueModifier : ValueModifier
{
	// Token: 0x060003BA RID: 954 RVA: 0x000149BD File Offset: 0x00012BBD
	public AddValueModifier(int sortOrder, float toAdd) : base(sortOrder)
	{
		this.toAdd = toAdd;
	}

	// Token: 0x060003BB RID: 955 RVA: 0x000149CD File Offset: 0x00012BCD
	public override float Modify(float fromValue, float toValue)
	{
		return toValue + this.toAdd;
	}

	// Token: 0x040001CC RID: 460
	public readonly float toAdd;
}
