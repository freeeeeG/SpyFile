using System;

// Token: 0x020000AB RID: 171
public class FirerSkill : InitialSkill
{
	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000B8AB File Offset: 0x00009AAB
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Firer;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000B8AF File Offset: 0x00009AAF
	public override float KeyValue
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000435 RID: 1077 RVA: 0x0000B8B6 File Offset: 0x00009AB6
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000B8C0 File Offset: 0x00009AC0
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000437 RID: 1079 RVA: 0x0000B8FC File Offset: 0x00009AFC
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0000B937 File Offset: 0x00009B37
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.Duration = 9999f;
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0000B94C File Offset: 0x00009B4C
	public override void Tick(float delta)
	{
		base.Tick(delta);
		if (this.strategy.Concrete.IsAttacking)
		{
			if (this.intensifiedValue < this.KeyValue2)
			{
				float num = this.KeyValue * delta;
				this.intensifiedValue += num;
				this.strategy.TurnAttackIntensify += num;
				return;
			}
		}
		else
		{
			this.strategy.TurnAttackIntensify -= this.intensifiedValue;
			this.intensifiedValue = 0f;
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0000B9CE File Offset: 0x00009BCE
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifiedValue = 0f;
	}

	// Token: 0x04000197 RID: 407
	private float intensifiedValue;
}
