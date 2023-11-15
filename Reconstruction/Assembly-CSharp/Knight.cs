using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class Knight : Boss
{
	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x0000F1E2 File Offset: 0x0000D3E2
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Knight;
		}
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0000F1E6 File Offset: 0x0000D3E6
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		base.ShowBossText(1f);
		this.weaponInScene = 0;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0000F20C File Offset: 0x0000D40C
	protected override void OnEnemyUpdate()
	{
		base.OnEnemyUpdate();
		if (this.weaponInScene >= this.maxWeaponCount)
		{
			return;
		}
		this.level1Counter += Time.deltaTime;
		if (this.level1Counter > this.levelInterval[0])
		{
			base.StartCoroutine(this.SummonWeapon(1));
			this.level1Counter = 0f;
		}
		this.level2Counter += Time.deltaTime;
		if (this.level2Counter > this.levelInterval[1])
		{
			base.StartCoroutine(this.SummonWeapon(2));
			this.level2Counter = 0f;
		}
		this.level3Counter += Time.deltaTime;
		if (this.level3Counter > this.levelInterval[2])
		{
			base.StartCoroutine(this.SummonWeapon(3));
			this.level3Counter = 0f;
		}
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0000F2DF File Offset: 0x0000D4DF
	private IEnumerator SummonWeapon(int level)
	{
		this.weaponInScene++;
		Weapon weapon = null;
		float d = 0f;
		switch (level)
		{
		case 1:
			weapon = this.level1Weapon[Random.Range(0, this.level1Weapon.Length)];
			d = 8f;
			break;
		case 2:
			weapon = this.level2Weapon[Random.Range(0, this.level2Weapon.Length)];
			d = 15f;
			break;
		case 3:
			weapon = this.level3Weapon[Random.Range(0, this.level3Weapon.Length)];
			d = 20f;
			break;
		}
		Vector2 pos = base.transform.position + Random.insideUnitCircle.normalized * d;
		ParticalControl particalControl = Singleton<ObjectPool>.Instance.Spawn(this.summonEffect) as ParticalControl;
		particalControl.transform.position = pos;
		particalControl.PlayEffect();
		yield return new WaitForSeconds(1f);
		if (!base.DamageStrategy.IsDie)
		{
			weapon = (Singleton<ObjectPool>.Instance.Spawn(weapon) as Weapon);
			weapon.transform.localScale = Vector2.zero;
			weapon.transform.DOScale(Vector2.one, 1f);
			weapon.transform.position = pos;
			weapon.Initiate(this);
		}
		yield break;
	}

	// Token: 0x04000251 RID: 593
	[Header("武器")]
	[SerializeField]
	protected Weapon[] level1Weapon;

	// Token: 0x04000252 RID: 594
	[SerializeField]
	protected Weapon[] level2Weapon;

	// Token: 0x04000253 RID: 595
	[SerializeField]
	protected Weapon[] level3Weapon;

	// Token: 0x04000254 RID: 596
	[SerializeField]
	private ParticalControl summonEffect;

	// Token: 0x04000255 RID: 597
	private float level1Counter;

	// Token: 0x04000256 RID: 598
	private float level2Counter;

	// Token: 0x04000257 RID: 599
	private float level3Counter;

	// Token: 0x04000258 RID: 600
	public int weaponInScene;

	// Token: 0x04000259 RID: 601
	[SerializeField]
	private int maxWeaponCount = 6;

	// Token: 0x0400025A RID: 602
	[SerializeField]
	protected float[] levelInterval;
}
