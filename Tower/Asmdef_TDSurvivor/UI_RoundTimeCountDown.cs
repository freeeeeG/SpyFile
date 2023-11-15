using System;
using TMPro;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class UI_RoundTimeCountDown : AUISituational
{
	// Token: 0x06000A4A RID: 2634 RVA: 0x000266F3 File Offset: 0x000248F3
	private void OnEnable()
	{
		EventMgr.Register<float, float>(eGameEvents.OnUpdateRoundTimer, new Action<float, float>(this.OnUpdateRoundTimer));
		EventMgr.Register<bool>(eGameEvents.UI_ToggleRoundTimerUI, new Action<bool>(this.OnToggleRoundTimerUI));
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00026725 File Offset: 0x00024925
	private void OnDisable()
	{
		EventMgr.Remove<float, float>(eGameEvents.OnUpdateRoundTimer, new Action<float, float>(this.OnUpdateRoundTimer));
		EventMgr.Remove<bool>(eGameEvents.UI_ToggleRoundTimerUI, new Action<bool>(this.OnToggleRoundTimerUI));
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00026758 File Offset: 0x00024958
	private void OnUpdateRoundTimer(float time, float percentage)
	{
		int num = Mathf.CeilToInt(time);
		if (this.lastUpdatedTimeInt != num)
		{
			this.text_RoundTime.text = LocalizationManager.Instance.GetString("UI", "ROUND_TIME_LEFT", new object[]
			{
				num
			});
			this.lastUpdatedTimeInt = num;
			if (num > 0 && num <= 10)
			{
				SoundManager.PlaySound("UI", "Time_Countdown_Tick", -1f, -1f, -1f);
			}
		}
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x000267D2 File Offset: 0x000249D2
	private void OnToggleRoundTimerUI(bool isOn)
	{
		base.Toggle(isOn);
	}

	// Token: 0x040007EB RID: 2027
	[SerializeField]
	private TMP_Text text_RoundTime;

	// Token: 0x040007EC RID: 2028
	private int lastUpdatedTimeInt = -1;
}
