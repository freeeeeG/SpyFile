using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class Bullet_AOEFreeze : AProjectile
{
	// Token: 0x06000165 RID: 357 RVA: 0x00006244 File Offset: 0x00004444
	private void LateUpdate()
	{
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00006246 File Offset: 0x00004446
	public void Setup(float range, int damage)
	{
		this.range = range;
		this.damage = damage;
		this.SetEffectScale(range / this.originalRange);
		this.particle_FreezeWave.Clear();
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000626F File Offset: 0x0000446F
	private void SetEffectScale(float scale)
	{
		this.node_ParticleScale.localScale = Vector3.one * scale;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00006288 File Offset: 0x00004488
	protected override void SpawnProc()
	{
		List<AMonsterBase> monstersInRange = Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position, this.range);
		float modifier = 0.7f;
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.ICE_TOWER_DAMAGE_INCREASE))
		{
			modifier = 0.5f;
		}
		foreach (AMonsterBase amonsterBase in monstersInRange)
		{
			amonsterBase.Hit(this.damage, eDamageType.ICE, default(Vector3));
			amonsterBase.ApplySpeedModifier(modifier, 1.5f, true);
			amonsterBase.ApplyDamageDebuff(1.5f, 1.5f, 0, eDamageType.ICE, base.GetInstanceID());
			base.OnHit(amonsterBase);
		}
		this.particle_FreezeWave.Play();
		this.Despawn();
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000635C File Offset: 0x0000455C
	protected override void DespawnProc()
	{
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000635E File Offset: 0x0000455E
	protected override void DestroyProc()
	{
	}

	// Token: 0x040000EF RID: 239
	[SerializeField]
	private ParticleSystem particle_FreezeWave;

	// Token: 0x040000F0 RID: 240
	[SerializeField]
	private Transform node_ParticleScale;

	// Token: 0x040000F1 RID: 241
	[SerializeField]
	private float originalRange = 3f;

	// Token: 0x040000F2 RID: 242
	private float range;

	// Token: 0x040000F3 RID: 243
	private int damage;
}
