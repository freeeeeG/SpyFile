using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class Tower_Drone : ABaseTower
{
	// Token: 0x06000729 RID: 1833 RVA: 0x0001A380 File Offset: 0x00018580
	private void Start()
	{
		this.shootTimer = 0f;
		this.targetTransform = base.transform;
		this.flyTargetPosition = base.transform.position + Vector3.up * 0.5f;
		this.droneToTargetDir = base.transform.forward * -1f;
		this.droneState = Tower_Drone.eDroneState.IDLE;
		this.targetPriority = eTowerTargetPriority.NEAREST;
		this.node_Drone.SetParent(null);
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0001A3FE File Offset: 0x000185FE
	protected override void CannonDespawnProc()
	{
		base.CannonDespawnProc();
		this.node_Drone.SetParent(base.transform);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0001A418 File Offset: 0x00018618
	private void Update()
	{
		if (Singleton<GameStateController>.Instance.IsInBattle)
		{
			if (this.currentTarget != null && this.currentTarget.IsAttackable())
			{
				if (Vector3.SqrMagnitude((base.transform.position - this.currentTarget.transform.position).WithY(0f)) > this.settingData.GetAttackRange(1f))
				{
					this.targetTransform = this.currentTarget.transform;
					this.flyTargetPosition = this.currentTarget.transform.position + Vector3.up * 3f + this.droneToTargetDir * 5f;
					this.droneState = Tower_Drone.eDroneState.FLYING;
				}
				else
				{
					AMonsterBase targetByTowerPriority = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, this.node_Drone.position.WithY(0f), this.detectRange);
					if (targetByTowerPriority != null)
					{
						this.currentTarget = targetByTowerPriority;
					}
					this.droneState = Tower_Drone.eDroneState.ATTACKING;
				}
			}
			else
			{
				this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position.WithY(0f), this.detectRange);
				if (this.currentTarget == null)
				{
					this.findTargetTimer += Time.deltaTime;
				}
				else
				{
					if (this.findTargetTimer >= 3f)
					{
						this.particle_FoundTarget.Play();
					}
					this.findTargetTimer = 0f;
				}
				this.targetTransform = base.transform;
				this.droneState = Tower_Drone.eDroneState.GO_HOME;
				this.flyTargetPosition = base.transform.position + Vector3.up * 0.5f;
			}
		}
		else if (Vector3.SqrMagnitude((this.node_Drone.position - base.transform.position).WithY(0f)) > 0.5f)
		{
			this.targetTransform = base.transform;
			this.flyTargetPosition = base.transform.position + Vector3.up * 0.5f;
			this.droneState = Tower_Drone.eDroneState.GO_HOME;
		}
		else
		{
			this.droneState = Tower_Drone.eDroneState.IDLE;
		}
		if (this.shootTimer <= 0f)
		{
			if (this.currentTarget != null && this.currentTarget.IsAttackable())
			{
				if (Vector3.SqrMagnitude((this.node_Drone.position - this.currentTarget.transform.position).WithY(0f)) <= this.settingData.GetAttackRange(1f))
				{
					this.shootTimer = this.settingData.GetShootInterval(1f);
					base.Shoot();
				}
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
		switch (this.droneState)
		{
		default:
			this.speed = Mathf.Lerp(this.speed, 0f, 1.5f * Time.deltaTime);
			break;
		case Tower_Drone.eDroneState.FLYING:
		case Tower_Drone.eDroneState.GO_HOME:
			if (this.speed < this.maxSpeed)
			{
				this.speed = Mathf.Min(this.maxSpeed, this.speed + Time.deltaTime * this.accelerate);
			}
			break;
		case Tower_Drone.eDroneState.ATTACKING:
			this.speed = Mathf.Lerp(this.speed, 0f, 0.5f * Time.deltaTime);
			break;
		}
		if (this.targetTransform != null)
		{
			if (Vector3.Magnitude(this.targetTransform.position - this.node_Drone.position) > 1f)
			{
				this.droneToTargetDir = (this.targetTransform.position - this.node_Drone.position).WithY(0f).normalized;
				this.node_Drone.forward = Vector3.Lerp(this.node_Drone.forward, this.droneToTargetDir, 5f * Time.deltaTime);
			}
			if (Vector3.Magnitude(this.targetTransform.position - this.node_Drone.position) > 1f)
			{
				this.node_Drone.position += this.node_Drone.forward * this.speed * Time.deltaTime;
				return;
			}
			this.node_Drone.position = Vector3.Lerp(this.node_Drone.position, this.targetTransform.position, Time.deltaTime);
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0001A918 File Offset: 0x00018B18
	protected override void ShootProc()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage, eDamageType.NONE);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(damage);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1011", -1f, -1f, -1f);
	}

	// Token: 0x040005CD RID: 1485
	[SerializeField]
	private float detectRange = 15f;

	// Token: 0x040005CE RID: 1486
	[SerializeField]
	private float maxSpeed;

	// Token: 0x040005CF RID: 1487
	[SerializeField]
	private float accelerate = 1f;

	// Token: 0x040005D0 RID: 1488
	[SerializeField]
	private Transform node_Drone;

	// Token: 0x040005D1 RID: 1489
	[SerializeField]
	private ParticleSystem particle_FoundTarget;

	// Token: 0x040005D2 RID: 1490
	private float speed;

	// Token: 0x040005D3 RID: 1491
	private Vector3 flyTargetPosition = Vector3.zero;

	// Token: 0x040005D4 RID: 1492
	private Vector3 headModelForward;

	// Token: 0x040005D5 RID: 1493
	private Vector3 droneToTargetDir;

	// Token: 0x040005D6 RID: 1494
	private Transform targetTransform;

	// Token: 0x040005D7 RID: 1495
	private float findTargetTimer;

	// Token: 0x040005D8 RID: 1496
	private Tower_Drone.eDroneState droneState;

	// Token: 0x0200026A RID: 618
	private enum eDroneState
	{
		// Token: 0x04000B90 RID: 2960
		IDLE,
		// Token: 0x04000B91 RID: 2961
		FLYING,
		// Token: 0x04000B92 RID: 2962
		ATTACKING,
		// Token: 0x04000B93 RID: 2963
		GO_HOME
	}
}
