using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200008E RID: 142
public class UI_Icon_GuideSingleTitle : UI_Icon
{
	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001D66A File Offset: 0x0001B86A
	private bool IfSelected
	{
		get
		{
			return this.panelManual.indexTotal == this.orderID;
		}
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0001D680 File Offset: 0x0001B880
	public void Init(int dataID, int orderID, Panel_Manual panel)
	{
		this.dataID = dataID;
		this.orderID = orderID;
		this.panelManual = panel;
		Guide guide = DataBase.Inst.dataGuides[dataID];
		this.textName.text = guide.Language_Name;
		this.UpdateState();
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001D6C6 File Offset: 0x0001B8C6
	private void UpdateState()
	{
		if (this.IfSelected || this.ifAbove)
		{
			base.TextSetHighlight();
			return;
		}
		base.TextSetNormal();
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0001D6E5 File Offset: 0x0001B8E5
	public override void OnPointerClick(PointerEventData eventData)
	{
		this.panelManual.GoToOrderIndex(this.orderID);
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0001D6F8 File Offset: 0x0001B8F8
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.ifAbove = true;
		this.UpdateState();
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0001D707 File Offset: 0x0001B907
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.ifAbove = false;
		this.UpdateState();
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000424 RID: 1060
	[SerializeField]
	private Panel_Manual panelManual;

	// Token: 0x04000425 RID: 1061
	[SerializeField]
	private Text textName;

	// Token: 0x04000426 RID: 1062
	[SerializeField]
	private int dataID = -1;

	// Token: 0x04000427 RID: 1063
	[SerializeField]
	private int orderID = -1;

	// Token: 0x04000428 RID: 1064
	[SerializeField]
	private bool ifAbove;
}
