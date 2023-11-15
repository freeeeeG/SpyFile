using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class Boomerrang : RecipeRefactor
{
	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00021933 File Offset: 0x0001FB33
	public float FlyTime
	{
		get
		{
			return (float)this.Strategy.FinalRange / ((float)this.Strategy.FinalRange + 4f) * 4f;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0002195A File Offset: 0x0001FB5A
	// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00021962 File Offset: 0x0001FB62
	private float RotSpeed
	{
		get
		{
			return this.rotSpeed;
		}
		set
		{
			this.rotSpeed = value;
		}
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x0002196B File Offset: 0x0001FB6B
	protected override void RotateTowards()
	{
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x0002196D File Offset: 0x0001FB6D
	protected override bool AngleCheck()
	{
		return Quaternion.Angle(this.rotTrans.rotation, this.look_Rotation) < 360f;
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x0002198F File Offset: 0x0001FB8F
	protected override void FireProjectile()
	{
		if (this.bulletOut)
		{
			return;
		}
		base.FireProjectile();
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x000219A0 File Offset: 0x0001FBA0
	public override bool GameUpdate()
	{
		base.GameUpdate();
		this.SelfRotateControl();
		return true;
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x000219B0 File Offset: 0x0001FBB0
	public override void OnSpawn()
	{
		base.OnSpawn();
		this.CannonSprite.transform.rotation = Quaternion.identity;
		this.cannonBullet = this.CannonSprite.GetComponent<SelfBullet>();
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x000219DE File Offset: 0x0001FBDE
	private void SelfRotateControl()
	{
		this.cannonBullet.transform.Rotate(Vector3.forward * this.RotSpeed * Time.deltaTime, Space.Self);
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x00021A0B File Offset: 0x0001FC0B
	protected override void Shoot()
	{
		base.StartCoroutine(this.ShootCor());
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x00021A1A File Offset: 0x0001FC1A
	private IEnumerator ShootCor()
	{
		this.bulletOut = true;
		this.cannonBullet.Initialize(this, base.Target[0], null);
		this.cannonBullet.UnFrostEffect = ((BoomerangStrategy)this.Strategy).UnfrostEffect;
		this.bulletInitScale = this.cannonBullet.transform.localScale;
		this.PlayAudio(this.ShootClip, false);
		this.initPos = this.cannonBullet.transform.position;
		if (this.Strategy.RangeType == RangeType.Line)
		{
			this.pos = this.initPos + base.transform.up * (float)this.Strategy.FinalRange;
		}
		else
		{
			this.dir = base.Target[0].transform.position - base.transform.position;
			this.pos = this.initPos + this.dir.normalized * (float)this.Strategy.FinalRange;
		}
		DOTween.To(() => this.RotSpeed, delegate(float x)
		{
			this.RotSpeed = x;
		}, -720f, this.FlyTime).SetEase(Ease.OutCubic);
		this.cannonBullet.transform.DOMove(this.pos, this.FlyTime, false).SetEase(Ease.OutCubic);
		this.cannonBullet.transform.DOScale(this.bulletInitScale * (1f + this.Strategy.FinalSplashRange / (this.Strategy.FinalSplashRange + 8f) * 5f) * (1f + this.Strategy.FinalBulletSize), this.FlyTime).SetEase(Ease.OutCubic);
		yield return new WaitForSeconds(this.FlyTime);
		this.cannonBullet.isCritical = true;
		DOTween.To(() => this.RotSpeed, delegate(float x)
		{
			this.RotSpeed = x;
		}, -360f, this.FlyTime).SetEase(Ease.InCubic);
		this.cannonBullet.transform.DOMove(this.initPos, this.FlyTime, false).SetEase(Ease.InCubic);
		this.cannonBullet.transform.DOScale(this.bulletInitScale, this.FlyTime).SetEase(Ease.InCubic);
		yield return new WaitForSeconds(this.FlyTime);
		this.bulletOut = false;
		yield break;
	}

	// Token: 0x04000650 RID: 1616
	private float rotSpeed = -360f;

	// Token: 0x04000651 RID: 1617
	private bool bulletOut;

	// Token: 0x04000652 RID: 1618
	private SelfBullet cannonBullet;

	// Token: 0x04000653 RID: 1619
	private Vector3 bulletInitScale;

	// Token: 0x04000654 RID: 1620
	private Vector2 dir;

	// Token: 0x04000655 RID: 1621
	private Vector3 pos;

	// Token: 0x04000656 RID: 1622
	private Vector3 initPos;
}
