using System;

// Token: 0x02000079 RID: 121
public class CoreBuff : GlobalSkill
{
	// Token: 0x17000152 RID: 338
	// (get) Token: 0x060002F3 RID: 755 RVA: 0x00009593 File Offset: 0x00007793
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.CoreBuff;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x060002F4 RID: 756 RVA: 0x00009597 File Offset: 0x00007797
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000959E File Offset: 0x0000779E
	public override float KeyValue2
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x060002F6 RID: 758 RVA: 0x000095A5 File Offset: 0x000077A5
	public override float KeyValue3
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x000095AC File Offset: 0x000077AC
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Core)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
		this.strategy.AttackIntensify -= this.KeyValue2;
		((CoreSkill)this.strategy.TurretSkills[0]).DetectRange += (int)this.KeyValue;
	}
}
