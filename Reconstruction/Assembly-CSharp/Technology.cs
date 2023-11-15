using System;

// Token: 0x02000194 RID: 404
public abstract class Technology
{
	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0001BBBB File Offset: 0x00019DBB
	public virtual bool Add
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0001BBBE File Offset: 0x00019DBE
	public string SpriteResDir
	{
		get
		{
			return "Sprites/PixelUI/Icon/" + this.TechName;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06000A36 RID: 2614
	public abstract TechnologyName TechnologyName { get; }

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0001BBD0 File Offset: 0x00019DD0
	public virtual RefactorTurretName RefactorBinding
	{
		get
		{
			return RefactorTurretName.None;
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0001BBD4 File Offset: 0x00019DD4
	public virtual bool ContainGlobalBuff
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0001BBD8 File Offset: 0x00019DD8
	public string TechName
	{
		get
		{
			return this.TechnologyName.ToString();
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0001BBFC File Offset: 0x00019DFC
	public string TechnologyDes
	{
		get
		{
			if (!this.IsAbnormal)
			{
				return GameMultiLang.GetTraduction(this.TechName + "INFO");
			}
			return StaticData.ElementDIC[ElementType.GOLD].Colorized("<sprite=8>" + GameMultiLang.GetTraduction(this.TechName + "INFO3")) + "\n" + StaticData.ElementDIC[ElementType.FIRE].Colorized("<sprite=9>" + GameMultiLang.GetTraduction(this.TechName + "INFO2"));
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0001BC8F File Offset: 0x00019E8F
	// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0001BC98 File Offset: 0x00019E98
	public virtual bool IsAbnormal
	{
		get
		{
			return this.isAbnormal;
		}
		set
		{
			this.isAbnormal = value;
			if (this.m_EnemyBuffInfo != null)
			{
				this.m_Buff.IsAbnormal = value;
				this.m_EnemyBuffInfo.IsAbnormal = value;
			}
			if (this.m_SkillInfo != null)
			{
				this.m_Skill.IsAbnormal = value;
				this.m_SkillInfo.IsAbnormal = value;
			}
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0001BCEC File Offset: 0x00019EEC
	// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0001BCF4 File Offset: 0x00019EF4
	public virtual bool CanAbnormal { get; set; }

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0001BCFD File Offset: 0x00019EFD
	// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0001BD05 File Offset: 0x00019F05
	public virtual float KeyValue { get; set; }

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0001BD0E File Offset: 0x00019F0E
	// (set) Token: 0x06000A42 RID: 2626 RVA: 0x0001BD16 File Offset: 0x00019F16
	public virtual float KeyValue2 { get; set; }

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0001BD1F File Offset: 0x00019F1F
	// (set) Token: 0x06000A44 RID: 2628 RVA: 0x0001BD27 File Offset: 0x00019F27
	public virtual float KeyValue3 { get; set; }

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0001BD30 File Offset: 0x00019F30
	// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0001BD38 File Offset: 0x00019F38
	public virtual float SaveValue { get; set; }

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0001BD41 File Offset: 0x00019F41
	public virtual string DisplayValue1 { get; }

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0001BD49 File Offset: 0x00019F49
	public virtual string DisplayValue2 { get; }

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0001BD51 File Offset: 0x00019F51
	public virtual string DisplayValue3 { get; }

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0001BD59 File Offset: 0x00019F59
	public virtual string DisplayValue4 { get; }

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0001BD61 File Offset: 0x00019F61
	public virtual string DisplayValue5 { get; }

	// Token: 0x06000A4C RID: 2636 RVA: 0x0001BD69 File Offset: 0x00019F69
	public virtual void InitializeTech()
	{
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0001BD6B File Offset: 0x00019F6B
	public virtual void OnGet()
	{
		if (this.m_EnemyBuffInfo != null)
		{
			EnemyBuffFactory.GlobalBuffs.Add(this.m_EnemyBuffInfo);
		}
		if (this.m_Skill != null)
		{
			TurretSkillFactory.AddGlobalSkill(this.m_SkillInfo);
		}
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0001BD98 File Offset: 0x00019F98
	public virtual bool OnGet2()
	{
		return false;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x0001BD9B File Offset: 0x00019F9B
	public virtual void OnEquip(StrategyBase strategy)
	{
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x0001BD9D File Offset: 0x00019F9D
	public virtual void OnTurnStart()
	{
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0001BD9F File Offset: 0x00019F9F
	public virtual void OnTurnEnd()
	{
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0001BDA1 File Offset: 0x00019FA1
	public virtual void OnWaveEnd()
	{
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0001BDA3 File Offset: 0x00019FA3
	public virtual void OnRefactor(StrategyBase strategy)
	{
	}

	// Token: 0x0400059B RID: 1435
	private bool isAbnormal;

	// Token: 0x040005A6 RID: 1446
	protected EnemyBuff m_Buff;

	// Token: 0x040005A7 RID: 1447
	protected BuffInfo m_EnemyBuffInfo;

	// Token: 0x040005A8 RID: 1448
	protected GlobalSkillInfo m_SkillInfo;

	// Token: 0x040005A9 RID: 1449
	protected GlobalSkill m_Skill;
}
