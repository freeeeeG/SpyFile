using System;

// Token: 0x0200023B RID: 571
public class MajorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x06000B7E RID: 2942 RVA: 0x000409FE File Offset: 0x0003EBFE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00040A11 File Offset: 0x0003EC11
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MajorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00040A2C File Offset: 0x0003EC2C
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x040006B9 RID: 1721
	private MajorFossilDigSite.Instance digsite;
}
