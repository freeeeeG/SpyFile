using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000188 RID: 392
public class UI_StageProgress : AUISituational
{
	// Token: 0x06000A62 RID: 2658 RVA: 0x00026B1C File Offset: 0x00024D1C
	private void OnEnable()
	{
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Register<float, float>(eGameEvents.OnUpdateRoundTimer, new Action<float, float>(this.OnUpdateRoundTimer));
		EventMgr.Register<bool, bool>(eGameEvents.UI_ToggleRoundTimerUI, new Action<bool, bool>(this.OnToggleRoundTimerUI));
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00026B74 File Offset: 0x00024D74
	private void OnDisable()
	{
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Remove<float, float>(eGameEvents.OnUpdateRoundTimer, new Action<float, float>(this.OnUpdateRoundTimer));
		EventMgr.Remove<bool, bool>(eGameEvents.UI_ToggleRoundTimerUI, new Action<bool, bool>(this.OnToggleRoundTimerUI));
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00026BC9 File Offset: 0x00024DC9
	private void Start()
	{
		this.timeBarWidth = this.rectTransform_TimeBar_BG.sizeDelta.x;
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00026BE1 File Offset: 0x00024DE1
	private void OnRoundStart(int round, int totalRound)
	{
		if (round == totalRound)
		{
			this.text_RoundCount.text = "FINAL WAVE";
			return;
		}
		this.text_RoundCount.text = string.Format("WAVE {0}/{1}", round, totalRound);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00026C1C File Offset: 0x00024E1C
	private void OnUpdateRoundTimer(float time, float percentage)
	{
		if (!this.doShowCountdown)
		{
			return;
		}
		int num = Mathf.CeilToInt(time);
		this.image_BarOutline.color = this.gradient_BarColor.Evaluate(percentage);
		this.slider_TimeBar.value = percentage;
		if (this.lastUpdatedTimeInt != num)
		{
			this.text_RoundTime.text = num.ToString();
			this.lastUpdatedTimeInt = num;
			if (num > 0 && num <= 10)
			{
				SoundManager.PlaySound("UI", "Time_Countdown_Tick", -1f, -1f, -1f);
			}
		}
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00026CA8 File Offset: 0x00024EA8
	private void OnToggleRoundTimerUI(bool isOn, bool doShowCountdown)
	{
		this.doShowCountdown = doShowCountdown;
		base.Toggle(isOn);
		if (isOn)
		{
			this.text_PrepareFirstRound.gameObject.SetActive(!doShowCountdown);
			this.node_ProgressBar.gameObject.SetActive(doShowCountdown);
			this.node_Countdown.gameObject.SetActive(doShowCountdown);
		}
	}

	// Token: 0x040007F3 RID: 2035
	[SerializeField]
	private Image image_TimeBar;

	// Token: 0x040007F4 RID: 2036
	[SerializeField]
	private Image image_BarOutline;

	// Token: 0x040007F5 RID: 2037
	[SerializeField]
	private Transform node_Countdown;

	// Token: 0x040007F6 RID: 2038
	[SerializeField]
	private Transform node_ProgressBar;

	// Token: 0x040007F7 RID: 2039
	[SerializeField]
	private RectTransform rectTransform_TimeBar;

	// Token: 0x040007F8 RID: 2040
	[SerializeField]
	private RectTransform rectTransform_TimeBar_BG;

	// Token: 0x040007F9 RID: 2041
	[SerializeField]
	private TMP_Text text_RoundCount;

	// Token: 0x040007FA RID: 2042
	[SerializeField]
	private TMP_Text text_RoundTime;

	// Token: 0x040007FB RID: 2043
	[SerializeField]
	private TMP_Text text_PrepareFirstRound;

	// Token: 0x040007FC RID: 2044
	[SerializeField]
	private Gradient gradient_BarColor;

	// Token: 0x040007FD RID: 2045
	[SerializeField]
	private Slider slider_TimeBar;

	// Token: 0x040007FE RID: 2046
	private int lastUpdatedTimeInt = -1;

	// Token: 0x040007FF RID: 2047
	private float timeBarWidth = -1f;

	// Token: 0x04000800 RID: 2048
	private bool doShowCountdown;
}
