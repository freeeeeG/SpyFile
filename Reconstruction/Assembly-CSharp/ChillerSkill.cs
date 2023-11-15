using System;

// Token: 0x020000AA RID: 170
public class ChillerSkill : InitialSkill
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000B82F File Offset: 0x00009A2F
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Chiller;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000B833 File Offset: 0x00009A33
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x0000B83C File Offset: 0x00009A3C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0000B877 File Offset: 0x00009A77
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixSlowRate += this.strategy.FinalSlowRate * this.KeyValue;
	}
}
