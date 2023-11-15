using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class Tower_FrontalDamage : ABaseTower
{
	// Token: 0x0600073D RID: 1853 RVA: 0x0001AEAD File Offset: 0x000190AD
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0001AEBC File Offset: 0x000190BC
	private void Update()
	{
		if (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			return;
		}
		if (this.shootTimer <= 0f)
		{
			this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
			if (this.currentTarget != null && this.currentTarget.IsAttackable())
			{
				this.shootTimer = this.settingData.GetShootInterval(1f);
				base.Shoot();
			}
			else
			{
				this.shootTimer = 0.05f;
			}
		}
		else
		{
			this.shootTimer -= Time.deltaTime;
		}
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.headModelForward = this.currentTarget.HeadWorldPosition - this.node_CannonHeadModel.position;
			this.headModelForward.y = 0f;
			this.node_CannonHeadModel.forward = this.headModelForward;
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0001AFCC File Offset: 0x000191CC
	protected override void ShootProc()
	{
		float d = this.settingData.GetAttackRange(1f) / this.originalShootRange;
		this.particle_ShootEffect.transform.localScale = Vector3.one * d;
		base.StartCoroutine(this.CR_ShootProc());
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1003", -1f, -1f, -1f);
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0001B048 File Offset: 0x00019248
	private IEnumerator CR_ShootProc()
	{
		int damage = this.settingData.GetDamage(1f);
		int splitDamage = damage / this.damageSplitCount;
		int num;
		for (int i = 0; i < this.damageSplitCount; i = num + 1)
		{
			if (!this.currentTarget.IsAttackable())
			{
				yield break;
			}
			Bullet_FrontalDamage component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, Quaternion.LookRotation(this.headModelForward, Vector3.up), null).GetComponent<Bullet_FrontalDamage>();
			component.Setup(this.settingData.GetAttackRange(1f), 1f, splitDamage, eDamageType.FIRE);
			component.Spawn(this.currentTarget, null);
			base.OnCreateBullet(component);
			EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, (base.transform.position + this.headModelForward * 2f).WithY(0f), 1.5f);
			yield return new WaitForSeconds(this.damageTriggerInterval);
			num = i;
		}
		yield break;
	}

	// Token: 0x040005DE RID: 1502
	[SerializeField]
	private int damageSplitCount = 5;

	// Token: 0x040005DF RID: 1503
	[SerializeField]
	private float damageTriggerInterval = 0.1f;

	// Token: 0x040005E0 RID: 1504
	[SerializeField]
	private float originalShootRange = 3f;

	// Token: 0x040005E1 RID: 1505
	private Vector3 headModelForward;
}
