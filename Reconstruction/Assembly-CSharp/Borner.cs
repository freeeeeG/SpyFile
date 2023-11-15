using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class Borner : Boss
{
	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000543 RID: 1347 RVA: 0x0000E84E File Offset: 0x0000CA4E
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Borner;
		}
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0000E852 File Offset: 0x0000CA52
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		this.bornCounter = this.bornCD / 2f;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0000E878 File Offset: 0x0000CA78
	protected override void OnEnemyUpdate()
	{
		base.OnEnemyUpdate();
		this.bornCounter -= Time.deltaTime;
		if (this.bornCounter <= 0f)
		{
			this.bornCounter = this.bornCD;
			if (GameRes.EnemyRemain < StaticData.MaxEnemyAmount)
			{
				base.StartCoroutine(this.CastleBorn());
				base.ShowBossText(0.3f);
			}
		}
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0000E8DA File Offset: 0x0000CADA
	private IEnumerator CastleBorn()
	{
		base.DamageStrategy.StunTime += this.bornTime;
		base.DamageStrategy.ApplyBuffDmgIntensify(this.dmgIntensifyWhenBorning);
		this.anim.SetBool("Transform", true);
		Singleton<Sound>.Instance.PlayEffect("Sound_BornerTransform");
		int count = Mathf.RoundToInt((float)this.bornCount);
		int num;
		for (int i = 0; i < count; i = num + 1)
		{
			this.Born();
			yield return new WaitForSeconds(this.bornTime / (float)count);
			num = i;
		}
		this.anim.SetBool("Transform", false);
		base.DamageStrategy.ApplyBuffDmgIntensify(-this.dmgIntensifyWhenBorning);
		Singleton<Sound>.Instance.PlayEffect("Sound_BornerTransform");
		yield break;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0000E8EC File Offset: 0x0000CAEC
	private void Born()
	{
		int eType = Random.Range(this.minIndex, this.maxIndex);
		Singleton<GameManager>.Instance.SpawnEnemy((EnemyType)eType, this.PointIndex, this.Intensify / 4f * GameRes.EnemyIntensifyAdjust, this.DmgResist, BoardSystem.shortestPoints);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0000E93A File Offset: 0x0000CB3A
	public override void OnDie()
	{
		base.OnDie();
		Singleton<LevelManager>.Instance.SetAchievement("ACH_FORTRESS");
	}

	// Token: 0x04000227 RID: 551
	[SerializeField]
	private float bornCD;

	// Token: 0x04000228 RID: 552
	[SerializeField]
	protected int bornCount;

	// Token: 0x04000229 RID: 553
	[SerializeField]
	protected float bornTime;

	// Token: 0x0400022A RID: 554
	private float bornCounter;

	// Token: 0x0400022B RID: 555
	[SerializeField]
	private float dmgIntensifyWhenBorning;

	// Token: 0x0400022C RID: 556
	[SerializeField]
	protected int minIndex;

	// Token: 0x0400022D RID: 557
	[SerializeField]
	protected int maxIndex;
}
