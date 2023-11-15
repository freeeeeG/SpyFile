using System;
using GameModes;
using UnityEngine;

// Token: 0x02000BBC RID: 3004
public abstract class PortalMapNode : MapNode
{
	// Token: 0x06003D78 RID: 15736 RVA: 0x00124310 File Offset: 0x00122710
	protected virtual void Awake()
	{
		this.m_worldMapFlowController = GameUtils.RequireManager<WorldMapFlowController>();
		this.m_sceneDirectoryEntry = this.m_worldMapFlowController.GetSceneDirectory().Scenes.TryAtIndex(this.LevelIndex);
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_gameProgress = gameSession.Progress;
		this.m_gameType = gameSession.TypeSettings.Type;
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06003D79 RID: 15737 RVA: 0x0012436C File Offset: 0x0012276C
	public bool ForceUnlocked
	{
		get
		{
			return this.m_forceUnlocked;
		}
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x00124374 File Offset: 0x00122774
	public WorldMapKitchenLevelIconUI.State GetState()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (!MaskUtils.HasFlag<Kind>(this.m_sceneDirectoryEntry.m_supportedGameModes, gameSession.GameModeKind))
		{
			return WorldMapKitchenLevelIconUI.State.UnSupported;
		}
		GameProgress.GameProgressData.LevelProgress progress = this.m_gameProgress.GetProgress(this.LevelIndex);
		if (progress.Purchased || DebugManager.Instance.GetOption("Unlock all levels") || this.ForceUnlocked)
		{
			return WorldMapKitchenLevelIconUI.State.Purchased;
		}
		if (this.m_gameProgress.GetStarTotal() >= this.m_sceneDirectoryEntry.StarCost)
		{
			return WorldMapKitchenLevelIconUI.State.Affordable;
		}
		return WorldMapKitchenLevelIconUI.State.UnAffordable;
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06003D7B RID: 15739 RVA: 0x00124400 File Offset: 0x00122800
	public virtual int LevelIndex
	{
		get
		{
			return this.m_levelIndex;
		}
	}

	// Token: 0x06003D7C RID: 15740
	public abstract WorldMapLevelIconUI GetUIPrefab(Kind _kind);

	// Token: 0x0400315D RID: 12637
	[SerializeField]
	[LevelIndex]
	public int m_levelIndex;

	// Token: 0x0400315E RID: 12638
	[SerializeField]
	public bool m_forceUnlocked;

	// Token: 0x0400315F RID: 12639
	[SerializeField]
	public bool m_uiAlwaysActive;

	// Token: 0x04003160 RID: 12640
	[HideInInspector]
	public GameProgress m_gameProgress;

	// Token: 0x04003161 RID: 12641
	[HideInInspector]
	public SceneDirectoryData.SceneDirectoryEntry m_sceneDirectoryEntry;

	// Token: 0x04003162 RID: 12642
	[HideInInspector]
	public WorldMapFlowController m_worldMapFlowController;

	// Token: 0x04003163 RID: 12643
	protected GameSession.GameType m_gameType;
}
