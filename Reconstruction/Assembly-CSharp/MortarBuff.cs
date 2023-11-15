using System;

// Token: 0x02000074 RID: 116
public class MortarBuff : GlobalSkill
{
	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060002DC RID: 732 RVA: 0x000093D7 File Offset: 0x000075D7
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.MortarBuff;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060002DD RID: 733 RVA: 0x000093DB File Offset: 0x000075DB
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x000093E2 File Offset: 0x000075E2
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (bullet.isCritical)
		{
			bullet.SplashRange *= 1f + this.KeyValue;
		}
	}
}
