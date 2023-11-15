using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006E9 RID: 1769
[AddComponentMenu("KMonoBehaviour/scripts/Weapon")]
public class Weapon : KMonoBehaviour
{
	// Token: 0x0600307A RID: 12410 RVA: 0x0010043C File Offset: 0x000FE63C
	public void Configure(float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		this.properties = new AttackProperties();
		this.properties.base_damage_min = base_damage_min;
		this.properties.base_damage_max = base_damage_max;
		this.properties.maxHits = maxHits;
		this.properties.damageType = attackType;
		this.properties.aoe_radius = aoeRadius;
		this.properties.attacker = this;
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x0010049E File Offset: 0x000FE69E
	public void AddEffect(string effectID = "WasAttacked", float probability = 1f)
	{
		if (this.properties.effects == null)
		{
			this.properties.effects = new List<AttackEffect>();
		}
		this.properties.effects.Add(new AttackEffect(effectID, probability));
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x001004D4 File Offset: 0x000FE6D4
	public int AttackArea(Vector3 centerPoint)
	{
		Vector3 b = Vector3.zero;
		this.alignment = base.GetComponent<FactionAlignment>();
		if (this.alignment == null)
		{
			return 0;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Health health in Components.Health.Items)
		{
			if (!(health.gameObject == base.gameObject) && !health.IsDefeated())
			{
				FactionAlignment component = health.GetComponent<FactionAlignment>();
				if (!(component == null) && component.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, component.Alignment) == FactionManager.Disposition.Attack)
				{
					b = health.transform.GetPosition();
					b.z = centerPoint.z;
					if (Vector3.Distance(centerPoint, b) <= this.properties.aoe_radius)
					{
						list.Add(health.gameObject);
					}
				}
			}
		}
		this.AttackTargets(list.ToArray());
		return list.Count;
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x001005FC File Offset: 0x000FE7FC
	public void AttackTarget(GameObject target)
	{
		this.AttackTargets(new GameObject[]
		{
			target
		});
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x0010060E File Offset: 0x000FE80E
	public void AttackTargets(GameObject[] targets)
	{
		if (this.properties == null)
		{
			global::Debug.LogWarning(string.Format("Attack properties not configured. {0} cannot attack with weapon.", base.gameObject.name));
			return;
		}
		new Attack(this.properties, targets);
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x00100640 File Offset: 0x000FE840
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.properties.attacker = this;
	}

	// Token: 0x04001CA4 RID: 7332
	[MyCmpReq]
	private FactionAlignment alignment;

	// Token: 0x04001CA5 RID: 7333
	public AttackProperties properties;
}
