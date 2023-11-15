using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/TowerSettingData", order = 1)]
public class TowerSettingData : ATowerComponentSettingData
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000128 RID: 296 RVA: 0x00005772 File Offset: 0x00003972
	public eTowerTier TowerTier
	{
		get
		{
			return this.towerTier;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000129 RID: 297 RVA: 0x0000577A File Offset: 0x0000397A
	public eTowerSizeType TowerSizeType
	{
		get
		{
			return this.towerSizeType;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x0600012A RID: 298 RVA: 0x00005782 File Offset: 0x00003982
	public eDamageType DamageType
	{
		get
		{
			return this.damageType;
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0000578A File Offset: 0x0000398A
	public void ApplyTowerTalentBuff()
	{
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000578C File Offset: 0x0000398C
	public GameObject GetPrefab()
	{
		return this.prefab;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00005794 File Offset: 0x00003994
	public GameObject GetBulletPrefab()
	{
		return this.bulletPrefab;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000579C File Offset: 0x0000399C
	public float GetAttackRange(float multiplier = 1f)
	{
		TowerStats towerStats = base.GetTowerStats(eStatType.ATTACK_RANGE);
		if (towerStats == null)
		{
			return 0f;
		}
		return towerStats.FinalValue;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x000057C1 File Offset: 0x000039C1
	public void OverrideDamageType(eDamageType newType)
	{
		this.damageType = newType;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x000057CC File Offset: 0x000039CC
	public int GetDamage(float multiplier = 1f)
	{
		TowerStats towerStats = base.GetTowerStats(eStatType.DAMAGE);
		TowerStats towerStats2 = base.GetTowerStats(eStatType.DAMAGE);
		if (towerStats == null && towerStats2 == null)
		{
			return 0;
		}
		float finalValue = towerStats.FinalValue;
		float finalValue2 = towerStats2.FinalValue;
		int result;
		if (finalValue == finalValue2)
		{
			result = Mathf.CeilToInt(finalValue * multiplier);
		}
		else
		{
			result = Mathf.CeilToInt(Random.Range(finalValue, finalValue2 + 1f) * multiplier);
		}
		return result;
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000582B File Offset: 0x00003A2B
	public void OverrideBaseDamage(int value)
	{
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00005830 File Offset: 0x00003A30
	public float GetShootInterval(float multiplier = 1f)
	{
		TowerStats towerStats = base.GetTowerStats(eStatType.SHOOT_RATE);
		if (towerStats == null)
		{
			return 0f;
		}
		return 1f / towerStats.FinalValue * multiplier;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00005860 File Offset: 0x00003A60
	public int GetSellValue(int roundCountAfterDeploy)
	{
		int buildCost = base.GetBuildCost(1f);
		if (roundCountAfterDeploy == 0)
		{
			return buildCost;
		}
		return Mathf.CeilToInt((float)buildCost * Mathf.Max(0.4f, 1f - 0.2f * (float)roundCountAfterDeploy));
	}

	// Token: 0x040000D1 RID: 209
	[SerializeField]
	private eTowerTier towerTier;

	// Token: 0x040000D2 RID: 210
	[SerializeField]
	[Header("砲塔尺寸")]
	private eTowerSizeType towerSizeType;

	// Token: 0x040000D3 RID: 211
	[SerializeField]
	[Header("傷害屬性")]
	private eDamageType damageType;

	// Token: 0x040000D4 RID: 212
	[SerializeField]
	[Header("砲塔的Prefab")]
	private GameObject prefab;

	// Token: 0x040000D5 RID: 213
	[SerializeField]
	[Header("子彈Prefab")]
	private GameObject bulletPrefab;
}
