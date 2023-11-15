using System;
using KSerialization;

// Token: 0x02000B7C RID: 2940
public class EODReportMessage : Message
{
	// Token: 0x06005B4E RID: 23374 RVA: 0x00218C72 File Offset: 0x00216E72
	public EODReportMessage(string title, string tooltip)
	{
		this.day = GameUtil.GetCurrentCycle();
		this.title = title;
		this.tooltip = tooltip;
	}

	// Token: 0x06005B4F RID: 23375 RVA: 0x00218C93 File Offset: 0x00216E93
	public EODReportMessage()
	{
	}

	// Token: 0x06005B50 RID: 23376 RVA: 0x00218C9B File Offset: 0x00216E9B
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x06005B51 RID: 23377 RVA: 0x00218C9E File Offset: 0x00216E9E
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x06005B52 RID: 23378 RVA: 0x00218CA5 File Offset: 0x00216EA5
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x06005B53 RID: 23379 RVA: 0x00218CAD File Offset: 0x00216EAD
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x06005B54 RID: 23380 RVA: 0x00218CB5 File Offset: 0x00216EB5
	public void OpenReport()
	{
		ManagementMenu.Instance.OpenReports(this.day);
	}

	// Token: 0x06005B55 RID: 23381 RVA: 0x00218CC7 File Offset: 0x00216EC7
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x06005B56 RID: 23382 RVA: 0x00218CCA File Offset: 0x00216ECA
	public override void OnClick()
	{
		this.OpenReport();
	}

	// Token: 0x04003DA0 RID: 15776
	[Serialize]
	private int day;

	// Token: 0x04003DA1 RID: 15777
	[Serialize]
	private string title;

	// Token: 0x04003DA2 RID: 15778
	[Serialize]
	private string tooltip;
}
