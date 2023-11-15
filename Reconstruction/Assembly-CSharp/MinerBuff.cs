using System;

// Token: 0x0200007E RID: 126
public class MinerBuff : GlobalSkill
{
	// Token: 0x17000160 RID: 352
	// (get) Token: 0x0600030F RID: 783 RVA: 0x000098A1 File Offset: 0x00007AA1
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.MinerBuff;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000310 RID: 784 RVA: 0x000098A5 File Offset: 0x00007AA5
	public override float KeyValue
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000311 RID: 785 RVA: 0x000098AC File Offset: 0x00007AAC
	public override float KeyValue2
	{
		get
		{
			return 8f;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000312 RID: 786 RVA: 0x000098B3 File Offset: 0x00007AB3
	public override float KeyValue3
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x000098BA File Offset: 0x00007ABA
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Miner)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
	}

	// Token: 0x06000314 RID: 788 RVA: 0x000098E9 File Offset: 0x00007AE9
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		((MinerBullet)bullet).AttackIncreasePerSecond = this.KeyValue;
		((MinerBullet)bullet).MaxAttackIncrease = this.KeyValue2;
	}
}
