using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class GoldKeeperStrategy : BasicEnemyStrategy
{
	// Token: 0x170002FE RID: 766
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00014785 File Offset: 0x00012985
	public override bool IsEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00014788 File Offset: 0x00012988
	// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00014790 File Offset: 0x00012990
	public override float CurrentHealth
	{
		get
		{
			return base.CurrentHealth;
		}
		set
		{
			base.CurrentHealth = value;
			int num = (int)((1f - this.healthInterval * (float)this.gainCount - this.currentHealth / this.MaxHealth) / this.healthInterval);
			if (num > 0)
			{
				this.GainGold(num);
			}
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000147DA File Offset: 0x000129DA
	// (set) Token: 0x060007A7 RID: 1959 RVA: 0x000147E2 File Offset: 0x000129E2
	public override bool IsDie
	{
		get
		{
			return base.IsDie;
		}
		set
		{
			base.IsDie = value;
			if (value)
			{
				GameRes.GainPerfectBattleTurn++;
				Singleton<StaticData>.Instance.GainPerfectEffect(this.enemy.model.position, 1);
			}
		}
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0001481C File Offset: 0x00012A1C
	private void GainGold(int count)
	{
		this.gainCount += count;
		Singleton<StaticData>.Instance.GainMoneyEffect(this.enemy.model.position, Mathf.Min(10, Mathf.RoundToInt((float)GameRes.CurrentWave * 0.4f)) * count);
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x00014871 File Offset: 0x00012A71
	public GoldKeeperStrategy(IDamage damageTarget) : base(damageTarget)
	{
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0001488C File Offset: 0x00012A8C
	public override void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResist)
	{
		base.ResetStrategy(attribute, intensify, dmgResist);
		this.gainCount = 1;
	}

	// Token: 0x040003BA RID: 954
	private int gainCount = 1;

	// Token: 0x040003BB RID: 955
	private float healthInterval = 0.02f;
}
