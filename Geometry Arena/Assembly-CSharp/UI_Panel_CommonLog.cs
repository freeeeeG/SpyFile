using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BE RID: 190
public class UI_Panel_CommonLog : MonoBehaviour
{
	// Token: 0x0600068D RID: 1677 RVA: 0x0002546C File Offset: 0x0002366C
	private void UpdateLogType()
	{
		switch (this.logType)
		{
		case UI_Panel_CommonLog.EnumCommonLog.UNINITED:
			Debug.LogError("Erorr_LogType");
			return;
		case UI_Panel_CommonLog.EnumCommonLog.MANUAL:
			this.commonLog = LanguageText.Inst.asset_ManualLog;
			return;
		case UI_Panel_CommonLog.EnumCommonLog.CREDITS:
			this.commonLog = LanguageText.Inst.asset_Credits;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x000254C1 File Offset: 0x000236C1
	public void Open()
	{
		base.gameObject.SetActive(true);
		MainCanvas.ChildPanelOpen();
		this.UpdateLogType();
		this.UpdatePanel();
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00023B9D File Offset: 0x00021D9D
	public void Close()
	{
		base.gameObject.SetActive(false);
		MainCanvas.ChildPanelClose();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x000254E0 File Offset: 0x000236E0
	private void UpdatePanel()
	{
		foreach (GameObject obj in this.listGameobjects)
		{
			Object.Destroy(obj);
		}
		this.listGameobjects = new List<GameObject>();
		this.textTitle.text = this.commonLog.title;
		this.textClose.text = LanguageText.Inst.mainMenu.preview_Return;
		UI_Setting.CommonLog commonLog = UI_Setting.Inst.commonLog;
		for (int i = 0; i < this.commonLog.smallTitles.Length; i++)
		{
			CommonLog.SmallTitle smallTitle = this.commonLog.smallTitles[i];
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefabSingleText, this.textsLayout.transform);
			gameObject.GetComponent<Text>().text = smallTitle.name.Sized((float)commonLog.smallTitle.size);
			Button component = gameObject.GetComponent<Button>();
			ColorBlock colors = component.colors;
			colors.normalColor = commonLog.smallTitle.color;
			component.colors = colors;
			this.listGameobjects.Add(gameObject);
			for (int j = 0; j < smallTitle.infos.Length; j++)
			{
				string str = smallTitle.infos[j];
				gameObject = Object.Instantiate<GameObject>(this.prefabSingleText, this.textsLayout.transform);
				gameObject.GetComponent<Text>().text = str.TextSet(commonLog.normalText).ReplaceTabChar();
				this.listGameobjects.Add(gameObject);
			}
			for (int k = 0; k < smallTitle.specialTexts.Length; k++)
			{
				string str2 = smallTitle.specialTexts[k];
				gameObject = Object.Instantiate<GameObject>(this.prefabSingleText, this.textsLayout.transform);
				gameObject.GetComponent<Text>().text = str2.Sized((float)commonLog.specialText.size);
				Button component2 = gameObject.GetComponent<Button>();
				colors = component2.colors;
				colors.normalColor = commonLog.specialText.color;
				component2.colors = colors;
				this.listGameobjects.Add(gameObject);
			}
		}
		RectTransform[] array = this.rects;
		for (int l = 0; l < array.Length; l++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(array[l]);
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00025730 File Offset: 0x00023930
	private void Update()
	{
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
		}
	}

	// Token: 0x0400056C RID: 1388
	[SerializeField]
	private RectTransform[] rects;

	// Token: 0x0400056D RID: 1389
	[SerializeField]
	private CommonLog commonLog;

	// Token: 0x0400056E RID: 1390
	[SerializeField]
	private UI_Panel_CommonLog.EnumCommonLog logType = UI_Panel_CommonLog.EnumCommonLog.UNINITED;

	// Token: 0x0400056F RID: 1391
	[SerializeField]
	private GameObject prefabSingleText;

	// Token: 0x04000570 RID: 1392
	[SerializeField]
	private Text textTitle;

	// Token: 0x04000571 RID: 1393
	[SerializeField]
	private Text textClose;

	// Token: 0x04000572 RID: 1394
	[SerializeField]
	private GameObject textsLayout;

	// Token: 0x04000573 RID: 1395
	[SerializeField]
	private List<GameObject> listGameobjects = new List<GameObject>();

	// Token: 0x02000158 RID: 344
	private enum EnumCommonLog
	{
		// Token: 0x040009EA RID: 2538
		UNINITED = -1,
		// Token: 0x040009EB RID: 2539
		MANUAL,
		// Token: 0x040009EC RID: 2540
		CREDITS
	}
}
