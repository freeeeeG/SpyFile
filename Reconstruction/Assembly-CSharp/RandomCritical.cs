using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class RandomCritical : ElementSkill
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000204 RID: 516 RVA: 0x00007A64 File Offset: 0x00005C64
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				3,
				3,
				3
			};
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000205 RID: 517 RVA: 0x00007A80 File Offset: 0x00005C80
	public override float KeyValue
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000206 RID: 518 RVA: 0x00007A87 File Offset: 0x00005C87
	public override float KeyValue2
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000207 RID: 519 RVA: 0x00007A90 File Offset: 0x00005C90
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06000208 RID: 520 RVA: 0x00007ACC File Offset: 0x00005CCC
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00007B07 File Offset: 0x00005D07
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		bullet.DamageAdjust = Random.Range(this.KeyValue * bullet.BulletEffectIntensify, this.KeyValue2 * bullet.BulletEffectIntensify);
	}
}
