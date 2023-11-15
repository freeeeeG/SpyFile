using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000083 RID: 131
public class UI_Icon_AutoFuse : UI_Icon
{
	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001C684 File Offset: 0x0001A884
	private bool openFlag
	{
		get
		{
			return this.panelResult.autoFuse_Flag;
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0001C691 File Offset: 0x0001A891
	public void UpdateIcon()
	{
		this.UpdateOutline();
		this.textAutoFuse.text = LanguageText.Inst.runeInfo.autoFuse_IconName;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0001C6B3 File Offset: 0x0001A8B3
	public override void OnPointerClick(PointerEventData eventData)
	{
		this.panelResult.Button_SwitchAutoFuse();
		this.UpdateOutline();
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0001C6C6 File Offset: 0x0001A8C6
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.ifAbove = true;
		this.UpdateOutline();
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0001C6D5 File Offset: 0x0001A8D5
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.ifAbove = false;
		this.UpdateOutline();
		UI_ToolTip.inst.Close();
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0001C6F0 File Offset: 0x0001A8F0
	private void UpdateOutline()
	{
		if (this.openFlag)
		{
			this.imageOutline.gameObject.SetActive(true);
			this.imageOutline.color = this.colorSelected;
			return;
		}
		if (this.ifAbove)
		{
			this.imageOutline.gameObject.SetActive(true);
			this.imageOutline.color = this.colorAbove;
			return;
		}
		this.imageOutline.gameObject.SetActive(false);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x040003ED RID: 1005
	[SerializeField]
	private UI_Panel_Main_RuneSynResult panelResult;

	// Token: 0x040003EE RID: 1006
	[SerializeField]
	private Text textAutoFuse;

	// Token: 0x040003EF RID: 1007
	[SerializeField]
	private Color colorAbove = Color.white;

	// Token: 0x040003F0 RID: 1008
	[SerializeField]
	private Color colorSelected = Color.white;

	// Token: 0x040003F1 RID: 1009
	[SerializeField]
	private bool ifAbove;
}
