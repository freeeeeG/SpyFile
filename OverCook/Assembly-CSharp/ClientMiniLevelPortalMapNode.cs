using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BB2 RID: 2994
public class ClientMiniLevelPortalMapNode : ClientPortalMapNode
{
	// Token: 0x06003D3E RID: 15678 RVA: 0x00124E9F File Offset: 0x0012329F
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_baseLevelPortalMapNode = (MiniLevelPortalMapNode)synchronisedObject;
	}

	// Token: 0x06003D3F RID: 15679 RVA: 0x00124EB4 File Offset: 0x001232B4
	public override EntityType GetEntityType()
	{
		return EntityType.MiniLevelPortalMapNode;
	}

	// Token: 0x06003D40 RID: 15680 RVA: 0x00124EB8 File Offset: 0x001232B8
	protected override void SetupUI(WorldMapLevelIconUI _ui)
	{
		SceneDirectoryData sceneDirectory = this.m_baseLevelPortalMapNode.m_worldMapFlowController.GetSceneDirectory();
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes.TryAtIndex(this.m_baseLevelPortalMapNode.LevelIndex);
		_ui.SetTitle(sceneDirectoryEntry.Label);
	}

	// Token: 0x0400313D RID: 12605
	protected MiniLevelPortalMapNode m_baseLevelPortalMapNode;
}
