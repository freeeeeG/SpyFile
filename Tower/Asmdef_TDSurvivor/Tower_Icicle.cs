using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class Tower_Icicle : ABaseTower
{
	// Token: 0x06000742 RID: 1858 RVA: 0x0001B07C File Offset: 0x0001927C
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0001B08C File Offset: 0x0001928C
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

	// Token: 0x06000744 RID: 1860 RVA: 0x0001B19C File Offset: 0x0001939C
	protected override void ShootProc()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition + Random.insideUnitSphere * 0.2f, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		int num = this.settingData.GetDamage(1f);
		float hppercentage = this.currentTarget.GetHPPercentage();
		float num2 = (1f - hppercentage) * 3f;
		num = Mathf.Max(num, Mathf.RoundToInt((float)num * num2));
		component.Setup(num, eDamageType.ICE);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(num);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1000", -1f, -1f, -1f);
	}

	// Token: 0x040005E2 RID: 1506
	private Vector3 headModelForward;
}
