using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class CheetahStrateygy : BasicEnemyStrategy
{
	// Token: 0x170002FC RID: 764
	// (get) Token: 0x0600079C RID: 1948 RVA: 0x000146D1 File Offset: 0x000128D1
	public override bool IsEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x000146D4 File Offset: 0x000128D4
	public CheetahStrateygy(IDamage damageTarget) : base(damageTarget)
	{
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x000146E8 File Offset: 0x000128E8
	public override void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResist)
	{
		base.ResetStrategy(attribute, intensify, dmgResist);
		this.speedIntensified = 0f;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00014700 File Offset: 0x00012900
	public override void StrategyUpdate()
	{
		base.StrategyUpdate();
		if (this.speedIntensified < this.maxSpeedIntensify)
		{
			float num = 0.35f * Time.deltaTime;
			this.enemy.SpeedIntensify += num;
			this.speedIntensified += num;
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0001474E File Offset: 0x0001294E
	public override void UnFrost()
	{
		base.UnFrost();
		this.enemy.SpeedIntensify -= this.speedIntensified;
		this.speedIntensified = 0f;
	}

	// Token: 0x040003B7 RID: 951
	private float speedIntensified;

	// Token: 0x040003B8 RID: 952
	private float maxSpeedIntensify = 1.5f;
}
