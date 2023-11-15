using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200050A RID: 1290
public class KitchenBootstrapManager : BootstrapManager
{
	// Token: 0x0600181A RID: 6170 RVA: 0x0007AA00 File Offset: 0x00078E00
	public override void EnsureSetup()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (PlayerInputLookup.GetBaseInputConfig() == null)
		{
			PlayerInputLookup.SetBaseInputConfig(this.m_levelInputConfig.Config);
		}
		base.EnsureSetup();
		if (gameSession == null)
		{
			GameSession gameSession2 = GameUtils.GetGameSession();
			gameSession2.LevelSettings = new GameSession.GameLevelSettings();
			AvatarDirectoryData avatarDirectory = gameSession2.Progress.GetAvatarDirectory();
			if (!this.m_hasNoSceneDirectory)
			{
				SceneDirectoryData sceneDirectory = gameSession2.Progress.GetSceneDirectory();
				string name = SceneManager.GetActiveScene().name;
				for (int i = 0; i < sceneDirectory.Scenes.Length; i++)
				{
					SceneDirectoryData.PerPlayerCountDirectoryEntry[] sceneVarients = sceneDirectory.Scenes[i].SceneVarients;
					for (int j = 0; j < sceneVarients.Length; j++)
					{
						if (name == sceneVarients[j].SceneName)
						{
							gameSession2.LevelSettings.SceneDirectoryVarientEntry = sceneVarients[j];
							return;
						}
					}
				}
			}
			else
			{
				gameSession2.LevelSettings.SceneDirectoryVarientEntry = new SceneDirectoryData.PerPlayerCountDirectoryEntry();
				gameSession2.LevelSettings.SceneDirectoryVarientEntry.LevelConfig = this.m_bootstrapConfig;
				gameSession2.LevelSettings.SceneDirectoryVarientEntry.PlayerCount = 4;
				gameSession2.LevelSettings.SceneDirectoryVarientEntry.SceneName = SceneManager.GetActiveScene().name;
				gameSession2.LevelSettings.SceneDirectoryVarientEntry.Screenshot = null;
			}
		}
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x0007AB5C File Offset: 0x00078F5C
	public GameSession.SelectedChefData GetDefaultChef(int index)
	{
		GameSession.SelectedChefData result = null;
		switch (index)
		{
		case 0:
			result = this.m_playerOneChef;
			break;
		case 1:
			result = this.m_playerTwoChef;
			break;
		case 2:
			result = this.m_playerThreeChef;
			break;
		case 3:
			result = this.m_playerFourChef;
			break;
		}
		return result;
	}

	// Token: 0x04001363 RID: 4963
	[SerializeField]
	private GameInputConfigData m_levelInputConfig;

	// Token: 0x04001364 RID: 4964
	[SerializeField]
	private GameSession.SelectedChefData m_playerOneChef;

	// Token: 0x04001365 RID: 4965
	[SerializeField]
	private GameSession.SelectedChefData m_playerTwoChef;

	// Token: 0x04001366 RID: 4966
	[SerializeField]
	private GameSession.SelectedChefData m_playerThreeChef;

	// Token: 0x04001367 RID: 4967
	[SerializeField]
	private GameSession.SelectedChefData m_playerFourChef;

	// Token: 0x04001368 RID: 4968
	[SerializeField]
	private bool m_hasNoSceneDirectory;

	// Token: 0x04001369 RID: 4969
	[SerializeField]
	[HideInInspectorTest("m_hasNoSceneDirectory", true)]
	private LevelConfigBase m_bootstrapConfig;
}
