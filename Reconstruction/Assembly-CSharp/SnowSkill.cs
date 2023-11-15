using System;

// Token: 0x020000A1 RID: 161
public class SnowSkill : InitialSkill
{
	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000ADA7 File Offset: 0x00008FA7
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Snow;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000ADAA File Offset: 0x00008FAA
	public override float KeyValue
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000ADB4 File Offset: 0x00008FB4
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000ADEF File Offset: 0x00008FEF
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));
		}
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0000AE0B File Offset: 0x0000900B
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		target.DamageStrategy.ApplyFrost(target.DamageStrategy.MaxFrost * this.KeyValue * bullet.BulletEffectIntensify);
		return base.Hit(damage, target, bullet);
	}
}
