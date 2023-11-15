using System;
using STRINGS;

// Token: 0x02000B76 RID: 2934
public class AchievementEarnedMessage : Message
{
	// Token: 0x06005B24 RID: 23332 RVA: 0x00218A28 File Offset: 0x00216C28
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x06005B25 RID: 23333 RVA: 0x00218A2B File Offset: 0x00216C2B
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06005B26 RID: 23334 RVA: 0x00218A32 File Offset: 0x00216C32
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x06005B27 RID: 23335 RVA: 0x00218A39 File Offset: 0x00216C39
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.NAME;
	}

	// Token: 0x06005B28 RID: 23336 RVA: 0x00218A45 File Offset: 0x00216C45
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.TOOLTIP;
	}

	// Token: 0x06005B29 RID: 23337 RVA: 0x00218A51 File Offset: 0x00216C51
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x06005B2A RID: 23338 RVA: 0x00218A54 File Offset: 0x00216C54
	public override void OnClick()
	{
		RetireColonyUtility.SaveColonySummaryData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
	}
}
