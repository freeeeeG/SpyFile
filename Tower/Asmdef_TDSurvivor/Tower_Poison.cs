using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class Tower_Poison : ABaseTower
{
	// Token: 0x0600075C RID: 1884 RVA: 0x0001B94B File Offset: 0x00019B4B
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0001B958 File Offset: 0x00019B58
	private void Update()
	{
		if (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			return;
		}
		if (this.shootTimer > 0f)
		{
			this.shootTimer -= Time.deltaTime;
			return;
		}
		this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.shootTimer = this.settingData.GetShootInterval(1f);
			base.Shoot();
			return;
		}
		this.shootTimer = 0.05f;
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0001BA06 File Offset: 0x00019C06
	protected override void CannonSpawnProc()
	{
		base.CannonSpawnProc();
		SoundManager.PlaySound("Cannon", "Cannon_Build_1008", -1f, -1f, 0.5f);
		this.particle_PoisonDrip.Play();
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0001BA38 File Offset: 0x00019C38
	protected override void ShootProc()
	{
		float damage = (float)this.settingData.GetDamage(1f);
		float num = GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.POISON_FASTER) ? 0.25f : 0.2f;
		float duration = damage / num;
		List<AMonsterBase> monstersInRange = Singleton<MonsterManager>.Instance.GetMonstersInRange(base.ShootWorldPosition.WithY(0f), 1f);
		if (monstersInRange != null && monstersInRange.Count > 0)
		{
			foreach (AMonsterBase amonsterBase in monstersInRange)
			{
				amonsterBase.ApplyDamageDebuff(duration, num, 1, eDamageType.POISON, base.GetInstanceID());
			}
		}
	}

	// Token: 0x040005F9 RID: 1529
	[SerializeField]
	private ParticleSystem particle_PoisonDrip;

	// Token: 0x040005FA RID: 1530
	private Vector3 headModelForward;
}
