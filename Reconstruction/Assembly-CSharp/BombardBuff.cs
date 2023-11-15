using System;

// Token: 0x0200007D RID: 125
public class BombardBuff : GlobalSkill
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x0600030A RID: 778 RVA: 0x000097F4 File Offset: 0x000079F4
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.BombardBuff;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x0600030B RID: 779 RVA: 0x000097F8 File Offset: 0x000079F8
	public override float KeyValue
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x0600030C RID: 780 RVA: 0x000097FF File Offset: 0x000079FF
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00009808 File Offset: 0x00007A08
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Bombard)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		((BombardSkill)this.strategy.TurretSkills[0]).BulletCount += (int)this.KeyValue;
		((BombardSkill)this.strategy.TurretSkills[0]).BulletOffset *= 1f + this.KeyValue2;
	}
}
