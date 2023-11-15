using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class Tower_Boulder : ABaseTower
{
	// Token: 0x06000718 RID: 1816 RVA: 0x00019C7A File Offset: 0x00017E7A
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00019C88 File Offset: 0x00017E88
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

	// Token: 0x0600071A RID: 1818 RVA: 0x00019D95 File Offset: 0x00017F95
	protected override void CannonSpawnProc()
	{
		base.StartCoroutine(this.SpawnProc());
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x00019DA4 File Offset: 0x00017FA4
	private IEnumerator SpawnProc()
	{
		yield return new WaitForSeconds(0.5f);
		this.particle_PlacementCloud.Play();
		yield break;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00019DB4 File Offset: 0x00017FB4
	protected override void ShootProc()
	{
		Bullet_Boulder component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_Boulder>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1005", -1f, -1f, -1f);
	}

	// Token: 0x040005C3 RID: 1475
	[SerializeField]
	private List<Collider> list_AdditionalColliders;

	// Token: 0x040005C4 RID: 1476
	[SerializeField]
	[Header("放置時的煙霧特效")]
	protected ParticleSystem particle_PlacementCloud;

	// Token: 0x040005C5 RID: 1477
	private Vector3 headModelForward;
}
