using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class TimeManager : MonoBehaviour
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000164 RID: 356 RVA: 0x00009EAE File Offset: 0x000080AE
	private float TargetEnemySpeed
	{
		get
		{
			if (BattleManager.inst.GameOn)
			{
				return this.sniperTimeScale * this.buffTimeScale;
			}
			return 1f;
		}
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00009ECF File Offset: 0x000080CF
	private void Awake()
	{
		TimeManager.inst = this;
		Time.timeScale = 1f;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00009EE1 File Offset: 0x000080E1
	public void PauseSet()
	{
		this.ifPause = true;
		this.pause_TimeScaleSave = Time.timeScale;
		Time.timeScale = 0f;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00009EFF File Offset: 0x000080FF
	public void PauseRecover()
	{
		this.ifPause = false;
		Time.timeScale = this.pause_TimeScaleSave;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00009F13 File Offset: 0x00008113
	public void SetSniperTimeScale(float num)
	{
		this.sniperTimeScale = num;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00009F1C File Offset: 0x0000811C
	public void SetBuffTimeScale(float scale, float timeLeft)
	{
		this.buffTimeScale = scale;
		if (this.buffTimeLeft >= timeLeft)
		{
			return;
		}
		this.buffTimeLeft = timeLeft;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00009F36 File Offset: 0x00008136
	private void Update()
	{
		if (this.ifPause)
		{
			return;
		}
		this.Update_TimeScale();
		this.Update_EnemySpeed();
		this.UpdateBuffTimeScale();
		this.Update_BGMSpeed();
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00009F59 File Offset: 0x00008159
	private void Update_TimeScale()
	{
		this.currentTimeScale = Mathf.SmoothDamp(this.currentTimeScale, 1f, ref this.ref_TimeScaleSpeed, this.smoothTime);
		Time.timeScale = this.currentTimeScale;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00009F88 File Offset: 0x00008188
	private void Update_EnemySpeed()
	{
		this.currentEnemySpeed = Mathf.SmoothDamp(this.currentEnemySpeed, this.TargetEnemySpeed, ref this.ref_EnemySpeed, this.smoothTime);
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00009FAD File Offset: 0x000081AD
	private void Update_BGMSpeed()
	{
		BGMControl.inst.UpdateTimeScale_SetPitch(this.GetFloat_BGMSpeeed());
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00009FBF File Offset: 0x000081BF
	public float GetFloat_BGMSpeeed()
	{
		if (!BattleManager.inst.GameOn)
		{
			return this.currentTimeScale;
		}
		if (TempData.inst.skillModuleFlags[1][4])
		{
			return this.currentEnemySpeed / this.sniperTimeScale;
		}
		return this.currentEnemySpeed;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00009FF8 File Offset: 0x000081F8
	private void UpdateBuffTimeScale()
	{
		if (this.buffTimeLeft > 0f)
		{
			this.buffTimeLeft -= Time.deltaTime;
			if (this.buffTimeLeft <= 0f)
			{
				this.buffTimeScale = 1f;
			}
		}
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000A031 File Offset: 0x00008231
	public void PlayerDie()
	{
		this.currentTimeScale = this.playerDie_TimeScale;
		this.smoothTime = this.playerDie_SmoothTime;
		BGMControl.inst.UpdateTimeScale_SetPitch(this.currentTimeScale);
	}

	// Token: 0x0400017B RID: 379
	public static TimeManager inst;

	// Token: 0x0400017C RID: 380
	[SerializeField]
	private float sniperTimeScale = 1f;

	// Token: 0x0400017D RID: 381
	[SerializeField]
	private float buffTimeScale = 1f;

	// Token: 0x0400017E RID: 382
	[SerializeField]
	private float buffTimeLeft;

	// Token: 0x0400017F RID: 383
	[SerializeField]
	public bool ifPause;

	// Token: 0x04000180 RID: 384
	[SerializeField]
	private float pause_TimeScaleSave;

	// Token: 0x04000181 RID: 385
	private float ref_TimeScaleSpeed;

	// Token: 0x04000182 RID: 386
	private float ref_EnemySpeed;

	// Token: 0x04000183 RID: 387
	[SerializeField]
	[Range(0f, 1f)]
	private float smoothTime = 0.2f;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	private float currentTimeScale = 1f;

	// Token: 0x04000185 RID: 389
	[SerializeField]
	[Header("PlayerDie")]
	private float playerDie_TimeScale = 0.1f;

	// Token: 0x04000186 RID: 390
	[SerializeField]
	private float playerDie_SmoothTime = 2f;

	// Token: 0x04000187 RID: 391
	[Header("EnemySpeed")]
	[SerializeField]
	public float currentEnemySpeed = 1f;
}
