using System;
using STRINGS;

// Token: 0x02000B7B RID: 2939
public class DuplicantsLeftMessage : Message
{
	// Token: 0x06005B49 RID: 23369 RVA: 0x00218C3F File Offset: 0x00216E3F
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06005B4A RID: 23370 RVA: 0x00218C46 File Offset: 0x00216E46
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;
	}

	// Token: 0x06005B4B RID: 23371 RVA: 0x00218C52 File Offset: 0x00216E52
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;
	}

	// Token: 0x06005B4C RID: 23372 RVA: 0x00218C5E File Offset: 0x00216E5E
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
	}
}
