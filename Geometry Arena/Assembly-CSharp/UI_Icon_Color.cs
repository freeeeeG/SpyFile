using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000088 RID: 136
public class UI_Icon_Color : UI_Icon
{
	// Token: 0x060004CE RID: 1230 RVA: 0x0001CEC0 File Offset: 0x0001B0C0
	private VarColor GetVarColor()
	{
		return DataBase.Inst.Data_VarColors[this.varColorID];
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0001CED4 File Offset: 0x0001B0D4
	public void Init(int ID)
	{
		this.varColorID = ID;
		Color color = this.GetVarColor().ColorRGB.ApplyColorSet(ResourceLibrary.Inst.colorSet_UI);
		this.image.color = color;
		if (ID == TempData.inst.varColorId)
		{
			this.Outline_Show_Selected();
		}
		else
		{
			this.Outline_Close();
		}
		base.UpdateLockIcon();
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001CF30 File Offset: 0x0001B130
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!this.ifUnlocked())
		{
			return;
		}
		UI_ToolTip.inst.TryClose();
		MainCanvas.inst.Button_SelectVarColor(this.varColorID);
		this.Outline_Show_Selected();
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0001CF5B File Offset: 0x0001B15B
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithVarColor(this);
		if (this.varColorID != TempData.inst.varColorId)
		{
			this.Outline_Show_Above();
		}
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0001CF80 File Offset: 0x0001B180
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		if (this.varColorID != TempData.inst.varColorId)
		{
			this.Outline_Close();
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0001CFA4 File Offset: 0x0001B1A4
	protected override void Outline_Close()
	{
		this.image.sprite = this.spriteSquare;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0001CFB7 File Offset: 0x0001B1B7
	protected override void Outline_Show_Above()
	{
		this.image.sprite = this.spriteSquareR;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0001CFCA File Offset: 0x0001B1CA
	protected override void Outline_Show_Selected()
	{
		this.image.sprite = this.spriteCircle;
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0001CFDD File Offset: 0x0001B1DD
	protected override bool ifUnlocked()
	{
		return GameData.inst.IfColorUnlockedToCurJob(this.varColorID);
	}

	// Token: 0x0400040C RID: 1036
	public int varColorID;

	// Token: 0x0400040D RID: 1037
	public Image image;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private Sprite spriteSquare;

	// Token: 0x0400040F RID: 1039
	[SerializeField]
	private Sprite spriteCircle;

	// Token: 0x04000410 RID: 1040
	[SerializeField]
	private Sprite spriteSquareR;
}
