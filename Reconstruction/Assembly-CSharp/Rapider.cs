using System;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class Rapider : RecipeRefactor
{
	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06000D40 RID: 3392 RVA: 0x000222B4 File Offset: 0x000204B4
	public float BulletOffset
	{
		get
		{
			return 0.02f;
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x000222BC File Offset: 0x000204BC
	protected override void Shoot()
	{
		this.turretAnim.SetTrigger("Attack");
		this.ShootEffect.Play();
		this.PlayAudio(this.ShootClip, false);
		Bullet component = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
		float d = Random.Range(-this.BulletOffset, this.BulletOffset);
		component.transform.position = this.shootPoint.position + d * this.shootPoint.right;
		Vector2 vector = component.transform.position - base.transform.position;
		Vector2 value = this.shootPoint.position + vector.normalized * ((float)this.Strategy.FinalRange + 0.25f);
		component.Initialize(this, base.Target[0], new Vector2?(value));
	}
}
