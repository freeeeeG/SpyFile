using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BF1 RID: 3057
public class ServerWorldMapSwitch : ServerSynchroniserBase, IServerMapSelectable
{
	// Token: 0x06003E64 RID: 15972 RVA: 0x0012AC40 File Offset: 0x00129040
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_baseObject = (WorldMapSwitch)synchronisedObject;
		if (null != this.m_baseObject.m_switchOwnerData.m_switchMapNode)
		{
			this.m_ServerSwitchMapNode = this.m_baseObject.m_switchOwnerData.m_switchMapNode.gameObject.RequestComponent<ServerSwitchMapNode>();
		}
		this.m_flipper = base.gameObject.RequireComponent<WorldMapFlipperBase>();
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x0012ACA5 File Offset: 0x001290A5
	private bool IsIdle()
	{
		return this.m_flipper == null || (this.m_flipper.IsFlipped() && this.m_flipper.IsFinishedFlipping());
	}

	// Token: 0x06003E66 RID: 15974 RVA: 0x0012ACDC File Offset: 0x001290DC
	public void AvatarEnteringSelectable(MapAvatarControls _avatar)
	{
		if (this.IsIdle() && this.m_baseObject.CanBePressed() && this.m_ServerSwitchMapNode != null)
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.WorldMapRampButton, base.gameObject.layer);
			this.m_ServerSwitchMapNode.OnSwitchPressed(_avatar, this.m_baseObject);
		}
	}

	// Token: 0x06003E67 RID: 15975 RVA: 0x0012AD3A File Offset: 0x0012913A
	public void AvatarLeavingSelectable(MapAvatarControls _avatar)
	{
	}

	// Token: 0x06003E68 RID: 15976 RVA: 0x0012AD3C File Offset: 0x0012913C
	public void OnSelected(MapAvatarControls _avatar)
	{
	}

	// Token: 0x04003223 RID: 12835
	private WorldMapSwitch m_baseObject;

	// Token: 0x04003224 RID: 12836
	private WorldMapFlipperBase m_flipper;

	// Token: 0x04003225 RID: 12837
	private ServerSwitchMapNode m_ServerSwitchMapNode;

	// Token: 0x04003226 RID: 12838
	private bool m_bVisualsPressed;
}
