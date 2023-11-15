using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class Bullet_Boulder : ASingleTargetProjectile
{
	// Token: 0x06000172 RID: 370 RVA: 0x0000643C File Offset: 0x0000463C
	private void LateUpdate()
	{
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00006440 File Offset: 0x00004640
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
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position.WithY(0f), this.explodeRange))
		{
			amonsterBase.Hit(this.damage, eDamageType.EARTH, default(Vector3));
			base.OnHit(amonsterBase);
		}
		EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Explosion, base.transform.position.WithY(0f), this.explodeRange);
		Singleton<CameraManager>.Instance.ShakeCamera(0.066f, 0.004f, 0f);
		SoundManager.PlaySound("Cannon", "Cannon_Explode_1005", -1f, -1f, -1f);
		this.Despawn();
	}

	// Token: 0x06000174 RID: 372 RVA: 0x000065D4 File Offset: 0x000047D4
	public void Setup(int damage)
	{
		this.damage = damage;
	}

	// Token: 0x06000175 RID: 373 RVA: 0x000065E0 File Offset: 0x000047E0
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

	// Token: 0x06000176 RID: 374 RVA: 0x00006695 File Offset: 0x00004895
	protected override void DespawnProc()
	{
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00006697 File Offset: 0x00004897
	protected override void DestroyProc()
	{
	}

	// Token: 0x040000F7 RID: 247
	[SerializeField]
	private float speed = 1f;

	// Token: 0x040000F8 RID: 248
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x040000F9 RID: 249
	[SerializeField]
	private float maxFlightHeight = 10f;

	// Token: 0x040000FA RID: 250
	[SerializeField]
	private float decreaseFlightHeightRange = 5f;

	// Token: 0x040000FB RID: 251
	[SerializeField]
	private float explodeRange = 3f;

	// Token: 0x040000FC RID: 252
	private float totalFlyTime;

	// Token: 0x040000FD RID: 253
	private float flyTimer;

	// Token: 0x040000FE RID: 254
	private Vector3 startPosition;

	// Token: 0x040000FF RID: 255
	private int damage;

	// Token: 0x04000100 RID: 256
	private float flyHeight;
}
