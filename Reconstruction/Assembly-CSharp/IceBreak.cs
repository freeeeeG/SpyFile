using System;

// Token: 0x02000067 RID: 103
public class IceBreak : GlobalSkill
{
	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000294 RID: 660 RVA: 0x00008D9D File Offset: 0x00006F9D
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.IceBreak;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000295 RID: 661 RVA: 0x00008DA0 File Offset: 0x00006FA0
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00008DA7 File Offset: 0x00006FA7
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (target == null)
		{
			return;
		}
		if (target.DamageStrategy.IsFrost)
		{
			bullet.BulletDamageIntensify += this.KeyValue;
		}
	}
}
