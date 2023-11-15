using System;

// Token: 0x0200007F RID: 127
public class NuclearBuff : GlobalSkill
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000316 RID: 790 RVA: 0x0000991D File Offset: 0x00007B1D
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.NuclearBuff;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000317 RID: 791 RVA: 0x00009921 File Offset: 0x00007B21
	public override float KeyValue
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00009928 File Offset: 0x00007B28
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Nuclear)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		((NuclearSkill)this.strategy.TurretSkills[0]).IntentValue += this.KeyValue;
	}
}
