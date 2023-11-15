using System;

// Token: 0x02000075 RID: 117
public class SniperBuff : GlobalSkill
{
	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x00009415 File Offset: 0x00007615
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.SniperBuff;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x00009419 File Offset: 0x00007619
	public override float KeyValue
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060002E2 RID: 738 RVA: 0x00009420 File Offset: 0x00007620
	public override float KeyValue2
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060002E3 RID: 739 RVA: 0x00009427 File Offset: 0x00007627
	public override float KeyValue3
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0000942E File Offset: 0x0000762E
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (bullet.GetTargetDistance() > this.KeyValue)
		{
			bullet.isCritical = true;
		}
	}
}
