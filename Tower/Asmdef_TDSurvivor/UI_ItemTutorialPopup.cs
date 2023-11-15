using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000169 RID: 361
public class UI_ItemTutorialPopup : APopupWindow
{
	// Token: 0x0600097A RID: 2426 RVA: 0x00023BC4 File Offset: 0x00021DC4
	private void OnEnable()
	{
		this.button_Close.onClick.AddListener(new UnityAction(this.OnClickButton_Close));
		foreach (KeyValuePair<eTutorialType, GameObject> keyValuePair in this.dic_Tutorials)
		{
			if (this.dic_Tutorials[keyValuePair.Key] != null)
			{
				this.dic_Tutorials[keyValuePair.Key].SetActive(false);
			}
		}
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00023C58 File Offset: 0x00021E58
	private void OnDisable()
	{
		this.button_Close.onClick.RemoveListener(new UnityAction(this.OnClickButton_Close));
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00023C76 File Offset: 0x00021E76
	private void OnClickButton_Close()
	{
		base.CloseWindow();
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00023C7E File Offset: 0x00021E7E
	public void SetTutorialType(eTutorialType type)
	{
		if (!this.dic_Tutorials.ContainsKey(type))
		{
			Debug.LogError(string.Format("[UI_ItemTutorialPopup] SetTutorialType: {0} not found", type));
			return;
		}
		this.dic_Tutorials[type].SetActive(true);
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00023CB6 File Offset: 0x00021EB6
	protected override void ShowWindowProc()
	{
		SoundManager.PlaySound("UI", "InfoPopupWindow", -1f, -1f, -1f);
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00023CE8 File Offset: 0x00021EE8
	protected override void CloseWindowProc()
	{
		SoundManager.PlaySound("UI", "InfoPopupWindow_Hide", -1f, -1f, -1f);
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x04000778 RID: 1912
	[SerializeField]
	private Button button_Close;

	// Token: 0x04000779 RID: 1913
	[SerializeField]
	[FormerlySerializedAs("node_Tutorial_BuffGrid")]
	private UI_ItemTutorialPopup.Dict_TutorialTypeToNode dic_Tutorials;

	// Token: 0x02000296 RID: 662
	[Serializable]
	public class Dict_TutorialTypeToNode : SerializableDictionary<eTutorialType, GameObject>
	{
	}
}
