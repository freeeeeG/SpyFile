using System;
using UnityEngine;

// Token: 0x02000B32 RID: 2866
[RequireComponent(typeof(Animator))]
public class LevelTimerUIController : DisplayTimeUIController
{
	// Token: 0x06003A19 RID: 14873 RVA: 0x00114954 File Offset: 0x00112D54
	protected override void Awake()
	{
		this.m_animator = base.gameObject.RequireComponentRecursive<Animator>();
		this.m_ProgressBar = base.gameObject.RequestComponentRecursive<ProgressBarUI>();
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
		this.m_dataStore.Register(LevelTimerUIController.k_timeUpdatedId, new DataStore.OnChangeNotification(this.OnTimeUpdatedNotification));
		base.Awake();
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x001149B0 File Offset: 0x00112DB0
	private void OnDestroy()
	{
		if (this.m_dataStore != null)
		{
			this.m_dataStore.Unregister(LevelTimerUIController.k_timeUpdatedId, new DataStore.OnChangeNotification(this.OnTimeUpdatedNotification));
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06003A1C RID: 14876 RVA: 0x00114AD6 File Offset: 0x00112ED6
	// (set) Token: 0x06003A1B RID: 14875 RVA: 0x001149E0 File Offset: 0x00112DE0
	public override int Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			KitchenLevelConfigBase kitchenLevelConfigBase = GameUtils.GetLevelConfig() as KitchenLevelConfigBase;
			float timeLimit = kitchenLevelConfigBase.GetTimeLimit();
			if (value != this.Value && this.m_pulseStart != 0 && value <= this.m_pulseStart && this.m_animator != null && value > 0)
			{
				this.m_animator.SetTrigger(LevelTimerUIController.m_iPulse);
				GameUtils.TriggerAudio(GameOneShotAudioTag.LevelTimerBeep, base.gameObject.layer);
			}
			else if (value != this.Value && (float)value != timeLimit && this.m_alertInterval != 0 && value % this.m_alertInterval == 0 && this.m_animator != null)
			{
				this.m_animator.SetTrigger(LevelTimerUIController.m_iAlert);
			}
			if (this.m_ProgressBar != null)
			{
				this.m_ProgressBar.Value = (float)value / timeLimit;
			}
			base.Value = value;
		}
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x00114ADE File Offset: 0x00112EDE
	private void OnTimeUpdatedNotification(DataStore.Id id, object data)
	{
		this.Value = Convert.ToInt32(data);
	}

	// Token: 0x04002F0B RID: 12043
	[SerializeField]
	private int m_alertInterval = 30;

	// Token: 0x04002F0C RID: 12044
	[SerializeField]
	private int m_pulseStart = 5;

	// Token: 0x04002F0D RID: 12045
	private ProgressBarUI m_ProgressBar;

	// Token: 0x04002F0E RID: 12046
	private Animator m_animator;

	// Token: 0x04002F0F RID: 12047
	private static readonly int m_iAlert = Animator.StringToHash("Alert");

	// Token: 0x04002F10 RID: 12048
	private static readonly int m_iPulse = Animator.StringToHash("Pulse");

	// Token: 0x04002F11 RID: 12049
	private DataStore m_dataStore;

	// Token: 0x04002F12 RID: 12050
	private static readonly DataStore.Id k_timeUpdatedId = new DataStore.Id("time.updated");
}
