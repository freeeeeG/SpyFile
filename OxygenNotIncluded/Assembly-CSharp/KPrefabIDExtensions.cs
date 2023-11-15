using System;
using UnityEngine;

// Token: 0x0200083A RID: 2106
public static class KPrefabIDExtensions
{
	// Token: 0x06003D40 RID: 15680 RVA: 0x001544BA File Offset: 0x001526BA
	public static Tag PrefabID(this Component cmp)
	{
		return cmp.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06003D41 RID: 15681 RVA: 0x001544C7 File Offset: 0x001526C7
	public static Tag PrefabID(this GameObject go)
	{
		return go.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06003D42 RID: 15682 RVA: 0x001544D4 File Offset: 0x001526D4
	public static Tag PrefabID(this StateMachine.Instance smi)
	{
		return smi.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06003D43 RID: 15683 RVA: 0x001544E1 File Offset: 0x001526E1
	public static bool IsPrefabID(this Component cmp, Tag id)
	{
		return cmp.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06003D44 RID: 15684 RVA: 0x001544EF File Offset: 0x001526EF
	public static bool IsPrefabID(this GameObject go, Tag id)
	{
		return go.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06003D45 RID: 15685 RVA: 0x001544FD File Offset: 0x001526FD
	public static bool HasTag(this Component cmp, Tag tag)
	{
		return cmp.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06003D46 RID: 15686 RVA: 0x0015450B File Offset: 0x0015270B
	public static bool HasTag(this GameObject go, Tag tag)
	{
		return go.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06003D47 RID: 15687 RVA: 0x00154519 File Offset: 0x00152719
	public static bool HasAnyTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x06003D48 RID: 15688 RVA: 0x00154527 File Offset: 0x00152727
	public static bool HasAnyTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x06003D49 RID: 15689 RVA: 0x00154535 File Offset: 0x00152735
	public static bool HasAllTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x06003D4A RID: 15690 RVA: 0x00154543 File Offset: 0x00152743
	public static bool HasAllTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x06003D4B RID: 15691 RVA: 0x00154551 File Offset: 0x00152751
	public static void AddTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06003D4C RID: 15692 RVA: 0x00154560 File Offset: 0x00152760
	public static void AddTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x0015456F File Offset: 0x0015276F
	public static void RemoveTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().RemoveTag(tag);
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x0015457D File Offset: 0x0015277D
	public static void RemoveTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().RemoveTag(tag);
	}
}
