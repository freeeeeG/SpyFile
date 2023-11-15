using System;
using KSerialization;
using STRINGS;

// Token: 0x02000B87 RID: 2951
public class SkillMasteredMessage : Message
{
	// Token: 0x06005BA7 RID: 23463 RVA: 0x002198BC File Offset: 0x00217ABC
	public SkillMasteredMessage()
	{
	}

	// Token: 0x06005BA8 RID: 23464 RVA: 0x002198C4 File Offset: 0x00217AC4
	public SkillMasteredMessage(MinionResume resume)
	{
		this.minionName = resume.GetProperName();
	}

	// Token: 0x06005BA9 RID: 23465 RVA: 0x002198D8 File Offset: 0x00217AD8
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06005BAA RID: 23466 RVA: 0x002198E0 File Offset: 0x00217AE0
	public override string GetMessageBody()
	{
		Debug.Assert(this.minionName != null);
		string arg = string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, this.minionName);
		return string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, arg);
	}

	// Token: 0x06005BAB RID: 23467 RVA: 0x00219921 File Offset: 0x00217B21
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x06005BAC RID: 23468 RVA: 0x00219938 File Offset: 0x00217B38
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x06005BAD RID: 23469 RVA: 0x0021994F File Offset: 0x00217B4F
	public override bool IsValid()
	{
		return this.minionName != null;
	}

	// Token: 0x04003DC0 RID: 15808
	[Serialize]
	private string minionName;
}
