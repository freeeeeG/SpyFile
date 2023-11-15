using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class Tower_SnowBall : ABaseTower
{
	// Token: 0x06000773 RID: 1907 RVA: 0x0001C2BD File Offset: 0x0001A4BD
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0001C2CC File Offset: 0x0001A4CC
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

	// Token: 0x06000775 RID: 1909 RVA: 0x0001C3D9 File Offset: 0x0001A5D9
	protected override void CannonSpawnProc()
	{
		base.StartCoroutine(this.SpawnProc());
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0001C3E8 File Offset: 0x0001A5E8
	private IEnumerator SpawnProc()
	{
		yield return new WaitForSeconds(0.5f);
		this.particle_PlacementCloud.Play();
		yield break;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
	protected override void ShootProc()
	{
		Bullet_Snowball component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_Snowball>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1018", -1f, -1f, -1f);
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_2_1018", -1f, -1f, -1f);
	}

	// Token: 0x0400060C RID: 1548
	[SerializeField]
	private List<Collider> list_AdditionalColliders;

	// Token: 0x0400060D RID: 1549
	[SerializeField]
	[Header("放置時的煙霧特效")]
	protected ParticleSystem particle_PlacementCloud;

	// Token: 0x0400060E RID: 1550
	private Vector3 headModelForward;
}
