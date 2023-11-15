using System;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class RestorerStrategy : BasicEnemyStrategy
{
	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x0001464F File Offset: 0x0001284F
	public override bool IsEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00014652 File Offset: 0x00012852
	public RestorerStrategy(IDamage damageTarget) : base(damageTarget)
	{
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0001465B File Offset: 0x0001285B
	public override void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
	{
		base.ApplyDamage(amount, out realDamage, bullet, acceptIntensify);
		this.damagedCounter = 0f;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00014674 File Offset: 0x00012874
	public override void StrategyUpdate()
	{
		base.StrategyUpdate();
		this.damagedCounter += Time.deltaTime;
		if (this.damagedCounter > 2f)
		{
			this.CurrentHealth += this.MaxHealth * 0.05f * Time.deltaTime;
		}
	}

	// Token: 0x040003B6 RID: 950
	public float damagedCounter;
}
