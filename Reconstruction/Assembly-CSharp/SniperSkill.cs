using System;

// Token: 0x0200009B RID: 155
public class SniperSkill : InitialSkill
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060003C5 RID: 965 RVA: 0x0000A8FE File Offset: 0x00008AFE
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Sniper;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000A901 File Offset: 0x00008B01
	public override float KeyValue
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000A908 File Offset: 0x00008B08
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0000A943 File Offset: 0x00008B43
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		bullet.CriticalPercentage += this.KeyValue * bullet.GetTargetDistance() * bullet.BulletEffectIntensify;
	}
}
