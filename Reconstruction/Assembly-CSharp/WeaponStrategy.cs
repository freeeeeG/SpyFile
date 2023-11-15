using System;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class WeaponStrategy : DamageStrategy
{
	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x00014293 File Offset: 0x00012493
	public override bool IsEnemy
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x00014296 File Offset: 0x00012496
	public override float PathProgress
	{
		get
		{
			return (float)(this.weapon.Knight.PointIndex / this.weapon.Knight.PathPoints.Count);
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06000780 RID: 1920 RVA: 0x000142BF File Offset: 0x000124BF
	public override int PathIndex
	{
		get
		{
			return this.weapon.Knight.PointIndex;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06000781 RID: 1921 RVA: 0x000142D1 File Offset: 0x000124D1
	public override float DamageIntensify
	{
		get
		{
			return this.weapon.Knight.DamageStrategy.DamageIntensify;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06000782 RID: 1922 RVA: 0x000142E8 File Offset: 0x000124E8
	public override float FrostIntensify
	{
		get
		{
			return this.weapon.Knight.DamageStrategy.FrostIntensify;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06000783 RID: 1923 RVA: 0x000142FF File Offset: 0x000124FF
	// (set) Token: 0x06000784 RID: 1924 RVA: 0x00014307 File Offset: 0x00012507
	public override bool IsDie
	{
		get
		{
			return base.IsDie;
		}
		set
		{
			base.IsDie = value;
			if (this.IsDie)
			{
				this.UnFrost();
			}
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x0001431E File Offset: 0x0001251E
	public override bool IsFrost
	{
		get
		{
			return this.FrostTime > 0f;
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06000786 RID: 1926 RVA: 0x0001432D File Offset: 0x0001252D
	// (set) Token: 0x06000787 RID: 1927 RVA: 0x00014338 File Offset: 0x00012538
	public override float CurrentFrost
	{
		get
		{
			return this.currentFrost;
		}
		set
		{
			if (this.UnfrostableTime > 0f)
			{
				return;
			}
			this.currentFrost = value;
			if (this.currentFrost >= this.MaxFrost && this.weapon.gameObject.activeSelf)
			{
				this.currentFrost = 0f;
				this.FrostWeapon(GameRes.EnemyFrostTime * (1f + this.FrostIntensify - GameRes.EnemyFrostResist));
			}
			this.weapon.HealthBar.FrostAmount = this.CurrentFrost / this.MaxFrost;
		}
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x000143C0 File Offset: 0x000125C0
	private void FrostWeapon(float time)
	{
		FrostEffect frostEffect = Singleton<StaticData>.Instance.FrostEffect(this.ModelTrans.position);
		frostEffect.transform.localScale = Vector3.one * 0.85f;
		this.FrostTime += time;
		this.UnfrostableTime += time + 3f;
		this.m_FrostEffect = frostEffect;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0001442B File Offset: 0x0001262B
	public override void UnFrost()
	{
		if (this.m_FrostEffect != null)
		{
			this.m_FrostEffect.Broke();
			this.m_FrostEffect = null;
			this.UnfrostableTime -= this.FrostTime;
			this.FrostTime = 0f;
		}
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0001446C File Offset: 0x0001266C
	public override void StrategyUpdate()
	{
		if (this.FrostTime > 0f)
		{
			this.FrostTime -= Time.deltaTime;
			if (this.FrostTime <= 0.2f)
			{
				this.UnFrost();
			}
		}
		if (this.UnfrostableTime > 0f)
		{
			this.UnfrostableTime -= Time.deltaTime;
		}
		this.damageCounter += Time.deltaTime;
		if (this.damageCounter > 2f)
		{
			this.weapon.SpeedModify = 2f;
			return;
		}
		this.weapon.SpeedModify = 1f;
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0001450A File Offset: 0x0001270A
	public override void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
	{
		base.ApplyDamage(amount, out realDamage, bullet, acceptIntensify);
		this.damageCounter = 0f;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00014524 File Offset: 0x00012724
	public WeaponStrategy(IDamage damageTarget, float dmgResist, float frost) : base(damageTarget)
	{
		this.HiddenResist = dmgResist;
		this.damageTarget = damageTarget;
		this.weapon = (damageTarget as Weapon);
		this.ModelTrans = this.weapon.transform;
		this.MaxFrost = (1f + (float)GameRes.CurrentWave * (1f + (float)GameRes.CurrentWave / 30f)) * frost;
		this.damageCounter = 2f;
	}

	// Token: 0x040003AF RID: 943
	public FrostEffect m_FrostEffect;

	// Token: 0x040003B0 RID: 944
	private Weapon weapon;

	// Token: 0x040003B1 RID: 945
	public float UnfrostableTime;

	// Token: 0x040003B2 RID: 946
	private float damageCounter;

	// Token: 0x040003B3 RID: 947
	private float currentFrost;
}
