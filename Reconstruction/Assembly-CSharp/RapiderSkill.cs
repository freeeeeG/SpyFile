using System;

// Token: 0x0200009F RID: 159
public class RapiderSkill : InitialSkill
{
	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060003E0 RID: 992 RVA: 0x0000AB89 File Offset: 0x00008D89
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Rapider;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000AB8C File Offset: 0x00008D8C
	public override float KeyValue
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x060003E2 RID: 994 RVA: 0x0000AB93 File Offset: 0x00008D93
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000AB9A File Offset: 0x00008D9A
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000ABB8 File Offset: 0x00008DB8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x060003E5 RID: 997 RVA: 0x0000ABE3 File Offset: 0x00008DE3
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.MaxValue.ToString());
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0000AC00 File Offset: 0x00008E00
	public override void Shoot(IDamage target = null, Bullet bullet = null)
	{
		base.Shoot(target, bullet);
		if (this.intensifiedValue < this.MaxValue)
		{
			this.strategy.TurnFixAttack += this.KeyValue;
			this.intensifiedValue += this.KeyValue;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0000AC4E File Offset: 0x00008E4E
	public override void EndTurn()
	{
		base.EndTurn();
		this.intensifiedValue = 0f;
	}

	// Token: 0x04000188 RID: 392
	public float MaxValue = 300f;

	// Token: 0x04000189 RID: 393
	private float intensifiedValue;
}
