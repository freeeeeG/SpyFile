using System;

// Token: 0x0200006E RID: 110
public class ConstructorBuff : GlobalSkill
{
	// Token: 0x17000132 RID: 306
	// (get) Token: 0x060002B8 RID: 696 RVA: 0x00009093 File Offset: 0x00007293
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.ConstructorBuff;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x060002B9 RID: 697 RVA: 0x00009097 File Offset: 0x00007297
	public override float KeyValue
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x060002BA RID: 698 RVA: 0x0000909E File Offset: 0x0000729E
	public override float KeyValue2
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x000090A8 File Offset: 0x000072A8
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Constructor)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		((ConstructorSkill)this.strategy.TurretSkills[0]).IntentValue += this.KeyValue;
	}
}
