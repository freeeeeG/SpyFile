using System;

// Token: 0x020000A3 RID: 163
public class BoomerrangSkill : InitialSkill
{
	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000AF7A File Offset: 0x0000917A
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Boomerrang;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000AF7E File Offset: 0x0000917E
	public override float KeyValue
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000AF88 File Offset: 0x00009188
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000AFC3 File Offset: 0x000091C3
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));
		}
	}
}
