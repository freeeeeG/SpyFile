using System;

// Token: 0x02000066 RID: 102
public class TechMassSkill : GlobalSkill
{
	// Token: 0x1700011C RID: 284
	// (get) Token: 0x0600028C RID: 652 RVA: 0x00008CD0 File Offset: 0x00006ED0
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechMassSkill;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x0600028D RID: 653 RVA: 0x00008CD3 File Offset: 0x00006ED3
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x0600028E RID: 654 RVA: 0x00008CDA File Offset: 0x00006EDA
	public override float KeyValue2
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600028F RID: 655 RVA: 0x00008CE1 File Offset: 0x00006EE1
	public override float KeyValue3
	{
		get
		{
			return 0.08f;
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00008CE8 File Offset: 0x00006EE8
	public override void Build()
	{
		base.Build();
		if (!this.IsAbnormal)
		{
			this.strategy.AttackIntensify += this.KeyValue;
			return;
		}
		this.strategy.MaxRange = (int)this.KeyValue2;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00008D24 File Offset: 0x00006F24
	public override void Detect()
	{
		base.Detect();
		if (this.IsAbnormal)
		{
			this.strategy.AttackIntensify -= this.intensifiedValue;
			this.intensifiedValue = this.KeyValue3 * (float)GameRes.TotalLandedRefactor;
			this.strategy.AttackIntensify += this.intensifiedValue;
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00008D82 File Offset: 0x00006F82
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifiedValue = 0f;
	}

	// Token: 0x0400016B RID: 363
	private float intensifiedValue;
}
