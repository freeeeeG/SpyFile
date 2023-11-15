using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/攻擊到的目標打雷", order = 1)]
public class ThunderOnHitBuff : ABaseBuffSettingData
{
	// Token: 0x06000096 RID: 150 RVA: 0x00003CC0 File Offset: 0x00001EC0
	protected override void ApplyEffect()
	{
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, "套用 攻擊打雷 Buff", null);
		this.triggerTimer = this.triggerInterval;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00003CDB File Offset: 0x00001EDB
	protected override void RemoveEffect()
	{
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00003CDD File Offset: 0x00001EDD
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

	// Token: 0x06000099 RID: 153 RVA: 0x00003D1D File Offset: 0x00001F1D
	public override void OnTowerShoot(ABaseTower tower, AMonsterBase targetMonster)
	{
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00003D20 File Offset: 0x00001F20
	public override void OnTowerBulletHit(ABaseTower tower, AMonsterBase targetMonster, int shootIndex, int bulletIndex)
	{
		if (!this.canProc && shootIndex != this.lastShootIndex)
		{
			return;
		}
		Object.Instantiate<GameObject>(Resources.Load<GameObject>("VFX/VFX_LightningStrike")).transform.position = targetMonster.HeadWorldPosition;
		SoundManager.PlaySound("VFX", "Thunder", -1f, -1f, -1f);
		targetMonster.Hit(this.damage, eDamageType.ELECTRIC, default(Vector3));
		this.lastShootIndex = shootIndex;
		this.canProc = false;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00003DC8 File Offset: 0x00001FC8
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			(int)this.triggerInterval,
			this.damage
		});
	}

	// Token: 0x04000067 RID: 103
	[SerializeField]
	private int damage = 5;

	// Token: 0x04000068 RID: 104
	[SerializeField]
	private float stunDuration = 1f;

	// Token: 0x04000069 RID: 105
	[SerializeField]
	private float triggerInterval = 2f;

	// Token: 0x0400006A RID: 106
	private float triggerTimer;

	// Token: 0x0400006B RID: 107
	private bool canProc = true;

	// Token: 0x0400006C RID: 108
	private int lastShootIndex = -999;
}
