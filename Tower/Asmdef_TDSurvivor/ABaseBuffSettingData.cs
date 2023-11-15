using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public abstract class ABaseBuffSettingData : AItemSettingData
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000029 RID: 41 RVA: 0x00002451 File Offset: 0x00000651
	public ABaseTower Tower
	{
		get
		{
			return this.tower;
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002459 File Offset: 0x00000659
	private bool ShowIfIsTimeDuration()
	{
		return this.DurationType == eBuffDurationType.TIME;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002464 File Offset: 0x00000664
	public void Initialize(ABaseTower tower)
	{
		this.tower = tower;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002470 File Offset: 0x00000670
	public void Tick(float delta)
	{
		if (this.IsFinished)
		{
			return;
		}
		this.TickProc(delta);
		if (this.DurationType == eBuffDurationType.TIME)
		{
			this.durationLeft_Time -= delta;
			if (this.durationLeft_Time <= 0f)
			{
				this.EndBuff();
			}
			Action<int> onTimeUpdate = this.OnTimeUpdate;
			if (onTimeUpdate == null)
			{
				return;
			}
			onTimeUpdate(Mathf.CeilToInt(this.durationLeft_Time));
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000024D1 File Offset: 0x000006D1
	protected virtual void TickProc(float delta)
	{
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000024D3 File Offset: 0x000006D3
	public void Update_Round()
	{
		if (this.DurationType != eBuffDurationType.ROUND)
		{
			return;
		}
		this.durationLeft_Round--;
		if (this.durationLeft_Round <= 0)
		{
			this.EndBuff();
		}
		Action<int> onTimeUpdate = this.OnTimeUpdate;
		if (onTimeUpdate == null)
		{
			return;
		}
		onTimeUpdate(this.durationLeft_Round);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002514 File Offset: 0x00000714
	public void Activate()
	{
		if (this.IsEffectStacked || this.Duration_Time <= 0f)
		{
			this.ApplyEffect();
			this.effectStacks++;
		}
		if (this.DurationType == eBuffDurationType.TIME)
		{
			if (this.IsDurationStacked || this.durationLeft_Time <= 0f)
			{
				this.durationLeft_Time += this.Duration_Time;
			}
		}
		else if (this.IsDurationStacked || this.durationLeft_Round <= 0)
		{
			this.durationLeft_Round += this.Duration_Round;
		}
		EventMgr.Register(eGameEvents.OnRoundEnd, new Action(this.OnRoundEnd));
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000025B8 File Offset: 0x000007B8
	public void ForceRemove()
	{
		this.EndBuff();
	}

	// Token: 0x06000031 RID: 49 RVA: 0x000025C0 File Offset: 0x000007C0
	private void EndBuff()
	{
		this.RemoveEffect();
		this.IsFinished = true;
		Action onBuffRemove = this.OnBuffRemove;
		if (onBuffRemove != null)
		{
			onBuffRemove();
		}
		EventMgr.Remove(eGameEvents.OnRoundEnd, new Action(this.OnRoundEnd));
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000025F8 File Offset: 0x000007F8
	private void OnRoundEnd()
	{
		this.Update_Round();
	}

	// Token: 0x06000033 RID: 51
	protected abstract void ApplyEffect();

	// Token: 0x06000034 RID: 52
	protected abstract void RemoveEffect();

	// Token: 0x06000035 RID: 53 RVA: 0x00002600 File Offset: 0x00000800
	public virtual void OnTowerShoot(ABaseTower tower, AMonsterBase targetMonster)
	{
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002602 File Offset: 0x00000802
	public virtual void OnTowerBulletHit(ABaseTower tower, AMonsterBase targetMonster, int shootIndex, int bulletIndex)
	{
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002604 File Offset: 0x00000804
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000262B File Offset: 0x0000082B
	public override string GetLocStatsString()
	{
		return this.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002664 File Offset: 0x00000864
	protected string GetLocDurationString(string str)
	{
		if (this.DurationType == eBuffDurationType.ROUND)
		{
			str = str + "<color=#eecc33>" + LocalizationManager.Instance.GetString("UI", "BUFF_DURATION_ROUND", new object[]
			{
				this.Duration_Round
			}) + "</color><color=#ff9933>";
		}
		else if (this.DurationType == eBuffDurationType.TIME)
		{
			str = str + "<color=#eecc33>" + LocalizationManager.Instance.GetString("UI", "BUFF_DURATION_TIME", new object[]
			{
				this.Duration_Time
			}) + "</color><color=#ff9933>";
		}
		str += "\n\n";
		return str;
	}

	// Token: 0x04000025 RID: 37
	[Header("Buff持續時間類型")]
	public eBuffDurationType DurationType;

	// Token: 0x04000026 RID: 38
	[Header("持續時間")]
	public float Duration_Time;

	// Token: 0x04000027 RID: 39
	[Header("持續回合")]
	public int Duration_Round;

	// Token: 0x04000028 RID: 40
	[Header("重複施放是否疊加時間")]
	public bool IsDurationStacked;

	// Token: 0x04000029 RID: 41
	[Header("重複施放是否疊加數值")]
	public bool IsEffectStacked;

	// Token: 0x0400002A RID: 42
	[Header("是否是跟發射有關的特效")]
	public bool IsShootingEffect;

	// Token: 0x0400002B RID: 43
	[Header("是否是跟打中目標有關的特效")]
	public bool IsHitTargetEffect;

	// Token: 0x0400002C RID: 44
	protected float durationLeft_Time;

	// Token: 0x0400002D RID: 45
	protected int durationLeft_Round;

	// Token: 0x0400002E RID: 46
	protected int effectStacks;

	// Token: 0x0400002F RID: 47
	public bool IsFinished;

	// Token: 0x04000030 RID: 48
	protected ABaseTower tower;

	// Token: 0x04000031 RID: 49
	public Action OnBuffRemove;

	// Token: 0x04000032 RID: 50
	public Action<int> OnTimeUpdate;
}
