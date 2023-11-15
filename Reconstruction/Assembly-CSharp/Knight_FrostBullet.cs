using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class Knight_FrostBullet : ReusableObject, IGameBehavior
{
	// Token: 0x0600058F RID: 1423 RVA: 0x0000F310 File Offset: 0x0000D510
	public bool GameUpdate()
	{
		if (Vector2.Distance(base.transform.position, this.targetPos) < 0.1f)
		{
			Singleton<StaticData>.Instance.FrostTurretEffect(base.transform.position, this.frostRange, this.frostTime);
			Singleton<ObjectPool>.Instance.UnSpawn(this);
			return false;
		}
		base.transform.position = Vector2.MoveTowards(base.transform.position, this.targetPos, this.bulletSpeed * Time.deltaTime);
		return true;
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0000F3AA File Offset: 0x0000D5AA
	public void SetBullet(Vector2 targetPos, float frostRange, float frostTime)
	{
		this.targetPos = targetPos;
		this.frostRange = frostRange;
		this.frostTime = frostTime;
		this.bulletSpeed = Random.Range(3f, 6f);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0000F3D6 File Offset: 0x0000D5D6
	public override void OnSpawn()
	{
		base.OnSpawn();
		Singleton<GameManager>.Instance.nonEnemies.Add(this);
	}

	// Token: 0x0400025B RID: 603
	private Vector2 targetPos;

	// Token: 0x0400025C RID: 604
	private float bulletSpeed = 5f;

	// Token: 0x0400025D RID: 605
	private float frostRange;

	// Token: 0x0400025E RID: 606
	private float frostTime;
}
