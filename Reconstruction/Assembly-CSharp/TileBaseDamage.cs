using System;

// Token: 0x02000089 RID: 137
public class TileBaseDamage : GlobalSkill
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x0600033D RID: 829 RVA: 0x00009B95 File Offset: 0x00007D95
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TileBaseDamage;
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x0600033E RID: 830 RVA: 0x00009B99 File Offset: 0x00007D99
	public override float KeyValue
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00009BA0 File Offset: 0x00007DA0
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (target == null)
		{
			return;
		}
		bullet.BulletDamageIntensify += (float)target.DamageStrategy.PathIndex * this.KeyValue;
	}
}
