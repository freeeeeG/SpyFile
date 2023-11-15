using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class Bullet_AOEStaticField : AProjectile
{
	// Token: 0x0600016C RID: 364 RVA: 0x00006373 File Offset: 0x00004573
	private void LateUpdate()
	{
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00006375 File Offset: 0x00004575
	public void Setup(float range, int damage)
	{
		this.range = range;
		this.damage = damage;
		this.particle_StaticField.Clear();
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00006390 File Offset: 0x00004590
	protected override void SpawnProc()
	{
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position, this.range))
		{
			amonsterBase.Hit(this.damage, eDamageType.ELECTRIC, default(Vector3));
			amonsterBase.ApplySpeedModifier(0.5f, 1.5f, true);
			base.OnHit(amonsterBase);
		}
		this.particle_StaticField.Play();
		this.Despawn();
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00006430 File Offset: 0x00004630
	protected override void DespawnProc()
	{
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00006432 File Offset: 0x00004632
	protected override void DestroyProc()
	{
	}

	// Token: 0x040000F4 RID: 244
	[SerializeField]
	private ParticleSystem particle_StaticField;

	// Token: 0x040000F5 RID: 245
	private float range;

	// Token: 0x040000F6 RID: 246
	private int damage;
}
