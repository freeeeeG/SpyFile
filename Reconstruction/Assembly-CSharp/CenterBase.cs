using System;
using System.Collections.Generic;

// Token: 0x0200008F RID: 143
public class CenterBase : ElementSkill
{
	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000359 RID: 857 RVA: 0x00009E77 File Offset: 0x00008077
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				1,
				3
			};
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x0600035A RID: 858 RVA: 0x00009E93 File Offset: 0x00008093
	public override float KeyValue
	{
		get
		{
			return 15f;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x0600035B RID: 859 RVA: 0x00009E9A File Offset: 0x0000809A
	public override float KeyValue2
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x0600035C RID: 860 RVA: 0x00009EA4 File Offset: 0x000080A4
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x0600035D RID: 861 RVA: 0x00009ED0 File Offset: 0x000080D0
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00009F0B File Offset: 0x0000810B
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.Duration = 9999f;
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00009F20 File Offset: 0x00008120
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		this.strategy.TurnFixAttack += this.KeyValue * bullet.BulletEffectIntensify;
		this.intensifiedValue += this.KeyValue * bullet.BulletEffectIntensify;
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00009F70 File Offset: 0x00008170
	public override void Tick(float delta)
	{
		base.Tick(delta);
		this.reduce = this.intensifiedValue * this.KeyValue2 * delta;
		this.strategy.TurnFixAttack -= this.reduce;
		this.intensifiedValue -= this.reduce;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00009FC4 File Offset: 0x000081C4
	public override void EndTurn()
	{
		base.EndTurn();
		this.reduce = 0f;
		this.intensifiedValue = 0f;
	}

	// Token: 0x04000174 RID: 372
	private float intensifiedValue;

	// Token: 0x04000175 RID: 373
	private float reduce;
}
