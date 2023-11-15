using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000218 RID: 536
public class Scatter : RecipeRefactor
{
	// Token: 0x06000D4C RID: 3404 RVA: 0x000224D8 File Offset: 0x000206D8
	protected override void Shoot()
	{
		base.StartCoroutine(this.ShootCor());
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x000224E7 File Offset: 0x000206E7
	private IEnumerator ShootCor()
	{
		this.fireInterval = 0.25f / (this.Strategy.FinalFireRate * (float)this.Strategy.FinalTargetCount);
		foreach (TargetPoint targetPoint in base.Target.ToList<TargetPoint>())
		{
			if (targetPoint.gameObject.activeSelf)
			{
				this.turretAnim.SetTrigger("Attack");
				this.PlayAudio(this.ShootClip, false);
				Bullet component = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
				component.transform.position = (this.shootDir ? this.shootPoint1.position : this.shootPoint2.position);
				(this.shootDir ? this.ShootEffect : this.shootEffect2).Play();
				this.shootDir = !this.shootDir;
				component.Initialize(this, targetPoint, null);
				yield return new WaitForSeconds(this.fireInterval);
			}
		}
		List<TargetPoint>.Enumerator enumerator = default(List<TargetPoint>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x000224F8 File Offset: 0x000206F8
	public override void SetGraphic()
	{
		base.SetGraphic();
		this.shootPoint1.position = this.shootPoint.position + this.shootPoint.right * 0.16f;
		this.shootPoint2.position = this.shootPoint.position - this.shootPoint.right * 0.16f;
	}

	// Token: 0x0400066D RID: 1645
	private const float ShootPointSideOffset = 0.16f;

	// Token: 0x0400066E RID: 1646
	private float fireInterval;

	// Token: 0x0400066F RID: 1647
	[SerializeField]
	private ParticleSystem shootEffect2;

	// Token: 0x04000670 RID: 1648
	[SerializeField]
	private Transform shootPoint1;

	// Token: 0x04000671 RID: 1649
	[SerializeField]
	private Transform shootPoint2;

	// Token: 0x04000672 RID: 1650
	private bool shootDir;
}
