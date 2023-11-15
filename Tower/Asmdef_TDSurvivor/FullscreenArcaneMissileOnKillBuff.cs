using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/殺死怪物全螢幕祕法飛彈", order = 1)]
public class FullscreenArcaneMissileOnKillBuff : ABaseBuffSettingData
{
	// Token: 0x06000066 RID: 102 RVA: 0x0000300A File Offset: 0x0000120A
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 殺死怪物全螢幕祕法飛彈 buff", null);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003019 File Offset: 0x00001219
	protected override void RemoveEffect()
	{
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0000301C File Offset: 0x0000121C
	public override void OnTowerBulletHit(ABaseTower tower, AMonsterBase targetMonster, int shootIndex, int bulletIndex)
	{
		base.OnTowerBulletHit(tower, targetMonster, shootIndex, bulletIndex);
		if (this.currentMonster != targetMonster)
		{
			this.currentMonster = targetMonster;
			targetMonster.OnMonsterKilled = (Action<AMonsterBase>)Delegate.Combine(targetMonster.OnMonsterKilled, new Action<AMonsterBase>(this.OnMonsterDeadCallback));
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0000306C File Offset: 0x0000126C
	private void OnMonsterDeadCallback(AMonsterBase monster)
	{
		if (monster != null)
		{
			monster.OnMonsterKilled = (Action<AMonsterBase>)Delegate.Remove(monster.OnMonsterKilled, new Action<AMonsterBase>(this.OnMonsterDeadCallback));
		}
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonsterOnField())
		{
			if (!(amonsterBase == null) && amonsterBase.IsAttackable() && !amonsterBase.IsDead())
			{
				Bullet_HomingMissile component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.prefab_Bullet, this.tower.ShootWorldPosition, this.tower.transform.rotation, null).GetComponent<Bullet_HomingMissile>();
				component.Setup(this.baseDamage, eDamageType.ARCANE);
				component.Spawn(amonsterBase, null);
				SoundManager.PlaySound("Cannon", "Cannon_Shoot_1000", -1f, -1f, -1f);
			}
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0000316C File Offset: 0x0000136C
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003194 File Offset: 0x00001394
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			this.baseDamage
		});
	}

	// Token: 0x0400004F RID: 79
	[SerializeField]
	private GameObject prefab_Bullet;

	// Token: 0x04000050 RID: 80
	[SerializeField]
	[Header("額外子彈的傷害")]
	private int baseDamage = 3;

	// Token: 0x04000051 RID: 81
	private AMonsterBase currentMonster;
}
