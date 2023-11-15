using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BA3 RID: 2979
[AddComponentMenu("KMonoBehaviour/scripts/NextUpdateTimer")]
public class NextUpdateTimer : KMonoBehaviour
{
	// Token: 0x06005CE9 RID: 23785 RVA: 0x002205D8 File Offset: 0x0021E7D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.initialAnimScale = this.UpdateAnimController.animScale;
	}

	// Token: 0x06005CEA RID: 23786 RVA: 0x002205F1 File Offset: 0x0021E7F1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005CEB RID: 23787 RVA: 0x002205F9 File Offset: 0x0021E7F9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshReleaseTimes();
	}

	// Token: 0x06005CEC RID: 23788 RVA: 0x00220608 File Offset: 0x0021E808
	public void UpdateReleaseTimes(string lastUpdateTime, string nextUpdateTime, string textOverride)
	{
		if (!System.DateTime.TryParse(lastUpdateTime, out this.currentReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse last_update_time: " + lastUpdateTime);
		}
		if (!System.DateTime.TryParse(nextUpdateTime, out this.nextReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse next_update_time: " + nextUpdateTime);
		}
		this.m_releaseTextOverride = textOverride;
		this.RefreshReleaseTimes();
	}

	// Token: 0x06005CED RID: 23789 RVA: 0x00220660 File Offset: 0x0021E860
	private void RefreshReleaseTimes()
	{
		TimeSpan timeSpan = this.nextReleaseDate - this.currentReleaseDate;
		TimeSpan timeSpan2 = this.nextReleaseDate - System.DateTime.UtcNow;
		TimeSpan timeSpan3 = System.DateTime.UtcNow - this.currentReleaseDate;
		string s = "4";
		string text;
		if (!string.IsNullOrEmpty(this.m_releaseTextOverride))
		{
			text = this.m_releaseTextOverride;
		}
		else if (timeSpan2.TotalHours < 8.0)
		{
			text = UI.DEVELOPMENTBUILDS.UPDATES.TWENTY_FOUR_HOURS;
			s = "4";
		}
		else if (timeSpan2.TotalDays < 1.0)
		{
			text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, 1);
			s = "3";
		}
		else
		{
			int num = timeSpan2.Days % 7;
			int num2 = (timeSpan2.Days - num) / 7;
			if (num2 <= 0)
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, num);
				s = "2";
			}
			else
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.BIGGER_TIMES, num, num2);
				s = "1";
			}
		}
		this.TimerText.text = text;
		this.UpdateAnimController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		float positionPercent = Mathf.Clamp01((float)(timeSpan3.TotalSeconds / timeSpan.TotalSeconds));
		this.UpdateAnimMeterController.SetPositionPercent(positionPercent);
	}

	// Token: 0x04003E80 RID: 16000
	public LocText TimerText;

	// Token: 0x04003E81 RID: 16001
	public KBatchedAnimController UpdateAnimController;

	// Token: 0x04003E82 RID: 16002
	public KBatchedAnimController UpdateAnimMeterController;

	// Token: 0x04003E83 RID: 16003
	public float initialAnimScale;

	// Token: 0x04003E84 RID: 16004
	public System.DateTime nextReleaseDate;

	// Token: 0x04003E85 RID: 16005
	public System.DateTime currentReleaseDate;

	// Token: 0x04003E86 RID: 16006
	private string m_releaseTextOverride;
}
