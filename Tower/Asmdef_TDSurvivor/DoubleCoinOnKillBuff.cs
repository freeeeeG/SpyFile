using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/殺死怪物雙倍金幣", order = 1)]
public class DoubleCoinOnKillBuff : ABaseBuffSettingData
{
	// Token: 0x06000053 RID: 83 RVA: 0x00002C2E File Offset: 0x00000E2E
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 殺死怪物雙倍金幣 buff", null);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00002C3D File Offset: 0x00000E3D
	protected override void RemoveEffect()
	{
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00002C40 File Offset: 0x00000E40
	public override void OnTowerBulletHit(ABaseTower tower, AMonsterBase targetMonster, int shootIndex, int bulletIndex)
	{
		base.OnTowerBulletHit(tower, targetMonster, shootIndex, bulletIndex);
		if (this.currentMonster != targetMonster)
		{
			this.currentMonster = targetMonster;
			targetMonster.OnMonsterKilled = (Action<AMonsterBase>)Delegate.Combine(targetMonster.OnMonsterKilled, new Action<AMonsterBase>(this.OnMonsterDeadCallback));
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00002C90 File Offset: 0x00000E90
	private void OnMonsterDeadCallback(AMonsterBase monster)
	{
		if (this.currentMonster != null)
		{
			AMonsterBase amonsterBase = this.currentMonster;
			amonsterBase.OnMonsterKilled = (Action<AMonsterBase>)Delegate.Remove(amonsterBase.OnMonsterKilled, new Action<AMonsterBase>(this.OnMonsterDeadCallback));
		}
		int reward = this.currentMonster.MonsterData.GetReward(1f);
		EventMgr.SendEvent<int>(eGameEvents.RequestAddCoin, reward);
		Object.Instantiate(Resources.Load("VFX/VFX_CoinBlast_Small"), this.currentMonster.HeadWorldPosition, Quaternion.identity);
		SoundManager.PlaySound("VFX", "Coin", -1f, -1f, -1f);
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00002D35 File Offset: 0x00000F35
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00002D5C File Offset: 0x00000F5C
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x04000045 RID: 69
	private AMonsterBase currentMonster;
}
