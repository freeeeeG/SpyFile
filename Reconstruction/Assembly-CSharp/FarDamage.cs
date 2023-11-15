using System;

// Token: 0x0200008C RID: 140
public class FarDamage : GlobalSkill
{
	// Token: 0x1700017E RID: 382
	// (get) Token: 0x0600034A RID: 842 RVA: 0x00009CC8 File Offset: 0x00007EC8
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.FarDamage;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x0600034B RID: 843 RVA: 0x00009CCC File Offset: 0x00007ECC
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00009CD4 File Offset: 0x00007ED4
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		if (bullet.GetTargetDistance() > 5f)
		{
			if (!this.intensify)
			{
				this.strategy.TurnFixDamageBonus += this.KeyValue;
				this.intensify = true;
				return;
			}
		}
		else if (this.intensify)
		{
			this.strategy.TurnFixDamageBonus -= this.KeyValue;
			this.intensify = false;
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00009D45 File Offset: 0x00007F45
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensify = false;
	}

	// Token: 0x04000172 RID: 370
	private bool intensify;
}
