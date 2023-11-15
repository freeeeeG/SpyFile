using System;
using KSerialization;
using STRINGS;

// Token: 0x02000B85 RID: 2949
public class ResearchCompleteMessage : Message
{
	// Token: 0x06005B95 RID: 23445 RVA: 0x0021950C File Offset: 0x0021770C
	public ResearchCompleteMessage()
	{
	}

	// Token: 0x06005B96 RID: 23446 RVA: 0x0021951F File Offset: 0x0021771F
	public ResearchCompleteMessage(Tech tech)
	{
		this.tech.Set(tech);
	}

	// Token: 0x06005B97 RID: 23447 RVA: 0x0021953E File Offset: 0x0021773E
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06005B98 RID: 23448 RVA: 0x00219548 File Offset: 0x00217748
	public override string GetMessageBody()
	{
		Tech tech = this.tech.Get();
		string text = "";
		for (int i = 0; i < tech.unlockedItems.Count; i++)
		{
			if (i != 0)
			{
				text += ", ";
			}
			text += tech.unlockedItems[i].Name;
		}
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.MESSAGEBODY, tech.Name, text);
	}

	// Token: 0x06005B99 RID: 23449 RVA: 0x002195BA File Offset: 0x002177BA
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.RESEARCHCOMPLETE.NAME;
	}

	// Token: 0x06005B9A RID: 23450 RVA: 0x002195C8 File Offset: 0x002177C8
	public override string GetTooltip()
	{
		Tech tech = this.tech.Get();
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.TOOLTIP, tech.Name);
	}

	// Token: 0x06005B9B RID: 23451 RVA: 0x002195F6 File Offset: 0x002177F6
	public override bool IsValid()
	{
		return this.tech.Get() != null;
	}

	// Token: 0x04003DBD RID: 15805
	[Serialize]
	private ResourceRef<Tech> tech = new ResourceRef<Tech>();
}
