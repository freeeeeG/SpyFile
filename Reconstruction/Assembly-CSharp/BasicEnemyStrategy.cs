using System;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class BasicEnemyStrategy : DamageStrategy
{
	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06000760 RID: 1888 RVA: 0x00013E74 File Offset: 0x00012074
	public override bool IsEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x00013E77 File Offset: 0x00012077
	public override float PathProgress
	{
		get
		{
			return (float)this.enemy.PointIndex / (float)this.enemy.PathPoints.Count;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06000762 RID: 1890 RVA: 0x00013E97 File Offset: 0x00012097
	public override int PathIndex
	{
		get
		{
			return this.enemy.PointIndex;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x00013EA4 File Offset: 0x000120A4
	// (set) Token: 0x06000764 RID: 1892 RVA: 0x00013EAC File Offset: 0x000120AC
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
				this.enemy.OnDie();
			}
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x00013ECE File Offset: 0x000120CE
	public override bool IsFrost
	{
		get
		{
			return this.FrostTime > 0f;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06000766 RID: 1894 RVA: 0x00013EDD File Offset: 0x000120DD
	// (set) Token: 0x06000767 RID: 1895 RVA: 0x00013EE8 File Offset: 0x000120E8
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
			if (this.currentFrost >= this.MaxFrost && this.enemy.gameObject.activeSelf)
			{
				this.currentFrost = 0f;
				this.FrostEnemy(GameRes.EnemyFrostTime * (1f + this.FrostIntensify - GameRes.EnemyFrostResist));
			}
			this.enemy.HealthBar.FrostAmount = this.CurrentFrost / this.MaxFrost;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06000768 RID: 1896 RVA: 0x00013F70 File Offset: 0x00012170
	// (set) Token: 0x06000769 RID: 1897 RVA: 0x00013F78 File Offset: 0x00012178
	public override float FrostIntensify
	{
		get
		{
			return base.FrostIntensify;
		}
		set
		{
			base.FrostIntensify = value;
			this.enemy.HealthBar.FrostIntensify = value;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x0600076A RID: 1898 RVA: 0x00013F92 File Offset: 0x00012192
	// (set) Token: 0x0600076B RID: 1899 RVA: 0x00013F9A File Offset: 0x0001219A
	public override int TrapIntensify
	{
		get
		{
			return base.TrapIntensify;
		}
		set
		{
			base.TrapIntensify = value;
		}
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00013FA3 File Offset: 0x000121A3
	public BasicEnemyStrategy(IDamage damageTarget) : base(damageTarget)
	{
		this.enemy = (damageTarget as Enemy);
		this.ModelTrans = this.enemy.model;
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00013FC9 File Offset: 0x000121C9
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

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x0600076E RID: 1902 RVA: 0x00014009 File Offset: 0x00012209
	// (set) Token: 0x0600076F RID: 1903 RVA: 0x00014011 File Offset: 0x00012211
	public override float BuffDamageIntensify
	{
		get
		{
			return base.BuffDamageIntensify;
		}
		set
		{
			base.BuffDamageIntensify = value;
			this.enemy.HealthBar.DamageIntensify = value;
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0001402C File Offset: 0x0001222C
	public override void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResist)
	{
		base.ResetStrategy(attribute, intensify, dmgResist);
		this.MaxFrost = (1f + (float)GameRes.CurrentWave * (1f + (float)GameRes.CurrentWave / 25f)) * attribute.Frost;
		this.TrapIntensify = 0;
		this.CurrentFrost = 0f;
		this.StunTime = 0f;
		this.FrostTime = 0f;
		this.UnfrostableTime = 0f;
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x000140A4 File Offset: 0x000122A4
	public virtual void FrostEnemy(float time)
	{
		FrostEffect frostEffect = Singleton<StaticData>.Instance.FrostEffect(this.ModelTrans.position);
		frostEffect.transform.localScale = Vector3.one * 0.85f;
		this.FrostTime += time;
		this.UnfrostableTime += time + 3f;
		this.m_FrostEffect = frostEffect;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00014110 File Offset: 0x00012310
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
		if (this.StunTime > 0f)
		{
			this.StunTime -= Time.deltaTime;
		}
		if (this.UnfrostableTime > 0f)
		{
			this.UnfrostableTime -= Time.deltaTime;
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0001418D File Offset: 0x0001238D
	public override void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
	{
		base.ApplyDamage(amount, out realDamage, bullet, acceptIntensify);
		this.enemy.Buffable.OnHit();
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x000141AA File Offset: 0x000123AA
	public override void ApplyBuff(BuffInfo buffInfo)
	{
		this.enemy.Buffable.AddBuff(buffInfo);
	}

	// Token: 0x040003AA RID: 938
	protected Enemy enemy;

	// Token: 0x040003AB RID: 939
	public float UnfrostableTime;

	// Token: 0x040003AC RID: 940
	public FrostEffect m_FrostEffect;

	// Token: 0x040003AD RID: 941
	private float currentFrost;
}
