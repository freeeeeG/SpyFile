using System;

// Token: 0x0200008D RID: 141
public class CloseDamage : GlobalSkill
{
	// Token: 0x17000180 RID: 384
	// (get) Token: 0x0600034F RID: 847 RVA: 0x00009D5C File Offset: 0x00007F5C
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.CloseDamage;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000350 RID: 848 RVA: 0x00009D60 File Offset: 0x00007F60
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00009D68 File Offset: 0x00007F68
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		if (bullet.GetTargetDistance() < 5f)
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

	// Token: 0x06000352 RID: 850 RVA: 0x00009DD9 File Offset: 0x00007FD9
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensify = false;
	}

	// Token: 0x04000173 RID: 371
	private bool intensify;
}
