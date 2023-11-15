using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class ClampValueModifier : ValueModifier
{
	// Token: 0x060003BC RID: 956 RVA: 0x000149D7 File Offset: 0x00012BD7
	public ClampValueModifier(int sortOrder, float min, float max) : base(sortOrder)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x000149EE File Offset: 0x00012BEE
	public override float Modify(float fromValue, float toValue)
	{
		return Mathf.Clamp(toValue, this.min, this.max);
	}

	// Token: 0x040001CD RID: 461
	public readonly float min;

	// Token: 0x040001CE RID: 462
	public readonly float max;
}
