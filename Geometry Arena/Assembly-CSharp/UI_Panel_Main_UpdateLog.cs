using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CB RID: 203
public class UI_Panel_Main_UpdateLog : MonoBehaviour
{
	// Token: 0x060006F1 RID: 1777 RVA: 0x00026B92 File Offset: 0x00024D92
	public void Open()
	{
		base.gameObject.SetActive(true);
		MainCanvas.ChildPanelOpen();
		this.updateLog = UpdateLog.Inst;
		this.logIndex = this.updateLog.versions.Length - 1;
		this.UpdateLanguages();
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00023B9D File Offset: 0x00021D9D
	public void Close()
	{
		base.gameObject.SetActive(false);
		MainCanvas.ChildPanelClose();
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00026BCB File Offset: 0x00024DCB
	private void Update()
	{
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00026BDC File Offset: 0x00024DDC
	public void NextPage()
	{
		this.updateLog = UpdateLog.Inst;
		if (this.logIndex == this.updateLog.versions.Length - 1)
		{
			return;
		}
		if (this.logIndex > this.updateLog.versions.Length - 1)
		{
			Debug.LogError("IndexBig!");
			this.logIndex = this.updateLog.versions.Length - 1;
		}
		this.logIndex++;
		this.UpdateLanguages();
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00026C58 File Offset: 0x00024E58
	public void PreviousPage()
	{
		this.updateLog = UpdateLog.Inst;
		if (this.logIndex == 0)
		{
			return;
		}
		if (this.logIndex < 0)
		{
			Debug.LogError("IndexSmall!");
			this.logIndex = 0;
		}
		this.logIndex--;
		this.UpdateLanguages();
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00026CA7 File Offset: 0x00024EA7
	public void GoPage(int num)
	{
		this.logIndex += num;
		this.IndexFix();
		this.UpdateLanguages();
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00026CC3 File Offset: 0x00024EC3
	private void IndexFix()
	{
		this.logIndex = Mathf.Clamp(this.logIndex, 0, this.updateLog.versions.Length - 1);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00026CE8 File Offset: 0x00024EE8
	private void UpdateLanguages()
	{
		UI_Setting.UpdateLog updateLog = UI_Setting.Inst.updateLog;
		LanguageText.Lang_UpdateLog main_UpdateLog = LanguageText.Inst.main_UpdateLog;
		UpdateLog.Versions versions = this.updateLog.versions[this.logIndex];
		this.lang_Title.text = main_UpdateLog.title.TextSet(updateLog.setTitle);
		this.lang_Version.text = ("v" + versions.version + "\n" + versions.updateDate).TextSet(updateLog.setVersion);
		this.lang_Button_Close.text = main_UpdateLog.close.TextSet(updateLog.setButton);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < versions.titles.Length; i++)
		{
			UpdateLog.SmallTitle smallTitle = versions.titles[i];
			if (smallTitle.infos.Length != 0 && (this.logIndex >= this.updateLog.versions.Length - 1 || (!(smallTitle.name == "预告") && !smallTitle.name.Contains("已知") && !smallTitle.name.Contains("的话"))) && (!GameParameters.Inst.ifDemo || !smallTitle.name.Contains("的话")))
			{
				stringBuilder.Append(smallTitle.name.TextSet(updateLog.setSmallTitle)).AppendLine();
				for (int j = 0; j < smallTitle.infos.Length; j++)
				{
					stringBuilder.Append(smallTitle.infos[j].TextSet(updateLog.setNormal)).AppendLine();
				}
			}
		}
		this.lang_Info.text = stringBuilder.ToString();
		this.lang_PageNum.text = (this.logIndex + 1).ToString();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rect);
	}

	// Token: 0x040005C1 RID: 1473
	[SerializeField]
	private Text lang_Title;

	// Token: 0x040005C2 RID: 1474
	[SerializeField]
	private Text lang_Version;

	// Token: 0x040005C3 RID: 1475
	[SerializeField]
	private Text lang_Button_Close;

	// Token: 0x040005C4 RID: 1476
	[SerializeField]
	private Text lang_Info;

	// Token: 0x040005C5 RID: 1477
	[SerializeField]
	private Text lang_PageNum;

	// Token: 0x040005C6 RID: 1478
	[SerializeField]
	private int logIndex;

	// Token: 0x040005C7 RID: 1479
	[SerializeField]
	private UpdateLog updateLog;

	// Token: 0x040005C8 RID: 1480
	[SerializeField]
	private RectTransform rect;
}
