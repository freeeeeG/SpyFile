using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class Cannon_Basic : ABaseCannon
{
	// Token: 0x06000709 RID: 1801 RVA: 0x0001966A File Offset: 0x0001786A
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00019678 File Offset: 0x00017878
	private void Update()
	{
		if (this.shootTimer <= 0f)
		{
			if (this.currentTarget == null || !this.currentTarget.IsAttackable() || !this.currentTarget.IsInRange(base.transform.position, this.settingData.GetAttackRange(1f)))
			{
				this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
			}
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

	// Token: 0x0600070B RID: 1803 RVA: 0x000197C0 File Offset: 0x000179C0
	protected override void ShootProc()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		component.Setup(this.settingData.GetDamage(1f), eDamageType.NONE);
		component.Spawn(this.currentTarget, null);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_001", -1f, -1f, -1f);
	}

	// Token: 0x040005BA RID: 1466
	private Vector3 headModelForward;
}
