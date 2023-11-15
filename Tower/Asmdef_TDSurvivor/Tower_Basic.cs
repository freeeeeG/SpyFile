using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class Tower_Basic : ABaseTower
{
	// Token: 0x06000714 RID: 1812 RVA: 0x00019AB3 File Offset: 0x00017CB3
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00019AC0 File Offset: 0x00017CC0
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

	// Token: 0x06000716 RID: 1814 RVA: 0x00019BD0 File Offset: 0x00017DD0
	protected override void ShootProc()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage, eDamageType.NONE);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(damage);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1000", -1f, -1f, -1f);
	}

	// Token: 0x040005C2 RID: 1474
	private Vector3 headModelForward;
}
