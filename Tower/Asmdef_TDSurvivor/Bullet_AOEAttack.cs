using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class Bullet_AOEAttack : AProjectile
{
	// Token: 0x0600015F RID: 351 RVA: 0x00006178 File Offset: 0x00004378
	private void LateUpdate()
	{
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000617A File Offset: 0x0000437A
	public void Setup(float range, int damage)
	{
		this.range = range;
		this.damage = damage;
		this.particle_ShockWave.Clear();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00006198 File Offset: 0x00004398
	protected override void SpawnProc()
	{
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position, this.range))
		{
			amonsterBase.Hit(this.damage, eDamageType.NONE, default(Vector3));
			amonsterBase.ApplySpeedModifier(0.1f, 0.5f, true);
			base.OnHit(amonsterBase);
		}
		this.particle_ShockWave.Play();
		this.Despawn();
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00006238 File Offset: 0x00004438
	protected override void DespawnProc()
	{
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000623A File Offset: 0x0000443A
	protected override void DestroyProc()
	{
	}

	// Token: 0x040000EC RID: 236
	[SerializeField]
	private ParticleSystem particle_ShockWave;

	// Token: 0x040000ED RID: 237
	private float range;

	// Token: 0x040000EE RID: 238
	private int damage;
}
