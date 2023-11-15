using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000098 RID: 152
public class UI_Icon_Skill : UI_Icon
{
	// Token: 0x06000551 RID: 1361 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
	public void Init(int jobID, int skillLevel)
	{
		this.jobID = jobID;
		this.skillLevel = skillLevel;
		Sprite spriteWithId = ResourceLibrary.Inst.Sprite_Icon_Jobs.GetSpriteWithId(jobID);
		this.image.sprite = spriteWithId;
		this.image.color = Color.red.SetHue((float)DataBase.Inst.DataPlayerModels[jobID].GetColorID() / 12f).SetSaturation(0.6f).SetValue(1f);
		if (jobID > 8)
		{
			skillLevel = 3;
		}
		this.imageOutline.color = UI_Setting.Inst.rankColors[skillLevel];
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0001F03B File Offset: 0x0001D23B
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithIconSkill(this.jobID, this.skillLevel);
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0001E3C5 File Offset: 0x0001C5C5
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x0400045B RID: 1115
	[SerializeField]
	private int jobID = -1;

	// Token: 0x0400045C RID: 1116
	[SerializeField]
	private int skillLevel = -1;

	// Token: 0x0400045D RID: 1117
	[SerializeField]
	private Image image;
}
