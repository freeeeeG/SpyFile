using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public static class eDamageTypeExtensions
{
	// Token: 0x06000280 RID: 640 RVA: 0x0000A9CC File Offset: 0x00008BCC
	public static Color GetColor(this eDamageType damageType)
	{
		switch (damageType)
		{
		case eDamageType.FIRE:
			return new Color(1f, 0.3f, 0.25f);
		case eDamageType.ICE:
			return new Color(0.3f, 0.7f, 1f);
		case eDamageType.ELECTRIC:
			return new Color(1f, 0.9f, 0.2f);
		case eDamageType.EARTH:
			return new Color(0.9f, 0.65f, 0.3f);
		case eDamageType.POISON:
			return new Color(0.45f, 0.95f, 0.2f);
		case eDamageType.ARCANE:
			return new Color(0.8f, 0.4f, 1f);
		case eDamageType.NPC:
			return new Color(0.5f, 0.5f, 0.5f);
		}
		return Color.white;
	}
}
