using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[CreateAssetMenu(fileName = "CannonSettingData_", menuName = "設定檔/CannonSettingData", order = 1)]
public class CannonSettingData : ATowerComponentSettingData
{
	// Token: 0x0600011D RID: 285 RVA: 0x0000561A File Offset: 0x0000381A
	public GameObject GetCannonPrefab()
	{
		return this.cannonPrefab;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00005622 File Offset: 0x00003822
	public GameObject GetBulletPrefab()
	{
		return this.bulletPrefab;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000562C File Offset: 0x0000382C
	public float GetAttackRange(float multiplier = 1f)
	{
		TowerStats towerStats = base.GetTowerStats(eStatType.ATTACK_RANGE);
		if (towerStats == null)
		{
			return 0f;
		}
		return towerStats.FinalValue;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00005654 File Offset: 0x00003854
	public int GetDamage(float multiplier = 1f)
	{
		TowerStats towerStats = base.GetTowerStats(eStatType.ATTACK_RANGE);
		TowerStats towerStats2 = base.GetTowerStats(eStatType.ATTACK_RANGE);
		if (towerStats == null && towerStats2 == null)
		{
			return 0;
		}
		float finalValue = towerStats.FinalValue;
		float finalValue2 = towerStats2.FinalValue;
		if (finalValue == finalValue2)
		{
			return (int)(finalValue * multiplier);
		}
		return (int)(Random.Range(finalValue, finalValue2 + 1f) * multiplier);
	}

	// Token: 0x06000121 RID: 289 RVA: 0x000056A4 File Offset: 0x000038A4
	public float GetShootInterval(float multiplier = 1f)
	{
		TowerStats towerStats = base.GetTowerStats(eStatType.SHOOT_RATE);
		if (towerStats == null)
		{
			return 0f;
		}
		return 1f / towerStats.FinalValue * multiplier;
	}

	// Token: 0x040000C7 RID: 199
	[SerializeField]
	[Header("砲台的Prefab")]
	private GameObject cannonPrefab;

	// Token: 0x040000C8 RID: 200
	[SerializeField]
	[Header("子彈Prefab")]
	private GameObject bulletPrefab;
}
