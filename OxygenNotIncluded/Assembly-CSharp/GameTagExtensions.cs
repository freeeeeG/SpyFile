using System;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
public static class GameTagExtensions
{
	// Token: 0x060037DF RID: 14303 RVA: 0x00134603 File Offset: 0x00132803
	public static GameObject Prefab(this Tag tag)
	{
		return Assets.GetPrefab(tag);
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x0013460B File Offset: 0x0013280B
	public static string ProperName(this Tag tag)
	{
		return TagManager.GetProperName(tag, false);
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x00134614 File Offset: 0x00132814
	public static string ProperNameStripLink(this Tag tag)
	{
		return TagManager.GetProperName(tag, true);
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x0013461D File Offset: 0x0013281D
	public static Tag Create(SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x00134631 File Offset: 0x00132831
	public static Tag CreateTag(this SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}
}
