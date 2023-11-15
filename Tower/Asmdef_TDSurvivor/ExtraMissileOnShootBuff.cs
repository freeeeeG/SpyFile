using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/射擊產生額外子彈", order = 1)]
public class ExtraMissileOnShootBuff : ABaseBuffSettingData
{
	// Token: 0x0600005A RID: 90 RVA: 0x00002D9B File Offset: 0x00000F9B
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 射擊產生額外子彈傷害 buff", null);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00002DAA File Offset: 0x00000FAA
	protected override void RemoveEffect()
	{
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00002DAC File Offset: 0x00000FAC
	public override void OnTowerShoot(ABaseTower tower, AMonsterBase targetMonster)
	{
		if (targetMonster == null || !targetMonster.IsAttackable())
		{
			return;
		}
		Bullet_HomingMissile component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.prefab_Bullet, tower.ShootWorldPosition, tower.transform.rotation, null).GetComponent<Bullet_HomingMissile>();
		component.Setup(this.baseDamage, eDamageType.ARCANE);
		component.Spawn(targetMonster, null);
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1000", -1f, -1f, -1f);
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00002E25 File Offset: 0x00001025
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00002E4C File Offset: 0x0000104C
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			1,
			this.baseDamage
		});
	}

	// Token: 0x04000046 RID: 70
	[SerializeField]
	private GameObject prefab_Bullet;

	// Token: 0x04000047 RID: 71
	[SerializeField]
	[Header("額外子彈的傷害")]
	private int baseDamage = 3;

	// Token: 0x04000048 RID: 72
	private TowerStats buffModifierStats;
}
