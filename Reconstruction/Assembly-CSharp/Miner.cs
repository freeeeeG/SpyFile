using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class Miner : RecipeRefactor
{
	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06000D2F RID: 3375 RVA: 0x000220C0 File Offset: 0x000202C0
	// (set) Token: 0x06000D30 RID: 3376 RVA: 0x000220C8 File Offset: 0x000202C8
	public float DeployInterval { get; set; }

	// Token: 0x06000D31 RID: 3377 RVA: 0x000220D1 File Offset: 0x000202D1
	protected override bool TrackTarget()
	{
		return true;
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x000220D4 File Offset: 0x000202D4
	protected override void FireProjectile()
	{
		if (Time.time - base.NextAttackTime > this.DeployInterval)
		{
			this.Deploy();
			base.NextAttackTime = Time.time;
		}
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x000220FB File Offset: 0x000202FB
	public void Deploy()
	{
		this.Shoot();
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00022104 File Offset: 0x00020304
	protected override void Shoot()
	{
		List<BasicTile> list = new List<BasicTile>();
		foreach (BasicTile basicTile in this.PathTiles)
		{
			if (!basicTile.hasMine)
			{
				list.Add(basicTile);
			}
		}
		if (list.Count > 0)
		{
			BasicTile basicTile2 = list[Random.Range(0, list.Count)];
			this.turretAnim.SetTrigger("Attack");
			this.ShootEffect.Play();
			this.PlayAudio(this.ShootClip, false);
			MinerBullet minerBullet = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab) as MinerBullet;
			minerBullet.transform.position = this.shootPoint.position;
			minerBullet.Initialize(this, null, new Vector2?(basicTile2.transform.position));
			minerBullet.TargetTile = basicTile2;
			basicTile2.hasMine = true;
		}
	}

	// Token: 0x04000666 RID: 1638
	public List<BasicTile> PathTiles;
}
