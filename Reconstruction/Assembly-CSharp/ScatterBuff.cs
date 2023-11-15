using System;

// Token: 0x02000070 RID: 112
public class ScatterBuff : GlobalSkill
{
	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000918E File Offset: 0x0000738E
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.ScatterBuff;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060002C4 RID: 708 RVA: 0x00009192 File Offset: 0x00007392
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060002C5 RID: 709 RVA: 0x00009199 File Offset: 0x00007399
	public override float KeyValue2
	{
		get
		{
			return 0.65f;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060002C6 RID: 710 RVA: 0x000091A0 File Offset: 0x000073A0
	public override float KeyValue3
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x000091A8 File Offset: 0x000073A8
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Scatter)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		this.strategy.BaseFixBulletEffectIntensify += this.KeyValue;
	}
}
