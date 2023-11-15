using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class Tower_Missile : ABaseTower
{
	// Token: 0x0600074A RID: 1866 RVA: 0x0001B41C File Offset: 0x0001961C
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0001B42C File Offset: 0x0001962C
	private void Update()
	{
		if (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			return;
		}
		if (this.shootTimer <= 0f)
		{
			List<AMonsterBase> monstersInRange = Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position, this.settingData.GetAttackRange(1f));
			if (monstersInRange != null && monstersInRange.Count > 0)
			{
				List<AMonsterBase> list = new List<AMonsterBase>();
				for (int i = 0; i < 10; i++)
				{
					AMonsterBase amonsterBase;
					if (monstersInRange.Count == 0)
					{
						amonsterBase = list.RandomItem<AMonsterBase>();
					}
					else
					{
						amonsterBase = monstersInRange.RemoveRandom<AMonsterBase>();
					}
					if (amonsterBase.IsAttackable())
					{
						list.Add(amonsterBase);
					}
				}
				this.firstTargetInLastShoot = list[0];
				base.Shoot();
				base.StartCoroutine(this.CR_Shoot(list));
				this.shootTimer = this.settingData.GetShootInterval(1f);
			}
			else
			{
				this.shootTimer = 0.1f;
			}
		}
		else
		{
			this.shootTimer -= Time.deltaTime;
		}
		if (this.firstTargetInLastShoot != null && this.firstTargetInLastShoot.IsAttackable())
		{
			this.headModelForward = this.firstTargetInLastShoot.HeadWorldPosition - this.node_CannonHeadModel.position;
			this.headModelForward.y = 0f;
			this.node_CannonHeadModel.forward = this.headModelForward;
		}
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0001B579 File Offset: 0x00019779
	private IEnumerator CR_Shoot(List<AMonsterBase> targets)
	{
		if (targets.Count == 0)
		{
			yield break;
		}
		foreach (AMonsterBase amonsterBase in targets)
		{
			if (!(amonsterBase == null) && amonsterBase.IsAttackable())
			{
				if (!base.gameObject.activeInHierarchy)
				{
					yield break;
				}
				this.currentTarget = amonsterBase;
				this.CreateBullet();
				yield return new WaitForSeconds(0.05f);
			}
		}
		List<AMonsterBase>.Enumerator enumerator = default(List<AMonsterBase>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0001B58F File Offset: 0x0001978F
	protected override void ShootProc()
	{
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0001B594 File Offset: 0x00019794
	protected void CreateBullet()
	{
		Vector3 position = (this.bulletShootCount % 2 == 0) ? base.ShootWorldPosition : this.node_SecondShootPosition.position;
		Bullet_HomingMissile component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), position, this.node_CannonHeadModel.rotation, null).GetComponent<Bullet_HomingMissile>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage, eDamageType.ELECTRIC);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(damage);
		base.OnCreateBullet(component);
		if (this.bulletShootCount % 2 == 0)
		{
			this.particle_Shoot_1.Play();
		}
		else
		{
			this.particle_Shoot_2.Play();
		}
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1015", -1f, -1f, -1f);
		this.bulletShootCount++;
	}

	// Token: 0x040005E7 RID: 1511
	private Vector3 headModelForward;

	// Token: 0x040005E8 RID: 1512
	[SerializeField]
	private Transform node_SecondShootPosition;

	// Token: 0x040005E9 RID: 1513
	[SerializeField]
	private ParticleSystem particle_Shoot_1;

	// Token: 0x040005EA RID: 1514
	[SerializeField]
	private ParticleSystem particle_Shoot_2;

	// Token: 0x040005EB RID: 1515
	private int bulletShootCount;

	// Token: 0x040005EC RID: 1516
	private AMonsterBase firstTargetInLastShoot;
}
