using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BAA RID: 2986
public class ServerLevelPortalMapNode : ServerPortalMapNode
{
	// Token: 0x06003D15 RID: 15637 RVA: 0x00124542 File Offset: 0x00122942
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_baseLevelPortalMapNode = (LevelPortalMapNode)synchronisedObject;
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x00124557 File Offset: 0x00122957
	public override EntityType GetEntityType()
	{
		return EntityType.LevelPortalMapNode;
	}

	// Token: 0x06003D17 RID: 15639 RVA: 0x0012455C File Offset: 0x0012295C
	protected override void OnAllowedSelection(MapAvatarControls _avatar)
	{
		WorldMapKitchenLevelIconUI.State state = this.m_baseLevelPortalMapNode.GetState();
		if (state != WorldMapKitchenLevelIconUI.State.Affordable)
		{
			if (state == WorldMapKitchenLevelIconUI.State.Purchased)
			{
				ServerWorldMapFlowController component = this.m_worldMapFlowController.GetComponent<ServerWorldMapFlowController>();
				component.OnSelectLevelPortal(_avatar, this.m_baseLevelPortalMapNode);
			}
		}
		else
		{
			GameProgress.GameProgressData.LevelProgress levelProgress = this.m_baseLevelPortalMapNode.GetLevelProgress();
			levelProgress.Purchased = true;
			this.SendServerEvent(this.m_Message);
		}
	}

	// Token: 0x04003124 RID: 12580
	protected LevelPortalMapNode m_baseLevelPortalMapNode;

	// Token: 0x04003125 RID: 12581
	protected LevelPortalMapNodeMessage m_Message = new LevelPortalMapNodeMessage();
}
