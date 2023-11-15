using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class Tower_Scrap : ABaseTower
{
	// Token: 0x06000765 RID: 1893 RVA: 0x0001BCBB File Offset: 0x00019EBB
	protected override void CannonSpawnProc()
	{
		base.CannonSpawnProc();
		if (Singleton<GameStateController>.Instance.IsInBattle)
		{
			this.deployedRoundCount++;
		}
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0001BCE0 File Offset: 0x00019EE0
	protected override void OnRoundStartProc()
	{
		if (this.deployedRoundCount >= this.SELF_DESTRUCT_ROUND_LIMIT)
		{
			base.StartCoroutine(this.CR_SelfDestruct());
			return;
		}
		if (this.deployedRoundCount == this.SELF_DESTRUCT_ROUND_LIMIT - 1)
		{
			string @string = LocalizationManager.Instance.GetString("UI", "NOTIFICATION_SCRAP_TOWER_ALMOST_BROKEN", Array.Empty<object>());
			EventMgr.SendEvent<string>(eGameEvents.TriggerNotification, @string);
			this.particle_Smoke.Play();
		}
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0001BD4F File Offset: 0x00019F4F
	private IEnumerator CR_SelfDestruct()
	{
		string @string = LocalizationManager.Instance.GetString("UI", "NOTIFICATION_SCRAP_TOWER_BROKEN", Array.Empty<object>());
		EventMgr.SendEvent<string>(eGameEvents.TriggerNotification, @string);
		this.collider.enabled = false;
		yield return new WaitForSeconds(Random.Range(0f, 0.5f));
		this.particle_SelfDestruct.Play();
		this.renderer_Tower.enabled = false;
		SoundManager.PlaySound("Cannon", "Cannon_Destroy_1001", -1f, -1f, -1f);
		yield return new WaitForSeconds(0.5f);
		yield return new WaitForSeconds(0.5f);
		base.Despawn();
		yield break;
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0001BD5E File Offset: 0x00019F5E
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0001BD6C File Offset: 0x00019F6C
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

	// Token: 0x0600076A RID: 1898 RVA: 0x0001BE7C File Offset: 0x0001A07C
	protected override void ShootProc()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage, eDamageType.NONE);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.currentTarget.PreregisterAttack(damage);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1001", -1f, -1f, -1f);
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x0001BF1E File Offset: 0x0001A11E
	public override int GetSellValue()
	{
		return 0;
	}

	// Token: 0x040005FC RID: 1532
	private Vector3 headModelForward;

	// Token: 0x040005FD RID: 1533
	[SerializeField]
	private ParticleSystem particle_SelfDestruct;

	// Token: 0x040005FE RID: 1534
	[SerializeField]
	private ParticleSystem particle_Smoke;

	// Token: 0x040005FF RID: 1535
	private readonly int SELF_DESTRUCT_ROUND_LIMIT = 3;
}
