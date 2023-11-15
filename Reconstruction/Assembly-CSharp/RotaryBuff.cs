using System;

// Token: 0x02000072 RID: 114
public class RotaryBuff : GlobalSkill
{
	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060002CD RID: 717 RVA: 0x00009242 File Offset: 0x00007442
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.RotaryBuff;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060002CE RID: 718 RVA: 0x00009246 File Offset: 0x00007446
	public override float KeyValue
	{
		get
		{
			return 0.06f;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060002CF RID: 719 RVA: 0x0000924D File Offset: 0x0000744D
	public override float KeyValue2
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00009254 File Offset: 0x00007454
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Rotary)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00009284 File Offset: 0x00007484
	public override void OnEnter(IDamage target)
	{
		base.OnEnter(target);
		if (this.intensifiedValue < this.KeyValue2)
		{
			this.strategy.TurnAttackIntensify += this.KeyValue;
		}
		this.intensifiedValue += this.KeyValue;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000092D4 File Offset: 0x000074D4
	public override void OnExit(IDamage target)
	{
		base.OnExit(target);
		if (this.intensifiedValue < this.KeyValue2)
		{
			this.strategy.TurnAttackIntensify -= this.KeyValue;
		}
		this.intensifiedValue -= this.KeyValue;
	}

	// Token: 0x0400016F RID: 367
	private float intensifiedValue;
}
