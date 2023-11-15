using System;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class Tower_MachineGun : ABaseCannon
{
	// Token: 0x0600070D RID: 1805 RVA: 0x00019853 File Offset: 0x00017A53
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00019860 File Offset: 0x00017A60
	private void Update()
	{
		if (this.shootTimer <= 0f)
		{
			this.shootInterval = Mathf.Lerp(this.shootInterval_Start, this.shootInterval_Full, this.curve_ShootInterval.Evaluate(Mathf.Clamp01(this.lockTargetTime / this.chargeTime)));
			if (this.currentTarget == null || !this.currentTarget.IsAttackable() || !this.currentTarget.IsInRange(base.transform.position, this.settingData.GetAttackRange(1f)))
			{
				this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
			}
			if (this.currentTarget != null && this.currentTarget.IsAttackable())
			{
				this.shootTimer = this.shootInterval;
				base.Shoot();
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
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.lockTargetTime = Mathf.Min(this.chargeTime, this.lockTargetTime + Time.deltaTime);
		}
		else
		{
			this.lockTargetTime = Mathf.Max(0f, this.lockTargetTime - Time.deltaTime);
		}
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.headModelForward = this.currentTarget.HeadWorldPosition - this.node_CannonHeadModel.position;
			this.headModelForward.y = 0f;
			this.node_CannonHeadModel.forward = this.headModelForward;
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00019A28 File Offset: 0x00017C28
	protected override void ShootProc()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		component.Setup(this.settingData.GetDamage(1f), eDamageType.NONE);
		component.Spawn(this.currentTarget, null);
		this.animator.SetTrigger("Shoot");
	}

	// Token: 0x040005BB RID: 1467
	[SerializeField]
	[Header("初始發射速度")]
	private float shootInterval_Start;

	// Token: 0x040005BC RID: 1468
	[SerializeField]
	[Header("最高發射速度")]
	private float shootInterval_Full;

	// Token: 0x040005BD RID: 1469
	[SerializeField]
	[Header("換目標後發射速度到最高速需要多久")]
	private float chargeTime = 1f;

	// Token: 0x040005BE RID: 1470
	[SerializeField]
	private AnimationCurve curve_ShootInterval;

	// Token: 0x040005BF RID: 1471
	protected float shootInterval;

	// Token: 0x040005C0 RID: 1472
	private Vector3 headModelForward;

	// Token: 0x040005C1 RID: 1473
	private float lockTargetTime;
}
