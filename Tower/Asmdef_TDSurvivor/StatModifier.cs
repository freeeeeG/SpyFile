using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
[Serializable]
public class StatModifier
{
	// Token: 0x06000788 RID: 1928 RVA: 0x0001C9E6 File Offset: 0x0001ABE6
	public StatModifier(eModifierType type, float value)
	{
		this.ModifierType = type;
		this.Value = value;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0001C9FC File Offset: 0x0001ABFC
	public float ApplyModifier(float originalValue)
	{
		if (this.ModifierType == eModifierType.ADD)
		{
			originalValue += this.Value;
		}
		else if (this.ModifierType == eModifierType.MULTIPLY)
		{
			originalValue *= this.Value;
		}
		return Mathf.Max(originalValue, 0f);
	}

	// Token: 0x04000618 RID: 1560
	public eModifierType ModifierType;

	// Token: 0x04000619 RID: 1561
	public float Value;
}
