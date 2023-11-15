using System;

// Token: 0x020001B5 RID: 437
public class TechAmplifier : Technology
{
	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0001D356 File Offset: 0x0001B556
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHAMPLIFIER;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06000B3B RID: 2875 RVA: 0x0001D35A File Offset: 0x0001B55A
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0001D364 File Offset: 0x0001B564
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x0001D37F File Offset: 0x0001B57F
	public override bool OnGet2()
	{
		ConstructHelper.GetRefactorTurretByNameAndElement("AMPLIFIER", 99, 99, 99);
		Singleton<GameManager>.Instance.TransitionToState(StateName.PickingState);
		return true;
	}
}
