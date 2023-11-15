using System;
using System.Collections.Generic;

// Token: 0x0200005F RID: 95
public class GoldenBullet : ElementSkill
{
	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000269 RID: 617 RVA: 0x00008705 File Offset: 0x00006905
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				10,
				10,
				10
			};
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x0600026A RID: 618 RVA: 0x00008724 File Offset: 0x00006924
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x0600026B RID: 619 RVA: 0x0000872B File Offset: 0x0000692B
	public override float KeyValue2
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x0600026C RID: 620 RVA: 0x00008734 File Offset: 0x00006934
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x0600026D RID: 621 RVA: 0x00008770 File Offset: 0x00006970
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000879B File Offset: 0x0000699B
	public override void StartTurn2()
	{
		base.StartTurn2();
		if (!this.triggered)
		{
			this.initMaxFirerate = this.strategy.MaxFireRate;
			this.shootCount = 0;
			this.triggered = true;
		}
	}

	// Token: 0x0600026F RID: 623 RVA: 0x000087CC File Offset: 0x000069CC
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		this.shootCount++;
		this.intentsifyValue = this.KeyValue * (float)this.shootCount * bullet.BulletEffectIntensify;
		this.strategy.TurnFixDamageBonus += this.intentsifyValue;
		bullet.BulletDamageIntensify += this.intentsifyValue;
		if (this.shootCount >= (int)this.KeyValue2)
		{
			this.strategy.MaxFireRate = 0f;
		}
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00008854 File Offset: 0x00006A54
	public override void EndTurn()
	{
		base.EndTurn();
		this.triggered = false;
		this.strategy.MaxFireRate = this.initMaxFirerate;
	}

	// Token: 0x04000134 RID: 308
	private float initMaxFirerate;

	// Token: 0x04000135 RID: 309
	private int shootCount;

	// Token: 0x04000136 RID: 310
	private float intentsifyValue;

	// Token: 0x04000137 RID: 311
	private bool triggered;
}
