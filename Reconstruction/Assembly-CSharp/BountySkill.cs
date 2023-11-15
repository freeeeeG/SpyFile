using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class BountySkill : InitialSkill
{
	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000B6EC File Offset: 0x000098EC
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Bounty;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000B6F0 File Offset: 0x000098F0
	public override float KeyValue
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000B6F7 File Offset: 0x000098F7
	public override float KeyValue2
	{
		get
		{
			return 50f;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000B700 File Offset: 0x00009900
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000B72C File Offset: 0x0000992C
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0000B757 File Offset: 0x00009957
	public override void StartTurn()
	{
		base.StartTurn();
		if (!this.triggered)
		{
			this.initMaxFirerate = this.strategy.MaxFireRate;
			this.shootCount = 0;
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0000B780 File Offset: 0x00009980
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		this.shootCount++;
		if (this.shootCount >= (int)this.KeyValue)
		{
			this.strategy.MaxFireRate = 0f;
		}
		Singleton<StaticData>.Instance.GainMoneyEffect(this.strategy.Concrete.transform.position, Mathf.RoundToInt((this.KeyValue2 + (float)GameRes.CurrentWave) * bullet.BulletEffectIntensify));
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0000B800 File Offset: 0x00009A00
	public override void EndTurn()
	{
		base.EndTurn();
		this.strategy.MaxFireRate = this.initMaxFirerate;
		this.shootCount = 0;
		this.triggered = false;
	}

	// Token: 0x04000194 RID: 404
	private bool triggered;

	// Token: 0x04000195 RID: 405
	private int shootCount;

	// Token: 0x04000196 RID: 406
	private float initMaxFirerate;
}
