using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class DamageCalculator : Singleton<DamageCalculator>
{
	// Token: 0x060001FD RID: 509 RVA: 0x00008C96 File Offset: 0x00006E96
	public static int CalculateDamage(int damage, eDamageType element, AMonsterBase monster)
	{
		return Singleton<DamageCalculator>.Instance.calculateDamage(damage, element, monster);
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00008CA8 File Offset: 0x00006EA8
	private int calculateDamage(int damage, eDamageType element, AMonsterBase monster)
	{
		float num = 1f;
		int num2 = 0;
		if (monster.MonsterData.GetMonsterSize() == eMonsterSize.BOSS && this.HasTalent(eTalentType.BOSS_KILLER))
		{
			num += 0.2f;
		}
		if (this.HasTalent(eTalentType.CRIT_CHANCE))
		{
			num2 += 10;
		}
		int num3 = Mathf.RoundToInt((float)damage * num);
		if (Random.Range(0, 100) < num2)
		{
			num3 *= -2;
		}
		return num3;
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00008D07 File Offset: 0x00006F07
	public static float CalculateShootInterval(float baseInterval, eDamageType element)
	{
		return Singleton<DamageCalculator>.Instance.calculateShootInterval(baseInterval, element);
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00008D18 File Offset: 0x00006F18
	private float calculateShootInterval(float baseInterval, eDamageType element)
	{
		float num = 1f;
		if (element == eDamageType.ELECTRIC && this.HasTalent(eTalentType.ELECTRIC_TOWER_DAMAGE_INCREASE))
		{
			num += 0.2f;
		}
		return (float)Mathf.RoundToInt(baseInterval * num);
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00008D4A File Offset: 0x00006F4A
	private bool HasTalent(eTalentType talent)
	{
		return GameDataManager.instance.Playerdata.IsTalentLearned(talent);
	}
}
