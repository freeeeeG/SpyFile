using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/攻擊到的目標周圍緩速", order = 1)]
public class AreaFreezeOnHitBuff : ABaseBuffSettingData
{
	// Token: 0x0600004B RID: 75 RVA: 0x00002A50 File Offset: 0x00000C50
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 攻擊到的目標周圍緩速 Buff", null);
		this.triggerTimer = this.triggerInterval;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00002A6B File Offset: 0x00000C6B
	protected override void RemoveEffect()
	{
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00002A6D File Offset: 0x00000C6D
	protected override void TickProc(float delta)
	{
		if (this.canProc)
		{
			return;
		}
		this.triggerTimer -= delta;
		if (this.triggerTimer <= 0f)
		{
			this.canProc = true;
			this.triggerTimer += this.triggerInterval;
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00002AAD File Offset: 0x00000CAD
	public override void OnTowerShoot(ABaseTower tower, AMonsterBase targetMonster)
	{
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00002AB0 File Offset: 0x00000CB0
	public override void OnTowerBulletHit(ABaseTower tower, AMonsterBase targetMonster, int shootIndex, int bulletIndex)
	{
		if (!this.canProc && shootIndex != this.lastShootIndex)
		{
			return;
		}
		float modifier = 0.7f;
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.ICE_TOWER_DAMAGE_INCREASE))
		{
			modifier = 0.5f;
		}
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(targetMonster.transform.position, 2f))
		{
			amonsterBase.ApplySpeedModifier(modifier, this.freezeDuration, true);
			amonsterBase.ApplyDamageDebuff(this.freezeDuration, this.freezeDuration, 0, eDamageType.ICE, base.GetInstanceID());
		}
		this.lastShootIndex = shootIndex;
		this.canProc = false;
		Object.Instantiate(Resources.Load("VFX/VFX_NovaFrost"), targetMonster.transform.position + Vector3.up, Quaternion.identity);
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002BA0 File Offset: 0x00000DA0
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00002BC7 File Offset: 0x00000DC7
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x04000040 RID: 64
	[SerializeField]
	private float freezeDuration = 1.5f;

	// Token: 0x04000041 RID: 65
	[SerializeField]
	private float triggerInterval = 2f;

	// Token: 0x04000042 RID: 66
	private float triggerTimer;

	// Token: 0x04000043 RID: 67
	private bool canProc = true;

	// Token: 0x04000044 RID: 68
	private int lastShootIndex = -999;
}
