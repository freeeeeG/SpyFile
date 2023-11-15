using System;

// Token: 0x020001B4 RID: 436
public class TechPrism : Technology
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0001D308 File Offset: 0x0001B508
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHPRISM;
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0001D30C File Offset: 0x0001B50C
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0001D314 File Offset: 0x0001B514
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x0001D32F File Offset: 0x0001B52F
	public override bool OnGet2()
	{
		ConstructHelper.GetRefactorTurretByNameAndElement("PRISM", 99, 99, 99);
		Singleton<GameManager>.Instance.TransitionToState(StateName.PickingState);
		return true;
	}
}
