using System;

// Token: 0x020000A6 RID: 166
public class PrismSkill : InitialSkill
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000B24F File Offset: 0x0000944F
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Prism;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000B253 File Offset: 0x00009453
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000B25C File Offset: 0x0000945C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}
}
