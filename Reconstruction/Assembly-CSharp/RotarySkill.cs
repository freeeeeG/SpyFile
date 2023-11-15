using System;

// Token: 0x0200009A RID: 154
public class RotarySkill : InitialSkill
{
	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x060003BC RID: 956 RVA: 0x0000A815 File Offset: 0x00008A15
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Rotary;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x060003BD RID: 957 RVA: 0x0000A818 File Offset: 0x00008A18
	public override float KeyValue
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x060003BE RID: 958 RVA: 0x0000A81F File Offset: 0x00008A1F
	private float maxValue
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x060003BF RID: 959 RVA: 0x0000A826 File Offset: 0x00008A26
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000A82D File Offset: 0x00008A2D
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000A84C File Offset: 0x00008A4C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000A888 File Offset: 0x00008A88
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.maxValue.ToString());
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000A8B4 File Offset: 0x00008AB4
	public override string DisplayValue4
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * this.KeyValue * 100f).ToString() + "%");
		}
	}
}
