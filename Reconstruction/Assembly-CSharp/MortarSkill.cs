using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class MortarSkill : InitialSkill
{
	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060003D6 RID: 982 RVA: 0x0000AA63 File Offset: 0x00008C63
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Mortar;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000AA66 File Offset: 0x00008C66
	public override float KeyValue
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000AA6D File Offset: 0x00008C6D
	public override float KeyValue2
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000AA74 File Offset: 0x00008C74
	public override float KeyValue3
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060003DA RID: 986 RVA: 0x0000AA7C File Offset: 0x00008C7C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060003DB RID: 987 RVA: 0x0000AAB8 File Offset: 0x00008CB8
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060003DC RID: 988 RVA: 0x0000AAF3 File Offset: 0x00008CF3
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060003DD RID: 989 RVA: 0x0000AB10 File Offset: 0x00008D10
	public override string DisplayValue4
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue3 * 100f).ToString() + "%");
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0000AB4B File Offset: 0x00008D4B
	public override void PreHit(Bullet bullet = null)
	{
		base.PreHit(bullet);
		bullet.SplashPercentage += Mathf.Min(this.KeyValue3, (float)bullet.HitSize * this.KeyValue2 * bullet.BulletEffectIntensify);
	}
}
