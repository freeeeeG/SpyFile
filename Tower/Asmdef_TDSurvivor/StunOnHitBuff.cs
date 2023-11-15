using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/攻擊到的目標暈眩", order = 1)]
public class StunOnHitBuff : ABaseBuffSettingData
{
	// Token: 0x06000089 RID: 137 RVA: 0x00003A60 File Offset: 0x00001C60
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 攻擊暈眩 Buff", null);
		this.triggerTimer = this.triggerInterval;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00003A7B File Offset: 0x00001C7B
	protected override void RemoveEffect()
	{
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00003A7D File Offset: 0x00001C7D
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

	// Token: 0x0600008C RID: 140 RVA: 0x00003ABD File Offset: 0x00001CBD
	public override void OnTowerShoot(ABaseTower tower, AMonsterBase targetMonster)
	{
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00003ABF File Offset: 0x00001CBF
	public override void OnTowerBulletHit(ABaseTower tower, AMonsterBase targetMonster, int shootIndex, int bulletIndex)
	{
		if (!this.canProc && shootIndex != this.lastShootIndex)
		{
			return;
		}
		targetMonster.ApplySpeedModifier(0.1f, this.stunDuration, true);
		this.lastShootIndex = shootIndex;
		this.canProc = false;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00003AF3 File Offset: 0x00001CF3
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00003B1C File Offset: 0x00001D1C
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			(int)this.triggerInterval,
			(int)this.stunDuration
		});
	}

	// Token: 0x04000060 RID: 96
	[SerializeField]
	private float stunDuration = 1f;

	// Token: 0x04000061 RID: 97
	[SerializeField]
	private float triggerInterval = 2f;

	// Token: 0x04000062 RID: 98
	private float triggerTimer;

	// Token: 0x04000063 RID: 99
	private bool canProc = true;

	// Token: 0x04000064 RID: 100
	private int lastShootIndex = -999;
}
