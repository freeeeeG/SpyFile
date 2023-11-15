using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class Tower_Freeze : ABaseTower
{
	// Token: 0x06000739 RID: 1849 RVA: 0x0001AD43 File Offset: 0x00018F43
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0001AD50 File Offset: 0x00018F50
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

	// Token: 0x0600073B RID: 1851 RVA: 0x0001AE00 File Offset: 0x00019000
	protected override void ShootProc()
	{
		Bullet_AOEFreeze component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_AOEFreeze>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(this.settingData.GetAttackRange(1f), damage);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1006", -1f, -1f, -1f);
	}

	// Token: 0x040005DD RID: 1501
	private Vector3 headModelForward;
}
