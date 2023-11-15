using System;

// Token: 0x02000098 RID: 152
public abstract class TurretSkill
{
	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000395 RID: 917 RVA: 0x0000A70C File Offset: 0x0000890C
	public virtual RefactorTurretName EffectName { get; }

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000396 RID: 918 RVA: 0x0000A714 File Offset: 0x00008914
	// (set) Token: 0x06000397 RID: 919 RVA: 0x0000A71C File Offset: 0x0000891C
	public virtual string SkillDescription { get; set; }

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000398 RID: 920 RVA: 0x0000A725 File Offset: 0x00008925
	// (set) Token: 0x06000399 RID: 921 RVA: 0x0000A72D File Offset: 0x0000892D
	public virtual string SkillName { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600039A RID: 922 RVA: 0x0000A736 File Offset: 0x00008936
	public virtual string DisplayValue { get; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x0600039B RID: 923 RVA: 0x0000A73E File Offset: 0x0000893E
	public virtual string DisplayValue2 { get; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x0600039C RID: 924 RVA: 0x0000A746 File Offset: 0x00008946
	public virtual string DisplayValue3 { get; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x0600039D RID: 925 RVA: 0x0000A74E File Offset: 0x0000894E
	public virtual string DisplayValue4 { get; }

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x0600039E RID: 926 RVA: 0x0000A756 File Offset: 0x00008956
	public virtual string DisplayValue5 { get; }

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x0600039F RID: 927 RVA: 0x0000A75E File Offset: 0x0000895E
	// (set) Token: 0x060003A0 RID: 928 RVA: 0x0000A766 File Offset: 0x00008966
	public virtual float KeyValue { get; set; }

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000A76F File Offset: 0x0000896F
	// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000A777 File Offset: 0x00008977
	public virtual float KeyValue2 { get; set; }

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000A780 File Offset: 0x00008980
	// (set) Token: 0x060003A4 RID: 932 RVA: 0x0000A788 File Offset: 0x00008988
	public virtual float KeyValue3 { get; set; }

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000A791 File Offset: 0x00008991
	public virtual ElementType IntensifyElement
	{
		get
		{
			return ElementType.None;
		}
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0000A794 File Offset: 0x00008994
	public virtual void Composite()
	{
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0000A796 File Offset: 0x00008996
	public virtual void Detect()
	{
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0000A798 File Offset: 0x00008998
	public virtual void Build()
	{
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0000A79A File Offset: 0x0000899A
	public virtual void OnUnFrost()
	{
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0000A79C File Offset: 0x0000899C
	public virtual void Shoot(IDamage target = null, Bullet bullet = null)
	{
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0000A79E File Offset: 0x0000899E
	public virtual void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public virtual void PreHit(Bullet bullet = null)
	{
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0000A7A2 File Offset: 0x000089A2
	public virtual float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		return damage;
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0000A7A5 File Offset: 0x000089A5
	public virtual void Splash(ConcreteContent content, Bullet bullet = null)
	{
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0000A7A7 File Offset: 0x000089A7
	public virtual void Draw()
	{
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0000A7A9 File Offset: 0x000089A9
	public virtual void Tick(float delta)
	{
		this.Duration -= delta;
		if (this.Duration <= 0f)
		{
			this.IsFinish = true;
			this.TickEnd();
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0000A7D3 File Offset: 0x000089D3
	public virtual void TickEnd()
	{
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0000A7D5 File Offset: 0x000089D5
	public virtual void StartTurn()
	{
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0000A7D7 File Offset: 0x000089D7
	public virtual void StartTurn2()
	{
		this.IsFinish = false;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0000A7E0 File Offset: 0x000089E0
	public virtual void StartTurn3()
	{
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0000A7E2 File Offset: 0x000089E2
	public virtual void EndTurn()
	{
		this.IsFinish = true;
		this.Duration = 0f;
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0000A7F6 File Offset: 0x000089F6
	public virtual void OnEquip()
	{
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0000A7F8 File Offset: 0x000089F8
	public virtual void OnEquipped()
	{
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0000A7FA File Offset: 0x000089FA
	public virtual void OnEnter(IDamage target)
	{
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0000A7FC File Offset: 0x000089FC
	public virtual void OnExit(IDamage target)
	{
	}

	// Token: 0x0400017C RID: 380
	public StrategyBase strategy;

	// Token: 0x04000186 RID: 390
	public float Duration;

	// Token: 0x04000187 RID: 391
	public bool IsFinish = true;
}
