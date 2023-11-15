using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B2 RID: 178
public class UI_Panel_Setting : MonoBehaviour
{
	// Token: 0x0600062D RID: 1581 RVA: 0x000237BD File Offset: 0x000219BD
	private void Update()
	{
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			return;
		}
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
			UI_ToolTip.inst.Close();
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x000237E4 File Offset: 0x000219E4
	public void ResolutionLeft()
	{
		Setting.Inst.resolutionIndex--;
		Setting.Inst.resolutionIndex = Mathf.Clamp(Setting.Inst.resolutionIndex, 0, UI_Setting.Inst.resolutions.Length - 1);
		this.UpdateIcons();
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00023834 File Offset: 0x00021A34
	public void ResolutionRight()
	{
		Setting.Inst.resolutionIndex++;
		Setting.Inst.resolutionIndex = Mathf.Clamp(Setting.Inst.resolutionIndex, 0, UI_Setting.Inst.resolutions.Length - 1);
		this.UpdateIcons();
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00023884 File Offset: 0x00021A84
	public void UpdateIcons()
	{
		global::Resolution currentResolution = Setting.Inst.GetCurrentResolution();
		this.textResolution.text = currentResolution.Width + " x " + currentResolution.Height;
		for (int i = 0; i < this.iconSettings.Length; i++)
		{
			if (!(this.iconSettings[i] == null))
			{
				this.iconSettings[i].Init(i);
			}
		}
		LanguageText.PanelSetting panelSetting = LanguageText.Inst.panelSetting;
		this.lang_Resolution.text = panelSetting.resolution;
		for (int j = 0; j < 3; j++)
		{
			this.lang_TextTitles[j].text = panelSetting.titles[j];
		}
		this.lang_Confirm.text = panelSetting.confirm;
		RectTransform[] array = this.rects;
		for (int k = 0; k < array.Length; k++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(array[k]);
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0002396B File Offset: 0x00021B6B
	public void Open()
	{
		base.gameObject.SetActive(true);
		MainCanvas.ChildPanelOpen();
		this.UpdateIcons();
		if (TempData.inst.currentSceneType == EnumSceneType.MAINMENU)
		{
			this.imagePanel.enabled = false;
			return;
		}
		this.imagePanel.enabled = true;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x000239A9 File Offset: 0x00021BA9
	public void Close()
	{
		base.gameObject.SetActive(false);
		MainCanvas.ChildPanelClose();
		Debug.Log("关闭设置界面 应用且保存Settings文件");
		Setting.Inst.ApplySettingSuddenly();
		PostProcess.inst.ApplySetting();
		SaveFile_Settings.SaveByJson_Settings();
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x000239DF File Offset: 0x00021BDF
	public void ChangeLanguage(int id)
	{
		if (Setting.Inst.language == (EnumLanguage)id)
		{
			return;
		}
		Setting.Inst.language = (EnumLanguage)id;
		Setting.Inst.ApplyLanguage();
		SaveFile_Settings.SaveByJson_Settings();
	}

	// Token: 0x04000520 RID: 1312
	[SerializeField]
	private Image imagePanel;

	// Token: 0x04000521 RID: 1313
	[SerializeField]
	private UI_Icon_Setting[] iconSettings = new UI_Icon_Setting[0];

	// Token: 0x04000522 RID: 1314
	[SerializeField]
	private Text[] lang_TextTitles;

	// Token: 0x04000523 RID: 1315
	[SerializeField]
	private Text textResolution;

	// Token: 0x04000524 RID: 1316
	[SerializeField]
	private Text lang_Resolution;

	// Token: 0x04000525 RID: 1317
	[SerializeField]
	private Text lang_Confirm;

	// Token: 0x04000526 RID: 1318
	[SerializeField]
	private RectTransform[] rects = new RectTransform[0];
}
