using System;

// Token: 0x0200006F RID: 111
public class RapiderBuff : GlobalSkill
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060002BD RID: 701 RVA: 0x00009111 File Offset: 0x00007311
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.RapiderBuff;
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060002BE RID: 702 RVA: 0x00009115 File Offset: 0x00007315
	public override float KeyValue
	{
		get
		{
			return 600f;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060002BF RID: 703 RVA: 0x0000911C File Offset: 0x0000731C
	public override float KeyValue2
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060002C0 RID: 704 RVA: 0x00009123 File Offset: 0x00007323
	public override float KeyValue3
	{
		get
		{
			return 800f;
		}
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0000912C File Offset: 0x0000732C
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Rapider)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		((RapiderSkill)this.strategy.TurretSkills[0]).MaxValue = this.KeyValue;
	}
}
