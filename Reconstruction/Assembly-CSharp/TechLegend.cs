using System;

// Token: 0x0200019C RID: 412
public class TechLegend : Technology
{
	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0001C258 File Offset: 0x0001A458
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0001C25B File Offset: 0x0001A45B
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHLEGEND;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0001C25E File Offset: 0x0001A45E
	public override float KeyValue2
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0001C268 File Offset: 0x0001A468
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue2.ToString();
		}
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0001C283 File Offset: 0x0001A483
	public override bool OnGet2()
	{
		if (!this.IsAbnormal)
		{
			ConstructHelper.GetRefactorTurretByNameAndElement("BOUNTY", 99, 99, 99);
		}
		else
		{
			ConstructHelper.GetRefactorTurretByNameAndElement("TELEPORTOR", 99, 99, 99);
		}
		Singleton<GameManager>.Instance.TransitionToState(StateName.PickingState);
		return true;
	}
}
