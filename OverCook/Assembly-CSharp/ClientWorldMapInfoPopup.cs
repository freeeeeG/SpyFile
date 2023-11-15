using System;
using System.Collections;
using GameModes.Horde;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BE5 RID: 3045
public class ClientWorldMapInfoPopup : ClientSynchroniserBase
{
	// Token: 0x06003E3F RID: 15935 RVA: 0x00129FD1 File Offset: 0x001283D1
	public override EntityType GetEntityType()
	{
		return EntityType.WorldPopup;
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x00129FD8 File Offset: 0x001283D8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_popupInfo = (synchronisedObject as WorldMapInfoPopup);
		this.InitialiseForType();
		bool enabled = !ConnectionStatus.IsInSession() || ConnectionStatus.IsHost();
		this.m_popupInfo.m_buttonImage.enabled = enabled;
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x0012A022 File Offset: 0x00128422
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.m_hasShown = true;
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x0012A02C File Offset: 0x0012842C
	protected void InitialiseForType()
	{
		WorldMapInfoPopup.Type type = this.m_popupInfo.m_type;
		if (type == WorldMapInfoPopup.Type.Switch)
		{
			GameProgress.GameProgressData saveData = GameUtils.GetGameSession().Progress.SaveData;
			this.m_anyRevealedSwitches = (saveData.Switches.Length > 0);
		}
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x0012A078 File Offset: 0x00128478
	public bool CanShow()
	{
		GameProgress.GameProgressData saveData = GameUtils.GetGameSession().Progress.SaveData;
		switch (this.m_popupInfo.m_type)
		{
		case WorldMapInfoPopup.Type.Switch:
			if (this.m_anyRevealedSwitches)
			{
				return false;
			}
			break;
		case WorldMapInfoPopup.Type.HiddenLevel:
		{
			SceneDirectoryData sceneDirectory = GameUtils.GetGameSession().Progress.GetSceneDirectory();
			for (int i = 0; i < saveData.Levels.Length; i++)
			{
				GameProgress.GameProgressData.LevelProgress levelProgress = saveData.Levels[i];
				if (levelProgress.Revealed && sceneDirectory.Scenes[levelProgress.LevelId].IsHidden)
				{
					return false;
				}
			}
			break;
		}
		case WorldMapInfoPopup.Type.NewGamePlus:
		{
			GameSession gameSession = GameUtils.GetGameSession();
			GameProgress progress = gameSession.Progress;
			if (progress.HasShownNGPlusDialog(progress.SaveData) || (!progress.SaveData.IsNGPEnabledForAnyLevel() && !progress.CanUnlockNewGamePlus(progress.SaveData)))
			{
				return false;
			}
			break;
		}
		case WorldMapInfoPopup.Type.PracticeMode:
		{
			GameSession gameSession2 = GameUtils.GetGameSession();
			if (gameSession2.HasShownMetaDialog(MetaGameProgress.MetaDialogType.PracticeMode))
			{
				return false;
			}
			break;
		}
		case WorldMapInfoPopup.Type.HordeMode:
		{
			GameSession gameSession3 = GameUtils.GetGameSession();
			if (gameSession3.HasShownMetaDialog(MetaGameProgress.MetaDialogType.HordeMode))
			{
				return false;
			}
			bool flag = false;
			SceneDirectoryData sceneDirectory2 = GameUtils.GetGameSession().Progress.GetSceneDirectory();
			for (int j = 0; j < saveData.Levels.Length; j++)
			{
				GameProgress.GameProgressData.LevelProgress levelProgress2 = saveData.Levels[j];
				if (levelProgress2.Revealed)
				{
					SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory2.Scenes.TryAtIndex(levelProgress2.LevelId);
					if (sceneDirectoryEntry != null && sceneDirectoryEntry.SceneVarients[0].LevelConfig.GetType() == typeof(HordeLevelConfig))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			break;
		}
		}
		return !this.m_hasShown;
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x0012A254 File Offset: 0x00128654
	public IEnumerator PopupRoutine()
	{
		base.gameObject.SetActive(true);
		while (!this.m_hasShown)
		{
			yield return null;
		}
		WorldMapInfoPopup.Type type = this.m_popupInfo.m_type;
		if (type != WorldMapInfoPopup.Type.NewGamePlus)
		{
			if (type != WorldMapInfoPopup.Type.PracticeMode)
			{
				if (type == WorldMapInfoPopup.Type.HordeMode)
				{
					GameSession gameSession = GameUtils.GetGameSession();
					if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
					{
						MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
						metaGameProgress.SetMetaDialogShown(MetaGameProgress.MetaDialogType.HordeMode);
					}
					gameSession.SetMetaDialogShown(MetaGameProgress.MetaDialogType.HordeMode);
				}
			}
			else
			{
				GameSession gameSession2 = GameUtils.GetGameSession();
				if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
				{
					MetaGameProgress metaGameProgress2 = GameUtils.GetMetaGameProgress();
					metaGameProgress2.SetMetaDialogShown(MetaGameProgress.MetaDialogType.PracticeMode);
				}
				gameSession2.SetMetaDialogShown(MetaGameProgress.MetaDialogType.PracticeMode);
			}
		}
		else
		{
			GameSession gameSession3 = GameUtils.GetGameSession();
			gameSession3.Progress.SetNGPlusDialogShown(gameSession3.Progress.SaveData);
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040031FB RID: 12795
	private WorldMapInfoPopup m_popupInfo;

	// Token: 0x040031FC RID: 12796
	private bool m_hasShown;

	// Token: 0x040031FD RID: 12797
	private bool m_anyRevealedSwitches;

	// Token: 0x02000BE6 RID: 3046
	public struct InfoPopupShowRequest
	{
		// Token: 0x06003E45 RID: 15941 RVA: 0x0012A26F File Offset: 0x0012866F
		public InfoPopupShowRequest(Transform _requester, ClientWorldMapInfoPopup _popup)
		{
			this.m_requester = _requester;
			this.m_popup = _popup;
		}

		// Token: 0x040031FE RID: 12798
		public Transform m_requester;

		// Token: 0x040031FF RID: 12799
		public ClientWorldMapInfoPopup m_popup;
	}
}
