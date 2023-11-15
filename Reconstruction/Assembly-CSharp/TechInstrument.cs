using System;

// Token: 0x020001AC RID: 428
public class TechInstrument : Technology
{
	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0001CDE0 File Offset: 0x0001AFE0
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06000B00 RID: 2816 RVA: 0x0001CDE3 File Offset: 0x0001AFE3
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHINSTRUMENT;
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0001CDE7 File Offset: 0x0001AFE7
	public override float KeyValue2
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0001CDF0 File Offset: 0x0001AFF0
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue2.ToString();
		}
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0001CE0B File Offset: 0x0001B00B
	public override bool OnGet2()
	{
		if (!this.IsAbnormal)
		{
			ConstructHelper.GetRefactorTurretByNameAndElement("PRISM", 99, 99, 99);
		}
		else
		{
			ConstructHelper.GetRefactorTurretByNameAndElement("AMPLIFIER", 99, 99, 99);
		}
		Singleton<GameManager>.Instance.TransitionToState(StateName.PickingState);
		return true;
	}
}
