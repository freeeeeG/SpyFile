using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class Laser : RecipeRefactor
{
	// Token: 0x06000D20 RID: 3360 RVA: 0x00021C54 File Offset: 0x0001FE54
	protected override void Shoot()
	{
		this.PlayAudio(this.ShootClip, false);
		this.turretAnim.SetTrigger("Attack");
		LaserBullet laserBullet = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab) as LaserBullet;
		laserBullet.transform.position = this.shootPoint.position;
		Vector2 value;
		if (this.Strategy.RangeType == RangeType.Line)
		{
			value = this.shootPoint.position + base.transform.up * (float)this.Strategy.FinalRange * 2f;
		}
		else
		{
			Vector2 vector = base.Target[0].transform.position - base.transform.position;
			value = this.shootPoint.position + vector.normalized * (float)this.Strategy.FinalRange * 2f;
		}
		laserBullet.Initialize(this, base.Target[0], new Vector2?(value));
	}
}
