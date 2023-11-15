using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class SuperSkill : InitialSkill
{
	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000AFE7 File Offset: 0x000091E7
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Super;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000AFEB File Offset: 0x000091EB
	public override float KeyValue
	{
		get
		{
			return (float)this.BounceTime;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000AFF4 File Offset: 0x000091F4
	public override float KeyValue2
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000AFFC File Offset: 0x000091FC
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000B028 File Offset: 0x00009228
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0000B054 File Offset: 0x00009254
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		((SuperBullet)bullet).BonuceTimes = Mathf.RoundToInt((float)((int)this.KeyValue) * bullet.BulletEffectIntensify);
		((SuperBullet)bullet).BounceSplashValue = this.KeyValue2 * bullet.BulletEffectIntensify;
	}

	// Token: 0x0400018F RID: 399
	public int BounceTime = 2;
}
