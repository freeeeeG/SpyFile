using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BB1 RID: 2993
public class ServerMiniLevelPortalMapNode : ServerPortalMapNode
{
	// Token: 0x06003D3A RID: 15674 RVA: 0x00124E2B File Offset: 0x0012322B
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_baseLevelPortalMapNode = (MiniLevelPortalMapNode)synchronisedObject;
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x00124E40 File Offset: 0x00123240
	public override EntityType GetEntityType()
	{
		return EntityType.MiniLevelPortalMapNode;
	}

	// Token: 0x06003D3C RID: 15676 RVA: 0x00124E44 File Offset: 0x00123244
	protected override void OnAllowedSelection(MapAvatarControls _avatar)
	{
		SceneDirectoryData sceneDirectory = this.m_worldMapFlowController.GetSceneDirectory();
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes.TryAtIndex(this.m_baseLevelPortalMapNode.LevelIndex);
		SceneDirectoryData.PerPlayerCountDirectoryEntry perPlayerCountDirectoryEntry = sceneDirectoryEntry.SceneVarients[0];
		ServerWorldMapFlowController component = this.m_worldMapFlowController.GetComponent<ServerWorldMapFlowController>();
		component.OnSelectMiniLevelPortal(_avatar, this.m_baseLevelPortalMapNode, 0);
	}

	// Token: 0x0400313C RID: 12604
	protected MiniLevelPortalMapNode m_baseLevelPortalMapNode;
}
