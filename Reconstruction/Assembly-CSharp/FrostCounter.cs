using System;
using System.Collections.Generic;

// Token: 0x02000090 RID: 144
public class FrostCounter : ElementSkill
{
	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06000363 RID: 867 RVA: 0x00009FEA File Offset: 0x000081EA
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				1,
				4
			};
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06000364 RID: 868 RVA: 0x0000A006 File Offset: 0x00008206
	public override float KeyValue
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06000365 RID: 869 RVA: 0x0000A00D File Offset: 0x0000820D
	public override float KeyValue2
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06000366 RID: 870 RVA: 0x0000A014 File Offset: 0x00008214
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000367 RID: 871 RVA: 0x0000A050 File Offset: 0x00008250
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0000A07B File Offset: 0x0000827B
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.Duration = 9999f;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0000A090 File Offset: 0x00008290
	public override void Tick(float delta)
	{
		if (this.chargedTime > 0f)
		{
			if (!this.charged)
			{
				this.strategy.TurnAttackIntensify += this.KeyValue;
				this.strategy.TurnFireRateIntensify += this.KeyValue;
				this.charged = true;
			}
			this.chargedTime -= delta;
			if (this.chargedTime <= 0f && this.charged)
			{
				this.strategy.TurnAttackIntensify -= this.KeyValue;
				this.strategy.TurnFireRateIntensify -= this.KeyValue;
				this.charged = false;
			}
		}
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0000A146 File Offset: 0x00008346
	public override void OnUnFrost()
	{
		base.OnUnFrost();
		this.chargedTime = this.KeyValue2;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0000A15A File Offset: 0x0000835A
	public override void EndTurn()
	{
		base.EndTurn();
		this.chargedTime = 0f;
		this.charged = false;
	}

	// Token: 0x04000176 RID: 374
	private float chargedTime;

	// Token: 0x04000177 RID: 375
	private bool charged;
}
