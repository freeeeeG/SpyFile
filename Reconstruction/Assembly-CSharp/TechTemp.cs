using System;

// Token: 0x0200019E RID: 414
public class TechTemp : Technology
{
	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0001C38A File Offset: 0x0001A58A
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0001C38D File Offset: 0x0001A58D
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHTEMP;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0001C391 File Offset: 0x0001A591
	public override float KeyValue
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0001C398 File Offset: 0x0001A598
	public override float KeyValue2
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0001C39F File Offset: 0x0001A59F
	public override float KeyValue3
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0001C3A8 File Offset: 0x0001A5A8
	public override string DisplayValue1
	{
		get
		{
			return (this.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0001C3D4 File Offset: 0x0001A5D4
	public override string DisplayValue2
	{
		get
		{
			return (this.KeyValue2 * 100f).ToString() + "%";
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0001C400 File Offset: 0x0001A600
	public override string DisplayValue3
	{
		get
		{
			return (this.KeyValue * 100f).ToString() + "%";
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0001C42C File Offset: 0x0001A62C
	public override void OnGet()
	{
		base.OnGet();
		if (this.IsAbnormal)
		{
			GameRes.EnemyFrostResist -= this.KeyValue3;
			GameRes.TurretFrostResist -= this.KeyValue2;
			return;
		}
		GameRes.TurretFrostResist += this.KeyValue;
	}
}
