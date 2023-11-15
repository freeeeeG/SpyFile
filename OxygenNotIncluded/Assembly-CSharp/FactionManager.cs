using System;
using UnityEngine;

// Token: 0x020006E6 RID: 1766
[AddComponentMenu("KMonoBehaviour/scripts/FactionManager")]
public class FactionManager : KMonoBehaviour
{
	// Token: 0x0600306E RID: 12398 RVA: 0x000FFF2E File Offset: 0x000FE12E
	public static void DestroyInstance()
	{
		FactionManager.Instance = null;
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x000FFF36 File Offset: 0x000FE136
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		FactionManager.Instance = this;
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000FFF44 File Offset: 0x000FE144
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x000FFF4C File Offset: 0x000FE14C
	public Faction GetFaction(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			return this.Duplicant;
		case FactionManager.FactionID.Friendly:
			return this.Friendly;
		case FactionManager.FactionID.Hostile:
			return this.Hostile;
		case FactionManager.FactionID.Prey:
			return this.Prey;
		case FactionManager.FactionID.Predator:
			return this.Predator;
		case FactionManager.FactionID.Pest:
			return this.Pest;
		default:
			return null;
		}
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x000FFFA4 File Offset: 0x000FE1A4
	public FactionManager.Disposition GetDisposition(FactionManager.FactionID of_faction, FactionManager.FactionID to_faction)
	{
		if (FactionManager.Instance.GetFaction(of_faction).Dispositions.ContainsKey(to_faction))
		{
			return FactionManager.Instance.GetFaction(of_faction).Dispositions[to_faction];
		}
		return FactionManager.Disposition.Neutral;
	}

	// Token: 0x04001C98 RID: 7320
	public static FactionManager Instance;

	// Token: 0x04001C99 RID: 7321
	public Faction Duplicant = new Faction(FactionManager.FactionID.Duplicant);

	// Token: 0x04001C9A RID: 7322
	public Faction Friendly = new Faction(FactionManager.FactionID.Friendly);

	// Token: 0x04001C9B RID: 7323
	public Faction Hostile = new Faction(FactionManager.FactionID.Hostile);

	// Token: 0x04001C9C RID: 7324
	public Faction Predator = new Faction(FactionManager.FactionID.Predator);

	// Token: 0x04001C9D RID: 7325
	public Faction Prey = new Faction(FactionManager.FactionID.Prey);

	// Token: 0x04001C9E RID: 7326
	public Faction Pest = new Faction(FactionManager.FactionID.Pest);

	// Token: 0x02001430 RID: 5168
	public enum FactionID
	{
		// Token: 0x04006499 RID: 25753
		Duplicant,
		// Token: 0x0400649A RID: 25754
		Friendly,
		// Token: 0x0400649B RID: 25755
		Hostile,
		// Token: 0x0400649C RID: 25756
		Prey,
		// Token: 0x0400649D RID: 25757
		Predator,
		// Token: 0x0400649E RID: 25758
		Pest,
		// Token: 0x0400649F RID: 25759
		NumberOfFactions
	}

	// Token: 0x02001431 RID: 5169
	public enum Disposition
	{
		// Token: 0x040064A1 RID: 25761
		Assist,
		// Token: 0x040064A2 RID: 25762
		Neutral,
		// Token: 0x040064A3 RID: 25763
		Attack
	}
}
