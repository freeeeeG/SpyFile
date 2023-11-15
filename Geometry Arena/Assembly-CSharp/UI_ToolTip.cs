using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E3 RID: 227
public class UI_ToolTip : MonoBehaviour
{
	// Token: 0x060007E1 RID: 2017 RVA: 0x0002B655 File Offset: 0x00029855
	private void Awake()
	{
		UI_ToolTip.inst = this;
		this.rectTrans = this.mainPanel.GetComponent<RectTransform>();
		this.Close();
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0002B674 File Offset: 0x00029874
	private void Update()
	{
		this.FollowMouse();
		if (!this.flag_Open)
		{
			this.lifetime -= Time.deltaTime;
			if (this.lifetime < 0f)
			{
				this.mainPanel.SetActive(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.Close();
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0002B6CC File Offset: 0x000298CC
	private void FollowMouse()
	{
		if (!this.mainPanel.activeSelf)
		{
			return;
		}
		Vector2 vector = base.transform.root.InverseTransformPoint(Input.mousePosition);
		float b = this.rectTrans.sizeDelta.y - 540f;
		if (vector.x >= 0f)
		{
			this.SetPosition_Left();
		}
		else
		{
			this.SetPosition_Right();
		}
		this.contentSizeFitterText.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransText);
		float x = this.rectTransText.sizeDelta.x;
		float y = this.rectTransText.sizeDelta.y;
		if (this.ifRight)
		{
			if (vector.x + x > this.xPosMax)
			{
				this.contentSizeFitterText.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
				float x2 = this.xPosMax - vector.x;
				this.rectTransText.sizeDelta = new Vector2(x2, y);
			}
		}
		else if (vector.x - x < this.xPosMin)
		{
			this.contentSizeFitterText.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			float x3 = vector.x - this.xPosMin;
			this.rectTransText.sizeDelta = new Vector2(x3, y);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
		base.gameObject.transform.localPosition = new Vector2(vector.x, Mathf.Max(vector.y, b));
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0002B82C File Offset: 0x00029A2C
	private void ShowUp(int pos)
	{
		if (Setting.Inst.Option_NoToolTipInBattle && TempData.inst.currentSceneType == EnumSceneType.BATTLE && BattleManager.inst.timeStage != EnumTimeStage.REST && Time.timeScale != 0f)
		{
			return;
		}
		this.lifetime = 1f;
		this.flag_Open = true;
		this.mainPanel.SetActive(true);
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0002B889 File Offset: 0x00029A89
	public void Close()
	{
		if (this.mainPanel != null)
		{
			this.mainPanel.SetActive(false);
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0002B8A5 File Offset: 0x00029AA5
	public void TryClose()
	{
		this.flag_Open = false;
		this.lifetime = 0.06f;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0002B8B9 File Offset: 0x00029AB9
	public void SetPosition_Left()
	{
		RectTransform component = this.mainPanel.GetComponent<RectTransform>();
		component.pivot = new Vector2(1f, 1f);
		component.localPosition = this.vec2Left;
		this.ifRight = false;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0002B8F2 File Offset: 0x00029AF2
	public void SetPosition_Right()
	{
		RectTransform component = this.mainPanel.GetComponent<RectTransform>();
		component.pivot = new Vector2(0f, 1f);
		component.localPosition = this.vec2Right;
		this.ifRight = true;
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0002B92C File Offset: 0x00029B2C
	public void ShowWithTalent(UI_Icon_Talent talent)
	{
		this.ShowUp(1);
		string info_Talent = UI_ToolTipInfo.GetInfo_Talent(talent.jobID, talent.talentID);
		this.infoText.text = info_Talent;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0002B96C File Offset: 0x00029B6C
	public void ShowWithVarColor(UI_Icon_Color color)
	{
		this.ShowUp(1);
		string info_VarColor = UI_ToolTipInfo.GetInfo_VarColor(color.varColorID);
		this.infoText.text = info_VarColor;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0002B9A4 File Offset: 0x00029BA4
	public void ShowWithAbilityMain(int abiID)
	{
		this.ShowUp(0);
		string text = UI_ToolTipInfo.GetInfo_AbilityMain(abiID).ReplaceLineBreak();
		this.infoText.text = text;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0002B9DC File Offset: 0x00029BDC
	public void ShowWithAbilityBattle(int abiID)
	{
		this.ShowUp(1);
		string text = UI_ToolTipInfo.GetInfo_AbilityBattle(abiID).ReplaceLineBreak();
		this.infoText.text = text;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0002BA14 File Offset: 0x00029C14
	public void ShowWithFactorBattle(int fbID)
	{
		this.ShowUp(1);
		string info_FactorBattle = UI_ToolTipInfo.GetInfo_FactorBattle(fbID);
		this.infoText.text = info_FactorBattle;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0002BA48 File Offset: 0x00029C48
	public void ShowWithJob(UI_Icon_Job job)
	{
		this.ShowUp(1);
		string info_Job = UI_ToolTipInfo.GetInfo_Job(job.jobID);
		this.infoText.text = info_Job;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0002BA80 File Offset: 0x00029C80
	public void ShowWithTips(int index, int place)
	{
		this.ShowUp(place);
		string text = LanguageText.Inst.toolTip_TipStrings[index].Sized(UI_Setting.Inst.ToolTip_NormalSize).ReplaceLineBreak();
		this.infoText.text = text;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0002BACC File Offset: 0x00029CCC
	public void ShowWithStringAndPlace(string s, int place)
	{
		this.ShowUp(place);
		this.infoText.text = s;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x0002BAEC File Offset: 0x00029CEC
	public void ShowWithIconSkill(int jobNo, int skillLevel)
	{
		this.ShowUp(1);
		string info_Skill = UI_ToolTipInfo.GetInfo_Skill(jobNo, skillLevel);
		this.infoText.text = info_Skill;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0002BB20 File Offset: 0x00029D20
	public void ShowWithIconInfinity(int modeID, bool ifUnlocked, bool ifOpen)
	{
		this.ShowUp(0);
		string info_Mode = UI_ToolTipInfo.GetInfo_Mode(modeID, ifUnlocked, ifOpen);
		this.infoText.text = info_Mode;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x0002BB54 File Offset: 0x00029D54
	public void ShowWithUpgrade(int upID)
	{
		this.ShowUp(0);
		string info_Upgrade = UI_ToolTipInfo.GetInfo_Upgrade(upID);
		this.infoText.text = info_Upgrade;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0002BB88 File Offset: 0x00029D88
	public void ShowWithSettingInfo(int setID)
	{
		string text = LanguageText.Inst.panelSetting.setToolTipInfos[setID];
		if (text == "null")
		{
			return;
		}
		this.ShowUp(1);
		this.infoText.text = text.ReplaceLineBreak().ReplaceTabChar();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0002BBDD File Offset: 0x00029DDD
	public void ShowWithString(string s)
	{
		this.ShowUp(0);
		this.infoText.text = s.ReplaceLineBreak().ReplaceTabChar();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTrans);
	}

	// Token: 0x0400069A RID: 1690
	public static UI_ToolTip inst;

	// Token: 0x0400069B RID: 1691
	[SerializeField]
	private Text infoText;

	// Token: 0x0400069C RID: 1692
	[SerializeField]
	private GameObject mainPanel;

	// Token: 0x0400069D RID: 1693
	[SerializeField]
	private RectTransform rectTrans;

	// Token: 0x0400069E RID: 1694
	[SerializeField]
	private RectTransform rectTransText;

	// Token: 0x0400069F RID: 1695
	[SerializeField]
	private ContentSizeFitter contentSizeFitterText;

	// Token: 0x040006A0 RID: 1696
	[SerializeField]
	private Vector2 vec2Left = Vector2.zero;

	// Token: 0x040006A1 RID: 1697
	[SerializeField]
	private Vector2 vec2Right = Vector2.zero;

	// Token: 0x040006A2 RID: 1698
	[SerializeField]
	private float xPosMin = -900f;

	// Token: 0x040006A3 RID: 1699
	[SerializeField]
	private float xPosMax = 900f;

	// Token: 0x040006A4 RID: 1700
	public float lifetime;

	// Token: 0x040006A5 RID: 1701
	public bool flag_Open;

	// Token: 0x040006A6 RID: 1702
	public bool ifRight;
}
