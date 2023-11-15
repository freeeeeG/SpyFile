using System;

// Token: 0x020000A2 RID: 162
public class CoordinatorSkill : InitialSkill
{
	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000AE42 File Offset: 0x00009042
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Coordinator;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000AE48 File Offset: 0x00009048
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.IncreaseValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000AE84 File Offset: 0x00009084
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((StrategyBase.CoordinatorMaxIntensify * 100f).ToString() + "%");
		}
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0000AEBE File Offset: 0x000090BE
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.Duration = 9999f;
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0000AED4 File Offset: 0x000090D4
	public override void Tick(float delta)
	{
		base.Tick(delta);
		float num = this.IncreaseValue * delta;
		if (this.strategy.Concrete.IsAttacking)
		{
			if (this.intensifyValue < StrategyBase.CoordinatorMaxIntensify)
			{
				StrategyBase.CooporativeAttackIntensify += num;
				this.intensifyValue += num;
				return;
			}
		}
		else if (this.intensifyValue > 0f)
		{
			StrategyBase.CooporativeAttackIntensify -= this.intensifyValue;
			this.intensifyValue = 0f;
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0000AF54 File Offset: 0x00009154
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifyValue = 0f;
	}

	// Token: 0x0400018D RID: 397
	public float IncreaseValue = 0.05f;

	// Token: 0x0400018E RID: 398
	private float intensifyValue;
}
