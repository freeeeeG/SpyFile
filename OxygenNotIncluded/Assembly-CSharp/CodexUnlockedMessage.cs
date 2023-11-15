using System;
using STRINGS;

// Token: 0x02000B78 RID: 2936
public class CodexUnlockedMessage : Message
{
	// Token: 0x06005B31 RID: 23345 RVA: 0x00218ADA File Offset: 0x00216CDA
	public CodexUnlockedMessage()
	{
	}

	// Token: 0x06005B32 RID: 23346 RVA: 0x00218AE2 File Offset: 0x00216CE2
	public CodexUnlockedMessage(string lock_id, string unlock_message)
	{
		this.lockId = lock_id;
		this.unlockMessage = unlock_message;
	}

	// Token: 0x06005B33 RID: 23347 RVA: 0x00218AF8 File Offset: 0x00216CF8
	public string GetLockId()
	{
		return this.lockId;
	}

	// Token: 0x06005B34 RID: 23348 RVA: 0x00218B00 File Offset: 0x00216D00
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06005B35 RID: 23349 RVA: 0x00218B07 File Offset: 0x00216D07
	public override string GetMessageBody()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);
	}

	// Token: 0x06005B36 RID: 23350 RVA: 0x00218B1E File Offset: 0x00216D1E
	public override string GetTitle()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;
	}

	// Token: 0x06005B37 RID: 23351 RVA: 0x00218B2A File Offset: 0x00216D2A
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x06005B38 RID: 23352 RVA: 0x00218B32 File Offset: 0x00216D32
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x04003D9B RID: 15771
	private string unlockMessage;

	// Token: 0x04003D9C RID: 15772
	private string lockId;
}
