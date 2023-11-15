using System;

// Token: 0x02000199 RID: 409
public class TechMaster : Technology
{
	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0001C089 File Offset: 0x0001A289
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHMASTER;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0001C08C File Offset: 0x0001A28C
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0001C094 File Offset: 0x0001A294
	public override string DisplayValue1
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0001C0AF File Offset: 0x0001A2AF
	public override void OnGet()
	{
		base.OnGet();
		GameRes.LockCount += (int)this.KeyValue;
	}
}
