using System;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class Tower_PoisonDart : ABaseTower
{
	// Token: 0x06000761 RID: 1889 RVA: 0x0001BAF8 File Offset: 0x00019CF8
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0001BB08 File Offset: 0x00019D08
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

	// Token: 0x06000763 RID: 1891 RVA: 0x0001BC18 File Offset: 0x00019E18
	protected override void ShootProc()
	{
		Bullet_PoisionDart component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_PoisionDart>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage, eDamageType.NONE);
		component.Spawn(this.currentTarget, base.gameObject);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1007", -1f, -1f, -1f);
	}

	// Token: 0x040005FB RID: 1531
	private Vector3 headModelForward;
}
