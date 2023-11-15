using System;

// Token: 0x0200025A RID: 602
public class MinorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x06000C20 RID: 3104 RVA: 0x00044986 File Offset: 0x00042B86
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00044999 File Offset: 0x00042B99
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MinorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x000449B4 File Offset: 0x00042BB4
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x04000734 RID: 1844
	private MinorFossilDigSite.Instance digsite;
}
