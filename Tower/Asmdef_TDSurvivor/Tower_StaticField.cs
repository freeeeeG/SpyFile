using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class Tower_StaticField : ABaseTower
{
	// Token: 0x06000779 RID: 1913 RVA: 0x0001C4B4 File Offset: 0x0001A6B4
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
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

	// Token: 0x0600077B RID: 1915 RVA: 0x0001C574 File Offset: 0x0001A774
	protected override void ShootProc()
	{
		Bullet_AOEStaticField component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_AOEStaticField>();
		int damage = this.settingData.GetDamage(1f);
		float attackRange = this.settingData.GetAttackRange(1f);
		component.Setup(attackRange, damage);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(damage);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1004", -1f, -1f, -1f);
	}

	// Token: 0x0400060F RID: 1551
	[SerializeField]
	private ParticleSystem particle_StaticEffect;

	// Token: 0x04000610 RID: 1552
	private Vector3 headModelForward;
}
