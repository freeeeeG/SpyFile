using System;

// Token: 0x020000AF RID: 175
public class NuclearSkill : InitialSkill
{
	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000BCFB File Offset: 0x00009EFB
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Nuclear;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000453 RID: 1107 RVA: 0x0000BCFF File Offset: 0x00009EFF
	public override float KeyValue
	{
		get
		{
			return this.ChargedTime;
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000BD07 File Offset: 0x00009F07
	public override float KeyValue2
	{
		get
		{
			return this.IntentValue;
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000455 RID: 1109 RVA: 0x0000BD10 File Offset: 0x00009F10
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000BD3C File Offset: 0x00009F3C
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000BD77 File Offset: 0x00009F77
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0000BD93 File Offset: 0x00009F93
	public override void StartTurn()
	{
		base.StartTurn();
		this.charged = false;
		this.Duration = this.KeyValue;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0000BDAE File Offset: 0x00009FAE
	public override void TickEnd()
	{
		base.TickEnd();
		this.charged = true;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0000BDBD File Offset: 0x00009FBD
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		if (this.charged)
		{
			bullet.BulletEffectIntensify += this.IntentValue;
			this.charged = false;
			this.IsFinish = false;
			this.Duration = this.KeyValue;
		}
	}

	// Token: 0x0400019D RID: 413
	private bool charged;

	// Token: 0x0400019E RID: 414
	public float ChargedTime = 10f;

	// Token: 0x0400019F RID: 415
	public float IntentValue = 2.5f;
}
