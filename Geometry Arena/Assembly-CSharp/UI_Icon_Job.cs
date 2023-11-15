using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200008F RID: 143
public class UI_Icon_Job : UI_Icon
{
	// Token: 0x06000501 RID: 1281 RVA: 0x0001D72C File Offset: 0x0001B92C
	public void Init(int ID)
	{
		this.jobID = ID;
		Sprite spriteWithId = ResourceLibrary.Inst.Sprite_Icon_Jobs.GetSpriteWithId(ID);
		this.image.sprite = spriteWithId;
		this.image.color = ResourceLibrary.Inst.colorSet_UI.GetColorWithHue((float)DataBase.Inst.DataPlayerModels[this.jobID].GetColorID() / 12f);
		if (ID == TempData.inst.jobId)
		{
			this.Outline_Show_Selected();
		}
		else
		{
			this.OutlineNew_Close();
		}
		base.UpdateLockIcon();
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001D7B8 File Offset: 0x0001B9B8
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!this.ifUnlocked())
		{
			return;
		}
		if (TempData.inst.daily_Open)
		{
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_LockRole);
			return;
		}
		UI_ToolTip.inst.TryClose();
		MainCanvas.inst.Button_SelectJob(this.jobID);
		this.OutlineNew_Show_Selected();
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0001D814 File Offset: 0x0001BA14
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithJob(this);
		if (this.jobID != TempData.inst.jobId)
		{
			this.OutlineNew_Show_Above();
		}
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0001D839 File Offset: 0x0001BA39
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		if (this.jobID != TempData.inst.jobId)
		{
			this.OutlineNew_Close();
		}
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0001D85D File Offset: 0x0001BA5D
	protected override bool ifUnlocked()
	{
		return GameData.inst.IfJobUnlocked(this.jobID);
	}

	// Token: 0x04000429 RID: 1065
	public int jobID;

	// Token: 0x0400042A RID: 1066
	public Image image;
}
