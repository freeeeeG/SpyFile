using System;

// Token: 0x0200007C RID: 124
public class LaserBuff : GlobalSkill
{
	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000305 RID: 773 RVA: 0x00009796 File Offset: 0x00007996
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.LaserBuff;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000306 RID: 774 RVA: 0x0000979A File Offset: 0x0000799A
	public override float KeyValue
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x000097A1 File Offset: 0x000079A1
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Laser)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x000097D0 File Offset: 0x000079D0
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		((LaserBullet)bullet).SetTravelIncrease(true, this.KeyValue);
	}
}
