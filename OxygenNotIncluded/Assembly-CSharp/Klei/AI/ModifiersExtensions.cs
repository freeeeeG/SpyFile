using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E06 RID: 3590
	public static class ModifiersExtensions
	{
		// Token: 0x06006E28 RID: 28200 RVA: 0x002B6352 File Offset: 0x002B4552
		public static Attributes GetAttributes(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetAttributes();
		}

		// Token: 0x06006E29 RID: 28201 RVA: 0x002B6360 File Offset: 0x002B4560
		public static Attributes GetAttributes(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.attributes;
			}
			return null;
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x002B6385 File Offset: 0x002B4585
		public static Amounts GetAmounts(this KMonoBehaviour cmp)
		{
			if (cmp is Modifiers)
			{
				return ((Modifiers)cmp).amounts;
			}
			return cmp.gameObject.GetAmounts();
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x002B63A8 File Offset: 0x002B45A8
		public static Amounts GetAmounts(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.amounts;
			}
			return null;
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x002B63CD File Offset: 0x002B45CD
		public static Sicknesses GetSicknesses(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetSicknesses();
		}

		// Token: 0x06006E2D RID: 28205 RVA: 0x002B63DC File Offset: 0x002B45DC
		public static Sicknesses GetSicknesses(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.sicknesses;
			}
			return null;
		}
	}
}
