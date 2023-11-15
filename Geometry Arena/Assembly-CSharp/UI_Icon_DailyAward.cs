using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000089 RID: 137
public class UI_Icon_DailyAward : UI_Icon
{
	// Token: 0x060004D8 RID: 1240 RVA: 0x0001CFEF File Offset: 0x0001B1EF
	public override void OnPointerClick(PointerEventData eventData)
	{
		this.runePanel.DailyAward_GetAward();
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0001CFFC File Offset: 0x0001B1FC
	private void Update()
	{
		if (this.ifMouseAbove)
		{
			if (this.updateLeft > 0f)
			{
				this.updateLeft -= Time.unscaledDeltaTime;
				return;
			}
			this.updateLeft = 0.1f;
			UI_ToolTip.inst.ShowWithString(this.runePanel.DailyAward_GetString_ToolTipInfo());
		}
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0001D051 File Offset: 0x0001B251
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.ifMouseAbove = true;
		this.updateLeft = 0.1f;
		UI_ToolTip.inst.ShowWithString(this.runePanel.DailyAward_GetString_ToolTipInfo());
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0001D07A File Offset: 0x0001B27A
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.ifMouseAbove = false;
		UI_ToolTip.inst.Close();
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000411 RID: 1041
	[SerializeField]
	private UI_Panel_Main_RunePanel runePanel;

	// Token: 0x04000412 RID: 1042
	[SerializeField]
	private bool ifMouseAbove;

	// Token: 0x04000413 RID: 1043
	[SerializeField]
	private float updateLeft = 0.1f;
}
