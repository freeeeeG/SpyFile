using System;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class BomBard : RecipeRefactor
{
	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x000217E4 File Offset: 0x0001F9E4
	// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x000217EC File Offset: 0x0001F9EC
	public int BulletCount
	{
		get
		{
			return this.bulletCount;
		}
		set
		{
			this.bulletCount = value;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06000CFA RID: 3322 RVA: 0x000217F5 File Offset: 0x0001F9F5
	// (set) Token: 0x06000CFB RID: 3323 RVA: 0x000217FD File Offset: 0x0001F9FD
	public float BulletOffset
	{
		get
		{
			return this.bulletOffset;
		}
		set
		{
			this.bulletOffset = value;
		}
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x00021808 File Offset: 0x0001FA08
	protected override void Shoot()
	{
		this.turretAnim.SetTrigger("Attack");
		this.ShootEffect.Play();
		this.PlayAudio(this.ShootClip, false);
		foreach (TargetPoint target in base.Target)
		{
			this.ShootMultiBullets(target);
		}
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x00021864 File Offset: 0x0001FA64
	private void ShootMultiBullets(TargetPoint target)
	{
		Vector2.Distance(target.Position, base.transform.position);
		for (int i = 0; i < this.BulletCount; i++)
		{
			Bullet bullet = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab) as Bullet;
			bullet.transform.position = this.shootPoint.position;
			Vector2 b = Random.insideUnitCircle.normalized * this.BulletOffset;
			bullet.Initialize(this, target, new Vector2?(target.Position + b));
			bullet.BulletSpeed *= Random.Range(0.9f, 1.1f);
		}
	}

	// Token: 0x0400064E RID: 1614
	private int bulletCount = 3;

	// Token: 0x0400064F RID: 1615
	private float bulletOffset = 1.2f;
}
