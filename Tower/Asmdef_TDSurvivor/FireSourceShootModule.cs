using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class FireSourceShootModule : MonoBehaviour
{
	// Token: 0x06000299 RID: 665 RVA: 0x0000AFEF File Offset: 0x000091EF
	private void OnValidate()
	{
		if (this.fireSource == null)
		{
			this.fireSource = base.GetComponent<Obj_FireSource>();
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000B00B File Offset: 0x0000920B
	private void Start()
	{
		this.shootTimer = 0f;
		this.IsTalentLearned = GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.FLAME_ATTACK_ENEMY);
		if (!this.IsTalentLearned)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000B040 File Offset: 0x00009240
	private void Update()
	{
		if (!this.IsTalentLearned)
		{
			return;
		}
		if (this.shootTimer > 0f)
		{
			this.shootTimer -= Time.deltaTime;
			return;
		}
		this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(eTowerTargetPriority.NEAREST, base.transform.position, this.attackRange);
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.shootTimer = this.shootInterval;
			this.Shoot();
			return;
		}
		this.shootTimer = 0.05f;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000B0D4 File Offset: 0x000092D4
	protected void Shoot()
	{
		Vector3 insideUnitSphere = Random.insideUnitSphere;
		if (insideUnitSphere.y < 0f)
		{
			insideUnitSphere.y *= -1f;
		}
		Bullet_EmberFire component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.bulletPrefab, this.shootPosition.position, Quaternion.Euler(insideUnitSphere), null).GetComponent<Bullet_EmberFire>();
		component.Setup(this.damage);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(this.damage);
		SoundManager.PlaySound("Cannon", "NPC_Shoot_Arrow", -1f, -1f, -1f);
	}

	// Token: 0x04000318 RID: 792
	[SerializeField]
	private Obj_FireSource fireSource;

	// Token: 0x04000319 RID: 793
	[SerializeField]
	private GameObject bulletPrefab;

	// Token: 0x0400031A RID: 794
	[SerializeField]
	private Transform shootPosition;

	// Token: 0x0400031B RID: 795
	[SerializeField]
	private int damage = 3;

	// Token: 0x0400031C RID: 796
	[SerializeField]
	private float attackRange = 20f;

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private float shootInterval = 1f;

	// Token: 0x0400031E RID: 798
	private float shootTimer;

	// Token: 0x0400031F RID: 799
	private AMonsterBase currentTarget;

	// Token: 0x04000320 RID: 800
	private bool IsTalentLearned;
}
