using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class Tower_Earthquake : ABaseTower
{
	// Token: 0x0600072E RID: 1838 RVA: 0x0001A9E3 File Offset: 0x00018BE3
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0001A9F0 File Offset: 0x00018BF0
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

	// Token: 0x06000730 RID: 1840 RVA: 0x0001AAA0 File Offset: 0x00018CA0
	protected override void ShootProc()
	{
		Bullet_AOEAttack component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_AOEAttack>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(this.settingData.GetAttackRange(1f), damage);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1006", -1f, -1f, -1f);
	}

	// Token: 0x040005D9 RID: 1497
	private Vector3 headModelForward;
}
