using System;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public static class KSelectableExtensions
{
	// Token: 0x06001BE3 RID: 7139 RVA: 0x00094C2F File Offset: 0x00092E2F
	public static string GetProperName(this Component cmp)
	{
		if (cmp != null && cmp.gameObject != null)
		{
			return cmp.gameObject.GetProperName();
		}
		return "";
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x00094C5C File Offset: 0x00092E5C
	public static string GetProperName(this GameObject go)
	{
		if (go != null)
		{
			KSelectable component = go.GetComponent<KSelectable>();
			if (component != null)
			{
				return component.GetName();
			}
		}
		return "";
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x00094C8E File Offset: 0x00092E8E
	public static string GetProperName(this KSelectable cmp)
	{
		if (cmp != null)
		{
			return cmp.GetName();
		}
		return "";
	}
}
