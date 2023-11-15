using System;
using UnityEngine;

// Token: 0x0200027E RID: 638
[AttributeUsage(AttributeTargets.Field)]
public class MaskAttribute : PropertyAttribute
{
	// Token: 0x06000BCF RID: 3023 RVA: 0x0003DC22 File Offset: 0x0003C022
	public MaskAttribute(Type _enumType)
	{
		this.EnumType = _enumType;
	}

	// Token: 0x04000902 RID: 2306
	public Type EnumType;
}
