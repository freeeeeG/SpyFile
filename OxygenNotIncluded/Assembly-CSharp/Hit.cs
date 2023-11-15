using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020006E8 RID: 1768
public class Hit
{
	// Token: 0x06003077 RID: 12407 RVA: 0x001002FD File Offset: 0x000FE4FD
	public Hit(AttackProperties properties, GameObject target)
	{
		this.properties = properties;
		this.target = target;
		this.DeliverHit();
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x00100319 File Offset: 0x000FE519
	private float rollDamage()
	{
		return (float)Mathf.RoundToInt(UnityEngine.Random.Range(this.properties.base_damage_min, this.properties.base_damage_max));
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x0010033C File Offset: 0x000FE53C
	private void DeliverHit()
	{
		Health component = this.target.GetComponent<Health>();
		if (!component)
		{
			return;
		}
		this.target.Trigger(-787691065, this.properties.attacker.GetComponent<FactionAlignment>());
		float num = this.rollDamage();
		AttackableBase component2 = this.target.GetComponent<AttackableBase>();
		num *= 1f + component2.GetDamageMultiplier();
		component.Damage(num);
		if (this.properties.effects == null)
		{
			return;
		}
		Effects component3 = this.target.GetComponent<Effects>();
		if (component3)
		{
			foreach (AttackEffect attackEffect in this.properties.effects)
			{
				if (UnityEngine.Random.Range(0f, 100f) < attackEffect.effectProbability * 100f)
				{
					component3.Add(attackEffect.effectID, true);
				}
			}
		}
	}

	// Token: 0x04001CA2 RID: 7330
	private AttackProperties properties;

	// Token: 0x04001CA3 RID: 7331
	private GameObject target;
}
