using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class TeleportorSkill : InitialSkill
{
	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000B623 File Offset: 0x00009823
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Teleportor;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000B627 File Offset: 0x00009827
	public override float KeyValue
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000B630 File Offset: 0x00009830
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0000B65B File Offset: 0x0000985B
	public override void Build()
	{
		base.Build();
		this.HitList = new List<IDamage>();
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0000B670 File Offset: 0x00009870
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		if (target.DamageStrategy.IsEnemy && !this.HitList.Contains(target))
		{
			((Enemy)target).Flash(Mathf.RoundToInt((float)GameRes.CurrentWave / this.KeyValue * bullet.BulletEffectIntensify));
			this.HitList.Add(target);
		}
		return base.Hit(damage, target, bullet);
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0000B6D1 File Offset: 0x000098D1
	public override void EndTurn()
	{
		base.EndTurn();
		this.HitList.Clear();
	}

	// Token: 0x04000193 RID: 403
	private List<IDamage> HitList;
}
