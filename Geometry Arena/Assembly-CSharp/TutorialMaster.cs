using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DB RID: 219
public class TutorialMaster : MonoBehaviour
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x0002A282 File Offset: 0x00028482
	private bool ifInited
	{
		get
		{
			return GameData.inst.record.tutorial_Inited;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x0002A293 File Offset: 0x00028493
	private bool ifSkipTutorial
	{
		get
		{
			return Setting.Inst.Option_SkipTutorial;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x0002A29F File Offset: 0x0002849F
	private bool[] flags
	{
		get
		{
			return GameData.inst.record.tutorial_Flags;
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0002A2B0 File Offset: 0x000284B0
	private void Awake()
	{
		TutorialMaster.inst = this;
		this.DeActivate();
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0002A2C0 File Offset: 0x000284C0
	private void Update()
	{
		if (!this.ifInited)
		{
			return;
		}
		this.UpdateColorfulOutline();
		if (this.ifActive)
		{
			if (!this.IfAvailable(this.currentID))
			{
				this.DeActivate();
			}
			if (TempData.inst.currentSceneType == EnumSceneType.BATTLE && BattleManager.inst != null && BattleManager.inst.ifGameOver)
			{
				this.DeActivate();
			}
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0002A324 File Offset: 0x00028524
	private void UpdateColorfulOutline()
	{
		if (this.rectColorfulTip.gameObject.activeInHierarchy)
		{
			this.color_Hue += Time.deltaTime * this.color_HueDelta;
			if (this.color_Hue > 1f)
			{
				this.color_Hue -= 1f;
			}
			Color color = Color.HSVToRGB(this.color_Hue, this.color_Sat, this.color_Value);
			this.imageColorfulOutline.color = color;
			if (this.currentID == -1)
			{
				Debug.LogError("Error_CurrentID=-1");
				this.DeActivate();
				return;
			}
			RectTransform rectTransform = this.rectsEachID[this.currentID];
			this.rectColorfulTip.sizeDelta = rectTransform.sizeDelta * 1.05f + Vector2.one * 9f;
			this.rectColorfulTip.position = rectTransform.position;
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0002A40C File Offset: 0x0002860C
	public void Activate()
	{
		this.ifActive = true;
		if (!this.ifInited)
		{
			if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
			{
				this.DeActivate();
				return;
			}
			this.rectColorfulTip.gameObject.SetActive(false);
			this.rectTexts.gameObject.SetActive(false);
			this.rectInit.gameObject.SetActive(true);
			LanguageText.Tutorial tutorial = LanguageText.Inst.tutorial;
			this.textInit_Title.text = tutorial.Init_AskTitle;
			this.textInit_Yes.text = tutorial.Init_Yes;
			this.textInit_No.text = tutorial.Init_No;
			RectTransform[] array = this.rectsInit;
			for (int i = 0; i < array.Length; i++)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(array[i]);
			}
			return;
		}
		else
		{
			if (Setting.Inst != null && this.ifSkipTutorial)
			{
				this.DeActivate();
				return;
			}
			this.FindNewTutorialID();
			return;
		}
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0002A4EA File Offset: 0x000286EA
	public void DeActivate()
	{
		this.rectColorfulTip.gameObject.SetActive(false);
		this.rectTexts.gameObject.SetActive(false);
		this.rectInit.gameObject.SetActive(false);
		this.ifActive = false;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002A528 File Offset: 0x00028728
	private void InitAllPanels()
	{
		if (this.currentID == -1)
		{
			Debug.LogError("Error_没有教程内容");
			this.DeActivate();
			return;
		}
		this.ifActive = true;
		this.rectColorfulTip.gameObject.SetActive(true);
		this.rectTexts.gameObject.SetActive(true);
		this.rectInit.gameObject.SetActive(false);
		TutorialData tutorialData = DataBase.Inst.dataTutorials[this.currentID];
		this.textTitle.text = tutorialData.name;
		this.textInfo.text = tutorialData.info.GetString().ReplaceLineBreak();
		this.textInTextSkip.text = LanguageText.Inst.tutorial.InText_Skip;
		switch (tutorialData.posType)
		{
		case 1:
			this.rectTexts.anchorMin = new Vector2(0f, 1f);
			this.rectTexts.anchorMax = new Vector2(0f, 1f);
			this.rectTexts.pivot = new Vector2(0f, 1f);
			break;
		case 2:
			this.rectTexts.anchorMin = new Vector2(1f, 1f);
			this.rectTexts.anchorMax = new Vector2(1f, 1f);
			this.rectTexts.pivot = new Vector2(1f, 1f);
			break;
		case 3:
			this.rectTexts.anchorMin = new Vector2(0f, 0f);
			this.rectTexts.anchorMax = new Vector2(0f, 0f);
			this.rectTexts.pivot = new Vector2(0f, 0f);
			break;
		case 4:
			this.rectTexts.anchorMin = new Vector2(1f, 0f);
			this.rectTexts.anchorMax = new Vector2(1f, 0f);
			this.rectTexts.pivot = new Vector2(1f, 0f);
			break;
		case 5:
			this.rectTexts.anchorMin = new Vector2(0.5f, 1f);
			this.rectTexts.anchorMax = new Vector2(0.5f, 1f);
			this.rectTexts.pivot = new Vector2(0.5f, 1f);
			break;
		}
		this.rectTexts.anchoredPosition = Vector2.zero;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0002A7B5 File Offset: 0x000289B5
	public void Button_Init_Active()
	{
		GameData.inst.record.tutorial_Inited = true;
		GameData.inst.record.InitTutorialFlags();
		Setting.Inst.setBools[21] = false;
		SaveFile_Settings.SaveByJson_Settings();
		this.FindNewTutorialID();
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0002A7EF File Offset: 0x000289EF
	public void Button_Init_Skip()
	{
		GameData.inst.record.tutorial_Inited = true;
		GameData.inst.record.InitTutorialFlags();
		Setting.Inst.setBools[21] = true;
		SaveFile_Settings.SaveByJson_Settings();
		this.DeActivate();
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0002A829 File Offset: 0x00028A29
	public void Button_InText_Skip()
	{
		this.Button_Init_Skip();
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0002A831 File Offset: 0x00028A31
	public void TrigID(int ID)
	{
		if (this.ifSkipTutorial)
		{
			return;
		}
		if (ID != this.currentID)
		{
			return;
		}
		this.flags[ID] = true;
		this.FindNewTutorialID();
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0002A858 File Offset: 0x00028A58
	private void FindNewTutorialID()
	{
		int num = -1;
		foreach (TutorialData tutorialData in DataBase.Inst.dataTutorials)
		{
			if (this.IfAvailable(tutorialData.ID))
			{
				num = tutorialData.ID;
				break;
			}
		}
		if (num == -1)
		{
			this.DeActivate();
			return;
		}
		this.currentID = num;
		this.InitAllPanels();
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0002A8B4 File Offset: 0x00028AB4
	private bool IfAvailable(int ID)
	{
		if (this.flags[ID])
		{
			return false;
		}
		if (this.rectsEachID[ID] == null)
		{
			return false;
		}
		if (!this.rectsEachID[ID].gameObject.activeInHierarchy)
		{
			return false;
		}
		bool result = false;
		int preID = DataBase.Inst.dataTutorials[ID].preID;
		if (preID == -1)
		{
			result = true;
		}
		else if (this.flags[preID])
		{
			result = true;
		}
		switch (ID)
		{
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
			if (Battle.inst.SequalWave < 2)
			{
				return false;
			}
			break;
		case 10:
			if (GameData.inst.Star < 300L)
			{
				return false;
			}
			break;
		case 12:
		{
			int num = 0;
			Skill.SkillCurrent(ref num);
			int num2 = (TempData.inst.jobId <= 8) ? 3 : 1;
			if (num < num2)
			{
				return false;
			}
			if (GameData.inst.jobs[TempData.inst.jobId].mastery.GetRank() < 1)
			{
				return false;
			}
			break;
		}
		case 13:
			if (!GameData.inst.ifFinished)
			{
				return false;
			}
			break;
		}
		return result;
	}

	// Token: 0x0400065F RID: 1631
	[SerializeField]
	public static TutorialMaster inst;

	// Token: 0x04000660 RID: 1632
	[SerializeField]
	private RectTransform[] rectsEachID;

	// Token: 0x04000661 RID: 1633
	[SerializeField]
	private bool ifActive = true;

	// Token: 0x04000662 RID: 1634
	[SerializeField]
	private int currentID = -1;

	// Token: 0x04000663 RID: 1635
	[Header("各个面板")]
	[SerializeField]
	private RectTransform rectColorfulTip;

	// Token: 0x04000664 RID: 1636
	[SerializeField]
	private RectTransform rectTexts;

	// Token: 0x04000665 RID: 1637
	[SerializeField]
	private RectTransform rectInit;

	// Token: 0x04000666 RID: 1638
	[Header("教程描述")]
	[SerializeField]
	private Text textTitle;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	private Text textInfo;

	// Token: 0x04000668 RID: 1640
	[SerializeField]
	private Text textInTextSkip;

	// Token: 0x04000669 RID: 1641
	[Header("七彩边框参数")]
	[SerializeField]
	private Image imageColorfulOutline;

	// Token: 0x0400066A RID: 1642
	[SerializeField]
	private float color_Sat = 0.4f;

	// Token: 0x0400066B RID: 1643
	[SerializeField]
	private float color_Value = 0.9f;

	// Token: 0x0400066C RID: 1644
	[SerializeField]
	private float color_Hue;

	// Token: 0x0400066D RID: 1645
	[SerializeField]
	private float color_HueDelta = 0.3f;

	// Token: 0x0400066E RID: 1646
	[Header("初始化对话框")]
	[SerializeField]
	private RectTransform[] rectsInit;

	// Token: 0x0400066F RID: 1647
	[SerializeField]
	private Text textInit_Title;

	// Token: 0x04000670 RID: 1648
	[SerializeField]
	private Text textInit_Yes;

	// Token: 0x04000671 RID: 1649
	[SerializeField]
	private Text textInit_No;
}
