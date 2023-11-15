using System;

// Token: 0x0200009C RID: 156
public class ScatterSkill : InitialSkill
{
	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060003CA RID: 970 RVA: 0x0000A976 File Offset: 0x00008B76
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Scatter;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060003CB RID: 971 RVA: 0x0000A979 File Offset: 0x00008B79
	public override float KeyValue
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060003CC RID: 972 RVA: 0x0000A980 File Offset: 0x00008B80
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0000A9AB File Offset: 0x00008BAB
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixTargetCount += (int)this.KeyValue;
	}
}
