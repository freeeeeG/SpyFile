using System;

// Token: 0x02000065 RID: 101
public class TechFireSkill : GlobalSkill
{
	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000284 RID: 644 RVA: 0x00008BC8 File Offset: 0x00006DC8
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechFireSkill;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000285 RID: 645 RVA: 0x00008BCB File Offset: 0x00006DCB
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000286 RID: 646 RVA: 0x00008BD2 File Offset: 0x00006DD2
	public override float KeyValue2
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000287 RID: 647 RVA: 0x00008BD9 File Offset: 0x00006DD9
	public override float KeyValue3
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00008BE0 File Offset: 0x00006DE0
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixRange += (this.IsAbnormal ? ((int)this.KeyValue3) : ((int)this.KeyValue));
		if (this.strategy.Concrete != null)
		{
			this.strategy.Concrete.GenerateRange();
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00008C40 File Offset: 0x00006E40
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		if (this.IsAbnormal)
		{
			if (bullet.GetTargetDistance() < 3.5f)
			{
				if (!this.detensified)
				{
					this.strategy.TurnFixDamageBonus -= this.KeyValue2;
					this.detensified = true;
					return;
				}
			}
			else if (this.detensified)
			{
				this.strategy.TurnFixDamageBonus += this.KeyValue2;
				this.detensified = false;
			}
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00008CB9 File Offset: 0x00006EB9
	public override void EndTurn()
	{
		base.EndTurn();
		this.detensified = false;
	}

	// Token: 0x0400016A RID: 362
	private bool detensified;
}
