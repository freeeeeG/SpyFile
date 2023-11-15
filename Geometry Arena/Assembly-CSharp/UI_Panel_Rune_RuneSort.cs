using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D3 RID: 211
public class UI_Panel_Rune_RuneSort : MonoBehaviour
{
	// Token: 0x06000765 RID: 1893 RVA: 0x000297CF File Offset: 0x000279CF
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.UpdatePanel();
		UI_Panel_Rune_RuneDetail.inst.ClearAll();
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x000297F0 File Offset: 0x000279F0
	public void Close()
	{
		base.gameObject.SetActive(false);
		UI_Text_SimpleTooltip[] array = this.simpleTooltips;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TextSetNormal();
		}
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00029828 File Offset: 0x00027A28
	private void UpdatePanel()
	{
		LanguageText.RuneInfo runeInfo = LanguageText.Inst.runeInfo;
		for (int i = 0; i < this.textSortButton.Length; i++)
		{
			this.textSortButton[i].text = runeInfo.sort_ChooseButtons[i];
		}
		this.textTitle.text = runeInfo.sort_PanelTitle;
		for (int j = 0; j < this.rects.Length; j++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.rects[j]);
		}
		this.rectTitle.sizeDelta = new Vector2(this.rectPanel.sizeDelta.x, this.rectTitle.sizeDelta.y);
	}

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	private RectTransform[] rects;

	// Token: 0x0400062D RID: 1581
	[SerializeField]
	private Text[] textSortButton;

	// Token: 0x0400062E RID: 1582
	[SerializeField]
	private Text textTitle;

	// Token: 0x0400062F RID: 1583
	[SerializeField]
	private UI_Text_SimpleTooltip[] simpleTooltips;

	// Token: 0x04000630 RID: 1584
	[SerializeField]
	private RectTransform rectPanel;

	// Token: 0x04000631 RID: 1585
	[SerializeField]
	private RectTransform rectTitle;
}
