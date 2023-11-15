using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class Monster_TrainingDummy : Monster_Basic
{
	// Token: 0x0600044F RID: 1103 RVA: 0x00010CA4 File Offset: 0x0000EEA4
	private new void Start()
	{
		base.Start();
		MonsterSpawner spawner = Object.FindObjectOfType<GameSceneReferenceHandler>().List_MonsterSpawners[0];
		this.Spawn(spawner, false);
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00010CD0 File Offset: 0x0000EED0
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
		this.SpawnProc();
		this.collider.enabled = true;
		if (this.node_Model != null)
		{
			this.node_Model.SetActive(true);
		}
		EventMgr.SendEvent<AMonsterBase>(eGameEvents.MonsterSpawn, this);
		this.state = AMonsterBase.eState.ALIVE;
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00010DAB File Offset: 0x0000EFAB
	protected override void HitProc(int damage)
	{
		base.HitProc(damage);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00010DB4 File Offset: 0x0000EFB4
	protected override void SpawnProc()
	{
		base.SpawnProc();
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00010DBC File Offset: 0x0000EFBC
	protected override void UpdateProc(float deltaTime)
	{
		base.UpdateProc(deltaTime);
	}

	// Token: 0x04000438 RID: 1080
	public new float RemainingDistance = 1f;
}
