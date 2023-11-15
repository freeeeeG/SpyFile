using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D0 RID: 208
public class UI_Panel_Main_RuneSynResult : MonoBehaviour
{
	// Token: 0x06000733 RID: 1843 RVA: 0x00028757 File Offset: 0x00026957
	private void OnEnable()
	{
		UI_Panel_Main_RuneSynResult.inst = this;
		this.ResetAutoFuse();
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00028765 File Offset: 0x00026965
	private void OnDisable()
	{
		this.ResetAutoFuse();
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0002876D File Offset: 0x0002696D
	private void ResetAutoFuse()
	{
		this.autoFuse_Flag = false;
		this.autoFuse_OnProcess = false;
		this.autoFuse_CountDown = 0.5f;
		this.autoFuse_ListProps = new List<Rune_Property>();
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00028794 File Offset: 0x00026994
	private Rune[] GetRunes3_MaterialAndResult()
	{
		return new Rune[]
		{
			GameData.inst.runes[GameData.inst.runeFusion_MaterialIndexs[0]],
			GameData.inst.runes[GameData.inst.runeFusion_MaterialIndexs[1]],
			GameData.inst.runeFusion_Result
		};
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x000287F0 File Offset: 0x000269F0
	public void OpenAndInitWithRunes(int[] materialIndexs, Rune result, bool ifSave = true, bool ifFirstOpen = false)
	{
		GameData.inst.ifOnFusion = true;
		GameData.inst.runeFusion_MaterialIndexs = materialIndexs;
		GameData.inst.runeFusion_Result = result;
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		Rune[] runes3_MaterialAndResult = this.GetRunes3_MaterialAndResult();
		if (ifFirstOpen)
		{
			this.text_Accept.color = Color.white;
			this.text_ReFuse.color = Color.white;
			this.text_DecideLater.color = Color.white;
		}
		if (runes3_MaterialAndResult.Length < 3)
		{
			Debug.LogError("Error_合成预览runeIndexs长度不够！");
			return;
		}
		this.UpdatePanel();
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00028887 File Offset: 0x00026A87
	private void UpdatePanel()
	{
		this.UpdateLanguage();
		this.UpdateIcons();
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00028898 File Offset: 0x00026A98
	private void UpdateIcons()
	{
		Rune[] runes3_MaterialAndResult = this.GetRunes3_MaterialAndResult();
		for (int i = 0; i < 3; i++)
		{
			this.childPanels[i].index = i;
			this.childPanels[i].InitWithRune(runes3_MaterialAndResult[i], this.singleProp_MaxWidth, UI_Icon_Rune.EnumIconRuneType.SYNRESULT, this.autoFuse_Flag, i != 2);
		}
		this.autoFuse_Icon.UpdateIcon();
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x000288FC File Offset: 0x00026AFC
	private void UpdateLanguage()
	{
		LanguageText.RuneInfo runeInfo = LanguageText.Inst.runeInfo;
		this.text_Title.text = runeInfo.synResult_BigTitle;
		this.text_Tip.text = runeInfo.synResult_Tip;
		this.text_Accept.text = runeInfo.synResult_Choose.AppendKeycode("F");
		this.text_DecideLater.text = runeInfo.button_DecideLater.AppendKeycode("Esc");
		this.text_Rule.text = runeInfo.fuseRule;
		if (!this.autoFuse_Flag)
		{
			this.text_ReFuse.text = runeInfo.button_ReFuse.AppendKeycode("R");
			this.autoFuse_TextProcessTip.gameObject.SetActive(false);
			return;
		}
		if (this.autoFuse_OnProcess)
		{
			this.text_ReFuse.text = runeInfo.autoFuse_Stop.AppendKeycode("R");
			this.autoFuse_TextProcessTip.text = runeInfo.autoFuse_TipInfo_OnProcess.ReplaceLineBreak();
			this.autoFuse_TextProcessTip.gameObject.SetActive(true);
			return;
		}
		this.text_ReFuse.text = runeInfo.autoFuse_Start.AppendKeycode("R");
		int num = this.AutoFuse_MaxPropCount();
		int count = this.autoFuse_ListProps.Count;
		int num2 = num - count;
		this.autoFuse_TextProcessTip.text = runeInfo.autoFuse_TipInfo_NotProcess.Replace("propResultMax", num.ToString().Colored(Color.green)).Replace("propLock", count.ToString().Colored(Color.green)).Replace("propToLock", num2.ToString().Colored(Color.green)).ReplaceLineBreak();
		this.autoFuse_TextProcessTip.gameObject.SetActive(true);
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00028AAA File Offset: 0x00026CAA
	public void Button_TryReFuse()
	{
		if (this.autoFuse_Flag)
		{
			if (this.autoFuse_OnProcess)
			{
				this.autoFuse_OnProcess = false;
			}
			else
			{
				this.autoFuse_OnProcess = true;
			}
			this.autoFuse_CountDown = 0.5f;
			this.UpdatePanel();
			return;
		}
		this.TryReFuse();
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00028AE4 File Offset: 0x00026CE4
	private void TryReFuse()
	{
		int num = 1;
		if (GameData.inst.GeometryCoin >= (long)num)
		{
			GameData.inst.GeometryCoin_Use((long)num);
			UI_Panel_Main_RunePanel.inst.RuneSyn_Syn(false, false);
			UI_Panel_Main_RunePanel.inst.Open();
			this.autoSaveCount++;
			if (this.autoSaveCount >= 27)
			{
				SaveFile.SaveByJson(false);
				this.autoSaveCount = 0;
			}
			return;
		}
		UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.lackOfGeometryCoin);
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00028B64 File Offset: 0x00026D64
	public void Choose(int chooseIndex)
	{
		Rune[] runes3_MaterialAndResult = this.GetRunes3_MaterialAndResult();
		List<Rune> list = new List<Rune>();
		for (int i = 0; i < 2; i++)
		{
			if (i != chooseIndex)
			{
				list.Add(runes3_MaterialAndResult[i]);
			}
		}
		foreach (Rune rune in list)
		{
			Rune.RemoveRune(rune);
		}
		if (chooseIndex == 2)
		{
			Rune.AddNewRune(runes3_MaterialAndResult[2]);
		}
		GameData.inst.runeFusion_MaterialIndexs[0] = -1;
		GameData.inst.runeFusion_MaterialIndexs[1] = -1;
		GameData.inst.ifOnFusion = false;
		UI_Panel_Main_RunePanel.inst.Open();
		base.gameObject.SetActive(false);
		SaveFile.SaveByJson(false);
		UI_ToolTip.inst.TryClose();
		MySteamAchievement.TryUnlockAchievementWithName("RuneFusion");
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00028C38 File Offset: 0x00026E38
	public void Button_CancleFusion()
	{
		GameData.inst.ifOnFusion = false;
		UI_Panel_Main_RunePanel.inst.Open();
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00028C4F File Offset: 0x00026E4F
	private void Update()
	{
		this.UpdateShortCut();
		this.Update_AutoFuse();
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00028C5D File Offset: 0x00026E5D
	private void UpdateShortCut()
	{
		if (MyInput.GetKeyDownWithSound(KeyCode.F))
		{
			this.Choose(2);
		}
		if (MyInput.GetKeyDownWithSound(KeyCode.R))
		{
			this.Button_TryReFuse();
		}
		if (MyInput.GetKeyDownWithSound(KeyCode.Escape))
		{
			this.Button_CancleFusion();
		}
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00028C90 File Offset: 0x00026E90
	private void Update_AutoFuse()
	{
		if (!this.autoFuse_OnProcess)
		{
			return;
		}
		if (GameData.inst.GeometryCoin <= 0L)
		{
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.runeInfo.autoFuse_StopTip_LackOfGeometryCoin);
			this.autoFuse_OnProcess = false;
			this.autoFuse_CountDown = 0.5f;
			this.UpdatePanel();
			return;
		}
		this.autoFuse_CountDown -= Time.deltaTime;
		if (this.autoFuse_CountDown <= 0f)
		{
			this.TryReFuse();
			this.autoFuse_CountDown += 0.05f;
			if (this.AutoFuse_IfResultSuit())
			{
				UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.runeInfo.autoFuse_StopTip_Finish);
				this.autoFuse_OnProcess = false;
				this.autoFuse_CountDown = 0.5f;
				this.UpdatePanel();
				return;
			}
		}
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00028D58 File Offset: 0x00026F58
	public void Button_SwitchAutoFuse()
	{
		this.autoFuse_Flag = !this.autoFuse_Flag;
		this.autoFuse_OnProcess = false;
		this.autoFuse_CountDown = 0.5f;
		if (this.autoFuse_Flag)
		{
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.runeInfo.autoFuse_FloatTip_Enable);
		}
		else
		{
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.runeInfo.autoFuse_FloatTip_Disable);
		}
		this.UpdatePanel();
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00028DC8 File Offset: 0x00026FC8
	private bool AutoFuse_IfResultSuit()
	{
		Rune runeFusion_Result = GameData.inst.runeFusion_Result;
		int count = this.autoFuse_ListProps.Count;
		int num = 0;
		foreach (Rune_Property prop in this.autoFuse_ListProps)
		{
			foreach (Rune_Property prop2 in runeFusion_Result.props)
			{
				if (Rune_Property.AutoFuse_IfPropEqual(prop, prop2))
				{
					num++;
					break;
				}
			}
		}
		return count == num;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00028E68 File Offset: 0x00027068
	public void AutoFuse_AddProp(Rune_Property originProp)
	{
		Rune_Property rune_Property = new Rune_Property();
		rune_Property.Clone(originProp);
		foreach (Rune_Property rune_Property2 in this.autoFuse_ListProps)
		{
			if (Rune_Property.AutoFuse_IfPropEqual(rune_Property2, rune_Property))
			{
				this.autoFuse_ListProps.Remove(rune_Property2);
				this.UpdatePanel();
				return;
			}
		}
		foreach (Rune_Property rune_Property3 in this.autoFuse_ListProps)
		{
			if (Rune_Property.AutoFuse_IfPropConflict(rune_Property3, rune_Property))
			{
				this.autoFuse_ListProps.Remove(rune_Property3);
				break;
			}
		}
		if (this.autoFuse_ListProps.Count < this.AutoFuse_MaxPropCount())
		{
			this.autoFuse_ListProps.Add(rune_Property);
		}
		else
		{
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.runeInfo.autoFuse_StopTip_CantSelectMore);
		}
		this.UpdatePanel();
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00028F78 File Offset: 0x00027178
	public int AutoFuse_MaxPropCount()
	{
		Rune rune = this.GetRunes3_MaterialAndResult()[0];
		Rune rune2 = this.GetRunes3_MaterialAndResult()[1];
		int a = (int)(rune.rank + 2);
		int b = (int)(rune2.rank + 2);
		int num = Mathf.Min(Mathf.Max(a, b) + 1, 5);
		int num2 = Mathf.Min(rune.addiSlot, rune2.addiSlot);
		int num3 = Mathf.Max(rune.addiSlot, rune2.addiSlot);
		int num4;
		if (num3 - num2 >= 3)
		{
			num4 = num3 - 1;
		}
		else
		{
			num4 = Mathf.Min(num3 + 1, 4);
		}
		return num + num4;
	}

	// Token: 0x040005FB RID: 1531
	public static UI_Panel_Main_RuneSynResult inst;

	// Token: 0x040005FC RID: 1532
	[SerializeField]
	private UI_Panel_Main_RuneSynResult_Child[] childPanels;

	// Token: 0x040005FD RID: 1533
	[SerializeField]
	private Text text_Title;

	// Token: 0x040005FE RID: 1534
	[SerializeField]
	private Text text_Tip;

	// Token: 0x040005FF RID: 1535
	[SerializeField]
	private Text text_Accept;

	// Token: 0x04000600 RID: 1536
	[SerializeField]
	private Text text_ReFuse;

	// Token: 0x04000601 RID: 1537
	[SerializeField]
	private Text text_DecideLater;

	// Token: 0x04000602 RID: 1538
	[SerializeField]
	private Text text_Rule;

	// Token: 0x04000603 RID: 1539
	[SerializeField]
	private float singleProp_MaxWidth = 450f;

	// Token: 0x04000604 RID: 1540
	[Header("Save")]
	private int autoSaveCount;

	// Token: 0x04000605 RID: 1541
	[Header("AutoFuse")]
	[SerializeField]
	public bool autoFuse_Flag;

	// Token: 0x04000606 RID: 1542
	[SerializeField]
	private float autoFuse_CountDown = 0.5f;

	// Token: 0x04000607 RID: 1543
	[SerializeField]
	private UI_Icon_AutoFuse autoFuse_Icon;

	// Token: 0x04000608 RID: 1544
	[SerializeField]
	public List<Rune_Property> autoFuse_ListProps = new List<Rune_Property>();

	// Token: 0x04000609 RID: 1545
	[SerializeField]
	private bool autoFuse_OnProcess;

	// Token: 0x0400060A RID: 1546
	[SerializeField]
	private Text autoFuse_TextProcessTip;
}
