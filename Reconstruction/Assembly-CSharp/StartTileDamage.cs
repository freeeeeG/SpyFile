using System;

// Token: 0x0200008A RID: 138
public class StartTileDamage : GlobalSkill
{
	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000341 RID: 833 RVA: 0x00009BD6 File Offset: 0x00007DD6
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.StartTileDamage;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000342 RID: 834 RVA: 0x00009BDA File Offset: 0x00007DDA
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000343 RID: 835 RVA: 0x00009BE1 File Offset: 0x00007DE1
	public override float KeyValue2
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00009BE8 File Offset: 0x00007DE8
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (target == null)
		{
			return;
		}
		if ((float)target.DamageStrategy.PathIndex <= this.KeyValue2)
		{
			bullet.BulletDamageIntensify += this.KeyValue;
		}
	}
}
