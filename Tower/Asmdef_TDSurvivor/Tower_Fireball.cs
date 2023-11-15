using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class Tower_Fireball : ABaseTower
{
	// Token: 0x06000732 RID: 1842 RVA: 0x0001AB4D File Offset: 0x00018D4D
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0001AB5C File Offset: 0x00018D5C
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
			this.UpdateRotation();
		}
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x0001AC30 File Offset: 0x00018E30
	private void UpdateRotation()
	{
		this.headModelForward = this.currentTarget.HeadWorldPosition - this.node_CannonHeadModel.position;
		this.headModelForward.y = 0f;
		this.node_CannonHeadModel.forward = this.headModelForward;
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0001AC7F File Offset: 0x00018E7F
	protected override void CannonSpawnProc()
	{
		base.StartCoroutine(this.SpawnProc());
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x0001AC8E File Offset: 0x00018E8E
	private IEnumerator SpawnProc()
	{
		yield return new WaitForSeconds(0.5f);
		this.particle_PlacementCloud.Play();
		yield break;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x0001ACA0 File Offset: 0x00018EA0
	protected override void ShootProc()
	{
		this.UpdateRotation();
		Bullet_Fireball component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_Fireball>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1013", -1f, -1f, -1f);
	}

	// Token: 0x040005DA RID: 1498
	[SerializeField]
	private List<Collider> list_AdditionalColliders;

	// Token: 0x040005DB RID: 1499
	[SerializeField]
	[Header("放置時的煙霧特效")]
	protected ParticleSystem particle_PlacementCloud;

	// Token: 0x040005DC RID: 1500
	private Vector3 headModelForward;
}
