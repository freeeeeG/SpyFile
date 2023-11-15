using System;
using UnityEngine;

// Token: 0x0200021A RID: 538
public class Snow : RecipeRefactor
{
	// Token: 0x06000D51 RID: 3409 RVA: 0x0002259C File Offset: 0x0002079C
	protected override void Shoot()
	{
		this.PlayAudio(this.ShootClip, false);
		this.turretAnim.SetTrigger("Attack");
		SnowBullet snowBullet = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab) as SnowBullet;
		snowBullet.transform.position = this.shootPoint.position;
		Vector2 value;
		if (this.Strategy.RangeType == RangeType.Line)
		{
			value = this.shootPoint.position + base.transform.up * (float)this.Strategy.FinalRange;
		}
		else
		{
			Vector2 vector = base.Target[0].transform.position - base.transform.position;
			value = this.shootPoint.position + vector.normalized * (float)this.Strategy.FinalRange;
		}
		snowBullet.Initialize(this, base.Target[0], new Vector2?(value));
	}
}
