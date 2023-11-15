using System;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000BB5 RID: 2997
public class ClientMultiLevelMiniPortalMapNode : ClientMiniLevelPortalMapNode
{
	// Token: 0x06003D46 RID: 15686 RVA: 0x00124F1D File Offset: 0x0012331D
	public override EntityType GetEntityType()
	{
		return EntityType.MultiLevelMiniPortalMapNode;
	}

	// Token: 0x06003D47 RID: 15687 RVA: 0x00124F24 File Offset: 0x00123324
	protected override void SetupUI(WorldMapLevelIconUI _ui)
	{
		base.SetupUI(_ui);
		MultiLevelMiniPortalMapNode multiLevelMiniPortalMapNode = this.m_baseLevelPortalMapNode as MultiLevelMiniPortalMapNode;
		if (multiLevelMiniPortalMapNode != null)
		{
			GameSession gameSession = GameUtils.GetGameSession();
			if (gameSession != null)
			{
				GameProgress progress = gameSession.Progress;
				if (progress != null)
				{
					GameProgress.GameProgressData saveData = progress.SaveData;
					if (saveData != null && !saveData.IsLevelComplete(multiLevelMiniPortalMapNode.LevelIndex))
					{
						_ui.ActivateAttentionPopup();
					}
				}
			}
		}
	}
}
