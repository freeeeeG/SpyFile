using System;
using GameModes;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BB8 RID: 3000
public abstract class ServerPortalMapNode : ServerSynchroniserBase, IServerMapSelectable
{
	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06003D59 RID: 15705 RVA: 0x00124481 File Offset: 0x00122881
	public SceneDirectoryData.SceneDirectoryEntry SceneDirectoryEntry
	{
		get
		{
			return this.m_sceneDirectoryEntry;
		}
	}

	// Token: 0x06003D5A RID: 15706 RVA: 0x00124489 File Offset: 0x00122889
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_basePortalMapNode = (PortalMapNode)synchronisedObject;
		this.m_worldMapFlowController = GameUtils.RequireManager<WorldMapFlowController>();
		this.m_sceneDirectoryEntry = this.m_worldMapFlowController.GetSceneDirectory().Scenes.TryAtIndex(this.m_basePortalMapNode.LevelIndex);
	}

	// Token: 0x06003D5B RID: 15707 RVA: 0x001244C8 File Offset: 0x001228C8
	public override EntityType GetEntityType()
	{
		return EntityType.PortalMapNode;
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06003D5C RID: 15708 RVA: 0x001244CC File Offset: 0x001228CC
	protected bool InSelectable
	{
		get
		{
			return this.m_inSelectable;
		}
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x001244D4 File Offset: 0x001228D4
	public void AvatarEnteringSelectable(MapAvatarControls _avatar)
	{
		this.m_inSelectable = true;
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x001244DD File Offset: 0x001228DD
	public void AvatarLeavingSelectable(MapAvatarControls _avatar)
	{
		this.m_inSelectable = false;
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x001244E8 File Offset: 0x001228E8
	public void OnSelected(MapAvatarControls _avatar)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (!MaskUtils.HasFlag<Kind>(this.m_sceneDirectoryEntry.m_supportedGameModes, gameSession.GameModeKind))
		{
			return;
		}
		if (!this.m_basePortalMapNode.Unfolded)
		{
			return;
		}
		this.OnAllowedSelection(_avatar);
	}

	// Token: 0x06003D60 RID: 15712
	protected abstract void OnAllowedSelection(MapAvatarControls _avatar);

	// Token: 0x0400314B RID: 12619
	protected PortalMapNode m_basePortalMapNode;

	// Token: 0x0400314C RID: 12620
	protected WorldMapFlowController m_worldMapFlowController;

	// Token: 0x0400314D RID: 12621
	protected SceneDirectoryData.SceneDirectoryEntry m_sceneDirectoryEntry;

	// Token: 0x0400314E RID: 12622
	private bool m_inSelectable;
}
