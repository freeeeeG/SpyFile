using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class AircraftCarrier : Boss
{
	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060004FD RID: 1277 RVA: 0x0000DC77 File Offset: 0x0000BE77
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.AircraftCarrier;
		}
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0000DC7C File Offset: 0x0000BE7C
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		this.bornCD = 1f;
		this.airCrifatAmount = Mathf.Min(15, 4 + (GameRes.CurrentWave + 1) / 20);
		this.dmgIntenWhenAircraftDie = 1f / (float)this.airCrifatAmount;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x0000DCD0 File Offset: 0x0000BED0
	protected override void OnEnemyUpdate()
	{
		base.OnEnemyUpdate();
		if (this.bornCounter < this.bornCD)
		{
			this.bornCounter += Time.deltaTime;
			if (this.bornCounter > this.bornCD)
			{
				base.StartCoroutine(this.BornCor());
				base.ShowBossText(1f);
			}
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0000DD29 File Offset: 0x0000BF29
	private IEnumerator BornCor()
	{
		this.anim.SetBool("Transform", true);
		Singleton<Sound>.Instance.PlayEffect("Sound_BornerTransform");
		yield return new WaitForSeconds(0.5f);
		int num;
		for (int i = 0; i < Mathf.RoundToInt((float)this.airCrifatAmount); i = num + 1)
		{
			AirAttacker airAttacker = Singleton<ObjectPool>.Instance.Spawn(this.battleAirCraft) as AirAttacker;
			airAttacker.transform.position = this.model.position;
			airAttacker.Initiate(this, base.DamageStrategy.MaxHealth * this.aircarftIntensify * GameRes.EnemyIntensifyAdjust, this.dmgIntenWhenAircraftDie, this.DmgResist);
			Singleton<Sound>.Instance.PlayEffect("Sound_Aircraft");
			yield return new WaitForSeconds(0.3f);
			num = i;
		}
		yield return new WaitForSeconds(0.5f);
		this.anim.SetBool("Transform", false);
		yield break;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0000DD38 File Offset: 0x0000BF38
	public void AddAircraft(Aircraft a)
	{
		this.aircrafts.Add(a);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0000DD46 File Offset: 0x0000BF46
	public override void OnDie()
	{
		base.OnDie();
		Singleton<LevelManager>.Instance.SetAchievement("ACH_BATTLESHIP");
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0000DD60 File Offset: 0x0000BF60
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.bornCounter = 0f;
		for (int i = 0; i < this.aircrafts.Count; i++)
		{
			this.aircrafts[i].DamageStrategy.CurrentHealth = 0f;
		}
		this.aircrafts.Clear();
	}

	// Token: 0x040001FF RID: 511
	[SerializeField]
	private float aircarftIntensify;

	// Token: 0x04000200 RID: 512
	private List<Aircraft> aircrafts = new List<Aircraft>();

	// Token: 0x04000201 RID: 513
	private float bornCounter;

	// Token: 0x04000202 RID: 514
	private float bornCD;

	// Token: 0x04000203 RID: 515
	protected float dmgIntenWhenAircraftDie;

	// Token: 0x04000204 RID: 516
	[SerializeField]
	private Aircraft battleAirCraft;

	// Token: 0x04000205 RID: 517
	[SerializeField]
	private int airCrifatAmount;
}
