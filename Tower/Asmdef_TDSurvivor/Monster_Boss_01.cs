using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class Monster_Boss_01 : Monster_Basic
{
	// Token: 0x06000440 RID: 1088 RVA: 0x00010A34 File Offset: 0x0000EC34
	private new void Start()
	{
		base.Start();
		MonsterSpawner spawner = Object.FindObjectOfType<GameSceneReferenceHandler>().List_MonsterSpawners[0];
		this.Spawn(spawner, true);
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x00010A60 File Offset: 0x0000EC60
	public override void Spawn(MonsterSpawner spawner, bool isCorrupted)
	{
		if (this.list_SpeedModifier == null)
		{
			this.list_SpeedModifier = new List<AMonsterBase.MonsterSpeedModifier>();
		}
		this.list_SpeedModifier.Clear();
		if (this.list_MonsterDamageDebuff == null)
		{
			this.list_MonsterDamageDebuff = new List<MonsterDamageDebuff>();
		}
		this.list_MonsterDamageDebuff.Clear();
		this.monsterSpawner = spawner;
		this.hp = this.monsterData.GetMaxHP(1f);
		this.speed = this.monsterData.GetMoveSpeed(1f);
		this.isImpendingDeath = false;
		this.timeSinceSpawn = 0f;
		this.aiPath.canMove = false;
		this.aiPath.maxSpeed = 1f;
		this.aiPath.canSearch = false;
		this.aiPath.rotationSpeed = 360f * Mathf.Max(1f, this.speed / 5f);
		this.aiPath.constrainInsideGraph = true;
		this.SpawnProc();
		this.collider.enabled = true;
		if (this.node_Model != null)
		{
			this.node_Model.SetActive(true);
		}
		EventMgr.SendEvent<AMonsterBase>(eGameEvents.MonsterSpawn, this);
		this.state = AMonsterBase.eState.ALIVE;
		base.ToggleMoveable(false);
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x00010B91 File Offset: 0x0000ED91
	public override void Hit(int damage, eDamageType damageType, Vector3 hitDirection = default(Vector3))
	{
		if (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			return;
		}
		base.Hit(damage, damageType, hitDirection);
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x00010BA9 File Offset: 0x0000EDA9
	protected override void HitProc(int damage)
	{
		base.HitProc(damage);
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00010BB2 File Offset: 0x0000EDB2
	protected override void SpawnProc()
	{
		base.SpawnProc();
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x00010BBA File Offset: 0x0000EDBA
	protected override void UpdateProc(float deltaTime)
	{
		base.UpdateProc(deltaTime);
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00010BC3 File Offset: 0x0000EDC3
	protected override IEnumerator DeathProc(int damage, bool playAnimation = true)
	{
		this.particle_ConeDarkFlame.Stop();
		yield return base.StartCoroutine(base.DeathProc(damage, playAnimation));
		yield break;
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00010BE0 File Offset: 0x0000EDE0
	protected override void ReachEndOfPathProc()
	{
	}

	// Token: 0x04000430 RID: 1072
	public new float RemainingDistance = 1f;

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private ParticleSystem particle_ConeDarkFlame;
}
