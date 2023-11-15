using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000745 RID: 1861
public class DebugHandler : IInputHandler
{
	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06003357 RID: 13143 RVA: 0x001112B8 File Offset: 0x0010F4B8
	// (set) Token: 0x06003358 RID: 13144 RVA: 0x001112BF File Offset: 0x0010F4BF
	public static bool NotificationsDisabled { get; private set; }

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06003359 RID: 13145 RVA: 0x001112C7 File Offset: 0x0010F4C7
	// (set) Token: 0x0600335A RID: 13146 RVA: 0x001112CE File Offset: 0x0010F4CE
	public static bool enabled { get; private set; }

	// Token: 0x0600335B RID: 13147 RVA: 0x001112D8 File Offset: 0x0010F4D8
	public DebugHandler()
	{
		DebugHandler.enabled = File.Exists(Path.Combine(Application.dataPath, "debug_enable.txt"));
		DebugHandler.enabled = (DebugHandler.enabled || File.Exists(Path.Combine(Application.dataPath, "../debug_enable.txt")));
		DebugHandler.enabled = (DebugHandler.enabled || GenericGameSettings.instance.debugEnable);
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x0600335C RID: 13148 RVA: 0x00111340 File Offset: 0x0010F540
	public string handlerName
	{
		get
		{
			return "DebugHandler";
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x0600335D RID: 13149 RVA: 0x00111347 File Offset: 0x0010F547
	// (set) Token: 0x0600335E RID: 13150 RVA: 0x0011134F File Offset: 0x0010F54F
	public KInputHandler inputHandler { get; set; }

	// Token: 0x0600335F RID: 13151 RVA: 0x00111358 File Offset: 0x0010F558
	public static int GetMouseCell()
	{
		Vector3 mousePos = KInputManager.GetMousePos();
		mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
		return Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x001113A0 File Offset: 0x0010F5A0
	public static Vector3 GetMousePos()
	{
		Vector3 mousePos = KInputManager.GetMousePos();
		mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
		return Camera.main.ScreenToWorldPoint(mousePos);
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x001113E0 File Offset: 0x0010F5E0
	private void SpawnMinion(bool addAtmoSuit = false)
	{
		if (Immigration.Instance == null)
		{
			return;
		}
		if (!Grid.IsValidBuildingCell(DebugHandler.GetMouseCell()))
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.DEBUG_TOOLS.INVALID_LOCATION, null, DebugHandler.GetMousePos(), 1.5f, false, true);
			return;
		}
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
		gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(DebugHandler.GetMouseCell(), Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		new MinionStartingStats(false, null, null, true).Apply(gameObject);
		if (addAtmoSuit)
		{
			GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab("Atmo_Suit"), position, Grid.SceneLayer.Creatures, null, 0);
			gameObject2.SetActive(true);
			SuitTank component = gameObject2.GetComponent<SuitTank>();
			GameObject gameObject3 = GameUtil.KInstantiate(Assets.GetPrefab(GameTags.Oxygen), position, Grid.SceneLayer.Ore, null, 0);
			gameObject3.GetComponent<PrimaryElement>().Units = component.capacity;
			gameObject3.SetActive(true);
			component.storage.Store(gameObject3, true, false, true, false);
			Equippable component2 = gameObject2.GetComponent<Equippable>();
			gameObject.GetComponent<MinionIdentity>().ValidateProxy();
			Equipment component3 = gameObject.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<Equipment>();
			component2.Assign(component3.GetComponent<IAssignableIdentity>());
			gameObject2.GetComponent<EquippableWorkable>().CancelChore("Debug Handler");
			component3.Equip(component2);
		}
		gameObject.GetMyWorld().SetDupeVisited();
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x00111566 File Offset: 0x0010F766
	public static void SetDebugEnabled(bool debugEnabled)
	{
		DebugHandler.enabled = debugEnabled;
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x0011156E File Offset: 0x0010F76E
	public static void ToggleDisableNotifications()
	{
		DebugHandler.NotificationsDisabled = !DebugHandler.NotificationsDisabled;
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x00111580 File Offset: 0x0010F780
	private string GetScreenshotFileName()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		string text = Path.Combine(Path.GetDirectoryName(activeSaveFilePath), "screenshot");
		string fileName = Path.GetFileName(activeSaveFilePath);
		Directory.CreateDirectory(text);
		string path = string.Concat(new string[]
		{
			Path.GetFileNameWithoutExtension(fileName),
			"_",
			GameClock.Instance.GetCycle().ToString(),
			"_",
			System.DateTime.Now.ToString("yyyy-MM-dd_HH\\hmm\\mss\\s"),
			".png"
		});
		return Path.Combine(text, path);
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x00111610 File Offset: 0x0010F810
	public void OnKeyDown(KButtonEvent e)
	{
		if (!DebugHandler.enabled)
		{
			return;
		}
		if (e.TryConsume(global::Action.DebugSpawnMinion))
		{
			this.SpawnMinion(false);
		}
		else if (e.TryConsume(global::Action.DebugSpawnMinionAtmoSuit))
		{
			this.SpawnMinion(true);
		}
		else if (e.TryConsume(global::Action.DebugCheerEmote))
		{
			for (int i = 0; i < Components.MinionIdentities.Count; i++)
			{
				new EmoteChore(Components.MinionIdentities[i].GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[]
				{
					"cheer_pre",
					"cheer_loop",
					"cheer_pst"
				}, null);
				new EmoteChore(Components.MinionIdentities[i].GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[]
				{
					"cheer_pre",
					"cheer_loop",
					"cheer_pst"
				}, null);
			}
		}
		else if (e.TryConsume(global::Action.DebugSpawnStressTest))
		{
			for (int j = 0; j < 60; j++)
			{
				this.SpawnMinion(false);
			}
		}
		else if (e.TryConsume(global::Action.DebugSuperTestMode))
		{
			if (!this.superTestMode)
			{
				Time.timeScale = 15f;
				this.superTestMode = true;
			}
			else
			{
				Time.timeScale = 1f;
				this.superTestMode = false;
			}
		}
		else if (e.TryConsume(global::Action.DebugUltraTestMode))
		{
			if (!this.ultraTestMode)
			{
				Time.timeScale = 30f;
				this.ultraTestMode = true;
			}
			else
			{
				Time.timeScale = 1f;
				this.ultraTestMode = false;
			}
		}
		else if (e.TryConsume(global::Action.DebugSlowTestMode))
		{
			if (!this.slowTestMode)
			{
				Time.timeScale = 0.06f;
				this.slowTestMode = true;
			}
			else
			{
				Time.timeScale = 1f;
				this.slowTestMode = false;
			}
		}
		else if (e.TryConsume(global::Action.DebugDig))
		{
			SimMessages.Dig(DebugHandler.GetMouseCell(), -1, false);
		}
		else if (e.TryConsume(global::Action.DebugToggleFastWorkers))
		{
			Game.Instance.FastWorkersModeActive = !Game.Instance.FastWorkersModeActive;
		}
		else if (e.TryConsume(global::Action.DebugInstantBuildMode))
		{
			DebugHandler.InstantBuildMode = !DebugHandler.InstantBuildMode;
			InterfaceTool.ToggleConfig(global::Action.DebugInstantBuildMode);
			if (Game.Instance == null)
			{
				return;
			}
			Game.Instance.Trigger(1557339983, null);
			if (PlanScreen.Instance != null)
			{
				PlanScreen.Instance.Refresh();
			}
			if (BuildMenu.Instance != null)
			{
				BuildMenu.Instance.Refresh();
			}
			if (OverlayMenu.Instance != null)
			{
				OverlayMenu.Instance.Refresh();
			}
			if (ConsumerManager.instance != null)
			{
				ConsumerManager.instance.RefreshDiscovered(null);
			}
			if (ManagementMenu.Instance != null)
			{
				ManagementMenu.Instance.CheckResearch(null);
				ManagementMenu.Instance.CheckSkills(null);
				ManagementMenu.Instance.CheckStarmap(null);
			}
			if (SelectTool.Instance.selected != null)
			{
				DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
			}
			Game.Instance.Trigger(1594320620, "all_the_things");
		}
		else if (e.TryConsume(global::Action.DebugExplosion))
		{
			Vector3 mousePos = KInputManager.GetMousePos();
			mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
			GameUtil.CreateExplosion(Camera.main.ScreenToWorldPoint(mousePos));
		}
		else if (e.TryConsume(global::Action.DebugLockCursor))
		{
			if (GenericGameSettings.instance.developerDebugEnable)
			{
				KInputManager.isMousePosLocked = !KInputManager.isMousePosLocked;
				KInputManager.lockedMousePos = KInputManager.GetMousePos();
			}
		}
		else
		{
			if (e.TryConsume(global::Action.DebugDiscoverAllElements))
			{
				if (!(DiscoveredResources.Instance != null))
				{
					goto IL_B62;
				}
				using (List<Element>.Enumerator enumerator = ElementLoader.elements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Element element = enumerator.Current;
						DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
					}
					goto IL_B62;
				}
			}
			if (e.TryConsume(global::Action.DebugToggleUI))
			{
				DebugHandler.ToggleScreenshotMode();
			}
			else if (e.TryConsume(global::Action.SreenShot1x))
			{
				ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 1);
			}
			else if (e.TryConsume(global::Action.SreenShot2x))
			{
				ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 2);
			}
			else if (e.TryConsume(global::Action.SreenShot8x))
			{
				ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 8);
			}
			else if (e.TryConsume(global::Action.SreenShot32x))
			{
				ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 32);
			}
			else if (e.TryConsume(global::Action.DebugCellInfo))
			{
				DebugHandler.DebugCellInfo = !DebugHandler.DebugCellInfo;
			}
			else if (e.TryConsume(global::Action.DebugToggle))
			{
				if (Game.Instance != null)
				{
					SaveGame.Instance.worldGenSpawner.SpawnEverything();
				}
				InterfaceTool.ToggleConfig(global::Action.DebugToggle);
				if (DebugPaintElementScreen.Instance != null)
				{
					bool activeSelf = DebugPaintElementScreen.Instance.gameObject.activeSelf;
					DebugPaintElementScreen.Instance.gameObject.SetActive(!activeSelf);
					if (DebugElementMenu.Instance && DebugElementMenu.Instance.root.activeSelf)
					{
						DebugElementMenu.Instance.root.SetActive(false);
					}
					DebugBaseTemplateButton.Instance.gameObject.SetActive(!activeSelf);
					PropertyTextures.FogOfWarScale = (float)((!activeSelf) ? 1 : 0);
					if (CameraController.Instance != null)
					{
						CameraController.Instance.EnableFreeCamera(!activeSelf);
					}
					DebugHandler.RevealFogOfWar = !DebugHandler.RevealFogOfWar;
					Game.Instance.Trigger(-1991583975, null);
				}
			}
			else if (e.TryConsume(global::Action.DebugCollectGarbage))
			{
				GC.Collect();
			}
			else if (e.TryConsume(global::Action.DebugInvincible))
			{
				DebugHandler.InvincibleMode = !DebugHandler.InvincibleMode;
			}
			else if (e.TryConsume(global::Action.DebugVisualTest))
			{
				Scenario.Instance.SetupVisualTest();
			}
			else if (e.TryConsume(global::Action.DebugGameplayTest))
			{
				Scenario.Instance.SetupGameplayTest();
			}
			else if (e.TryConsume(global::Action.DebugElementTest))
			{
				Scenario.Instance.SetupElementTest();
			}
			else if (e.TryConsume(global::Action.ToggleProfiler))
			{
				Sim.SIM_HandleMessage(-409964931, 0, null);
			}
			else if (e.TryConsume(global::Action.DebugRefreshNavCell))
			{
				Pathfinding.Instance.RefreshNavCell(DebugHandler.GetMouseCell());
			}
			else if (e.TryConsume(global::Action.DebugToggleSelectInEditor))
			{
				DebugHandler.SetSelectInEditor(!DebugHandler.SelectInEditor);
			}
			else
			{
				if (e.TryConsume(global::Action.DebugGotoTarget))
				{
					global::Debug.Log("Debug GoTo");
					Game.Instance.Trigger(775300118, null);
					using (List<Brain>.Enumerator enumerator2 = Components.Brains.Items.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Brain cmp = enumerator2.Current;
							DebugGoToMonitor.Instance smi = cmp.GetSMI<DebugGoToMonitor.Instance>();
							if (smi != null)
							{
								smi.GoToCursor();
							}
							CreatureDebugGoToMonitor.Instance smi2 = cmp.GetSMI<CreatureDebugGoToMonitor.Instance>();
							if (smi2 != null)
							{
								smi2.GoToCursor();
							}
						}
						goto IL_B62;
					}
				}
				if (e.TryConsume(global::Action.DebugTeleport))
				{
					if (SelectTool.Instance == null)
					{
						return;
					}
					KSelectable selected = SelectTool.Instance.selected;
					if (selected != null)
					{
						int mouseCell = DebugHandler.GetMouseCell();
						if (!Grid.IsValidBuildingCell(mouseCell))
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.DEBUG_TOOLS.INVALID_LOCATION, null, DebugHandler.GetMousePos(), 1.5f, false, true);
							return;
						}
						selected.transform.SetPosition(Grid.CellToPosCBC(mouseCell, Grid.SceneLayer.Move));
					}
				}
				else if (!e.TryConsume(global::Action.DebugPlace) && !e.TryConsume(global::Action.DebugSelectMaterial))
				{
					if (e.TryConsume(global::Action.DebugNotification))
					{
						if (GenericGameSettings.instance.developerDebugEnable)
						{
							Tutorial.Instance.DebugNotification();
						}
					}
					else if (e.TryConsume(global::Action.DebugNotificationMessage))
					{
						if (GenericGameSettings.instance.developerDebugEnable)
						{
							Tutorial.Instance.DebugNotificationMessage();
						}
					}
					else if (e.TryConsume(global::Action.DebugSuperSpeed))
					{
						if (SpeedControlScreen.Instance != null)
						{
							SpeedControlScreen.Instance.ToggleRidiculousSpeed();
						}
					}
					else if (e.TryConsume(global::Action.DebugGameStep))
					{
						if (SpeedControlScreen.Instance != null)
						{
							SpeedControlScreen.Instance.DebugStepFrame();
						}
					}
					else if (e.TryConsume(global::Action.DebugSimStep))
					{
						Game.Instance.ForceSimStep();
					}
					else if (e.TryConsume(global::Action.DebugToggleMusic))
					{
						AudioDebug.Get().ToggleMusic();
					}
					else if (e.TryConsume(global::Action.DebugTileTest))
					{
						Scenario.Instance.SetupTileTest();
					}
					else if (e.TryConsume(global::Action.DebugForceLightEverywhere))
					{
						PropertyTextures.instance.ForceLightEverywhere = !PropertyTextures.instance.ForceLightEverywhere;
					}
					else if (e.TryConsume(global::Action.DebugPathFinding))
					{
						DebugHandler.DebugPathFinding = !DebugHandler.DebugPathFinding;
						global::Debug.Log("DebugPathFinding=" + DebugHandler.DebugPathFinding.ToString());
					}
					else if (!e.TryConsume(global::Action.DebugFocus))
					{
						if (e.TryConsume(global::Action.DebugReportBug))
						{
							if (GenericGameSettings.instance.developerDebugEnable)
							{
								int num = 0;
								string validSaveFilename;
								for (;;)
								{
									validSaveFilename = SaveScreen.GetValidSaveFilename("bug_report_savefile_" + num.ToString());
									if (!File.Exists(validSaveFilename))
									{
										break;
									}
									num++;
								}
								if (SaveLoader.Instance != null)
								{
									SaveLoader.Instance.Save(validSaveFilename, false, false);
								}
								KCrashReporter.ReportBug("Bug Report", GameObject.Find("ScreenSpaceOverlayCanvas"));
							}
							else
							{
								global::Debug.Log("Debug crash keys are not enabled.");
							}
						}
						else if (e.TryConsume(global::Action.DebugTriggerException))
						{
							if (GenericGameSettings.instance.developerDebugEnable)
							{
								throw new ArgumentException("My test exception");
							}
						}
						else if (e.TryConsume(global::Action.DebugTriggerError))
						{
							UnityEngine.Debug.Log("trigger error");
							if (GenericGameSettings.instance.developerDebugEnable)
							{
								KCrashReporter.disableDeduping = true;
								global::Debug.LogError("Oooops! Testing error!");
							}
						}
						else if (e.TryConsume(global::Action.DebugDumpGCRoots))
						{
							GarbageProfiler.DebugDumpRootItems();
						}
						else if (e.TryConsume(global::Action.DebugDumpGarbageReferences))
						{
							GarbageProfiler.DebugDumpGarbageStats();
						}
						else if (e.TryConsume(global::Action.DebugDumpEventData))
						{
							if (GenericGameSettings.instance.developerDebugEnable)
							{
								KObjectManager.Instance.DumpEventData();
							}
						}
						else if (e.TryConsume(global::Action.DebugDumpSceneParitionerLeakData))
						{
							if (GenericGameSettings.instance.developerDebugEnable)
							{
							}
						}
						else if (e.TryConsume(global::Action.DebugCrashSim))
						{
							if (GenericGameSettings.instance.developerDebugEnable)
							{
								Sim.SIM_DebugCrash();
							}
						}
						else if (e.TryConsume(global::Action.DebugNextCall))
						{
							DebugHandler.DebugNextCall = true;
						}
						else if (e.TryConsume(global::Action.DebugTogglePersonalPriorityComparison))
						{
							Chore.ENABLE_PERSONAL_PRIORITIES = !Chore.ENABLE_PERSONAL_PRIORITIES;
						}
						else if (e.TryConsume(global::Action.DebugToggleClusterFX))
						{
							CameraController.Instance.ToggleClusterFX();
						}
					}
				}
			}
		}
		IL_B62:
		if (e.Consumed && Game.Instance != null)
		{
			Game.Instance.debugWasUsed = true;
			KCrashReporter.debugWasUsed = true;
		}
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x001121C4 File Offset: 0x001103C4
	public static void SetSelectInEditor(bool select_in_editor)
	{
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x001121C8 File Offset: 0x001103C8
	public static void ToggleScreenshotMode()
	{
		DebugHandler.ScreenshotMode = !DebugHandler.ScreenshotMode;
		DebugHandler.UpdateUI();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.EnableFreeCamera(DebugHandler.ScreenshotMode);
		}
		if (KScreenManager.Instance != null)
		{
			KScreenManager.Instance.DisableInput(DebugHandler.ScreenshotMode);
		}
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x00112220 File Offset: 0x00110420
	public static void SetTimelapseMode(bool enabled, int world_id = 0)
	{
		DebugHandler.TimelapseMode = enabled;
		if (enabled)
		{
			DebugHandler.activeWorldBeforeOverride = ClusterManager.Instance.activeWorldId;
			ClusterManager.Instance.TimelapseModeOverrideActiveWorld(world_id);
		}
		else
		{
			ClusterManager.Instance.TimelapseModeOverrideActiveWorld(DebugHandler.activeWorldBeforeOverride);
		}
		World.Instance.zoneRenderData.OnActiveWorldChanged();
		DebugHandler.UpdateUI();
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x00112278 File Offset: 0x00110478
	private static void UpdateUI()
	{
		DebugHandler.HideUI = (DebugHandler.TimelapseMode || DebugHandler.ScreenshotMode);
		float num = DebugHandler.HideUI ? 0f : 1f;
		GameScreenManager.Instance.ssHoverTextCanvas.GetComponent<CanvasGroup>().alpha = num;
		GameScreenManager.Instance.ssCameraCanvas.GetComponent<CanvasGroup>().alpha = num;
		GameScreenManager.Instance.ssOverlayCanvas.GetComponent<CanvasGroup>().alpha = num;
		GameScreenManager.Instance.worldSpaceCanvas.GetComponent<CanvasGroup>().alpha = num;
		GameScreenManager.Instance.screenshotModeCanvas.GetComponent<CanvasGroup>().alpha = 1f - num;
	}

	// Token: 0x04001ED3 RID: 7891
	public static bool InstantBuildMode;

	// Token: 0x04001ED4 RID: 7892
	public static bool InvincibleMode;

	// Token: 0x04001ED5 RID: 7893
	public static bool SelectInEditor;

	// Token: 0x04001ED6 RID: 7894
	public static bool DebugPathFinding;

	// Token: 0x04001ED7 RID: 7895
	public static bool ScreenshotMode;

	// Token: 0x04001ED8 RID: 7896
	public static bool TimelapseMode;

	// Token: 0x04001ED9 RID: 7897
	public static bool HideUI;

	// Token: 0x04001EDA RID: 7898
	public static bool DebugCellInfo;

	// Token: 0x04001EDB RID: 7899
	public static bool DebugNextCall;

	// Token: 0x04001EDC RID: 7900
	public static bool RevealFogOfWar;

	// Token: 0x04001EE0 RID: 7904
	private bool superTestMode;

	// Token: 0x04001EE1 RID: 7905
	private bool ultraTestMode;

	// Token: 0x04001EE2 RID: 7906
	private bool slowTestMode;

	// Token: 0x04001EE3 RID: 7907
	private static int activeWorldBeforeOverride = -1;

	// Token: 0x020014E2 RID: 5346
	public enum PaintMode
	{
		// Token: 0x040066B9 RID: 26297
		None,
		// Token: 0x040066BA RID: 26298
		Element,
		// Token: 0x040066BB RID: 26299
		Hot,
		// Token: 0x040066BC RID: 26300
		Cold
	}
}
