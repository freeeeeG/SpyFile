using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public static class MaterialExtensions
{
	// Token: 0x06000620 RID: 1568 RVA: 0x00017878 File Offset: 0x00015A78
	public static bool HasAllProperty(this Material material, params string[] props)
	{
		bool result = true;
		for (int i = 0; i < props.Length; i++)
		{
			if (!material.HasProperty(props[i]))
			{
				result = false;
				break;
			}
		}
		return result;
	}
}
