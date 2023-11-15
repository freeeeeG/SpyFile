using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BAB RID: 2987
public class ClientLevelPortalMapNode : ClientPortalMapNode
{
	// Token: 0x06003D19 RID: 15641 RVA: 0x00124DB3 File Offset: 0x001231B3
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_baseLevelPortalMapNode = (LevelPortalMapNode)synchronisedObject;
	}

	// Token: 0x06003D1A RID: 15642 RVA: 0x00124DC8 File Offset: 0x001231C8
	public override EntityType GetEntityType()
	{
		return EntityType.LevelPortalMapNode;
	}

	// Token: 0x06003D1B RID: 15643 RVA: 0x00124DCC File Offset: 0x001231CC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.m_UIState = ClientPortalMapNode.UIState.RequestedUpdate;
	}

	// Token: 0x06003D1C RID: 15644 RVA: 0x00124DD8 File Offset: 0x001231D8
	protected override void SetupUI(WorldMapLevelIconUI _ui)
	{
		WorldMapKitchenLevelIconUI worldMapKitchenLevelIconUI = _ui as WorldMapKitchenLevelIconUI;
		if (this.m_baseLevelPortalMapNode.m_sceneDirectoryEntry != null)
		{
			worldMapKitchenLevelIconUI.Setup(this.m_baseLevelPortalMapNode.m_sceneDirectoryEntry, this.m_baseLevelPortalMapNode.m_sceneProgress, this.m_baseLevelPortalMapNode.GetState());
		}
	}

	// Token: 0x04003126 RID: 12582
	protected LevelPortalMapNode m_baseLevelPortalMapNode;
}
