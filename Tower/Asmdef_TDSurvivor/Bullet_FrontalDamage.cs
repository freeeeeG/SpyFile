using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class Bullet_FrontalDamage : AProjectile
{
	// Token: 0x06000199 RID: 409 RVA: 0x00006FFE File Offset: 0x000051FE
	private void LateUpdate()
	{
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00007000 File Offset: 0x00005200
	public void Setup(float range, float width, int damage, eDamageType damageType)
	{
		this.range = range;
		this.boxCenter = base.transform.position + base.transform.forward * (0.5f * range);
		this.halfExtent = new Vector3(width * 0.5f, 2f, range * 0.5f);
		this.damage = damage;
		this.damageType = damageType;
		this.boxRotation = Quaternion.LookRotation(base.transform.forward);
		this.isSetupDone = true;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000708C File Offset: 0x0000528C
	protected override void SpawnProc()
	{
		Collider[] array = Physics.OverlapBox(this.boxCenter, this.halfExtent, base.transform.rotation, LayerMask.GetMask(new string[]
		{
			"Monsters"
		}));
		Debug.DrawLine(this.boxCenter - base.transform.forward * this.range * 0.5f, this.boxCenter + base.transform.forward * this.range * 0.5f, Colors.White, 2f);
		Monster_Basic monster_Basic = null;
		Collider[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i].TryGetComponent<Monster_Basic>(out monster_Basic))
			{
				monster_Basic.Hit(this.damage, this.damageType, default(Vector3));
				base.OnHit(monster_Basic);
			}
			monster_Basic = null;
		}
		this.Despawn();
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00007174 File Offset: 0x00005374
	protected override void DespawnProc()
	{
		this.isSetupDone = false;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000717D File Offset: 0x0000537D
	protected override void DestroyProc()
	{
	}

	// Token: 0x04000127 RID: 295
	private Vector3 boxCenter;

	// Token: 0x04000128 RID: 296
	private Vector3 halfExtent;

	// Token: 0x04000129 RID: 297
	private int damage;

	// Token: 0x0400012A RID: 298
	private eDamageType damageType;

	// Token: 0x0400012B RID: 299
	private Quaternion boxRotation;

	// Token: 0x0400012C RID: 300
	private float range;

	// Token: 0x0400012D RID: 301
	private bool isSetupDone;
}
