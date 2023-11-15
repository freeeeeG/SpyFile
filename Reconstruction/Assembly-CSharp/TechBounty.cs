using System;

// Token: 0x020001B6 RID: 438
public class TechBounty : Technology
{
	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x0001D3A6 File Offset: 0x0001B5A6
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0001D3A9 File Offset: 0x0001B5A9
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHBOUNTY;
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0001D3AD File Offset: 0x0001B5AD
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0001D3B4 File Offset: 0x0001B5B4
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0001D3CF File Offset: 0x0001B5CF
	public override bool OnGet2()
	{
		ConstructHelper.GetRefactorTurretByNameAndElement("BOUNTY", 99, 99, 99);
		Singleton<GameManager>.Instance.TransitionToState(StateName.PickingState);
		return true;
	}
}
