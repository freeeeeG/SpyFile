using System;

// Token: 0x0200009D RID: 157
public class UltraSkill : InitialSkill
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060003CF RID: 975 RVA: 0x0000A9D4 File Offset: 0x00008BD4
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Ultra;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000A9D7 File Offset: 0x00008BD7
	public override float KeyValue
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000A9DE File Offset: 0x00008BDE
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000A9E8 File Offset: 0x00008BE8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000AA14 File Offset: 0x00008C14
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000AA3F File Offset: 0x00008C3F
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
		}
	}
}
