using System;
using KSerialization;
using STRINGS;

// Token: 0x02000B8D RID: 2957
public class WorldDetectedMessage : Message
{
	// Token: 0x06005BC4 RID: 23492 RVA: 0x00219BDF File Offset: 0x00217DDF
	public WorldDetectedMessage()
	{
	}

	// Token: 0x06005BC5 RID: 23493 RVA: 0x00219BE7 File Offset: 0x00217DE7
	public WorldDetectedMessage(WorldContainer world)
	{
		this.worldID = world.id;
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x00219BFB File Offset: 0x00217DFB
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x00219C04 File Offset: 0x00217E04
	public override string GetMessageBody()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.MESSAGEBODY, world.GetProperName());
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x00219C37 File Offset: 0x00217E37
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.WORLDDETECTED.NAME;
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x00219C44 File Offset: 0x00217E44
	public override string GetTooltip()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.TOOLTIP, world.GetProperName());
	}

	// Token: 0x06005BCA RID: 23498 RVA: 0x00219C77 File Offset: 0x00217E77
	public override bool IsValid()
	{
		return this.worldID != 255;
	}

	// Token: 0x04003DD0 RID: 15824
	[Serialize]
	private int worldID;
}
