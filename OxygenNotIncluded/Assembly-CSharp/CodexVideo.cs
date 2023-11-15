using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AD6 RID: 2774
public class CodexVideo : CodexWidget<CodexVideo>
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06005556 RID: 21846 RVA: 0x001F1002 File Offset: 0x001EF202
	// (set) Token: 0x06005557 RID: 21847 RVA: 0x001F100A File Offset: 0x001EF20A
	public string name { get; set; }

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06005559 RID: 21849 RVA: 0x001F101C File Offset: 0x001EF21C
	// (set) Token: 0x06005558 RID: 21848 RVA: 0x001F1013 File Offset: 0x001EF213
	public string videoName
	{
		get
		{
			return "--> " + (this.name ?? "NULL");
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x0600555A RID: 21850 RVA: 0x001F1037 File Offset: 0x001EF237
	// (set) Token: 0x0600555B RID: 21851 RVA: 0x001F103F File Offset: 0x001EF23F
	public string overlayName { get; set; }

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x0600555C RID: 21852 RVA: 0x001F1048 File Offset: 0x001EF248
	// (set) Token: 0x0600555D RID: 21853 RVA: 0x001F1050 File Offset: 0x001EF250
	public List<string> overlayTexts { get; set; }

	// Token: 0x0600555E RID: 21854 RVA: 0x001F1059 File Offset: 0x001EF259
	public void ConfigureVideo(VideoWidget videoWidget, string clipName, string overlayName = null, List<string> overlayTexts = null)
	{
		videoWidget.SetClip(Assets.GetVideo(clipName), overlayName, overlayTexts);
	}

	// Token: 0x0600555F RID: 21855 RVA: 0x001F106A File Offset: 0x001EF26A
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureVideo(contentGameObject.GetComponent<VideoWidget>(), this.name, this.overlayName, this.overlayTexts);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
