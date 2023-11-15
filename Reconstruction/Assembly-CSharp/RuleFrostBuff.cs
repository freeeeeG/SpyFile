using System;

// Token: 0x0200006A RID: 106
public class RuleFrostBuff : GlobalSkill
{
	// Token: 0x17000127 RID: 295
	// (get) Token: 0x060002A1 RID: 673 RVA: 0x00008E9D File Offset: 0x0000709D
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.RuleFrostBuff;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x060002A2 RID: 674 RVA: 0x00008EA0 File Offset: 0x000070A0
	public override float KeyValue
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060002A3 RID: 675 RVA: 0x00008EA7 File Offset: 0x000070A7
	public override float KeyValue2
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00008EAE File Offset: 0x000070AE
	public override void StartTurn()
	{
		base.StartTurn();
		this.Duration = 9999f;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00008EC4 File Offset: 0x000070C4
	public override void Tick(float delta)
	{
		base.Tick(delta);
		this.timeCounter += delta;
		if (this.timeCounter > this.KeyValue)
		{
			this.timeCounter = 0f;
			Singleton<StaticData>.Instance.FrostTurretEffect(this.strategy.Concrete.transform.position, 0.1f, this.KeyValue2);
		}
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00008F2E File Offset: 0x0000712E
	public override void EndTurn()
	{
		base.EndTurn();
		this.timeCounter = 0f;
	}

	// Token: 0x0400016C RID: 364
	private float timeCounter;
}
