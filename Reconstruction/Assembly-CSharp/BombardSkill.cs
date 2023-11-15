using System;

// Token: 0x020000AD RID: 173
public class BombardSkill : InitialSkill
{
	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000441 RID: 1089 RVA: 0x0000BA43 File Offset: 0x00009C43
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Bombard;
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000BA47 File Offset: 0x00009C47
	public override float KeyValue
	{
		get
		{
			return (float)this.BulletCount;
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000443 RID: 1091 RVA: 0x0000BA50 File Offset: 0x00009C50
	public override float KeyValue2
	{
		get
		{
			return this.BulletOffset;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000BA58 File Offset: 0x00009C58
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000BA84 File Offset: 0x00009C84
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000BABF File Offset: 0x00009CBF
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0000BADC File Offset: 0x00009CDC
	public override void StartTurn()
	{
		base.StartTurn();
		if (!this.triggered)
		{
			((BomBard)this.strategy.Concrete).BulletCount = (int)this.KeyValue;
			((BomBard)this.strategy.Concrete).BulletOffset = this.KeyValue2;
			this.triggered = true;
		}
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0000BB35 File Offset: 0x00009D35
	public override void EndTurn()
	{
		base.EndTurn();
		this.triggered = false;
	}

	// Token: 0x04000198 RID: 408
	public int BulletCount = 3;

	// Token: 0x04000199 RID: 409
	public float BulletOffset = 1.2f;

	// Token: 0x0400019A RID: 410
	private bool triggered;
}
