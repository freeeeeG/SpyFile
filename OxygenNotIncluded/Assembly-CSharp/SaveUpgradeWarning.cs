using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using UnityEngine;

// Token: 0x02000951 RID: 2385
[AddComponentMenu("KMonoBehaviour/scripts/SaveUpgradeWarning")]
public class SaveUpgradeWarning : KMonoBehaviour
{
	// Token: 0x060045BD RID: 17853 RVA: 0x0018A1F0 File Offset: 0x001883F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game game = this.game;
		game.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(game.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
	}

	// Token: 0x060045BE RID: 17854 RVA: 0x0018A21F File Offset: 0x0018841F
	protected override void OnCleanUp()
	{
		Game game = this.game;
		game.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(game.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
		base.OnCleanUp();
	}

	// Token: 0x060045BF RID: 17855 RVA: 0x0018A250 File Offset: 0x00188450
	private void OnLoad(Game.GameSaveData data)
	{
		List<SaveUpgradeWarning.Upgrade> list = new List<SaveUpgradeWarning.Upgrade>
		{
			new SaveUpgradeWarning.Upgrade(7, 5, new System.Action(this.SuddenMoraleHelper)),
			new SaveUpgradeWarning.Upgrade(7, 13, new System.Action(this.BedAndBathHelper)),
			new SaveUpgradeWarning.Upgrade(7, 16, new System.Action(this.NewAutomationWarning)),
			new SaveUpgradeWarning.Upgrade(7, 32, new System.Action(this.SpaceScannersAndTelescopeUpdateWarning))
		};
		if (DlcManager.IsPureVanilla())
		{
			list.Add(new SaveUpgradeWarning.Upgrade(7, 25, new System.Action(this.MergedownWarning)));
		}
		foreach (SaveUpgradeWarning.Upgrade upgrade in list)
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(upgrade.major, upgrade.minor))
			{
				upgrade.action();
			}
		}
	}

	// Token: 0x060045C0 RID: 17856 RVA: 0x0018A350 File Offset: 0x00188550
	private void SuddenMoraleHelper()
	{
		Effect morale_effect = Db.Get().effects.Get("SuddenMoraleHelper");
		CustomizableDialogScreen screen = Util.KInstantiateUI<CustomizableDialogScreen>(ScreenPrefabs.Instance.CustomizableDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_BUFF, delegate
		{
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				minionIdentity.GetComponent<Effects>().Add(morale_effect, true);
			}
			screen.Deactivate();
		});
		screen.AddOption(UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_DISABLE, delegate
		{
			SettingConfig morale = CustomGameSettingConfigs.Morale;
			CustomGameSettings.Instance.customGameMode = CustomGameSettings.CustomGameMode.Custom;
			CustomGameSettings.Instance.SetQualitySetting(morale, morale.GetLevel("Disabled").id);
			screen.Deactivate();
		});
		screen.PopupConfirmDialog(string.Format(UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER, Mathf.RoundToInt(morale_effect.duration / 600f)), UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_TITLE, null);
	}

	// Token: 0x060045C1 RID: 17857 RVA: 0x0018A42C File Offset: 0x0018862C
	private void BedAndBathHelper()
	{
		if (SaveGame.Instance == null)
		{
			return;
		}
		ColonyAchievementTracker component = SaveGame.Instance.GetComponent<ColonyAchievementTracker>();
		if (component == null)
		{
			return;
		}
		ColonyAchievement basicComforts = Db.Get().ColonyAchievements.BasicComforts;
		ColonyAchievementStatus colonyAchievementStatus = null;
		if (component.achievements.TryGetValue(basicComforts.Id, out colonyAchievementStatus))
		{
			colonyAchievementStatus.failed = false;
		}
	}

	// Token: 0x060045C2 RID: 17858 RVA: 0x0018A48C File Offset: 0x0018868C
	private void NewAutomationWarning()
	{
		SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.CONFIRMDIALOG.OK, delegate
		{
			screen.Deactivate();
		});
		string[] array = SaveUpgradeWarning.buildingIDsWithNewPorts;
		for (int i = 0; i < array.Length; i++)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(array[i]);
			screen.AddListRow(buildingDef.GetUISprite("ui", false), buildingDef.Name, 150f, 50f);
		}
		screen.PopupConfirmDialog(UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING, UI.FRONTEND.SAVEUPGRADEWARNINGS.NEWAUTOMATIONWARNING_TITLE);
		base.StartCoroutine(this.SendAutomationWarningNotifications());
	}

	// Token: 0x060045C3 RID: 17859 RVA: 0x0018A55A File Offset: 0x0018875A
	private IEnumerator SendAutomationWarningNotifications()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		if (Components.BuildingCompletes.Count == 0)
		{
			global::Debug.LogWarning("Could not send automation warnings because buildings have not yet loaded");
		}
		using (IEnumerator enumerator = Components.BuildingCompletes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				BuildingComplete buildingComplete = (BuildingComplete)obj;
				foreach (string text in SaveUpgradeWarning.buildingIDsWithNewPorts)
				{
					BuildingDef buildingDef = Assets.GetBuildingDef(text);
					if (buildingComplete.Def == buildingDef)
					{
						List<ILogicUIElement> list = new List<ILogicUIElement>();
						LogicPorts component = buildingComplete.GetComponent<LogicPorts>();
						if (component.outputPorts != null)
						{
							list.AddRange(component.outputPorts);
						}
						if (component.inputPorts != null)
						{
							list.AddRange(component.inputPorts);
						}
						foreach (ILogicUIElement logicUIElement in list)
						{
							if (Grid.Objects[logicUIElement.GetLogicUICell(), 31] != null)
							{
								global::Debug.Log("Triggering automation warning for building of type " + text);
								GenericMessage message = new GenericMessage(MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.NAME, MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, MISC.NOTIFICATIONS.NEW_AUTOMATION_WARNING.TOOLTIP, buildingComplete);
								Messenger.Instance.QueueMessage(message);
							}
						}
					}
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x060045C4 RID: 17860 RVA: 0x0018A562 File Offset: 0x00188762
	private IEnumerator TemporaryDisableMeteorShowers(float timeOffDurationInCycles)
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		float sleepTimer = GameUtil.GetCurrentTimeInCycles() + timeOffDurationInCycles;
		foreach (GameplayEvent gameplayEvent in Db.Get().GameplayEvents.resources)
		{
			if (gameplayEvent is MeteorShowerEvent && !(gameplayEvent.Id == Db.Get().GameplayEvents.GassyMooteorEvent.Id))
			{
				gameplayEvent.SetSleepTimer(sleepTimer);
			}
		}
		if (DlcManager.IsPureVanilla())
		{
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(ref list);
			using (List<GameplayEventInstance>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameplayEventInstance eventInstance = enumerator2.Current;
					GameplayEventManager.Instance.RemoveActiveEvent(eventInstance, "Cancelled by SaveUpgradeWarning for player's convenience by providing a window of time without meteors to allow players to adapt to new updates made to relevant buildings");
				}
				yield break;
			}
		}
		yield break;
	}

	// Token: 0x060045C5 RID: 17861 RVA: 0x0018A574 File Offset: 0x00188774
	private void SpaceScannersAndTelescopeUpdateWarning()
	{
		SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.CONFIRMDIALOG.OK, delegate
		{
			screen.Deactivate();
		});
		screen.AddListRow(Assets.GetSprite("space_scanner_range"), UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("telescope_range"), UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_TELESCOPES, 150f, 120f);
		screen.PopupConfirmDialog(UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_SUMMARY + "\n\n" + UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_WARNING, UI.FRONTEND.SAVEUPGRADEWARNINGS.SPACESCANNERANDTELESCOPECHANGES_TITLE);
		base.StartCoroutine(this.TemporaryDisableMeteorShowers(20f));
	}

	// Token: 0x060045C6 RID: 17862 RVA: 0x0018A674 File Offset: 0x00188874
	private void MergedownWarning()
	{
		SpriteListDialogScreen screen = Util.KInstantiateUI<SpriteListDialogScreen>(ScreenPrefabs.Instance.SpriteListDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
		screen.AddOption(UI.DEVELOPMENTBUILDS.FULL_PATCH_NOTES, delegate
		{
			App.OpenWebURL("https://forums.kleientertainment.com/game-updates/oni-alpha/");
		});
		screen.AddOption(UI.CONFIRMDIALOG.OK, delegate
		{
			screen.Deactivate();
		});
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_fridge"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_FOOD, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_deodorizer"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_AIRFILTER, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_steamturbine"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_SIMULATION, 150f, 120f);
		screen.AddListRow(Assets.GetSprite("upgrade_mergedown_oxygen_meter"), UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_BUILDINGS, 150f, 120f);
		screen.PopupConfirmDialog(UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES, UI.FRONTEND.SAVEUPGRADEWARNINGS.MERGEDOWNCHANGES_TITLE);
		base.StartCoroutine(this.SendAutomationWarningNotifications());
	}

	// Token: 0x04002E44 RID: 11844
	[MyCmpReq]
	private Game game;

	// Token: 0x04002E45 RID: 11845
	private static string[] buildingIDsWithNewPorts = new string[]
	{
		"LiquidVent",
		"GasVent",
		"GasVentHighPressure",
		"SolidVent",
		"LiquidReservoir",
		"GasReservoir"
	};

	// Token: 0x020017AD RID: 6061
	private struct Upgrade
	{
		// Token: 0x06008F12 RID: 36626 RVA: 0x003213BA File Offset: 0x0031F5BA
		public Upgrade(int major, int minor, System.Action action)
		{
			this.major = major;
			this.minor = minor;
			this.action = action;
		}

		// Token: 0x04006F9B RID: 28571
		public int major;

		// Token: 0x04006F9C RID: 28572
		public int minor;

		// Token: 0x04006F9D RID: 28573
		public System.Action action;
	}
}
