using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class Tower_Lightning : ABaseTower
{
	// Token: 0x06000746 RID: 1862 RVA: 0x0001B284 File Offset: 0x00019484
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0001B294 File Offset: 0x00019494
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

	// Token: 0x06000748 RID: 1864 RVA: 0x0001B344 File Offset: 0x00019544
	protected override void ShootProc()
	{
		Bullet_ChainLightning component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_ChainLightning>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage, this.targetCount, this.jumpRange, this.node_ShootPosition);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(damage);
		base.OnCreateBullet(component);
		this.particle_ShootLightningEffect.Play();
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1004", -1f, -1f, -1f);
	}

	// Token: 0x040005E3 RID: 1507
	[SerializeField]
	private ParticleSystem particle_ShootLightningEffect;

	// Token: 0x040005E4 RID: 1508
	[SerializeField]
	private int targetCount = 5;

	// Token: 0x040005E5 RID: 1509
	[SerializeField]
	private float jumpRange = 5f;

	// Token: 0x040005E6 RID: 1510
	private Vector3 headModelForward;
}
