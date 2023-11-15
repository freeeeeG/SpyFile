using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B7A RID: 2938
public class DiscoveredSpaceMessage : Message
{
	// Token: 0x06005B40 RID: 23360 RVA: 0x00218BB2 File Offset: 0x00216DB2
	public DiscoveredSpaceMessage()
	{
	}

	// Token: 0x06005B41 RID: 23361 RVA: 0x00218BBA File Offset: 0x00216DBA
	public DiscoveredSpaceMessage(Vector3 pos)
	{
		this.cameraFocusPos = pos;
		this.cameraFocusPos.z = -40f;
	}

	// Token: 0x06005B42 RID: 23362 RVA: 0x00218BD9 File Offset: 0x00216DD9
	public override string GetSound()
	{
		return "Discover_Space";
	}

	// Token: 0x06005B43 RID: 23363 RVA: 0x00218BE0 File Offset: 0x00216DE0
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.TOOLTIP;
	}

	// Token: 0x06005B44 RID: 23364 RVA: 0x00218BEC File Offset: 0x00216DEC
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.NAME;
	}

	// Token: 0x06005B45 RID: 23365 RVA: 0x00218BF8 File Offset: 0x00216DF8
	public override string GetTooltip()
	{
		return null;
	}

	// Token: 0x06005B46 RID: 23366 RVA: 0x00218BFB File Offset: 0x00216DFB
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x00218BFE File Offset: 0x00216DFE
	public override void OnClick()
	{
		this.OnDiscoveredSpaceClicked();
	}

	// Token: 0x06005B48 RID: 23368 RVA: 0x00218C06 File Offset: 0x00216E06
	private void OnDiscoveredSpaceClicked()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound(this.GetSound(), false));
		MusicManager.instance.PlaySong("Stinger_Surface", false);
		CameraController.Instance.SetTargetPos(this.cameraFocusPos, 8f, true);
	}

	// Token: 0x04003D9E RID: 15774
	[Serialize]
	private Vector3 cameraFocusPos;

	// Token: 0x04003D9F RID: 15775
	private const string MUSIC_STINGER = "Stinger_Surface";
}
