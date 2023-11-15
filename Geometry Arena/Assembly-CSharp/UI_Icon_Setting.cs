using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000096 RID: 150
public class UI_Icon_Setting : UI_Icon
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001ECCE File Offset: 0x0001CECE
	[SerializeField]
	private int SetIntID
	{
		get
		{
			return this.setBoolID - 30;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001ECD9 File Offset: 0x0001CED9
	[SerializeField]
	private int SetIntSetIndex
	{
		get
		{
			return Setting.Inst.setInts[this.SetIntID];
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001ECEC File Offset: 0x0001CEEC
	// (set) Token: 0x06000540 RID: 1344 RVA: 0x0001ECFF File Offset: 0x0001CEFF
	private bool OpenFlag
	{
		get
		{
			return Setting.Inst.setBools[this.setBoolID];
		}
		set
		{
			Setting.Inst.setBools[this.setBoolID] = value;
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0001ED13 File Offset: 0x0001CF13
	public void Init(int id)
	{
		this.setBoolID = id;
		this.UpdateShow();
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0001ED24 File Offset: 0x0001CF24
	public void UpdateShow()
	{
		if (this.setBoolID < 30)
		{
			Color color = this.OpenFlag ? Color.white : Color.white.SetAlpha(0f);
			this.switchButton.color = color;
			this.text.text = LanguageText.Inst.panelSetting.boolInfos[this.setBoolID];
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.rect);
			return;
		}
		this.text.text = LanguageText.Inst.panelSetting.boolInfos[this.setBoolID];
		this.intMode_TextInfo.text = GameParameters.Inst.settings_IntSets[this.SetIntID].GetString_WithIndex(this.SetIntSetIndex);
		RectTransform[] array = this.intMode_rects;
		for (int i = 0; i < array.Length; i++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(array[i]);
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (this.setBoolID >= 30)
		{
			return;
		}
		this.OpenFlag = !this.OpenFlag;
		this.UpdateShow();
		if (this.setBoolID == 8)
		{
			PostProcess.inst.ApplySetting();
		}
		if (this.setBoolID == 9)
		{
			Setting.Inst.ApplyVSync();
		}
		if (this.setBoolID == 10)
		{
			BackgroundParticle.inst.ApplySetting();
		}
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0001EE60 File Offset: 0x0001D060
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.TextSetHighlight();
		UI_ToolTip.inst.ShowWithSettingInfo(this.setBoolID);
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0001EE78 File Offset: 0x0001D078
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.TextSetNormal();
		UI_ToolTip.inst.Close();
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0001EE8C File Offset: 0x0001D08C
	public void IntMode_IndexMove(int move)
	{
		int num = this.SetIntSetIndex + move;
		int indexCount = GameParameters.Inst.settings_IntSets[this.SetIntID].GetIndexCount();
		if (num < 0)
		{
			num += indexCount;
		}
		if (num > indexCount - 1)
		{
			num -= indexCount;
		}
		Setting.Inst.setInts[this.SetIntID] = num;
		this.UpdateShow();
		if (this.SetIntID == 0)
		{
			PostProcess.inst.ApplySetting();
		}
	}

	// Token: 0x04000452 RID: 1106
	[SerializeField]
	private int setBoolID;

	// Token: 0x04000453 RID: 1107
	[SerializeField]
	private Text text;

	// Token: 0x04000454 RID: 1108
	[SerializeField]
	private Image switchButton;

	// Token: 0x04000455 RID: 1109
	[SerializeField]
	private RectTransform rect;

	// Token: 0x04000456 RID: 1110
	[Header("IntMode")]
	[SerializeField]
	private Text intMode_TextInfo;

	// Token: 0x04000457 RID: 1111
	[SerializeField]
	private RectTransform[] intMode_rects;
}
