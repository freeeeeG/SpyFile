using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using FMOD.Studio;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Token: 0x020007CA RID: 1994
[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour
{
	// Token: 0x06003763 RID: 14179 RVA: 0x0012BC2C File Offset: 0x00129E2C
	public static bool IsQuitting()
	{
		return Game.quitting;
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06003764 RID: 14180 RVA: 0x0012BC33 File Offset: 0x00129E33
	// (set) Token: 0x06003765 RID: 14181 RVA: 0x0012BC3B File Offset: 0x00129E3B
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06003766 RID: 14182 RVA: 0x0012BC44 File Offset: 0x00129E44
	// (set) Token: 0x06003767 RID: 14183 RVA: 0x0012BC4B File Offset: 0x00129E4B
	public static Game Instance { get; private set; }

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06003768 RID: 14184 RVA: 0x0012BC53 File Offset: 0x00129E53
	public static Camera MainCamera
	{
		get
		{
			if (Game.m_CachedCamera == null)
			{
				Game.m_CachedCamera = Camera.main;
			}
			return Game.m_CachedCamera;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06003769 RID: 14185 RVA: 0x0012BC71 File Offset: 0x00129E71
	// (set) Token: 0x0600376A RID: 14186 RVA: 0x0012BC94 File Offset: 0x00129E94
	public bool SaveToCloudActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, value2);
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x0600376B RID: 14187 RVA: 0x0012BCC1 File Offset: 0x00129EC1
	// (set) Token: 0x0600376C RID: 14188 RVA: 0x0012BCE4 File Offset: 0x00129EE4
	public bool FastWorkersModeActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.FastWorkersMode).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.FastWorkersMode, value2);
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x0600376D RID: 14189 RVA: 0x0012BD11 File Offset: 0x00129F11
	// (set) Token: 0x0600376E RID: 14190 RVA: 0x0012BD1C File Offset: 0x00129F1C
	public bool SandboxModeActive
	{
		get
		{
			return this.sandboxModeActive;
		}
		set
		{
			this.sandboxModeActive = value;
			base.Trigger(-1948169901, null);
			if (PlanScreen.Instance != null)
			{
				PlanScreen.Instance.Refresh();
			}
			if (BuildMenu.Instance != null)
			{
				BuildMenu.Instance.Refresh();
			}
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x0600376F RID: 14191 RVA: 0x0012BD6A File Offset: 0x00129F6A
	public bool DebugOnlyBuildingsAllowed
	{
		get
		{
			return DebugHandler.enabled && (this.SandboxModeActive || DebugHandler.InstantBuildMode);
		}
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06003770 RID: 14192 RVA: 0x0012BD84 File Offset: 0x00129F84
	// (set) Token: 0x06003771 RID: 14193 RVA: 0x0012BD8C File Offset: 0x00129F8C
	public StatusItemRenderer statusItemRenderer { get; private set; }

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06003772 RID: 14194 RVA: 0x0012BD95 File Offset: 0x00129F95
	// (set) Token: 0x06003773 RID: 14195 RVA: 0x0012BD9D File Offset: 0x00129F9D
	public PrioritizableRenderer prioritizableRenderer { get; private set; }

	// Token: 0x06003774 RID: 14196 RVA: 0x0012BDA8 File Offset: 0x00129FA8
	protected override void OnPrefabInit()
	{
		DebugUtil.LogArgs(new object[]
		{
			Time.realtimeSinceStartup,
			"Level Loaded....",
			SceneManager.GetActiveScene().name
		});
		Components.BuildingCellVisualizers.OnAdd += this.OnAddBuildingCellVisualizer;
		Components.BuildingCellVisualizers.OnRemove += this.OnRemoveBuildingCellVisualizer;
		Singleton<KBatchedAnimUpdater>.CreateInstance();
		Singleton<CellChangeMonitor>.CreateInstance();
		this.userMenu = new UserMenu();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.StopBE));
		Game.Instance = this;
		this.statusItemRenderer = new StatusItemRenderer();
		this.prioritizableRenderer = new PrioritizableRenderer();
		this.LoadEventHashes();
		this.savedInfo.InitializeEmptyVariables();
		this.gasFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasConduits) - 0.4f);
		this.liquidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.LiquidConduits) - 0.4f);
		this.solidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.SolidConduitContents) - 0.4f);
		Shader.WarmupAllShaders();
		Db.Get();
		Game.quitting = false;
		Game.PickupableLayer = LayerMask.NameToLayer("Pickupable");
		Game.BlockSelectionLayerMask = LayerMask.GetMask(new string[]
		{
			"BlockSelection"
		});
		this.world = World.Instance;
		KPrefabID.NextUniqueID = KPlayerPrefs.GetInt(Game.NextUniqueIDKey, 0);
		this.circuitManager = new CircuitManager();
		this.energySim = new EnergySim();
		this.gasConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 13);
		this.liquidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 17);
		this.electricalConduitSystem = new UtilityNetworkManager<ElectricalUtilityNetwork, Wire>(Grid.WidthInCells, Grid.HeightInCells, 27);
		this.logicCircuitSystem = new UtilityNetworkManager<LogicCircuitNetwork, LogicWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
		this.logicCircuitManager = new LogicCircuitManager(this.logicCircuitSystem);
		this.travelTubeSystem = new UtilityNetworkTubesManager(Grid.WidthInCells, Grid.HeightInCells, 35);
		this.solidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, SolidConduit>(Grid.WidthInCells, Grid.HeightInCells, 21);
		this.conduitTemperatureManager = new ConduitTemperatureManager();
		this.conduitDiseaseManager = new ConduitDiseaseManager(this.conduitTemperatureManager);
		this.gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, this.gasConduitSystem, 1f, 0.25f);
		this.liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, this.liquidConduitSystem, 10f, 0.75f);
		this.solidConduitFlow = new SolidConduitFlow(Grid.CellCount, this.solidConduitSystem, 0.75f);
		this.gasFlowVisualizer = new ConduitFlowVisualizer(this.gasConduitFlow, this.gasConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundGas, Lighting.Instance.Settings.GasConduit);
		this.liquidFlowVisualizer = new ConduitFlowVisualizer(this.liquidConduitFlow, this.liquidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundLiquid, Lighting.Instance.Settings.LiquidConduit);
		this.solidFlowVisualizer = new SolidConduitFlowVisualizer(this.solidConduitFlow, this.solidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundSolid, Lighting.Instance.Settings.SolidConduit);
		this.accumulators = new Accumulators();
		this.plantElementAbsorbers = new PlantElementAbsorbers();
		this.activeFX = new ushort[Grid.CellCount];
		this.UnsafePrefabInit();
		Shader.SetGlobalVector("_MetalParameters", new Vector4(0f, 0f, 0f, 0f));
		Shader.SetGlobalVector("_WaterParameters", new Vector4(0f, 0f, 0f, 0f));
		this.InitializeFXSpawners();
		PathFinder.Initialize();
		new GameNavGrids(Pathfinding.Instance);
		this.screenMgr = global::Util.KInstantiate(this.screenManagerPrefab, null, null).GetComponent<GameScreenManager>();
		this.roomProber = new RoomProber();
		this.spaceScannerNetworkManager = new SpaceScannerNetworkManager();
		this.fetchManager = base.gameObject.AddComponent<FetchManager>();
		this.ediblesManager = base.gameObject.AddComponent<EdiblesManager>();
		Singleton<CellChangeMonitor>.Instance.SetGridSize(Grid.WidthInCells, Grid.HeightInCells);
		this.unlocks = base.GetComponent<Unlocks>();
		this.changelistsPlayedOn = new List<uint>();
		this.changelistsPlayedOn.Add(577063U);
		this.dateGenerated = System.DateTime.UtcNow.ToString("U", CultureInfo.InvariantCulture);
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x0012C222 File Offset: 0x0012A422
	public void SetGameStarted()
	{
		this.gameStarted = true;
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x0012C22B File Offset: 0x0012A42B
	public bool GameStarted()
	{
		return this.gameStarted;
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x0012C233 File Offset: 0x0012A433
	private void UnsafePrefabInit()
	{
		this.StepTheSim(0f);
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x0012C241 File Offset: 0x0012A441
	protected override void OnLoadLevel()
	{
		base.Unsubscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate, false);
		base.Unsubscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate, false);
		base.OnLoadLevel();
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x0012C26B File Offset: 0x0012A46B
	private void MarkStatusItemRendererDirty(object data)
	{
		this.statusItemRenderer.MarkAllDirty();
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x0012C278 File Offset: 0x0012A478
	protected override void OnForcedCleanUp()
	{
		if (this.prioritizableRenderer != null)
		{
			this.prioritizableRenderer.Cleanup();
			this.prioritizableRenderer = null;
		}
		if (this.statusItemRenderer != null)
		{
			this.statusItemRenderer.Destroy();
			this.statusItemRenderer = null;
		}
		if (this.conduitTemperatureManager != null)
		{
			this.conduitTemperatureManager.Shutdown();
		}
		this.gasFlowVisualizer.FreeResources();
		this.liquidFlowVisualizer.FreeResources();
		this.solidFlowVisualizer.FreeResources();
		LightGridManager.Shutdown();
		RadiationGridManager.Shutdown();
		App.OnPreLoadScene = (System.Action)Delegate.Remove(App.OnPreLoadScene, new System.Action(this.StopBE));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x0012C320 File Offset: 0x0012A520
	protected override void OnSpawn()
	{
		global::Debug.Log("-- GAME --");
		Game.BrainScheduler = base.GetComponent<BrainScheduler>();
		PropertyTextures.FogOfWarScale = 0f;
		if (CameraController.Instance != null)
		{
			CameraController.Instance.EnableFreeCamera(false);
		}
		this.LocalPlayer = this.SpawnPlayer();
		WaterCubes.Instance.Init();
		SpeedControlScreen.Instance.Pause(false, false);
		LightGridManager.Initialise();
		RadiationGridManager.Initialise();
		this.RefreshRadiationLoop();
		this.UnsafeOnSpawn();
		Time.timeScale = 0f;
		if (this.tempIntroScreenPrefab != null)
		{
			global::Util.KInstantiate(this.tempIntroScreenPrefab, null, null);
		}
		if (SaveLoader.Instance.ClusterLayout != null)
		{
			foreach (WorldGen worldGen in SaveLoader.Instance.ClusterLayout.worlds)
			{
				this.Reset(worldGen.data.gameSpawnData, worldGen.WorldOffset);
			}
			NewBaseScreen.SetInitialCamera();
		}
		TagManager.FillMissingProperNames();
		CameraController.Instance.OrthographicSize = 20f;
		if (SaveLoader.Instance.loadedFromSave)
		{
			this.baseAlreadyCreated = true;
			base.Trigger(-1992507039, null);
			base.Trigger(-838649377, null);
		}
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
		for (int i = 0; i < array.Length; i++)
		{
			((MeshRenderer)array[i]).reflectionProbeUsage = ReflectionProbeUsage.Off;
		}
		base.Subscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate);
		base.Subscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate);
		this.solidConduitFlow.Initialize();
		SimAndRenderScheduler.instance.Add(this.roomProber, false);
		SimAndRenderScheduler.instance.Add(this.spaceScannerNetworkManager, false);
		SimAndRenderScheduler.instance.Add(KComponentSpawn.instance, false);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(new UpdateBucketWithUpdater<ISim200ms>.BatchUpdateDelegate(AmountInstance.BatchUpdate));
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(new UpdateBucketWithUpdater<ISim1000ms>.BatchUpdateDelegate(SolidTransferArm.BatchUpdate));
		if (!SaveLoader.Instance.loadedFromSave)
		{
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.SandboxMode.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SandboxMode);
			SaveGame.Instance.sandboxEnabled = !settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}
		this.mingleCellTracker = base.gameObject.AddComponent<MingleCellTracker>();
		if (Global.Instance != null)
		{
			Global.Instance.GetComponent<PerformanceMonitor>().Reset();
			Global.Instance.modManager.NotifyDialog(UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.TITLE, UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.MESSAGE, Global.Instance.globalCanvas);
		}
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x0012C5D8 File Offset: 0x0012A7D8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SimAndRenderScheduler.instance.Remove(KComponentSpawn.instance);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(null);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(null);
		this.DestroyInstances();
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x0012C60B File Offset: 0x0012A80B
	private new void OnDestroy()
	{
		base.OnDestroy();
		this.DestroyInstances();
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x0012C619 File Offset: 0x0012A819
	private void UnsafeOnSpawn()
	{
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, 0, null, 0, null);
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x0012C638 File Offset: 0x0012A838
	private void RefreshRadiationLoop()
	{
		GameScheduler.Instance.Schedule("UpdateRadiation", 1f, delegate(object obj)
		{
			RadiationGridManager.Refresh();
			this.RefreshRadiationLoop();
		}, null, null);
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x0012C65D File Offset: 0x0012A85D
	public void SetMusicEnabled(bool enabled)
	{
		if (enabled)
		{
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			return;
		}
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x0012C684 File Offset: 0x0012A884
	private Player SpawnPlayer()
	{
		Player component = global::Util.KInstantiate(this.playerPrefab, base.gameObject, null).GetComponent<Player>();
		component.ScreenManager = this.screenMgr;
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HudScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HoverTextScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.ToolTipScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		this.cameraController = global::Util.KInstantiate(this.cameraControllerPrefab, null, null).GetComponent<CameraController>();
		component.CameraController = this.cameraController;
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.cameraController, 1);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.cameraController, 1);
		}
		Global.GetInputManager().usedMenus.Add(this.cameraController);
		this.playerController = component.GetComponent<PlayerController>();
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.playerController, 20);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.playerController, 20);
		}
		Global.GetInputManager().usedMenus.Add(this.playerController);
		return component;
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x0012C7C9 File Offset: 0x0012A9C9
	public void SetDupePassableSolid(int cell, bool passable, bool solid)
	{
		Grid.DupePassable[cell] = passable;
		this.gameSolidInfo.Add(new SolidInfo(cell, solid));
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x0012C7EC File Offset: 0x0012A9EC
	private unsafe Sim.GameDataUpdate* StepTheSim(float dt)
	{
		Sim.GameDataUpdate* result;
		using (new KProfiler.Region("StepTheSim", null))
		{
			IntPtr intPtr = IntPtr.Zero;
			using (new KProfiler.Region("WaitingForSim", null))
			{
				if (Grid.Visible == null || Grid.Visible.Length == 0)
				{
					global::Debug.LogError("Invalid Grid.Visible, what have you done?!");
					return null;
				}
				intPtr = Sim.HandleMessage(SimMessageHashes.PrepareGameData, Grid.Visible.Length, Grid.Visible);
			}
			if (intPtr == IntPtr.Zero)
			{
				result = null;
			}
			else
			{
				Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)intPtr);
				Grid.elementIdx = ptr->elementIdx;
				Grid.temperature = ptr->temperature;
				Grid.mass = ptr->mass;
				Grid.radiation = ptr->radiation;
				Grid.properties = ptr->properties;
				Grid.strengthInfo = ptr->strengthInfo;
				Grid.insulation = ptr->insulation;
				Grid.diseaseIdx = ptr->diseaseIdx;
				Grid.diseaseCount = ptr->diseaseCount;
				Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
				Grid.exposedToSunlight = (byte*)((void*)ptr->propertyTextureExposedToSunlight);
				PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
				PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
				PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
				List<Element> elements = ElementLoader.elements;
				this.simData.emittedMassEntries = ptr->emittedMassEntries;
				this.simData.elementChunks = ptr->elementChunkInfos;
				this.simData.buildingTemperatures = ptr->buildingTemperatures;
				this.simData.diseaseEmittedInfos = ptr->diseaseEmittedInfos;
				this.simData.diseaseConsumedInfos = ptr->diseaseConsumedInfos;
				for (int i = 0; i < ptr->numSubstanceChangeInfo; i++)
				{
					Sim.SubstanceChangeInfo substanceChangeInfo = ptr->substanceChangeInfo[i];
					Element element = elements[(int)substanceChangeInfo.newElemIdx];
					Grid.Element[substanceChangeInfo.cellIdx] = element;
				}
				for (int j = 0; j < ptr->numSolidInfo; j++)
				{
					Sim.SolidInfo solidInfo = ptr->solidInfo[j];
					if (!this.solidChangedFilter.Contains(solidInfo.cellIdx))
					{
						this.solidInfo.Add(new SolidInfo(solidInfo.cellIdx, solidInfo.isSolid != 0));
						bool solid = solidInfo.isSolid != 0;
						Grid.SetSolid(solidInfo.cellIdx, solid, CellEventLogger.Instance.SimMessagesSolid);
					}
				}
				for (int k = 0; k < ptr->numCallbackInfo; k++)
				{
					Sim.CallbackInfo callbackInfo = ptr->callbackInfo[k];
					HandleVector<Game.CallbackInfo>.Handle handle = new HandleVector<Game.CallbackInfo>.Handle
					{
						index = callbackInfo.callbackIdx
					};
					if (!this.IsManuallyReleasedHandle(handle))
					{
						this.callbackInfo.Add(new Klei.CallbackInfo(handle));
					}
				}
				int numSpawnFallingLiquidInfo = ptr->numSpawnFallingLiquidInfo;
				for (int l = 0; l < numSpawnFallingLiquidInfo; l++)
				{
					Sim.SpawnFallingLiquidInfo spawnFallingLiquidInfo = ptr->spawnFallingLiquidInfo[l];
					FallingWater.instance.AddParticle(spawnFallingLiquidInfo.cellIdx, spawnFallingLiquidInfo.elemIdx, spawnFallingLiquidInfo.mass, spawnFallingLiquidInfo.temperature, spawnFallingLiquidInfo.diseaseIdx, spawnFallingLiquidInfo.diseaseCount, false, false, false, false);
				}
				int numDigInfo = ptr->numDigInfo;
				WorldDamage component = this.world.GetComponent<WorldDamage>();
				for (int m = 0; m < numDigInfo; m++)
				{
					Sim.SpawnOreInfo spawnOreInfo = ptr->digInfo[m];
					if (spawnOreInfo.temperature <= 0f && spawnOreInfo.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					component.OnDigComplete(spawnOreInfo.cellIdx, spawnOreInfo.mass, spawnOreInfo.temperature, spawnOreInfo.elemIdx, spawnOreInfo.diseaseIdx, spawnOreInfo.diseaseCount);
				}
				int numSpawnOreInfo = ptr->numSpawnOreInfo;
				for (int n = 0; n < numSpawnOreInfo; n++)
				{
					Sim.SpawnOreInfo spawnOreInfo2 = ptr->spawnOreInfo[n];
					Vector3 position = Grid.CellToPosCCC(spawnOreInfo2.cellIdx, Grid.SceneLayer.Ore);
					Element element2 = ElementLoader.elements[(int)spawnOreInfo2.elemIdx];
					if (spawnOreInfo2.temperature <= 0f && spawnOreInfo2.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					element2.substance.SpawnResource(position, spawnOreInfo2.mass, spawnOreInfo2.temperature, spawnOreInfo2.diseaseIdx, spawnOreInfo2.diseaseCount, false, false, false);
				}
				int numSpawnFXInfo = ptr->numSpawnFXInfo;
				for (int num = 0; num < numSpawnFXInfo; num++)
				{
					Sim.SpawnFXInfo spawnFXInfo = ptr->spawnFXInfo[num];
					this.SpawnFX((SpawnFXHashes)spawnFXInfo.fxHash, spawnFXInfo.cellIdx, spawnFXInfo.rotation);
				}
				UnstableGroundManager component2 = this.world.GetComponent<UnstableGroundManager>();
				int numUnstableCellInfo = ptr->numUnstableCellInfo;
				for (int num2 = 0; num2 < numUnstableCellInfo; num2++)
				{
					Sim.UnstableCellInfo unstableCellInfo = ptr->unstableCellInfo[num2];
					if (unstableCellInfo.fallingInfo == 0)
					{
						component2.Spawn(unstableCellInfo.cellIdx, ElementLoader.elements[(int)unstableCellInfo.elemIdx], unstableCellInfo.mass, unstableCellInfo.temperature, unstableCellInfo.diseaseIdx, unstableCellInfo.diseaseCount);
					}
				}
				int numWorldDamageInfo = ptr->numWorldDamageInfo;
				for (int num3 = 0; num3 < numWorldDamageInfo; num3++)
				{
					Sim.WorldDamageInfo damage_info = ptr->worldDamageInfo[num3];
					WorldDamage.Instance.ApplyDamage(damage_info);
				}
				for (int num4 = 0; num4 < ptr->numRemovedMassEntries; num4++)
				{
					ElementConsumer.AddMass(ptr->removedMassEntries[num4]);
				}
				int numMassConsumedCallbacks = ptr->numMassConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle2 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle);
				for (int num5 = 0; num5 < numMassConsumedCallbacks; num5++)
				{
					Sim.MassConsumedCallback massConsumedCallback = ptr->massConsumedCallbacks[num5];
					handle2.index = massConsumedCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.MassConsumedCallback> complexCallbackInfo = this.massConsumedCallbackManager.Release(handle2, "massConsumedCB");
					if (complexCallbackInfo.cb != null)
					{
						complexCallbackInfo.cb(massConsumedCallback, complexCallbackInfo.callbackData);
					}
				}
				int numMassEmittedCallbacks = ptr->numMassEmittedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle handle3 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle);
				for (int num6 = 0; num6 < numMassEmittedCallbacks; num6++)
				{
					Sim.MassEmittedCallback massEmittedCallback = ptr->massEmittedCallbacks[num6];
					handle3.index = massEmittedCallback.callbackIdx;
					if (this.massEmitCallbackManager.IsVersionValid(handle3))
					{
						Game.ComplexCallbackInfo<Sim.MassEmittedCallback> item = this.massEmitCallbackManager.GetItem(handle3);
						if (item.cb != null)
						{
							item.cb(massEmittedCallback, item.callbackData);
						}
					}
				}
				int numDiseaseConsumptionCallbacks = ptr->numDiseaseConsumptionCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle handle4 = default(HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle);
				for (int num7 = 0; num7 < numDiseaseConsumptionCallbacks; num7++)
				{
					Sim.DiseaseConsumptionCallback diseaseConsumptionCallback = ptr->diseaseConsumptionCallbacks[num7];
					handle4.index = diseaseConsumptionCallback.callbackIdx;
					if (this.diseaseConsumptionCallbackManager.IsVersionValid(handle4))
					{
						Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback> item2 = this.diseaseConsumptionCallbackManager.GetItem(handle4);
						if (item2.cb != null)
						{
							item2.cb(diseaseConsumptionCallback, item2.callbackData);
						}
					}
				}
				int numComponentStateChangedMessages = ptr->numComponentStateChangedMessages;
				HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle5 = default(HandleVector<Game.ComplexCallbackInfo<int>>.Handle);
				for (int num8 = 0; num8 < numComponentStateChangedMessages; num8++)
				{
					Sim.ComponentStateChangedMessage componentStateChangedMessage = ptr->componentStateChangedMessages[num8];
					handle5.index = componentStateChangedMessage.callbackIdx;
					if (this.simComponentCallbackManager.IsVersionValid(handle5))
					{
						Game.ComplexCallbackInfo<int> complexCallbackInfo2 = this.simComponentCallbackManager.Release(handle5, "component state changed cb");
						if (complexCallbackInfo2.cb != null)
						{
							complexCallbackInfo2.cb(componentStateChangedMessage.simHandle, complexCallbackInfo2.callbackData);
						}
					}
				}
				int numRadiationConsumedCallbacks = ptr->numRadiationConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle handle6 = default(HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle);
				for (int num9 = 0; num9 < numRadiationConsumedCallbacks; num9++)
				{
					Sim.ConsumedRadiationCallback consumedRadiationCallback = ptr->radiationConsumedCallbacks[num9];
					handle6.index = consumedRadiationCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback> complexCallbackInfo3 = this.radiationConsumedCallbackManager.Release(handle6, "radiationConsumedCB");
					if (complexCallbackInfo3.cb != null)
					{
						complexCallbackInfo3.cb(consumedRadiationCallback, complexCallbackInfo3.callbackData);
					}
				}
				int numElementChunkMeltedInfos = ptr->numElementChunkMeltedInfos;
				for (int num10 = 0; num10 < numElementChunkMeltedInfos; num10++)
				{
					SimTemperatureTransfer.DoOreMeltTransition(ptr->elementChunkMeltedInfos[num10].handle);
				}
				int numBuildingOverheatInfos = ptr->numBuildingOverheatInfos;
				for (int num11 = 0; num11 < numBuildingOverheatInfos; num11++)
				{
					StructureTemperatureComponents.DoOverheat(ptr->buildingOverheatInfos[num11].handle);
				}
				int numBuildingNoLongerOverheatedInfos = ptr->numBuildingNoLongerOverheatedInfos;
				for (int num12 = 0; num12 < numBuildingNoLongerOverheatedInfos; num12++)
				{
					StructureTemperatureComponents.DoNoLongerOverheated(ptr->buildingNoLongerOverheatedInfos[num12].handle);
				}
				int numBuildingMeltedInfos = ptr->numBuildingMeltedInfos;
				for (int num13 = 0; num13 < numBuildingMeltedInfos; num13++)
				{
					StructureTemperatureComponents.DoStateTransition(ptr->buildingMeltedInfos[num13].handle);
				}
				int numCellMeltedInfos = ptr->numCellMeltedInfos;
				for (int num14 = 0; num14 < numCellMeltedInfos; num14++)
				{
					int gameCell = ptr->cellMeltedInfos[num14].gameCell;
					GameObject gameObject = Grid.Objects[gameCell, 9];
					if (gameObject != null)
					{
						global::Util.KDestroyGameObject(gameObject);
					}
				}
				if (dt > 0f)
				{
					this.conduitTemperatureManager.Sim200ms(0.2f);
					this.conduitDiseaseManager.Sim200ms(0.2f);
					this.gasConduitFlow.Sim200ms(0.2f);
					this.liquidConduitFlow.Sim200ms(0.2f);
					this.solidConduitFlow.Sim200ms(0.2f);
					this.accumulators.Sim200ms(0.2f);
					this.plantElementAbsorbers.Sim200ms(0.2f);
				}
				Sim.DebugProperties debugProperties;
				debugProperties.buildingTemperatureScale = 100f;
				debugProperties.buildingToBuildingTemperatureScale = 0.001f;
				debugProperties.biomeTemperatureLerpRate = 0.001f;
				debugProperties.isDebugEditing = ((DebugPaintElementScreen.Instance != null && DebugPaintElementScreen.Instance.gameObject.activeSelf) ? 1 : 0);
				debugProperties.pad0 = (debugProperties.pad1 = (debugProperties.pad2 = 0));
				SimMessages.SetDebugProperties(debugProperties);
				if (dt > 0f)
				{
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msFirst(dt);
					}
					if (this.energySim != null)
					{
						this.energySim.EnergySim200ms(dt);
					}
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msLast(dt);
					}
				}
				result = ptr;
			}
		}
		return result;
	}

	// Token: 0x06003784 RID: 14212 RVA: 0x0012D2D0 File Offset: 0x0012B4D0
	public void AddSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Add(cell);
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x0012D2DF File Offset: 0x0012B4DF
	public void RemoveSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Remove(cell);
	}

	// Token: 0x06003786 RID: 14214 RVA: 0x0012D2EE File Offset: 0x0012B4EE
	public void SetIsLoading()
	{
		this.isLoading = true;
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x0012D2F7 File Offset: 0x0012B4F7
	public bool IsLoading()
	{
		return this.isLoading;
	}

	// Token: 0x06003788 RID: 14216 RVA: 0x0012D300 File Offset: 0x0012B500
	private void ShowDebugCellInfo()
	{
		int mouseCell = DebugHandler.GetMouseCell();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(mouseCell, out num, out num2);
		string text = string.Concat(new string[]
		{
			mouseCell.ToString(),
			" (",
			num.ToString(),
			", ",
			num2.ToString(),
			")"
		});
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x06003789 RID: 14217 RVA: 0x0012D37B File Offset: 0x0012B57B
	public void ForceSimStep()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Force-stepping the sim"
		});
		this.simDt = 0.2f;
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x0012D39C File Offset: 0x0012B59C
	private void Update()
	{
		if (this.isLoading)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (global::Debug.developerConsoleVisible)
		{
			global::Debug.developerConsoleVisible = false;
		}
		if (DebugHandler.DebugCellInfo)
		{
			this.ShowDebugCellInfo();
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		this.circuitManager.RenderEveryTick(deltaTime);
		this.logicCircuitManager.RenderEveryTick(deltaTime);
		this.solidConduitFlow.RenderEveryTick(deltaTime);
		Pathfinding.Instance.RenderEveryTick();
		Singleton<CellChangeMonitor>.Instance.RenderEveryTick();
		this.SimEveryTick(deltaTime);
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x0012D434 File Offset: 0x0012B634
	private void SimEveryTick(float dt)
	{
		dt = Mathf.Min(dt, 0.2f);
		this.simDt += dt;
		if (this.simDt >= 0.016666668f)
		{
			do
			{
				this.simSubTick++;
				this.simSubTick %= 12;
				if (this.simSubTick == 0)
				{
					this.hasFirstSimTickRun = true;
					this.UnsafeSim200ms(0.2f);
				}
				if (this.hasFirstSimTickRun)
				{
					Singleton<StateMachineUpdater>.Instance.AdvanceOneSimSubTick();
				}
				this.simDt -= 0.016666668f;
			}
			while (this.simDt >= 0.016666668f);
			return;
		}
		this.UnsafeSim200ms(0f);
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x0012D4E0 File Offset: 0x0012B6E0
	private unsafe void UnsafeSim200ms(float dt)
	{
		this.simActiveRegions.Clear();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered)
			{
				Game.SimActiveRegion simActiveRegion = new Game.SimActiveRegion();
				simActiveRegion.region = new Pair<Vector2I, Vector2I>(worldContainer.WorldOffset, worldContainer.WorldOffset + worldContainer.WorldSize);
				simActiveRegion.currentSunlightIntensity = worldContainer.currentSunlightIntensity;
				simActiveRegion.currentCosmicRadiationIntensity = worldContainer.currentCosmicIntensity;
				this.simActiveRegions.Add(simActiveRegion);
			}
		}
		global::Debug.Assert(this.simActiveRegions.Count > 0, "Cannot send a frame to the sim with zero active regions");
		SimMessages.NewGameFrame(dt, this.simActiveRegions);
		Sim.GameDataUpdate* ptr = this.StepTheSim(dt);
		if (ptr == null)
		{
			global::Debug.LogError("UNEXPECTED!");
			return;
		}
		if (ptr->numFramesProcessed <= 0)
		{
			return;
		}
		this.gameSolidInfo.AddRange(this.solidInfo);
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, ptr->numSolidSubstanceChangeInfo, ptr->solidSubstanceChangeInfo, ptr->numLiquidChangeInfo, ptr->liquidChangeInfo);
		this.gameSolidInfo.Clear();
		this.solidInfo.Clear();
		this.callbackInfo.Clear();
		this.callbackManagerManuallyReleasedHandles.Clear();
		Pathfinding.Instance.UpdateNavGrids(false);
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x0012D648 File Offset: 0x0012B848
	private void LateUpdateComponents()
	{
		this.UpdateOverlayScreen();
	}

	// Token: 0x0600378E RID: 14222 RVA: 0x0012D650 File Offset: 0x0012B850
	private void OnAddBuildingCellVisualizer(BuildingCellVisualizer building_cell_visualizer)
	{
		this.lastDrawnOverlayMode = default(HashedString);
		if (PlayerController.Instance != null)
		{
			BuildTool buildTool = PlayerController.Instance.ActiveTool as BuildTool;
			if (buildTool != null && buildTool.visualizer == building_cell_visualizer.gameObject)
			{
				this.previewVisualizer = building_cell_visualizer;
			}
		}
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x0012D6A9 File Offset: 0x0012B8A9
	private void OnRemoveBuildingCellVisualizer(BuildingCellVisualizer building_cell_visualizer)
	{
		if (this.previewVisualizer == building_cell_visualizer)
		{
			this.previewVisualizer = null;
		}
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x0012D6C0 File Offset: 0x0012B8C0
	private void UpdateOverlayScreen()
	{
		if (OverlayScreen.Instance == null)
		{
			return;
		}
		HashedString mode = OverlayScreen.Instance.GetMode();
		if (this.previewVisualizer != null)
		{
			this.previewVisualizer.DisableIcons();
			this.previewVisualizer.DrawIcons(mode);
		}
		if (mode == this.lastDrawnOverlayMode)
		{
			return;
		}
		foreach (BuildingCellVisualizer buildingCellVisualizer in Components.BuildingCellVisualizers.Items)
		{
			buildingCellVisualizer.DisableIcons();
			buildingCellVisualizer.DrawIcons(mode);
		}
		this.lastDrawnOverlayMode = mode;
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x0012D770 File Offset: 0x0012B970
	public void ForceOverlayUpdate(bool clearLastMode = false)
	{
		this.previousOverlayMode = OverlayModes.None.ID;
		if (clearLastMode)
		{
			this.lastDrawnOverlayMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x06003792 RID: 14226 RVA: 0x0012D78C File Offset: 0x0012B98C
	private void LateUpdate()
	{
		if (Time.timeScale == 0f && !this.IsPaused)
		{
			this.IsPaused = true;
			base.Trigger(-1788536802, this.IsPaused);
		}
		else if (Time.timeScale != 0f && this.IsPaused)
		{
			this.IsPaused = false;
			base.Trigger(-1788536802, this.IsPaused);
		}
		if (Input.GetMouseButton(0))
		{
			this.VisualTunerElement = null;
			int mouseCell = DebugHandler.GetMouseCell();
			if (Grid.IsValidCell(mouseCell))
			{
				Element visualTunerElement = Grid.Element[mouseCell];
				this.VisualTunerElement = visualTunerElement;
			}
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		HashedString mode = SimDebugView.Instance.GetMode();
		if (mode != this.previousOverlayMode)
		{
			this.previousOverlayMode = mode;
			if (mode == OverlayModes.LiquidConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(true, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.GasConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(true, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.SolidConveyor.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(true, true);
			}
			else
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, false);
				this.gasFlowVisualizer.ColourizePipeContents(false, false);
				this.solidFlowVisualizer.ColourizePipeContents(false, false);
			}
		}
		this.gasFlowVisualizer.Render(this.gasFlowPos.z, 0, this.gasConduitFlow.ContinuousLerpPercent, mode == OverlayModes.GasConduits.ID && this.gasConduitFlow.DiscreteLerpPercent != this.previousGasConduitFlowDiscreteLerpPercent);
		this.liquidFlowVisualizer.Render(this.liquidFlowPos.z, 0, this.liquidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.LiquidConduits.ID && this.liquidConduitFlow.DiscreteLerpPercent != this.previousLiquidConduitFlowDiscreteLerpPercent);
		this.solidFlowVisualizer.Render(this.solidFlowPos.z, 0, this.solidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.SolidConveyor.ID && this.solidConduitFlow.DiscreteLerpPercent != this.previousSolidConduitFlowDiscreteLerpPercent);
		this.previousGasConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.GasConduits.ID) ? this.gasConduitFlow.DiscreteLerpPercent : -1f);
		this.previousLiquidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.LiquidConduits.ID) ? this.liquidConduitFlow.DiscreteLerpPercent : -1f);
		this.previousSolidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.SolidConveyor.ID) ? this.solidConduitFlow.DiscreteLerpPercent : -1f);
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Shader.SetGlobalVector("_WsToCs", new Vector4(vector.x / (float)Grid.WidthInCells, vector.y / (float)Grid.HeightInCells, (vector2.x - vector.x) / (float)Grid.WidthInCells, (vector2.y - vector.y) / (float)Grid.HeightInCells));
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 value = new Vector4((vector.x - (float)worldOffset.x) / (float)worldSize.x, (vector.y - (float)worldOffset.y) / (float)worldSize.y, (vector2.x - vector.x) / (float)worldSize.x, (vector2.y - vector.y) / (float)worldSize.y);
		Shader.SetGlobalVector("_WsToCcs", value);
		if (this.drawStatusItems)
		{
			this.statusItemRenderer.RenderEveryTick();
			this.prioritizableRenderer.RenderEveryTick();
		}
		this.LateUpdateComponents();
		Singleton<StateMachineUpdater>.Instance.Render(Time.unscaledDeltaTime);
		Singleton<StateMachineUpdater>.Instance.RenderEveryTick(Time.unscaledDeltaTime);
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
			if (component != null)
			{
				component.DrawPath();
			}
		}
		KFMOD.RenderEveryTick(Time.deltaTime);
		if (GenericGameSettings.instance.performanceCapture.waitTime != 0f)
		{
			this.UpdatePerformanceCapture();
		}
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x0012DC60 File Offset: 0x0012BE60
	private void UpdatePerformanceCapture()
	{
		if (this.IsPaused && SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (Time.timeSinceLevelLoad < GenericGameSettings.instance.performanceCapture.waitTime)
		{
			return;
		}
		uint num = 577063U;
		string text = System.DateTime.Now.ToShortDateString();
		string text2 = System.DateTime.Now.ToShortTimeString();
		string fileName = Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
		string text3 = "Version,Date,Time,SaveGame";
		string text4 = string.Format("{0},{1},{2},{3}", new object[]
		{
			num,
			text,
			text2,
			fileName
		});
		float num2 = 0.1f;
		if (GenericGameSettings.instance.performanceCapture.gcStats)
		{
			global::Debug.Log("Begin GC profiling...");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			GC.Collect();
			num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
			global::Debug.Log("\tGC.Collect() took " + num2.ToString() + " seconds");
			MemorySnapshot memorySnapshot = new MemorySnapshot();
			string format = "{0},{1},{2},{3}";
			string path = "./memory/GCTypeMetrics.csv";
			if (!File.Exists(path))
			{
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.WriteLine(string.Format(format, new object[]
					{
						text3,
						"Type",
						"Instances",
						"References"
					}));
				}
			}
			using (StreamWriter streamWriter2 = new StreamWriter(path, true))
			{
				foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
				{
					streamWriter2.WriteLine(string.Format(format, new object[]
					{
						text4,
						"\"" + typeData.type.ToString() + "\"",
						typeData.instanceCount,
						typeData.refCount
					}));
				}
			}
			global::Debug.Log("...end GC profiling");
		}
		float fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
		Directory.CreateDirectory("./memory");
		string format2 = "{0},{1},{2}";
		string path2 = "./memory/GeneralMetrics.csv";
		if (!File.Exists(path2))
		{
			using (StreamWriter streamWriter3 = new StreamWriter(path2))
			{
				streamWriter3.WriteLine(string.Format(format2, text3, "GCDuration", "FPS"));
			}
		}
		using (StreamWriter streamWriter4 = new StreamWriter(path2, true))
		{
			streamWriter4.WriteLine(string.Format(format2, text4, num2, fps));
		}
		GenericGameSettings.instance.performanceCapture.waitTime = 0f;
		App.Quit();
	}

	// Token: 0x06003794 RID: 14228 RVA: 0x0012DF68 File Offset: 0x0012C168
	public void Reset(GameSpawnData gsd, Vector2I world_offset)
	{
		using (new KProfiler.Region("World.Reset", null))
		{
			if (gsd != null)
			{
				foreach (KeyValuePair<Vector2I, bool> keyValuePair in gsd.preventFoWReveal)
				{
					if (keyValuePair.Value)
					{
						Vector2I v = new Vector2I(keyValuePair.Key.X + world_offset.X, keyValuePair.Key.Y + world_offset.Y);
						Grid.PreventFogOfWarReveal[Grid.PosToCell(v)] = keyValuePair.Value;
					}
				}
			}
		}
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x0012E040 File Offset: 0x0012C240
	private void OnApplicationQuit()
	{
		Game.quitting = true;
		Sim.Shutdown();
		AudioMixer.Destroy();
		if (this.screenMgr != null && this.screenMgr.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.screenMgr.gameObject);
		}
		Console.WriteLine("Game.OnApplicationQuit()");
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x0012E098 File Offset: 0x0012C298
	private void InitializeFXSpawners()
	{
		for (int i = 0; i < this.fxSpawnData.Length; i++)
		{
			int fx_idx = i;
			this.fxSpawnData[fx_idx].fxPrefab.SetActive(false);
			ushort fx_mask = (ushort)(1 << fx_idx);
			Action<SpawnFXHashes, GameObject> destroyer = delegate(SpawnFXHashes fxid, GameObject go)
			{
				if (!Game.IsQuitting())
				{
					int num = Grid.PosToCell(go);
					ushort[] array = this.activeFX;
					int num2 = num;
					array[num2] &= ~fx_mask;
					go.GetComponent<KAnimControllerBase>().enabled = false;
					this.fxPools[(int)fxid].ReleaseInstance(go);
				}
			};
			Func<GameObject> instantiator = delegate()
			{
				GameObject gameObject = GameUtil.KInstantiate(this.fxSpawnData[fx_idx].fxPrefab, Grid.SceneLayer.Front, null, 0);
				KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				gameObject.SetActive(true);
				component.onDestroySelf = delegate(GameObject go)
				{
					destroyer(this.fxSpawnData[fx_idx].id, go);
				};
				return gameObject;
			};
			GameObjectPool pool = new GameObjectPool(instantiator, this.fxSpawnData[fx_idx].initialCount);
			this.fxPools[(int)this.fxSpawnData[fx_idx].id] = pool;
			this.fxSpawner[(int)this.fxSpawnData[fx_idx].id] = delegate(Vector3 pos, float rotation)
			{
				GameScheduler.Instance.Schedule("SpawnFX", 0f, delegate(object obj)
				{
					int num = Grid.PosToCell(pos);
					if ((this.activeFX[num] & fx_mask) == 0)
					{
						ushort[] array = this.activeFX;
						int num2 = num;
						array[num2] |= fx_mask;
						GameObject instance = pool.GetInstance();
						Game.SpawnPoolData spawnPoolData = this.fxSpawnData[fx_idx];
						Quaternion rotation = Quaternion.identity;
						bool flipX = false;
						string s = spawnPoolData.initialAnim;
						Game.SpawnRotationConfig rotationConfig = spawnPoolData.rotationConfig;
						if (rotationConfig != Game.SpawnRotationConfig.Normal)
						{
							if (rotationConfig == Game.SpawnRotationConfig.StringName)
							{
								int num3 = (int)(rotation / 90f);
								if (num3 < 0)
								{
									num3 += spawnPoolData.rotationData.Length;
								}
								s = spawnPoolData.rotationData[num3].animName;
								flipX = spawnPoolData.rotationData[num3].flip;
							}
						}
						else
						{
							rotation = Quaternion.Euler(0f, 0f, rotation);
						}
						pos += spawnPoolData.spawnOffset;
						Vector2 vector = UnityEngine.Random.insideUnitCircle;
						vector.x *= spawnPoolData.spawnRandomOffset.x;
						vector.y *= spawnPoolData.spawnRandomOffset.y;
						vector = rotation * vector;
						pos.x += vector.x;
						pos.y += vector.y;
						instance.transform.SetPosition(pos);
						instance.transform.rotation = rotation;
						KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
						component.FlipX = flipX;
						component.TintColour = spawnPoolData.colour;
						component.Play(s, KAnim.PlayMode.Once, 1f, 0f);
						component.enabled = true;
					}
				}, null, null);
			};
		}
	}

	// Token: 0x06003797 RID: 14231 RVA: 0x0012E198 File Offset: 0x0012C398
	public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation)
	{
		Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
		if (CameraController.Instance.IsVisiblePos(vector))
		{
			this.fxSpawner[(int)fx_id](vector, rotation);
		}
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x0012E1CE File Offset: 0x0012C3CE
	public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation)
	{
		this.fxSpawner[(int)fx_id](pos, rotation);
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x0012E1E3 File Offset: 0x0012C3E3
	public static void SaveSettings(BinaryWriter writer)
	{
		Serializer.Serialize(new Game.Settings(Game.Instance), writer);
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x0012E1F8 File Offset: 0x0012C3F8
	public static void LoadSettings(Deserializer deserializer)
	{
		Game.Settings settings = new Game.Settings();
		deserializer.Deserialize(settings);
		KPlayerPrefs.SetInt(Game.NextUniqueIDKey, settings.nextUniqueID);
		KleiMetrics.SetGameID(settings.gameID);
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0012E230 File Offset: 0x0012C430
	public void Save(BinaryWriter writer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = SaveLoader.Instance.clusterDetailSave;
		gameSaveData.debugWasUsed = this.debugWasUsed;
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		gameSaveData.autoPrioritizeRoles = this.autoPrioritizeRoles;
		gameSaveData.advancedPersonalPriorities = this.advancedPersonalPriorities;
		gameSaveData.savedInfo = this.savedInfo;
		global::Debug.Assert(gameSaveData.worldDetail != null, "World detail null");
		gameSaveData.dateGenerated = this.dateGenerated;
		if (!this.changelistsPlayedOn.Contains(577063U))
		{
			this.changelistsPlayedOn.Add(577063U);
		}
		gameSaveData.changelistsPlayedOn = this.changelistsPlayedOn;
		if (this.OnSave != null)
		{
			this.OnSave(gameSaveData);
		}
		Serializer.Serialize(gameSaveData, writer);
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0012E34C File Offset: 0x0012C54C
	public void Load(Deserializer deserializer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = new WorldDetailSave();
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		deserializer.Deserialize(gameSaveData);
		this.gasConduitFlow = gameSaveData.gasConduitFlow;
		this.liquidConduitFlow = gameSaveData.liquidConduitFlow;
		this.debugWasUsed = gameSaveData.debugWasUsed;
		this.autoPrioritizeRoles = gameSaveData.autoPrioritizeRoles;
		this.advancedPersonalPriorities = gameSaveData.advancedPersonalPriorities;
		this.dateGenerated = gameSaveData.dateGenerated;
		this.changelistsPlayedOn = (gameSaveData.changelistsPlayedOn ?? new List<uint>());
		if (gameSaveData.dateGenerated.IsNullOrWhiteSpace())
		{
			this.dateGenerated = "Before U41 (Feb 2022)";
		}
		DebugUtil.LogArgs(new object[]
		{
			"SAVEINFO"
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Generated: " + this.dateGenerated
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Played on: " + string.Join<uint>(", ", this.changelistsPlayedOn)
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Debug was used: " + Game.Instance.debugWasUsed.ToString()
		});
		this.savedInfo = gameSaveData.savedInfo;
		this.savedInfo.InitializeEmptyVariables();
		CustomGameSettings.Instance.Print();
		KCrashReporter.debugWasUsed = this.debugWasUsed;
		SaveLoader.Instance.SetWorldDetail(gameSaveData.worldDetail);
		if (this.OnLoad != null)
		{
			this.OnLoad(gameSaveData);
		}
	}

	// Token: 0x0600379D RID: 14237 RVA: 0x0012E517 File Offset: 0x0012C717
	public void SetAutoSaveCallbacks(Game.SavingPreCB activatePreCB, Game.SavingActiveCB activateActiveCB, Game.SavingPostCB activatePostCB)
	{
		this.activatePreCB = activatePreCB;
		this.activateActiveCB = activateActiveCB;
		this.activatePostCB = activatePostCB;
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x0012E52E File Offset: 0x0012C72E
	public void StartDelayedInitialSave()
	{
		base.StartCoroutine(this.DelayedInitialSave());
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x0012E53D File Offset: 0x0012C73D
	private IEnumerator DelayedInitialSave()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (GenericGameSettings.instance.devAutoWorldGenActive)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.SetDiscovered(true);
			}
			SaveGame.Instance.worldGenSpawner.SpawnEverything();
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().DEBUG_REVEAL_ENTIRE_MAP();
			if (CameraController.Instance != null)
			{
				CameraController.Instance.EnableFreeCamera(true);
			}
			for (int num2 = 0; num2 != Grid.WidthInCells * Grid.HeightInCells; num2++)
			{
				Grid.Reveal(num2, byte.MaxValue, false);
			}
			GenericGameSettings.instance.devAutoWorldGenActive = false;
		}
		SaveLoader.Instance.InitialSave();
		yield break;
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x0012E548 File Offset: 0x0012C748
	public void StartDelayedSave(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		if (this.activatePreCB != null)
		{
			this.activatePreCB(delegate
			{
				this.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
			});
			return;
		}
		base.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x0012E5B6 File Offset: 0x0012C7B6
	private IEnumerator DelayedSave(string filename, bool isAutoSave, bool updateSavePointer)
	{
		while (PlayerController.Instance.IsDragging())
		{
			yield return null;
		}
		PlayerController.Instance.CancelDragging();
		PlayerController.Instance.AllowDragging(false);
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (this.activateActiveCB != null)
		{
			this.activateActiveCB();
			for (int i = 0; i < 1; i = num)
			{
				yield return null;
				num = i + 1;
			}
		}
		SaveLoader.Instance.Save(filename, isAutoSave, updateSavePointer);
		if (this.activatePostCB != null)
		{
			this.activatePostCB();
		}
		for (int i = 0; i < 5; i = num)
		{
			yield return null;
			num = i + 1;
		}
		PlayerController.Instance.AllowDragging(true);
		yield break;
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x0012E5DA File Offset: 0x0012C7DA
	public void StartDelayed(int tick_delay, System.Action action)
	{
		base.StartCoroutine(this.DelayedExecutor(tick_delay, action));
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x0012E5EB File Offset: 0x0012C7EB
	private IEnumerator DelayedExecutor(int tick_delay, System.Action action)
	{
		int num;
		for (int i = 0; i < tick_delay; i = num)
		{
			yield return null;
			num = i + 1;
		}
		action();
		yield break;
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x0012E604 File Offset: 0x0012C804
	private void LoadEventHashes()
	{
		foreach (object obj in Enum.GetValues(typeof(GameHashes)))
		{
			GameHashes hash = (GameHashes)obj;
			HashCache.Get().Add((int)hash, hash.ToString());
		}
		foreach (object obj2 in Enum.GetValues(typeof(UtilHashes)))
		{
			UtilHashes hash2 = (UtilHashes)obj2;
			HashCache.Get().Add((int)hash2, hash2.ToString());
		}
		foreach (object obj3 in Enum.GetValues(typeof(UIHashes)))
		{
			UIHashes hash3 = (UIHashes)obj3;
			HashCache.Get().Add((int)hash3, hash3.ToString());
		}
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x0012E740 File Offset: 0x0012C940
	public void StopFE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = false;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		}
		MainMenu.Instance.StopMainMenuMusic();
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x0012E7A8 File Offset: 0x0012C9A8
	public void StartBE()
	{
		Resources.UnloadUnusedAssets();
		if (TimeOfDay.Instance != null && !MusicManager.instance.SongIsPlaying("Stinger_Loop_Night") && TimeOfDay.Instance.GetCurrentTimeRegion() == TimeOfDay.TimeRegion.Night)
		{
			MusicManager.instance.PlaySong("Stinger_Loop_Night", false);
			MusicManager.instance.SetSongParameter("Stinger_Loop_Night", "Music_PlayStinger", 0f, true);
		}
		AudioMixer.instance.Reset();
		AudioMixer.instance.StartPersistentSnapshots();
		if (MusicManager.instance.ShouldPlayDynamicMusicLoadedGame())
		{
			MusicManager.instance.PlayDynamicMusic();
		}
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x0012E83C File Offset: 0x0012CA3C
	public void StopBE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = true;
		}
		LoopingSoundManager loopingSoundManager = LoopingSoundManager.Get();
		if (loopingSoundManager != null)
		{
			loopingSoundManager.StopAllSounds();
		}
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.StopPersistentSnapshots();
		foreach (List<SaveLoadRoot> list in SaveLoader.Instance.saveManager.GetLists().Values)
		{
			foreach (SaveLoadRoot saveLoadRoot in list)
			{
				if (saveLoadRoot.gameObject != null)
				{
					global::Util.KDestroyGameObject(saveLoadRoot.gameObject);
				}
			}
		}
		base.GetComponent<EntombedItemVisualizer>().Clear();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		KComponentSpawn.instance.comps.Clear();
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.cameraController);
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.playerController);
		Sim.Shutdown();
		SimAndRenderScheduler.instance.Reset();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x0012E98C File Offset: 0x0012CB8C
	public void SetStatusItemOffset(Transform transform, Vector3 offset)
	{
		this.statusItemRenderer.SetOffset(transform, offset);
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x0012E99B File Offset: 0x0012CB9B
	public void AddStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Add(transform, status_item);
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x0012E9AA File Offset: 0x0012CBAA
	public void RemoveStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Remove(transform, status_item);
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x060037AB RID: 14251 RVA: 0x0012E9B9 File Offset: 0x0012CBB9
	public float LastTimeWorkStarted
	{
		get
		{
			return this.lastTimeWorkStarted;
		}
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x0012E9C1 File Offset: 0x0012CBC1
	public void StartedWork()
	{
		this.lastTimeWorkStarted = Time.time;
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x0012E9CE File Offset: 0x0012CBCE
	private void SpawnOxygenBubbles(Vector3 position, float angle)
	{
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x0012E9D0 File Offset: 0x0012CBD0
	public void ManualReleaseHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.callbackManagerManuallyReleasedHandles.Add(handle.index);
		this.callbackManager.Release(handle);
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x0012E9FB File Offset: 0x0012CBFB
	private bool IsManuallyReleasedHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		return !this.callbackManager.IsVersionValid(handle) && this.callbackManagerManuallyReleasedHandles.Contains(handle.index);
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x0012EA22 File Offset: 0x0012CC22
	[ContextMenu("Print")]
	private void Print()
	{
		Console.WriteLine("This is a console writeline test");
		global::Debug.Log("This is a debug log test");
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x0012EA38 File Offset: 0x0012CC38
	private void DestroyInstances()
	{
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		Db.Get().ResetProblematicDbs();
		GridSettings.ClearGrid();
		StateMachineManager.ResetParameters();
		ChoreTable.Instance.ResetParameters();
		BubbleManager.DestroyInstance();
		AmbientSoundManager.Destroy();
		AutoDisinfectableManager.DestroyInstance();
		BuildMenu.DestroyInstance();
		CancelTool.DestroyInstance();
		ClearTool.DestroyInstance();
		ChoreGroupManager.DestroyInstance();
		CO2Manager.DestroyInstance();
		ConsumerManager.DestroyInstance();
		CopySettingsTool.DestroyInstance();
		global::DateTime.DestroyInstance();
		DebugBaseTemplateButton.DestroyInstance();
		DebugPaintElementScreen.DestroyInstance();
		DetailsScreen.DestroyInstance();
		DietManager.DestroyInstance();
		DebugText.DestroyInstance();
		FactionManager.DestroyInstance();
		EmptyPipeTool.DestroyInstance();
		FetchListStatusItemUpdater.DestroyInstance();
		FishOvercrowingManager.DestroyInstance();
		FallingWater.DestroyInstance();
		GridCompositor.DestroyInstance();
		Infrared.DestroyInstance();
		KPrefabIDTracker.DestroyInstance();
		ManagementMenu.DestroyInstance();
		ClusterMapScreen.DestroyInstance();
		Messenger.DestroyInstance();
		LoopingSoundManager.DestroyInstance();
		MeterScreen.DestroyInstance();
		MinionGroupProber.DestroyInstance();
		NavPathDrawer.DestroyInstance();
		MinionIdentity.DestroyStatics();
		PathFinder.DestroyStatics();
		Pathfinding.DestroyInstance();
		PrebuildTool.DestroyInstance();
		PrioritizeTool.DestroyInstance();
		SelectTool.DestroyInstance();
		PopFXManager.DestroyInstance();
		ProgressBarsConfig.DestroyInstance();
		PropertyTextures.DestroyInstance();
		RationTracker.DestroyInstance();
		ReportManager.DestroyInstance();
		Research.DestroyInstance();
		RootMenu.DestroyInstance();
		SaveLoader.DestroyInstance();
		Scenario.DestroyInstance();
		SimDebugView.DestroyInstance();
		SpriteSheetAnimManager.DestroyInstance();
		ScheduleManager.DestroyInstance();
		Sounds.DestroyInstance();
		ToolMenu.DestroyInstance();
		WorldDamage.DestroyInstance();
		WaterCubes.DestroyInstance();
		WireBuildTool.DestroyInstance();
		VisibilityTester.DestroyInstance();
		Traces.DestroyInstance();
		TopLeftControlScreen.DestroyInstance();
		UtilityBuildTool.DestroyInstance();
		ReportScreen.DestroyInstance();
		ChorePreconditions.DestroyInstance();
		SandboxBrushTool.DestroyInstance();
		SandboxHeatTool.DestroyInstance();
		SandboxStressTool.DestroyInstance();
		SandboxCritterTool.DestroyInstance();
		SandboxClearFloorTool.DestroyInstance();
		GameScreenManager.DestroyInstance();
		GameScheduler.DestroyInstance();
		NavigationReservations.DestroyInstance();
		Tutorial.DestroyInstance();
		CameraController.DestroyInstance();
		CellEventLogger.DestroyInstance();
		GameFlowManager.DestroyInstance();
		Immigration.DestroyInstance();
		BuildTool.DestroyInstance();
		DebugTool.DestroyInstance();
		DeconstructTool.DestroyInstance();
		DisconnectTool.DestroyInstance();
		DigTool.DestroyInstance();
		DisinfectTool.DestroyInstance();
		HarvestTool.DestroyInstance();
		MopTool.DestroyInstance();
		MoveToLocationTool.DestroyInstance();
		PlaceTool.DestroyInstance();
		SpacecraftManager.DestroyInstance();
		GameplayEventManager.DestroyInstance();
		BuildingInventory.DestroyInstance();
		PlantSubSpeciesCatalog.DestroyInstance();
		SandboxDestroyerTool.DestroyInstance();
		SandboxFOWTool.DestroyInstance();
		SandboxFloodTool.DestroyInstance();
		SandboxSprinkleTool.DestroyInstance();
		StampTool.DestroyInstance();
		OnDemandUpdater.DestroyInstance();
		HoverTextScreen.DestroyInstance();
		ImmigrantScreen.DestroyInstance();
		OverlayMenu.DestroyInstance();
		NameDisplayScreen.DestroyInstance();
		PlanScreen.DestroyInstance();
		ResourceCategoryScreen.DestroyInstance();
		ResourceRemainingDisplayScreen.DestroyInstance();
		SandboxToolParameterMenu.DestroyInstance();
		SpeedControlScreen.DestroyInstance();
		Vignette.DestroyInstance();
		PlayerController.DestroyInstance();
		NotificationScreen.DestroyInstance();
		BuildingCellVisualizerResources.DestroyInstance();
		PauseScreen.DestroyInstance();
		SaveLoadRoot.DestroyStatics();
		KTime.DestroyInstance();
		DemoTimer.DestroyInstance();
		UIScheduler.DestroyInstance();
		SaveGame.DestroyInstance();
		GameClock.DestroyInstance();
		TimeOfDay.DestroyInstance();
		DeserializeWarnings.DestroyInstance();
		UISounds.DestroyInstance();
		RenderTextureDestroyer.DestroyInstance();
		HoverTextHelper.DestroyStatics();
		LoadScreen.DestroyInstance();
		LoadingOverlay.DestroyInstance();
		SimAndRenderScheduler.DestroyInstance();
		Singleton<CellChangeMonitor>.DestroyInstance();
		Singleton<StateMachineManager>.Instance.Clear();
		Singleton<StateMachineUpdater>.Instance.Clear();
		UpdateObjectCountParameter.Clear();
		MaterialSelectionPanel.ClearStatics();
		StarmapScreen.DestroyInstance();
		ClusterNameDisplayScreen.DestroyInstance();
		ClusterManager.DestroyInstance();
		ClusterGrid.DestroyInstance();
		PathFinderQueries.Reset();
		KBatchedAnimUpdater instance = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance != null)
		{
			instance.InitializeGrid();
		}
		GlobalChoreProvider.DestroyInstance();
		WorldSelector.DestroyInstance();
		ColonyDiagnosticUtility.DestroyInstance();
		DiscoveredResources.DestroyInstance();
		ClusterMapSelectTool.DestroyInstance();
		StoryManager.DestroyInstance();
		Game.Instance = null;
		Game.BrainScheduler = null;
		Grid.OnReveal = null;
		this.VisualTunerElement = null;
		Assets.ClearOnAddPrefab();
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		(KComponentSpawn.instance.comps as GameComps).Clear();
	}

	// Token: 0x04002220 RID: 8736
	private static readonly string NextUniqueIDKey = "NextUniqueID";

	// Token: 0x04002221 RID: 8737
	public static string clusterId = null;

	// Token: 0x04002222 RID: 8738
	private PlayerController playerController;

	// Token: 0x04002223 RID: 8739
	private CameraController cameraController;

	// Token: 0x04002224 RID: 8740
	public Action<Game.GameSaveData> OnSave;

	// Token: 0x04002225 RID: 8741
	public Action<Game.GameSaveData> OnLoad;

	// Token: 0x04002226 RID: 8742
	[NonSerialized]
	public bool baseAlreadyCreated;

	// Token: 0x04002227 RID: 8743
	[NonSerialized]
	public bool autoPrioritizeRoles;

	// Token: 0x04002228 RID: 8744
	[NonSerialized]
	public bool advancedPersonalPriorities;

	// Token: 0x04002229 RID: 8745
	public Game.SavedInfo savedInfo;

	// Token: 0x0400222A RID: 8746
	public static bool quitting = false;

	// Token: 0x0400222C RID: 8748
	public AssignmentManager assignmentManager;

	// Token: 0x0400222D RID: 8749
	public GameObject playerPrefab;

	// Token: 0x0400222E RID: 8750
	public GameObject screenManagerPrefab;

	// Token: 0x0400222F RID: 8751
	public GameObject cameraControllerPrefab;

	// Token: 0x04002231 RID: 8753
	private static Camera m_CachedCamera = null;

	// Token: 0x04002232 RID: 8754
	public GameObject tempIntroScreenPrefab;

	// Token: 0x04002233 RID: 8755
	public static int BlockSelectionLayerMask;

	// Token: 0x04002234 RID: 8756
	public static int PickupableLayer;

	// Token: 0x04002235 RID: 8757
	public static BrainScheduler BrainScheduler;

	// Token: 0x04002236 RID: 8758
	public Element VisualTunerElement;

	// Token: 0x04002237 RID: 8759
	public float currentFallbackSunlightIntensity;

	// Token: 0x04002238 RID: 8760
	public RoomProber roomProber;

	// Token: 0x04002239 RID: 8761
	public SpaceScannerNetworkManager spaceScannerNetworkManager;

	// Token: 0x0400223A RID: 8762
	public FetchManager fetchManager;

	// Token: 0x0400223B RID: 8763
	public EdiblesManager ediblesManager;

	// Token: 0x0400223C RID: 8764
	public SpacecraftManager spacecraftManager;

	// Token: 0x0400223D RID: 8765
	public UserMenu userMenu;

	// Token: 0x0400223E RID: 8766
	public Unlocks unlocks;

	// Token: 0x0400223F RID: 8767
	public Timelapser timelapser;

	// Token: 0x04002240 RID: 8768
	private bool sandboxModeActive;

	// Token: 0x04002241 RID: 8769
	public HandleVector<Game.CallbackInfo> callbackManager = new HandleVector<Game.CallbackInfo>(256);

	// Token: 0x04002242 RID: 8770
	public List<int> callbackManagerManuallyReleasedHandles = new List<int>();

	// Token: 0x04002243 RID: 8771
	public Game.ComplexCallbackHandleVector<int> simComponentCallbackManager = new Game.ComplexCallbackHandleVector<int>(256);

	// Token: 0x04002244 RID: 8772
	public Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);

	// Token: 0x04002245 RID: 8773
	public Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);

	// Token: 0x04002246 RID: 8774
	public Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager = new Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);

	// Token: 0x04002247 RID: 8775
	public Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback> radiationConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback>(256);

	// Token: 0x04002248 RID: 8776
	[NonSerialized]
	public Player LocalPlayer;

	// Token: 0x04002249 RID: 8777
	[SerializeField]
	public TextAsset maleNamesFile;

	// Token: 0x0400224A RID: 8778
	[SerializeField]
	public TextAsset femaleNamesFile;

	// Token: 0x0400224B RID: 8779
	[NonSerialized]
	public World world;

	// Token: 0x0400224C RID: 8780
	[NonSerialized]
	public CircuitManager circuitManager;

	// Token: 0x0400224D RID: 8781
	[NonSerialized]
	public EnergySim energySim;

	// Token: 0x0400224E RID: 8782
	[NonSerialized]
	public LogicCircuitManager logicCircuitManager;

	// Token: 0x0400224F RID: 8783
	private GameScreenManager screenMgr;

	// Token: 0x04002250 RID: 8784
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;

	// Token: 0x04002251 RID: 8785
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;

	// Token: 0x04002252 RID: 8786
	public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;

	// Token: 0x04002253 RID: 8787
	public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;

	// Token: 0x04002254 RID: 8788
	public UtilityNetworkTubesManager travelTubeSystem;

	// Token: 0x04002255 RID: 8789
	public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;

	// Token: 0x04002256 RID: 8790
	public ConduitFlow gasConduitFlow;

	// Token: 0x04002257 RID: 8791
	public ConduitFlow liquidConduitFlow;

	// Token: 0x04002258 RID: 8792
	public SolidConduitFlow solidConduitFlow;

	// Token: 0x04002259 RID: 8793
	public Accumulators accumulators;

	// Token: 0x0400225A RID: 8794
	public PlantElementAbsorbers plantElementAbsorbers;

	// Token: 0x0400225B RID: 8795
	public Game.TemperatureOverlayModes temperatureOverlayMode;

	// Token: 0x0400225C RID: 8796
	public bool showExpandedTemperatures;

	// Token: 0x0400225D RID: 8797
	public List<Tag> tileOverlayFilters = new List<Tag>();

	// Token: 0x0400225E RID: 8798
	public bool showGasConduitDisease;

	// Token: 0x0400225F RID: 8799
	public bool showLiquidConduitDisease;

	// Token: 0x04002260 RID: 8800
	public ConduitFlowVisualizer gasFlowVisualizer;

	// Token: 0x04002261 RID: 8801
	public ConduitFlowVisualizer liquidFlowVisualizer;

	// Token: 0x04002262 RID: 8802
	public SolidConduitFlowVisualizer solidFlowVisualizer;

	// Token: 0x04002263 RID: 8803
	public ConduitTemperatureManager conduitTemperatureManager;

	// Token: 0x04002264 RID: 8804
	public ConduitDiseaseManager conduitDiseaseManager;

	// Token: 0x04002265 RID: 8805
	public MingleCellTracker mingleCellTracker;

	// Token: 0x04002266 RID: 8806
	private int simSubTick;

	// Token: 0x04002267 RID: 8807
	private bool hasFirstSimTickRun;

	// Token: 0x04002268 RID: 8808
	private float simDt;

	// Token: 0x04002269 RID: 8809
	public string dateGenerated;

	// Token: 0x0400226A RID: 8810
	public List<uint> changelistsPlayedOn;

	// Token: 0x0400226B RID: 8811
	[SerializeField]
	public Game.ConduitVisInfo liquidConduitVisInfo;

	// Token: 0x0400226C RID: 8812
	[SerializeField]
	public Game.ConduitVisInfo gasConduitVisInfo;

	// Token: 0x0400226D RID: 8813
	[SerializeField]
	public Game.ConduitVisInfo solidConduitVisInfo;

	// Token: 0x0400226E RID: 8814
	[SerializeField]
	private Material liquidFlowMaterial;

	// Token: 0x0400226F RID: 8815
	[SerializeField]
	private Material gasFlowMaterial;

	// Token: 0x04002270 RID: 8816
	[SerializeField]
	private Color flowColour;

	// Token: 0x04002271 RID: 8817
	private Vector3 gasFlowPos;

	// Token: 0x04002272 RID: 8818
	private Vector3 liquidFlowPos;

	// Token: 0x04002273 RID: 8819
	private Vector3 solidFlowPos;

	// Token: 0x04002274 RID: 8820
	public bool drawStatusItems = true;

	// Token: 0x04002275 RID: 8821
	private List<SolidInfo> solidInfo = new List<SolidInfo>();

	// Token: 0x04002276 RID: 8822
	private List<Klei.CallbackInfo> callbackInfo = new List<Klei.CallbackInfo>();

	// Token: 0x04002277 RID: 8823
	private List<SolidInfo> gameSolidInfo = new List<SolidInfo>();

	// Token: 0x04002278 RID: 8824
	private bool IsPaused;

	// Token: 0x04002279 RID: 8825
	private HashSet<int> solidChangedFilter = new HashSet<int>();

	// Token: 0x0400227A RID: 8826
	private HashedString lastDrawnOverlayMode;

	// Token: 0x0400227B RID: 8827
	private BuildingCellVisualizer previewVisualizer;

	// Token: 0x0400227E RID: 8830
	public SafetyConditions safetyConditions = new SafetyConditions();

	// Token: 0x0400227F RID: 8831
	public SimData simData = new SimData();

	// Token: 0x04002280 RID: 8832
	[MyCmpGet]
	private GameScenePartitioner gameScenePartitioner;

	// Token: 0x04002281 RID: 8833
	private bool gameStarted;

	// Token: 0x04002282 RID: 8834
	private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.MarkStatusItemRendererDirty(data);
	});

	// Token: 0x04002283 RID: 8835
	private static readonly EventSystem.IntraObjectHandler<Game> ActiveWorldChangedDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.ForceOverlayUpdate(true);
	});

	// Token: 0x04002284 RID: 8836
	private ushort[] activeFX;

	// Token: 0x04002285 RID: 8837
	public bool debugWasUsed;

	// Token: 0x04002286 RID: 8838
	private bool isLoading;

	// Token: 0x04002287 RID: 8839
	private List<Game.SimActiveRegion> simActiveRegions = new List<Game.SimActiveRegion>();

	// Token: 0x04002288 RID: 8840
	private HashedString previousOverlayMode = OverlayModes.None.ID;

	// Token: 0x04002289 RID: 8841
	private float previousGasConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x0400228A RID: 8842
	private float previousLiquidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x0400228B RID: 8843
	private float previousSolidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x0400228C RID: 8844
	[SerializeField]
	private Game.SpawnPoolData[] fxSpawnData;

	// Token: 0x0400228D RID: 8845
	private Dictionary<int, Action<Vector3, float>> fxSpawner = new Dictionary<int, Action<Vector3, float>>();

	// Token: 0x0400228E RID: 8846
	private Dictionary<int, GameObjectPool> fxPools = new Dictionary<int, GameObjectPool>();

	// Token: 0x0400228F RID: 8847
	private Game.SavingPreCB activatePreCB;

	// Token: 0x04002290 RID: 8848
	private Game.SavingActiveCB activateActiveCB;

	// Token: 0x04002291 RID: 8849
	private Game.SavingPostCB activatePostCB;

	// Token: 0x04002292 RID: 8850
	[SerializeField]
	public Game.UIColours uiColours = new Game.UIColours();

	// Token: 0x04002293 RID: 8851
	private float lastTimeWorkStarted = float.NegativeInfinity;

	// Token: 0x0200153D RID: 5437
	[Serializable]
	public struct SavedInfo
	{
		// Token: 0x06008727 RID: 34599 RVA: 0x0030A65D File Offset: 0x0030885D
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeEmptyVariables();
		}

		// Token: 0x06008728 RID: 34600 RVA: 0x0030A665 File Offset: 0x00308865
		public void InitializeEmptyVariables()
		{
			if (this.creaturePoopAmount == null)
			{
				this.creaturePoopAmount = new Dictionary<Tag, float>();
			}
			if (this.powerCreatedbyGeneratorType == null)
			{
				this.powerCreatedbyGeneratorType = new Dictionary<Tag, float>();
			}
		}

		// Token: 0x0400679E RID: 26526
		public bool discoveredSurface;

		// Token: 0x0400679F RID: 26527
		public bool discoveredOilField;

		// Token: 0x040067A0 RID: 26528
		public bool curedDisease;

		// Token: 0x040067A1 RID: 26529
		public bool blockedCometWithBunkerDoor;

		// Token: 0x040067A2 RID: 26530
		public Dictionary<Tag, float> creaturePoopAmount;

		// Token: 0x040067A3 RID: 26531
		public Dictionary<Tag, float> powerCreatedbyGeneratorType;
	}

	// Token: 0x0200153E RID: 5438
	public struct CallbackInfo
	{
		// Token: 0x06008729 RID: 34601 RVA: 0x0030A68D File Offset: 0x0030888D
		public CallbackInfo(System.Action cb, bool manually_release = false)
		{
			this.cb = cb;
			this.manuallyRelease = manually_release;
		}

		// Token: 0x040067A4 RID: 26532
		public System.Action cb;

		// Token: 0x040067A5 RID: 26533
		public bool manuallyRelease;
	}

	// Token: 0x0200153F RID: 5439
	public struct ComplexCallbackInfo<DataType>
	{
		// Token: 0x0600872A RID: 34602 RVA: 0x0030A69D File Offset: 0x0030889D
		public ComplexCallbackInfo(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			this.cb = cb;
			this.debugInfo = debug_info;
			this.callbackData = callback_data;
		}

		// Token: 0x040067A6 RID: 26534
		public Action<DataType, object> cb;

		// Token: 0x040067A7 RID: 26535
		public object callbackData;

		// Token: 0x040067A8 RID: 26536
		public string debugInfo;
	}

	// Token: 0x02001540 RID: 5440
	public class ComplexCallbackHandleVector<DataType>
	{
		// Token: 0x0600872B RID: 34603 RVA: 0x0030A6B4 File Offset: 0x003088B4
		public ComplexCallbackHandleVector(int initial_size)
		{
			this.baseMgr = new HandleVector<Game.ComplexCallbackInfo<DataType>>(initial_size);
		}

		// Token: 0x0600872C RID: 34604 RVA: 0x0030A6D3 File Offset: 0x003088D3
		public HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle Add(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			return this.baseMgr.Add(new Game.ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
		}

		// Token: 0x0600872D RID: 34605 RVA: 0x0030A6E8 File Offset: 0x003088E8
		public Game.ComplexCallbackInfo<DataType> GetItem(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			Game.ComplexCallbackInfo<DataType> item;
			try
			{
				item = this.baseMgr.GetItem(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was already released by " + str);
				}
				else
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was released ...... magically");
				}
				throw ex;
			}
			return item;
		}

		// Token: 0x0600872E RID: 34606 RVA: 0x0030A758 File Offset: 0x00308958
		public Game.ComplexCallbackInfo<DataType> Release(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle, string release_info)
		{
			Game.ComplexCallbackInfo<DataType> result;
			try
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandle(handle, out b, out key);
				this.releaseInfo[key] = release_info;
				result = this.baseMgr.Release(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, release_info + "is trying to release handle but it was already released by " + str);
				}
				else
				{
					KCrashReporter.Assert(false, release_info + "is trying to release a handle that was already released by some unknown thing");
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x0600872F RID: 34607 RVA: 0x0030A7EC File Offset: 0x003089EC
		public void Clear()
		{
			this.baseMgr.Clear();
		}

		// Token: 0x06008730 RID: 34608 RVA: 0x0030A7F9 File Offset: 0x003089F9
		public bool IsVersionValid(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			return this.baseMgr.IsVersionValid(handle);
		}

		// Token: 0x040067A9 RID: 26537
		private HandleVector<Game.ComplexCallbackInfo<DataType>> baseMgr;

		// Token: 0x040067AA RID: 26538
		private Dictionary<int, string> releaseInfo = new Dictionary<int, string>();
	}

	// Token: 0x02001541 RID: 5441
	public enum TemperatureOverlayModes
	{
		// Token: 0x040067AC RID: 26540
		AbsoluteTemperature,
		// Token: 0x040067AD RID: 26541
		AdaptiveTemperature,
		// Token: 0x040067AE RID: 26542
		HeatFlow,
		// Token: 0x040067AF RID: 26543
		StateChange
	}

	// Token: 0x02001542 RID: 5442
	[Serializable]
	public class ConduitVisInfo
	{
		// Token: 0x040067B0 RID: 26544
		public GameObject prefab;

		// Token: 0x040067B1 RID: 26545
		[Header("Main View")]
		public Color32 tint;

		// Token: 0x040067B2 RID: 26546
		public Color32 insulatedTint;

		// Token: 0x040067B3 RID: 26547
		public Color32 radiantTint;

		// Token: 0x040067B4 RID: 26548
		[Header("Overlay")]
		public string overlayTintName;

		// Token: 0x040067B5 RID: 26549
		public string overlayInsulatedTintName;

		// Token: 0x040067B6 RID: 26550
		public string overlayRadiantTintName;

		// Token: 0x040067B7 RID: 26551
		public Vector2 overlayMassScaleRange = new Vector2f(1f, 1000f);

		// Token: 0x040067B8 RID: 26552
		public Vector2 overlayMassScaleValues = new Vector2f(0.1f, 1f);
	}

	// Token: 0x02001543 RID: 5443
	private class WorldRegion
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06008732 RID: 34610 RVA: 0x0030A843 File Offset: 0x00308A43
		public Vector2I regionMin
		{
			get
			{
				return this.min;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06008733 RID: 34611 RVA: 0x0030A84B File Offset: 0x00308A4B
		public Vector2I regionMax
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x06008734 RID: 34612 RVA: 0x0030A854 File Offset: 0x00308A54
		public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1)
		{
			this.min.x = Mathf.Max(0, x0);
			this.min.y = Mathf.Max(0, y0);
			this.max.x = Mathf.Max(x1, this.regionMax.x);
			this.max.y = Mathf.Max(y1, this.regionMax.y);
		}

		// Token: 0x06008735 RID: 34613 RVA: 0x0030A8BE File Offset: 0x00308ABE
		public void UpdateGameActiveRegion(Vector2I simActiveRegionMin, Vector2I simActiveRegionMax)
		{
			this.min = simActiveRegionMin;
			this.max = simActiveRegionMax;
		}

		// Token: 0x040067B9 RID: 26553
		private Vector2I min;

		// Token: 0x040067BA RID: 26554
		private Vector2I max;

		// Token: 0x040067BB RID: 26555
		public bool isActive;
	}

	// Token: 0x02001544 RID: 5444
	public class SimActiveRegion
	{
		// Token: 0x06008737 RID: 34615 RVA: 0x0030A8D6 File Offset: 0x00308AD6
		public SimActiveRegion()
		{
			this.region = default(Pair<Vector2I, Vector2I>);
			this.currentSunlightIntensity = (float)FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
			this.currentCosmicRadiationIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}

		// Token: 0x040067BC RID: 26556
		public Pair<Vector2I, Vector2I> region;

		// Token: 0x040067BD RID: 26557
		public float currentSunlightIntensity;

		// Token: 0x040067BE RID: 26558
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x02001545 RID: 5445
	private enum SpawnRotationConfig
	{
		// Token: 0x040067C0 RID: 26560
		Normal,
		// Token: 0x040067C1 RID: 26561
		StringName
	}

	// Token: 0x02001546 RID: 5446
	[Serializable]
	private struct SpawnRotationData
	{
		// Token: 0x040067C2 RID: 26562
		public string animName;

		// Token: 0x040067C3 RID: 26563
		public bool flip;
	}

	// Token: 0x02001547 RID: 5447
	[Serializable]
	private struct SpawnPoolData
	{
		// Token: 0x040067C4 RID: 26564
		[HashedEnum]
		public SpawnFXHashes id;

		// Token: 0x040067C5 RID: 26565
		public int initialCount;

		// Token: 0x040067C6 RID: 26566
		public Color32 colour;

		// Token: 0x040067C7 RID: 26567
		public GameObject fxPrefab;

		// Token: 0x040067C8 RID: 26568
		public string initialAnim;

		// Token: 0x040067C9 RID: 26569
		public Vector3 spawnOffset;

		// Token: 0x040067CA RID: 26570
		public Vector2 spawnRandomOffset;

		// Token: 0x040067CB RID: 26571
		public Game.SpawnRotationConfig rotationConfig;

		// Token: 0x040067CC RID: 26572
		public Game.SpawnRotationData[] rotationData;
	}

	// Token: 0x02001548 RID: 5448
	[Serializable]
	private class Settings
	{
		// Token: 0x06008738 RID: 34616 RVA: 0x0030A902 File Offset: 0x00308B02
		public Settings(Game game)
		{
			this.nextUniqueID = KPrefabID.NextUniqueID;
			this.gameID = KleiMetrics.GameID();
		}

		// Token: 0x06008739 RID: 34617 RVA: 0x0030A920 File Offset: 0x00308B20
		public Settings()
		{
		}

		// Token: 0x040067CD RID: 26573
		public int nextUniqueID;

		// Token: 0x040067CE RID: 26574
		public int gameID;
	}

	// Token: 0x02001549 RID: 5449
	public class GameSaveData
	{
		// Token: 0x040067CF RID: 26575
		public ConduitFlow gasConduitFlow;

		// Token: 0x040067D0 RID: 26576
		public ConduitFlow liquidConduitFlow;

		// Token: 0x040067D1 RID: 26577
		public FallingWater fallingWater;

		// Token: 0x040067D2 RID: 26578
		public UnstableGroundManager unstableGround;

		// Token: 0x040067D3 RID: 26579
		public WorldDetailSave worldDetail;

		// Token: 0x040067D4 RID: 26580
		public CustomGameSettings customGameSettings;

		// Token: 0x040067D5 RID: 26581
		public StoryManager storySetings;

		// Token: 0x040067D6 RID: 26582
		public SpaceScannerNetworkManager spaceScannerNetworkManager;

		// Token: 0x040067D7 RID: 26583
		public bool debugWasUsed;

		// Token: 0x040067D8 RID: 26584
		public bool autoPrioritizeRoles;

		// Token: 0x040067D9 RID: 26585
		public bool advancedPersonalPriorities;

		// Token: 0x040067DA RID: 26586
		public Game.SavedInfo savedInfo;

		// Token: 0x040067DB RID: 26587
		public string dateGenerated;

		// Token: 0x040067DC RID: 26588
		public List<uint> changelistsPlayedOn;
	}

	// Token: 0x0200154A RID: 5450
	// (Invoke) Token: 0x0600873C RID: 34620
	public delegate void CansaveCB();

	// Token: 0x0200154B RID: 5451
	// (Invoke) Token: 0x06008740 RID: 34624
	public delegate void SavingPreCB(Game.CansaveCB cb);

	// Token: 0x0200154C RID: 5452
	// (Invoke) Token: 0x06008744 RID: 34628
	public delegate void SavingActiveCB();

	// Token: 0x0200154D RID: 5453
	// (Invoke) Token: 0x06008748 RID: 34632
	public delegate void SavingPostCB();

	// Token: 0x0200154E RID: 5454
	[Serializable]
	public struct LocationColours
	{
		// Token: 0x040067DD RID: 26589
		public Color unreachable;

		// Token: 0x040067DE RID: 26590
		public Color invalidLocation;

		// Token: 0x040067DF RID: 26591
		public Color validLocation;

		// Token: 0x040067E0 RID: 26592
		public Color requiresRole;

		// Token: 0x040067E1 RID: 26593
		public Color unreachable_requiresRole;
	}

	// Token: 0x0200154F RID: 5455
	[Serializable]
	public class UIColours
	{
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600874B RID: 34635 RVA: 0x0030A930 File Offset: 0x00308B30
		public Game.LocationColours Dig
		{
			get
			{
				return this.digColours;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600874C RID: 34636 RVA: 0x0030A938 File Offset: 0x00308B38
		public Game.LocationColours Build
		{
			get
			{
				return this.buildColours;
			}
		}

		// Token: 0x040067E2 RID: 26594
		[SerializeField]
		private Game.LocationColours digColours;

		// Token: 0x040067E3 RID: 26595
		[SerializeField]
		private Game.LocationColours buildColours;
	}
}
