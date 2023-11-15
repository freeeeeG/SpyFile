using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B79 RID: 2937
public class DeathMessage : TargetMessage
{
	// Token: 0x06005B39 RID: 23353 RVA: 0x00218B35 File Offset: 0x00216D35
	public DeathMessage()
	{
	}

	// Token: 0x06005B3A RID: 23354 RVA: 0x00218B48 File Offset: 0x00216D48
	public DeathMessage(GameObject go, Death death) : base(go.GetComponent<KPrefabID>())
	{
		this.death.Set(death);
	}

	// Token: 0x06005B3B RID: 23355 RVA: 0x00218B6D File Offset: 0x00216D6D
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06005B3C RID: 23356 RVA: 0x00218B74 File Offset: 0x00216D74
	public override bool PlayNotificationSound()
	{
		return false;
	}

	// Token: 0x06005B3D RID: 23357 RVA: 0x00218B77 File Offset: 0x00216D77
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTDIED.NAME;
	}

	// Token: 0x06005B3E RID: 23358 RVA: 0x00218B83 File Offset: 0x00216D83
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x06005B3F RID: 23359 RVA: 0x00218B8B File Offset: 0x00216D8B
	public override string GetMessageBody()
	{
		return this.death.Get().description.Replace("{Target}", base.GetTarget().GetName());
	}

	// Token: 0x04003D9D RID: 15773
	[Serialize]
	private ResourceRef<Death> death = new ResourceRef<Death>();
}
