using System;

// Token: 0x020000A0 RID: 160
public class ConstructorSkill : InitialSkill
{
	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000AC74 File Offset: 0x00008E74
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Constructor;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000AC77 File Offset: 0x00008E77
	public override float KeyValue
	{
		get
		{
			return this.ChargedTime;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000AC7F File Offset: 0x00008E7F
	public override float KeyValue2
	{
		get
		{
			return this.IntentValue;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000AC88 File Offset: 0x00008E88
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000ACB4 File Offset: 0x00008EB4
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000ACEF File Offset: 0x00008EEF
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0000AD0B File Offset: 0x00008F0B
	public override void StartTurn()
	{
		base.StartTurn();
		this.charged = false;
		this.Duration = this.KeyValue;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0000AD26 File Offset: 0x00008F26
	public override void TickEnd()
	{
		base.TickEnd();
		this.charged = true;
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0000AD38 File Offset: 0x00008F38
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (this.charged)
		{
			bullet.AttackIntensify += this.KeyValue2 * bullet.BulletEffectIntensify;
			this.charged = false;
			this.Duration = this.KeyValue;
			this.IsFinish = false;
		}
	}

	// Token: 0x0400018A RID: 394
	private bool charged;

	// Token: 0x0400018B RID: 395
	public float ChargedTime = 3f;

	// Token: 0x0400018C RID: 396
	public float IntentValue = 1f;
}
