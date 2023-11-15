using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class Tower_Cannon : ABaseTower
{
	// Token: 0x0600071E RID: 1822 RVA: 0x00019E51 File Offset: 0x00018051
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00019E60 File Offset: 0x00018060
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

	// Token: 0x06000720 RID: 1824 RVA: 0x00019F70 File Offset: 0x00018170
	protected override void ShootProc()
	{
		Bullet_Cannon component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_Cannon>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1002", -1f, -1f, -1f);
	}

	// Token: 0x040005C6 RID: 1478
	private Vector3 headModelForward;
}
