using System;

// Token: 0x020000AC RID: 172
public class LaserSkill : InitialSkill
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000B9E9 File Offset: 0x00009BE9
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Laser;
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000B9ED File Offset: 0x00009BED
	public override float KeyValue
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000B9F4 File Offset: 0x00009BF4
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x0600043F RID: 1087 RVA: 0x0000BA1F File Offset: 0x00009C1F
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));
		}
	}
}
