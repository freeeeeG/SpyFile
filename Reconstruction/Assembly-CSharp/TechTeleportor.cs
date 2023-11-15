using System;

// Token: 0x020001B7 RID: 439
public class TechTeleportor : Technology
{
	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0001D3F6 File Offset: 0x0001B5F6
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0001D3F9 File Offset: 0x0001B5F9
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHTELEPORTOR;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0001D3FD File Offset: 0x0001B5FD
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0001D404 File Offset: 0x0001B604
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x0001D41F File Offset: 0x0001B61F
	public override bool OnGet2()
	{
		ConstructHelper.GetRefactorTurretByNameAndElement("TELEPORTOR", 99, 99, 99);
		Singleton<GameManager>.Instance.TransitionToState(StateName.PickingState);
		return true;
	}
}
