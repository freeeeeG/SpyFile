using System;

// Token: 0x0200006B RID: 107
public class RapidBuff : GlobalSkill
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060002A8 RID: 680 RVA: 0x00008F49 File Offset: 0x00007149
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.RapidBuff;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x060002A9 RID: 681 RVA: 0x00008F4D File Offset: 0x0000714D
	public override float KeyValue
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x060002AA RID: 682 RVA: 0x00008F54 File Offset: 0x00007154
	public override float KeyValue2
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x060002AB RID: 683 RVA: 0x00008F5B File Offset: 0x0000715B
	public override float KeyValue3
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00008F64 File Offset: 0x00007164
	public override void Build()
	{
		base.Build();
		if (this.IsAbnormal)
		{
			this.strategy.BaseFixFrostResist -= this.KeyValue2;
			return;
		}
		this.strategy.FirerateIntensify += this.KeyValue;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00008FB0 File Offset: 0x000071B0
	public override void Shoot(IDamage target, Bullet bullet = null)
	{
		if (this.IsAbnormal)
		{
			this.strategy.TurnFireRateIntensify -= this.intensifyValue;
			this.intensifyDistance = bullet.GetTargetDistance();
			this.intensifyValue = this.intensifyDistance * this.KeyValue3;
			this.strategy.TurnFireRateIntensify += this.intensifyValue;
		}
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00009014 File Offset: 0x00007214
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifyValue = 0f;
		this.intensifyDistance = 0f;
	}

	// Token: 0x0400016D RID: 365
	private float intensifyValue;

	// Token: 0x0400016E RID: 366
	private float intensifyDistance;
}
