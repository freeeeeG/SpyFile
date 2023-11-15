using System;
using System.Collections.Generic;

// Token: 0x02000096 RID: 150
public class CritBase : ElementSkill
{
	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000388 RID: 904 RVA: 0x0000A517 File Offset: 0x00008717
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				3,
				4
			};
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000389 RID: 905 RVA: 0x0000A533 File Offset: 0x00008733
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x0600038A RID: 906 RVA: 0x0000A53A File Offset: 0x0000873A
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x0600038B RID: 907 RVA: 0x0000A544 File Offset: 0x00008744
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x0600038C RID: 908 RVA: 0x0000A580 File Offset: 0x00008780
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0000A5BB File Offset: 0x000087BB
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (target == null)
		{
			return;
		}
		if (target.DamageStrategy.HealthPercent <= this.KeyValue)
		{
			bullet.CriticalPercentage += this.KeyValue2;
		}
	}
}
