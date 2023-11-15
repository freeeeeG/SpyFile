using System;

// Token: 0x020001C0 RID: 448
public class TechMark : Technology
{
	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHMARK;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0001D6EC File Offset: 0x0001B8EC
	public override string DisplayValue1
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0001D707 File Offset: 0x0001B907
	public override bool OnGet2()
	{
		ConstructHelper.GetTrapShapeByName("MARKTRAP");
		Singleton<GameManager>.Instance.TransitionToState(StateName.PickingState);
		return true;
	}
}
