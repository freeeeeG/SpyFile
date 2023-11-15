using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class Bullet_Snowball : ASingleTargetProjectile
{
	// Token: 0x060001C1 RID: 449 RVA: 0x000078BC File Offset: 0x00005ABC
	private void LateUpdate()
	{
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x000078C0 File Offset: 0x00005AC0
	private void Update()
	{
		if (this.state != AProjectile.eState.STARTED)
		{
			return;
		}
		this.flyTimer += Time.deltaTime;
		if (this.flyTimer < this.totalFlyTime)
		{
			base.transform.position = Vector3.Lerp(this.startPosition, base.GetFlyTargetPosition().WithY(0f), this.flyTimer / this.totalFlyTime);
			base.transform.position += this.maxFlightHeight * Vector3.up * Mathf.Sin(this.flyTimer / this.totalFlyTime * 3.1415927f);
			return;
		}
		float modifier = 0.7f;
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.ICE_TOWER_DAMAGE_INCREASE))
		{
			modifier = 0.5f;
		}
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position.WithY(0f), this.explodeRange))
		{
			amonsterBase.Hit(this.damage, eDamageType.ICE, default(Vector3));
			amonsterBase.ApplySpeedModifier(modifier, 1.5f, true);
			amonsterBase.ApplyDamageDebuff(1.5f, 1.5f, 0, eDamageType.ICE, base.GetInstanceID());
			base.OnHit(amonsterBase);
		}
		EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Explosion, base.transform.position.WithY(0f), this.explodeRange);
		Singleton<CameraManager>.Instance.ShakeCamera(0.066f, 0.004f, 0f);
		SoundManager.PlaySound("Cannon", "Cannon_Explode_1018", -1f, -1f, -1f);
		SoundManager.PlaySound("Cannon", "Cannon_Explode_2_1018", -1f, -1f, -1f);
		this.Despawn();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00007AB8 File Offset: 0x00005CB8
	public void Setup(int damage)
	{
		this.damage = damage;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00007AC4 File Offset: 0x00005CC4
	protected override void SpawnProc()
	{
		this.startPosition = base.transform.position;
		if (Vector3.Distance(base.transform.position, this.targetMonster.transform.position) < this.decreaseFlightHeightRange)
		{
			this.flyHeight = this.flyHeight.Map(0f, this.decreaseFlightHeightRange, this.maxFlightHeight * 0.33f, this.maxFlightHeight);
		}
		else
		{
			this.flyHeight = this.maxFlightHeight;
		}
		this.totalFlyTime = Vector3.Distance(this.targetMonster.HeadWorldPosition, base.transform.position) / this.speed;
		this.flyTimer = 0f;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00007B79 File Offset: 0x00005D79
	protected override void DespawnProc()
	{
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00007B7B File Offset: 0x00005D7B
	protected override void DestroyProc()
	{
	}

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private float speed = 1f;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private float maxFlightHeight = 10f;

	// Token: 0x0400014F RID: 335
	[SerializeField]
	private float decreaseFlightHeightRange = 5f;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	private float explodeRange = 3f;

	// Token: 0x04000151 RID: 337
	private float totalFlyTime;

	// Token: 0x04000152 RID: 338
	private float flyTimer;

	// Token: 0x04000153 RID: 339
	private Vector3 startPosition;

	// Token: 0x04000154 RID: 340
	private int damage;

	// Token: 0x04000155 RID: 341
	private float flyHeight;
}
