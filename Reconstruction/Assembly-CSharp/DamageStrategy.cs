using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x0200011F RID: 287
public abstract class DamageStrategy
{
	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x00013BD1 File Offset: 0x00011DD1
	// (set) Token: 0x0600073B RID: 1851 RVA: 0x00013BD9 File Offset: 0x00011DD9
	public virtual float CurrentFrost { get; set; }

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00013BE2 File Offset: 0x00011DE2
	// (set) Token: 0x0600073D RID: 1853 RVA: 0x00013BEA File Offset: 0x00011DEA
	public virtual float MaxFrost { get; set; }

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x00013BF3 File Offset: 0x00011DF3
	public virtual float PathProgress { get; }

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x0600073F RID: 1855 RVA: 0x00013BFB File Offset: 0x00011DFB
	public virtual int PathIndex { get; }

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x00013C03 File Offset: 0x00011E03
	public virtual bool IsStun
	{
		get
		{
			return this.StunTime > 0f;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x00013C12 File Offset: 0x00011E12
	// (set) Token: 0x06000742 RID: 1858 RVA: 0x00013C1A File Offset: 0x00011E1A
	public virtual float FrostTime { get; set; }

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x00013C23 File Offset: 0x00011E23
	public virtual bool IsFrost { get; }

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06000744 RID: 1860 RVA: 0x00013C2B File Offset: 0x00011E2B
	public bool IsControlled
	{
		get
		{
			return this.IsFrost || this.IsStun;
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x00013C3D File Offset: 0x00011E3D
	// (set) Token: 0x06000746 RID: 1862 RVA: 0x00013C45 File Offset: 0x00011E45
	public virtual float FrostIntensify
	{
		get
		{
			return this.frostIntensify;
		}
		set
		{
			this.frostIntensify = value;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06000747 RID: 1863 RVA: 0x00013C4E File Offset: 0x00011E4E
	// (set) Token: 0x06000748 RID: 1864 RVA: 0x00013C56 File Offset: 0x00011E56
	public virtual int TrapIntensify
	{
		get
		{
			return this.trapIntensify;
		}
		set
		{
			this.trapIntensify = value;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06000749 RID: 1865 RVA: 0x00013C5F File Offset: 0x00011E5F
	// (set) Token: 0x0600074A RID: 1866 RVA: 0x00013C67 File Offset: 0x00011E67
	public virtual bool IsDie
	{
		get
		{
			return this.isDie;
		}
		set
		{
			this.isDie = value;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x0600074B RID: 1867
	public abstract bool IsEnemy { get; }

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x00013C70 File Offset: 0x00011E70
	// (set) Token: 0x0600074D RID: 1869 RVA: 0x00013C78 File Offset: 0x00011E78
	public bool InVisible
	{
		get
		{
			return this.inVisible;
		}
		set
		{
			this.inVisible = value;
			this.GFXFade(true);
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x0600074E RID: 1870 RVA: 0x00013C88 File Offset: 0x00011E88
	public float MaxTransparentValue
	{
		get
		{
			if (!this.InVisible)
			{
				return 1f;
			}
			return 0.3f;
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00013C9D File Offset: 0x00011E9D
	public void GFXFade(bool show)
	{
		this.damageTarget.gfxSprite.DOColor(new Color(1f, 1f, 1f, show ? this.MaxTransparentValue : 0f), 0.5f);
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x00013CD9 File Offset: 0x00011ED9
	public virtual float DamageIntensify
	{
		get
		{
			return Mathf.Max(0.1f, 1f + this.BuffDamageIntensify);
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06000751 RID: 1873 RVA: 0x00013CF1 File Offset: 0x00011EF1
	// (set) Token: 0x06000752 RID: 1874 RVA: 0x00013CFC File Offset: 0x00011EFC
	public virtual float CurrentHealth
	{
		get
		{
			return this.currentHealth;
		}
		set
		{
			this.currentHealth = Mathf.Clamp(value, 0f, this.MaxHealth);
			if (this.currentHealth <= 0f && !this.IsDie)
			{
				this.IsDie = true;
			}
			this.damageTarget.HealthBar.FillAmount = this.currentHealth / this.MaxHealth;
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06000753 RID: 1875 RVA: 0x00013D59 File Offset: 0x00011F59
	// (set) Token: 0x06000754 RID: 1876 RVA: 0x00013D61 File Offset: 0x00011F61
	public virtual float MaxHealth
	{
		get
		{
			return this.maxHealth;
		}
		set
		{
			this.maxHealth = value;
			this.CurrentHealth = this.maxHealth;
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06000755 RID: 1877 RVA: 0x00013D76 File Offset: 0x00011F76
	public float HealthPercent
	{
		get
		{
			return this.CurrentHealth / this.MaxHealth;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06000756 RID: 1878 RVA: 0x00013D85 File Offset: 0x00011F85
	// (set) Token: 0x06000757 RID: 1879 RVA: 0x00013D8D File Offset: 0x00011F8D
	public virtual float BuffDamageIntensify
	{
		get
		{
			return this.buffDamageIntensify;
		}
		set
		{
			this.buffDamageIntensify = value;
		}
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00013D96 File Offset: 0x00011F96
	public virtual void ApplyBuffDmgIntensify(float value)
	{
		this.BuffDamageIntensify += value;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00013DA6 File Offset: 0x00011FA6
	public DamageStrategy(IDamage damageTarget)
	{
		this.damageTarget = damageTarget;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00013DB8 File Offset: 0x00011FB8
	public virtual void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
	{
		realDamage = amount * (acceptIntensify ? this.DamageIntensify : 1f) * (this.InVisible ? 0.5f : 1f);
		this.CurrentHealth -= realDamage * (1f - this.HiddenResist);
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00013E0B File Offset: 0x0001200B
	public virtual void ApplyBuff(BuffInfo buffInfo)
	{
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00013E10 File Offset: 0x00012010
	public virtual void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResit)
	{
		this.IsDie = false;
		this.MaxHealth = Mathf.Max(1f, attribute.Health * intensify);
		this.HiddenResist = dmgResit;
		this.InVisible = false;
		this.BuffDamageIntensify = 0f;
		this.FrostIntensify = 0f;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x00013E60 File Offset: 0x00012060
	public virtual void StrategyUpdate()
	{
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00013E62 File Offset: 0x00012062
	public virtual void ApplyFrost(float value)
	{
		this.CurrentFrost += value;
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00013E72 File Offset: 0x00012072
	public virtual void UnFrost()
	{
	}

	// Token: 0x04000399 RID: 921
	public IDamage damageTarget;

	// Token: 0x0400039A RID: 922
	public Transform ModelTrans;

	// Token: 0x0400039B RID: 923
	public float HiddenResist;

	// Token: 0x040003A0 RID: 928
	protected float currentHealth;

	// Token: 0x040003A1 RID: 929
	protected float maxHealth;

	// Token: 0x040003A2 RID: 930
	private bool isDie;

	// Token: 0x040003A3 RID: 931
	private int trapIntensify;

	// Token: 0x040003A4 RID: 932
	public float StunTime;

	// Token: 0x040003A7 RID: 935
	private float frostIntensify;

	// Token: 0x040003A8 RID: 936
	private bool inVisible;

	// Token: 0x040003A9 RID: 937
	private float buffDamageIntensify;
}
