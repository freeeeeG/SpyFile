using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
[Serializable]
public class MonsterDamageDebuff
{
	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
	// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000F9E0 File Offset: 0x0000DBE0
	public bool IsFinished { get; private set; }

	// Token: 0x060003F6 RID: 1014 RVA: 0x0000F9EC File Offset: 0x0000DBEC
	public MonsterDamageDebuff(AMonsterBase monster, float duration, float tickInterval, int damagePerTick, eDamageType damageType, int sourceID)
	{
		this.monster = monster;
		this.duration = duration;
		this.tickInterval = tickInterval;
		this.damagePerTick = damagePerTick;
		this.damageType = damageType;
		this.sourceID = sourceID;
		this.tickTimer = 0f;
		this.totalTimer = 0f;
		this.IsFinished = false;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0000FA50 File Offset: 0x0000DC50
	public void RenewDebuff(float duration, float tickInterval, int damagePerTick)
	{
		this.duration = duration;
		this.tickInterval = tickInterval;
		this.damagePerTick = damagePerTick;
		this.totalTimer = 0f;
		this.IsFinished = false;
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0000FA7C File Offset: 0x0000DC7C
	public void Update(float deltaTime)
	{
		if (this.totalTimer <= this.duration)
		{
			this.tickTimer += deltaTime;
			this.totalTimer += deltaTime;
			if (this.tickTimer >= this.tickInterval)
			{
				this.tickTimer -= this.tickInterval;
				this.DealDamage();
				return;
			}
		}
		else
		{
			this.IsFinished = true;
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0000FAE2 File Offset: 0x0000DCE2
	public bool IsSameSource(int sourceID, eDamageType damageType)
	{
		return sourceID == this.sourceID && damageType == this.damageType;
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0000FAF8 File Offset: 0x0000DCF8
	public eDamageType GetDamageType()
	{
		return this.damageType;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0000FB00 File Offset: 0x0000DD00
	private void DealDamage()
	{
		this.monster.Hit(this.damagePerTick, this.damageType, default(Vector3));
	}

	// Token: 0x04000409 RID: 1033
	private AMonsterBase monster;

	// Token: 0x0400040A RID: 1034
	private float duration;

	// Token: 0x0400040B RID: 1035
	private float tickInterval;

	// Token: 0x0400040C RID: 1036
	private int damagePerTick;

	// Token: 0x0400040D RID: 1037
	private eDamageType damageType;

	// Token: 0x0400040E RID: 1038
	private float tickTimer;

	// Token: 0x0400040F RID: 1039
	private float totalTimer;

	// Token: 0x04000410 RID: 1040
	private int sourceID = -1;

	// Token: 0x04000411 RID: 1041
	private static readonly float TICK_INTERVAL_POISON = 0.5f;

	// Token: 0x04000412 RID: 1042
	private static readonly float TICK_INTERVAL_BURNING = 0.5f;

	// Token: 0x04000413 RID: 1043
	private static readonly float TICK_INTERVAL_DEFAULT = 1f;
}
