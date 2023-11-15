using System;

// Token: 0x02000078 RID: 120
public class UltraBuff : GlobalSkill
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x060002EE RID: 750 RVA: 0x00009537 File Offset: 0x00007737
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.UltraBuff;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x060002EF RID: 751 RVA: 0x0000953B File Offset: 0x0000773B
	public override float KeyValue
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00009542 File Offset: 0x00007742
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Ultra)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00009570 File Offset: 0x00007770
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		((UltraBullet)bullet).SplashIncreasePerSecond = this.KeyValue;
	}
}
