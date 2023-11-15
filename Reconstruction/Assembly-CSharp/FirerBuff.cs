using System;

// Token: 0x0200007B RID: 123
public class FirerBuff : GlobalSkill
{
	// Token: 0x17000158 RID: 344
	// (get) Token: 0x060002FD RID: 765 RVA: 0x00009696 File Offset: 0x00007896
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.FirerBuff;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x060002FE RID: 766 RVA: 0x0000969A File Offset: 0x0000789A
	public override float KeyValue
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x060002FF RID: 767 RVA: 0x000096A1 File Offset: 0x000078A1
	public override float KeyValue2
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000096A8 File Offset: 0x000078A8
	public override void Build()
	{
		base.Build();
		if (this.strategy.Attribute.RefactorName != RefactorTurretName.Firer)
		{
			this.strategy.GlobalSkills.Remove(this);
			return;
		}
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000096D7 File Offset: 0x000078D7
	public override void StartTurn()
	{
		base.StartTurn();
		this.Duration = 9999f;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x000096EC File Offset: 0x000078EC
	public override void Tick(float delta)
	{
		base.Tick(delta);
		if (this.strategy.Concrete.IsAttacking)
		{
			if (this.intensifiedValue < this.KeyValue2)
			{
				float num = this.KeyValue * delta;
				this.intensifiedValue += num;
				this.strategy.TurnFixCriticalPercentage += num;
				return;
			}
		}
		else if (this.intensifiedValue > 0f)
		{
			this.strategy.TurnFixCriticalPercentage -= this.intensifiedValue;
			this.intensifiedValue = 0f;
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0000977B File Offset: 0x0000797B
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifiedValue = 0f;
	}

	// Token: 0x04000171 RID: 369
	private float intensifiedValue;
}
