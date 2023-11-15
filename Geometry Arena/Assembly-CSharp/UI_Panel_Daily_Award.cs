using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CD RID: 205
public class UI_Panel_Daily_Award : MonoBehaviour
{
	// Token: 0x060006FE RID: 1790 RVA: 0x00026EFA File Offset: 0x000250FA
	public void OpenAndInit(int ranking, int dayIndex)
	{
		base.gameObject.SetActive(true);
		this.ranking = ranking;
		this.dayIndex = dayIndex;
		this.UpdateLanguage();
		this.GetAward();
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00026F24 File Offset: 0x00025124
	private void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.DailyChallenge dailyChallenge = inst.dailyChallenge;
		LanguageText.RuneInfo runeInfo = inst.runeInfo;
		this.textTitle.text = dailyChallenge.awardPanelTitle;
		this.textInfo.text = string.Concat(new object[]
		{
			NetworkTime.GetString_TimeWithDayIndex(this.dayIndex),
			" ",
			dailyChallenge.ranking,
			" ",
			this.ranking
		});
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00026FA0 File Offset: 0x000251A0
	private void GetAward()
	{
		int num;
		if (this.ranking == 1)
		{
			num = 600;
		}
		else if (this.ranking <= 3)
		{
			num = 450;
		}
		else if (this.ranking <= 10)
		{
			num = 360;
		}
		else if (this.ranking <= 20)
		{
			num = 240;
		}
		else if (this.ranking <= 30)
		{
			num = 120;
		}
		else if (this.ranking <= 50)
		{
			num = 60;
		}
		else
		{
			num = 30;
		}
		int num2 = 0;
		if (this.ranking == 1)
		{
			num2 = 3;
		}
		else if (this.ranking <= 3)
		{
			num2 = 2;
		}
		else if (this.ranking <= 10)
		{
			num2 = 1;
		}
		this.textGeometryCoin.text = num.ToString();
		GameData.inst.GeometryCoin_Get((long)num);
		for (int i = 0; i < num2; i++)
		{
			Rune rune = Rune.NewBigRune(15);
			this.iconRunes[i].UpdateIcon_WithRune(rune, UI_Icon_Rune.EnumIconRuneType.NEWVIEW);
			this.iconRunes[i].gameObject.SetActive(true);
			Rune.AddNewRune(rune);
		}
		for (int j = num2; j < 3; j++)
		{
			this.iconRunes[j].gameObject.SetActive(false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.transRunes);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.transGC);
		if (num2 == 0)
		{
			this.rectPanel.sizeDelta = new Vector2(this.rectPanel.sizeDelta.x, this.panelHeight_NoRune);
		}
		else
		{
			this.rectPanel.sizeDelta = new Vector2(this.rectPanel.sizeDelta.x, this.panelHeight_HasRune);
		}
		UI_Panel_Main_RunePanel.inst.Open();
		SaveFile.SaveByJson(false);
		if (this.ranking == 1)
		{
			MySteamAchievement.TryUnlockAchievementWithName("Daily_1");
		}
		if (this.ranking <= 3)
		{
			MySteamAchievement.TryUnlockAchievementWithName("Daily_2");
		}
		if (this.ranking <= 10)
		{
			MySteamAchievement.TryUnlockAchievementWithName("Daily_3");
		}
		if (this.ranking <= 20)
		{
			MySteamAchievement.TryUnlockAchievementWithName("Daily_4");
		}
		if (this.ranking <= 30)
		{
			MySteamAchievement.TryUnlockAchievementWithName("Daily_5");
		}
		if (this.ranking <= 50)
		{
			MySteamAchievement.TryUnlockAchievementWithName("Daily_6");
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x040005C9 RID: 1481
	[SerializeField]
	private Text textTitle;

	// Token: 0x040005CA RID: 1482
	[SerializeField]
	private Text textInfo;

	// Token: 0x040005CB RID: 1483
	[SerializeField]
	private Text textGeometryCoin;

	// Token: 0x040005CC RID: 1484
	[SerializeField]
	private UI_Icon_Rune[] iconRunes;

	// Token: 0x040005CD RID: 1485
	[SerializeField]
	private RectTransform transRunes;

	// Token: 0x040005CE RID: 1486
	[SerializeField]
	private RectTransform transGC;

	// Token: 0x040005CF RID: 1487
	[SerializeField]
	private int ranking = -1;

	// Token: 0x040005D0 RID: 1488
	[SerializeField]
	private int dayIndex = -1;

	// Token: 0x040005D1 RID: 1489
	[Header("Panel")]
	[SerializeField]
	private RectTransform rectPanel;

	// Token: 0x040005D2 RID: 1490
	[SerializeField]
	private float panelHeight_NoRune = 237f;

	// Token: 0x040005D3 RID: 1491
	[SerializeField]
	private float panelHeight_HasRune = 339f;
}
