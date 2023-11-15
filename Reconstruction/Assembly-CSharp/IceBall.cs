using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class IceBall : Boss
{
	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000582 RID: 1410 RVA: 0x0000F0D2 File Offset: 0x0000D2D2
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.IceBall;
		}
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0000F0D8 File Offset: 0x0000D2D8
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		this.frostCounter = this.frostCD;
		Vector3 endValue = new Vector3(0f, 0f, 360f);
		this.tween = this.rotateObj.DORotate(endValue, 4f, RotateMode.Fast).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative<TweenerCore<Quaternion, Vector3, QuaternionOptions>>();
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0000F140 File Offset: 0x0000D340
	protected override void OnEnemyUpdate()
	{
		base.OnEnemyUpdate();
		this.frostCounter -= Time.deltaTime;
		if (this.frostCounter <= 0f)
		{
			this.frostCounter = this.frostCD;
			Singleton<StaticData>.Instance.FrostTurretEffect(this.model.position, this.freezeRange, this.freezeTime);
			base.ShowBossText(0.15f);
		}
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0000F1AF File Offset: 0x0000D3AF
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.tween.Kill(false);
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0000F1C3 File Offset: 0x0000D3C3
	public override void OnDie()
	{
		base.OnDie();
		Singleton<LevelManager>.Instance.SetAchievement("ACH_BEAR");
	}

	// Token: 0x0400024B RID: 587
	public float freezeRange;

	// Token: 0x0400024C RID: 588
	public float freezeTime;

	// Token: 0x0400024D RID: 589
	private Tween tween;

	// Token: 0x0400024E RID: 590
	private float frostCounter;

	// Token: 0x0400024F RID: 591
	[SerializeField]
	private float frostCD;

	// Token: 0x04000250 RID: 592
	[SerializeField]
	private Transform rotateObj;
}
