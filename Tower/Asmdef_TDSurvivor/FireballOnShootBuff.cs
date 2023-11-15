using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/射擊產生火球", order = 1)]
public class FireballOnShootBuff : ABaseBuffSettingData
{
	// Token: 0x06000060 RID: 96 RVA: 0x00002EB5 File Offset: 0x000010B5
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 射擊產生火球 buff", null);
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00002EC4 File Offset: 0x000010C4
	protected override void RemoveEffect()
	{
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00002EC8 File Offset: 0x000010C8
	public override void OnTowerShoot(ABaseTower tower, AMonsterBase targetMonster)
	{
		if (targetMonster == null || !targetMonster.IsAttackable())
		{
			return;
		}
		this.shootCount++;
		if (this.shootCount >= this.countToShoot)
		{
			Bullet_Fireball component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.prefab_Bullet, tower.ShootWorldPosition, tower.transform.rotation, null).GetComponent<Bullet_Fireball>();
			component.Setup(this.baseDamage);
			component.OverrideMaxBounceCount(0);
			component.OverrideMaxFlightHeight(1f);
			component.Spawn(targetMonster, null);
			SoundManager.PlaySound("Cannon", "Cannon_Shoot_1013", -1f, -1f, -1f);
			this.shootCount = 0;
		}
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00002F75 File Offset: 0x00001175
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00002F9C File Offset: 0x0000119C
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			this.explosionDamage
		});
	}

	// Token: 0x04000049 RID: 73
	[SerializeField]
	private GameObject prefab_Bullet;

	// Token: 0x0400004A RID: 74
	[SerializeField]
	[Header("要幾次攻擊才發射額外子彈")]
	private int countToShoot = 1;

	// Token: 0x0400004B RID: 75
	[SerializeField]
	[Header("額外子彈的傷害")]
	private int baseDamage = 3;

	// Token: 0x0400004C RID: 76
	[SerializeField]
	[Header("額外子彈的爆炸傷害")]
	private int explosionDamage = 3;

	// Token: 0x0400004D RID: 77
	private TowerStats buffModifierStats;

	// Token: 0x0400004E RID: 78
	private int shootCount;
}
