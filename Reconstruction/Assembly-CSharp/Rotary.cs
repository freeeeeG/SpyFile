using System;
using UnityEngine;

// Token: 0x02000217 RID: 535
public class Rotary : RecipeRefactor
{
	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06000D43 RID: 3395 RVA: 0x000223C4 File Offset: 0x000205C4
	// (set) Token: 0x06000D44 RID: 3396 RVA: 0x000223E2 File Offset: 0x000205E2
	private float RotSpeed
	{
		get
		{
			return Mathf.Max(-720f, this.rotSpeed * this.Strategy.FinalFireRate);
		}
		set
		{
			this.rotSpeed = value;
		}
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x000223EB File Offset: 0x000205EB
	public override void InitializeTurret(StrategyBase strategy)
	{
		base.InitializeTurret(strategy);
		this.Strategy.CheckAngle = 360f;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x00022404 File Offset: 0x00020604
	protected override void RotateTowards()
	{
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00022406 File Offset: 0x00020606
	public override bool GameUpdate()
	{
		base.GameUpdate();
		this.SelfRotateControl();
		return true;
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00022416 File Offset: 0x00020616
	public override void OnSpawn()
	{
		base.OnSpawn();
		this.rotTrans.rotation = Quaternion.identity;
		this.RotSpeed = 120f;
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x00022439 File Offset: 0x00020639
	private void SelfRotateControl()
	{
		this.rotTrans.Rotate(Vector3.forward * -this.RotSpeed * Time.deltaTime, Space.Self);
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x00022464 File Offset: 0x00020664
	protected override void Shoot()
	{
		Bullet component = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
		this.PlayAudio(this.ShootClip, false);
		component.transform.position = this.shootPoint.position;
		component.Initialize(this, base.Target[0], new Vector2?(base.transform.position));
	}

	// Token: 0x0400066C RID: 1644
	private float rotSpeed;
}
