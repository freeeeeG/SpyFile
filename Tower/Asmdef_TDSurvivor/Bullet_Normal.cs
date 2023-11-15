using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class Bullet_Normal : AProjectile
{
	// Token: 0x060001A6 RID: 422 RVA: 0x00007440 File Offset: 0x00005640
	private void LateUpdate()
	{
		if (!Singleton<CameraManager>.Instance.IsInScreen(base.transform.position, 0.1f))
		{
			this.Despawn();
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00007464 File Offset: 0x00005664
	private void OnCollisionEnter(Collision other)
	{
		AMonsterBase amonsterBase;
		if (other.gameObject.TryGetComponent<AMonsterBase>(out amonsterBase))
		{
			int num = this.damage;
			amonsterBase.Hit(num, eDamageType.NONE, default(Vector3));
			base.OnHit(amonsterBase);
			this.Despawn();
			return;
		}
		this.Despawn();
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x000074AC File Offset: 0x000056AC
	public void Setup(int damage)
	{
		this.damage = damage;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x000074B5 File Offset: 0x000056B5
	protected override void SpawnProc()
	{
		this.rigidbody.isKinematic = false;
		this.rigidbody.velocity = base.transform.forward * this.speed;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x000074E4 File Offset: 0x000056E4
	protected override void DespawnProc()
	{
		this.rigidbody.velocity = Vector3.zero;
		this.rigidbody.isKinematic = true;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00007502 File Offset: 0x00005702
	protected override void DestroyProc()
	{
	}

	// Token: 0x04000139 RID: 313
	[SerializeField]
	private float speed = 1f;

	// Token: 0x0400013A RID: 314
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x0400013B RID: 315
	private int damage;
}
