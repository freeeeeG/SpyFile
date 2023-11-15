using System;
using System.Collections.Generic;

// Token: 0x02000043 RID: 67
public class SameTarget : ElementSkill
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060001AD RID: 429 RVA: 0x0000711C File Offset: 0x0000531C
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				1,
				1
			};
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060001AE RID: 430 RVA: 0x00007138 File Offset: 0x00005338
	public override float KeyValue
	{
		get
		{
			return 0.005f;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060001AF RID: 431 RVA: 0x0000713F File Offset: 0x0000533F
	public override float KeyValue2
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060001B0 RID: 432 RVA: 0x00007148 File Offset: 0x00005348
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060001B1 RID: 433 RVA: 0x00007174 File Offset: 0x00005374
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x000071A0 File Offset: 0x000053A0
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		if (this.intensifiedValue < this.KeyValue2)
		{
			float num = this.KeyValue * bullet.BulletEffectIntensify;
			this.strategy.TurnFixFirerate += num;
			this.intensifiedValue += num;
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x000071F2 File Offset: 0x000053F2
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifiedValue = 0f;
	}

	// Token: 0x0400011F RID: 287
	private float intensifiedValue;
}
