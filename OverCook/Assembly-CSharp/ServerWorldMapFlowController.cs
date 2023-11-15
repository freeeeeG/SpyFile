using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BDF RID: 3039
public class ServerWorldMapFlowController : ServerSynchroniserBase
{
	// Token: 0x06003E0D RID: 15885 RVA: 0x001285CD File Offset: 0x001269CD
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_baseObject = (WorldMapFlowController)synchronisedObject;
		this.m_State = GameState.RunMapUnfoldRoutine;
		this.RegisterPopups();
	}

	// Token: 0x06003E0E RID: 15886 RVA: 0x001285EC File Offset: 0x001269EC
	public override void UpdateSynchronising()
	{
		GameState state = this.m_State;
		if (state == GameState.RunMapUnfoldRoutine)
		{
			if (UserSystemUtils.AreAllUsersInGameState(ServerUserSystem.m_Users, GameState.RanMapUnfoldRoutine))
			{
				this.SpawnPopups();
				this.ChangeGameState(GameState.InMap);
			}
		}
	}

	// Token: 0x06003E0F RID: 15887 RVA: 0x00128631 File Offset: 0x00126A31
	public void ChangeGameState(GameState state)
	{
		UserSystemUtils.ChangeGameState(state, null);
		this.m_State = state;
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x00128644 File Offset: 0x00126A44
	private void RegisterPopups()
	{
		if (this.m_baseObject.m_newGamePlusDialogPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_baseObject.m_newGamePlusDialogPrefab.gameObject);
		}
		if (this.m_baseObject.m_practiceModeDialogPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_baseObject.m_practiceModeDialogPrefab.gameObject);
		}
		if (this.m_baseObject.m_hordeModeDialogPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_baseObject.m_hordeModeDialogPrefab.gameObject);
		}
	}

	// Token: 0x06003E11 RID: 15889 RVA: 0x001286E4 File Offset: 0x00126AE4
	private void SpawnPopups()
	{
		if (this.m_baseObject.m_newGamePlusDialogPrefab != null)
		{
			NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_baseObject.m_newGamePlusDialogPrefab.gameObject);
		}
		if (this.m_baseObject.m_practiceModeDialogPrefab != null)
		{
			NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_baseObject.m_practiceModeDialogPrefab.gameObject);
		}
		if (this.m_baseObject.m_hordeModeDialogPrefab != null)
		{
			NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_baseObject.m_hordeModeDialogPrefab.gameObject);
		}
	}

	// Token: 0x06003E12 RID: 15890 RVA: 0x00128788 File Offset: 0x00126B88
	public void OnSelectLevelPortal(MapAvatarControls _controls, LevelPortalMapNode _levelNode)
	{
		if (InviteMonitor.CheckStatus(InviteMonitor.StatusFlags.HandlerIsValid | InviteMonitor.StatusFlags.HandlerIsIdle))
		{
			int layer = LayerMask.NameToLayer("Administration");
			GameUtils.TriggerAudio(GameOneShotAudioTag.UISelect, layer);
			int levelIndex = _levelNode.LevelIndex;
			int playerCount = ClientUserSystem.m_Users.Count;
			if (GameUtils.GetGameSession().TypeSettings.Type == GameSession.GameType.Competitive)
			{
				playerCount = 4;
			}
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_baseObject.GetSceneDirectory().Scenes[levelIndex];
			this.LoadLevel(levelIndex, sceneDirectoryEntry, sceneDirectoryEntry.GetSceneVarient(playerCount), _controls);
		}
	}

	// Token: 0x06003E13 RID: 15891 RVA: 0x00128800 File Offset: 0x00126C00
	public void OnSelectMiniLevelPortal(MapAvatarControls _controls, MiniLevelPortalMapNode _levelNode, int _varient)
	{
		if (InviteMonitor.CheckStatus(InviteMonitor.StatusFlags.HandlerIsValid | InviteMonitor.StatusFlags.HandlerIsIdle))
		{
			int layer = LayerMask.NameToLayer("Administration");
			GameUtils.TriggerAudio(GameOneShotAudioTag.UISelect, layer);
			int levelIndex = _levelNode.LevelIndex;
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_baseObject.GetSceneDirectory().Scenes[levelIndex];
			this.LoadLevel(levelIndex, sceneDirectoryEntry, sceneDirectoryEntry.GetSceneVarient(_varient), _controls);
		}
	}

	// Token: 0x06003E14 RID: 15892 RVA: 0x00128858 File Offset: 0x00126C58
	private void LoadLevel(int _levelIndex, SceneDirectoryData.SceneDirectoryEntry _entry, SceneDirectoryData.PerPlayerCountDirectoryEntry _varient, MapAvatarControls _controls)
	{
		_controls.gameObject.SetActive(false);
		if (_varient != null)
		{
			string sceneName = _varient.SceneName;
			if (sceneName != string.Empty)
			{
				GameSession.GameLevelSettings levelSettings = GameUtils.GetGameSession().LevelSettings;
				levelSettings.SceneDirectoryVarientEntry = _varient;
				GameSession gameSession = GameUtils.GetGameSession();
				gameSession.LevelSettings = levelSettings;
				gameSession.Progress.SetLastLevelEntered(_levelIndex);
				GameUtils.GetMetaGameProgress().SetLastPlayedTheme(_entry.Theme);
				GameProgress.GameProgressData.LevelProgress levelProgress = gameSession.Progress.SaveData.GetLevelProgress(_levelIndex);
				if (levelProgress != null && levelProgress.ScoreStars >= 3)
				{
					Analytics.LogEvent("Replayed", 0L, Analytics.Flags.LevelName | Analytics.Flags.PlayerCount);
				}
				base.StartCoroutine(this.SaveBeforeLoading(_levelIndex, levelSettings.SceneDirectoryVarientEntry.PlayerCount, _controls));
				return;
			}
		}
		Vector2 vector = new Vector2(0.5f * (float)Camera.main.pixelWidth, 0.5f * (float)Camera.main.pixelHeight);
	}

	// Token: 0x06003E15 RID: 15893 RVA: 0x00128940 File Offset: 0x00126D40
	private IEnumerator SaveBeforeLoading(int _levelIndex, int _playerCount, MapAvatarControls _controls)
	{
		TimeManager timeManager = GameUtils.RequestManager<TimeManager>();
		TimeManager.PauseLayer layerToPause = (!ConnectionStatus.IsInSession()) ? TimeManager.PauseLayer.Main : TimeManager.PauseLayer.Network;
		if (timeManager != null)
		{
			timeManager.SetPaused(layerToPause, true, this);
		}
		SaveLoadResult? result = null;
		ScreenTransitionManager transitionManager = GameUtils.RequireManager<ScreenTransitionManager>();
		transitionManager.StartTransitionUp(delegate
		{
			GameUtils.GetGameSession().SaveSession(delegate(SaveSystemStatus _status)
			{
				if (_status.Status == SaveSystemStatus.SaveStatus.Complete)
				{
					if (_status.Result == SaveLoadResult.Exists)
					{
						ServerMessenger.LoadLevel((uint)_levelIndex, (uint)_playerCount, GameState.LoadKitchen, GameState.RunKitchen);
					}
					result = new SaveLoadResult?(_status.Result);
				}
			});
		});
		while (result == null)
		{
			if (T17DialogBoxManager.HasAnyOpenDialogs())
			{
				transitionManager.StartTransitionDown(null);
				break;
			}
			yield return null;
		}
		_controls.gameObject.SetActive(true);
		if (timeManager == null)
		{
			timeManager = GameUtils.RequestManager<TimeManager>();
		}
		if (timeManager != null)
		{
			if (!TimeManager.IsPaused(layerToPause))
			{
				timeManager.SetPaused(layerToPause, true, this);
			}
			timeManager.SetPaused(layerToPause, false, this);
		}
		yield break;
	}

	// Token: 0x040031DA RID: 12762
	private WorldMapFlowController m_baseObject;

	// Token: 0x040031DB RID: 12763
	private GameState m_State;
}
