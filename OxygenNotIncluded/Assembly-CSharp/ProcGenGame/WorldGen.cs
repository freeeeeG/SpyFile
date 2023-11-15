using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Delaunay.Geo;
using Klei;
using KSerialization;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using ProcGen;
using ProcGen.Map;
using ProcGen.Noise;
using STRINGS;
using UnityEngine;
using VoronoiTree;

namespace ProcGenGame
{
	// Token: 0x02000CB1 RID: 3249
	[Serializable]
	public class WorldGen
	{
		// Token: 0x0600679A RID: 26522 RVA: 0x0026FE75 File Offset: 0x0026E075
		public static string GetSIMSaveFilename(int baseID = -1)
		{
			return System.IO.Path.Combine(global::Util.RootFolder(), (baseID == -1) ? "WorldGenSimSave.dat" : string.Format("{0}{1}{2}", "WorldGenSimSave", baseID, ".dat"));
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x0600679B RID: 26523 RVA: 0x0026FEA6 File Offset: 0x0026E0A6
		public static string WORLDGEN_SAVE_FILENAME
		{
			get
			{
				return System.IO.Path.Combine(global::Util.RootFolder(), "WorldGenDataSave.dat");
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x0600679C RID: 26524 RVA: 0x0026FEB7 File Offset: 0x0026E0B7
		public static Diseases diseaseStats
		{
			get
			{
				if (WorldGen.m_diseasesDb == null)
				{
					WorldGen.m_diseasesDb = new Diseases(null, true);
				}
				return WorldGen.m_diseasesDb;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x0600679D RID: 26525 RVA: 0x0026FED1 File Offset: 0x0026E0D1
		public int BaseLeft
		{
			get
			{
				return this.Settings.GetBaseLocation().left;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x0600679E RID: 26526 RVA: 0x0026FEE3 File Offset: 0x0026E0E3
		public int BaseRight
		{
			get
			{
				return this.Settings.GetBaseLocation().right;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x0600679F RID: 26527 RVA: 0x0026FEF5 File Offset: 0x0026E0F5
		public int BaseTop
		{
			get
			{
				return this.Settings.GetBaseLocation().top;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060067A0 RID: 26528 RVA: 0x0026FF07 File Offset: 0x0026E107
		public int BaseBot
		{
			get
			{
				return this.Settings.GetBaseLocation().bottom;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060067A1 RID: 26529 RVA: 0x0026FF19 File Offset: 0x0026E119
		// (set) Token: 0x060067A2 RID: 26530 RVA: 0x0026FF21 File Offset: 0x0026E121
		public Data data { get; private set; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060067A3 RID: 26531 RVA: 0x0026FF2A File Offset: 0x0026E12A
		public bool HasData
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060067A4 RID: 26532 RVA: 0x0026FF35 File Offset: 0x0026E135
		public bool HasNoiseData
		{
			get
			{
				return this.HasData && this.data.world != null;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060067A5 RID: 26533 RVA: 0x0026FF4F File Offset: 0x0026E14F
		public float[] DensityMap
		{
			get
			{
				return this.data.world.density;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060067A6 RID: 26534 RVA: 0x0026FF61 File Offset: 0x0026E161
		public float[] HeatMap
		{
			get
			{
				return this.data.world.heatOffset;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060067A7 RID: 26535 RVA: 0x0026FF73 File Offset: 0x0026E173
		public float[] OverrideMap
		{
			get
			{
				return this.data.world.overrides;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060067A8 RID: 26536 RVA: 0x0026FF85 File Offset: 0x0026E185
		public float[] BaseNoiseMap
		{
			get
			{
				return this.data.world.data;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060067A9 RID: 26537 RVA: 0x0026FF97 File Offset: 0x0026E197
		public float[] DefaultTendMap
		{
			get
			{
				return this.data.world.defaultTemp;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060067AA RID: 26538 RVA: 0x0026FFA9 File Offset: 0x0026E1A9
		public Chunk World
		{
			get
			{
				return this.data.world;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060067AB RID: 26539 RVA: 0x0026FFB6 File Offset: 0x0026E1B6
		public Vector2I WorldSize
		{
			get
			{
				return this.data.world.size;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060067AC RID: 26540 RVA: 0x0026FFC8 File Offset: 0x0026E1C8
		public Vector2I WorldOffset
		{
			get
			{
				return this.data.world.offset;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060067AD RID: 26541 RVA: 0x0026FFDA File Offset: 0x0026E1DA
		public WorldLayout WorldLayout
		{
			get
			{
				return this.data.worldLayout;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060067AE RID: 26542 RVA: 0x0026FFE7 File Offset: 0x0026E1E7
		public List<TerrainCell> OverworldCells
		{
			get
			{
				return this.data.overworldCells;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060067AF RID: 26543 RVA: 0x0026FFF4 File Offset: 0x0026E1F4
		public List<TerrainCell> TerrainCells
		{
			get
			{
				return this.data.terrainCells;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060067B0 RID: 26544 RVA: 0x00270001 File Offset: 0x0026E201
		public List<River> Rivers
		{
			get
			{
				return this.data.rivers;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060067B1 RID: 26545 RVA: 0x0027000E File Offset: 0x0026E20E
		public GameSpawnData SpawnData
		{
			get
			{
				return this.data.gameSpawnData;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060067B2 RID: 26546 RVA: 0x0027001B File Offset: 0x0026E21B
		public int ChunkEdgeSize
		{
			get
			{
				return this.data.chunkEdgeSize;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060067B3 RID: 26547 RVA: 0x00270028 File Offset: 0x0026E228
		public HashSet<int> ClaimedCells
		{
			get
			{
				return this.claimedCells;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060067B4 RID: 26548 RVA: 0x00270030 File Offset: 0x0026E230
		public HashSet<int> HighPriorityClaimedCells
		{
			get
			{
				return this.highPriorityClaims;
			}
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x00270038 File Offset: 0x0026E238
		public void ClearClaimedCells()
		{
			this.claimedCells.Clear();
			this.highPriorityClaims.Clear();
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x00270050 File Offset: 0x0026E250
		public void AddHighPriorityCells(HashSet<int> cells)
		{
			this.highPriorityClaims.Union(cells);
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060067B7 RID: 26551 RVA: 0x0027005F File Offset: 0x0026E25F
		// (set) Token: 0x060067B8 RID: 26552 RVA: 0x00270067 File Offset: 0x0026E267
		public WorldGenSettings Settings { get; private set; }

		// Token: 0x060067B9 RID: 26553 RVA: 0x00270070 File Offset: 0x0026E270
		public WorldGen(string worldName, List<string> chosenWorldTraits, List<string> chosenStoryTraits, bool assertMissingTraits)
		{
			WorldGen.LoadSettings(false);
			this.Settings = new WorldGenSettings(worldName, chosenWorldTraits, chosenStoryTraits, assertMissingTraits);
			this.data = new Data();
			this.data.chunkEdgeSize = this.Settings.GetIntSetting("ChunkEdgeSize");
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x00270100 File Offset: 0x0026E300
		public WorldGen(string worldName, Data data, List<string> chosenTraits, List<string> chosenStoryTraits, bool assertMissingTraits)
		{
			WorldGen.LoadSettings(false);
			this.Settings = new WorldGenSettings(worldName, chosenTraits, chosenStoryTraits, assertMissingTraits);
			this.data = data;
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x00270172 File Offset: 0x0026E372
		public static void SetupDefaultElements()
		{
			WorldGen.voidElement = ElementLoader.FindElementByHash(SimHashes.Void);
			WorldGen.vacuumElement = ElementLoader.FindElementByHash(SimHashes.Vacuum);
			WorldGen.katairiteElement = ElementLoader.FindElementByHash(SimHashes.Katairite);
			WorldGen.unobtaniumElement = ElementLoader.FindElementByHash(SimHashes.Unobtanium);
		}

		// Token: 0x060067BC RID: 26556 RVA: 0x002701B0 File Offset: 0x0026E3B0
		public void Reset()
		{
			this.wasLoaded = false;
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x002701BC File Offset: 0x0026E3BC
		public static void LoadSettings(bool in_async_thread = false)
		{
			bool is_playing = Application.isPlaying;
			if (in_async_thread)
			{
				WorldGen.loadSettingsTask = Task.Run(delegate()
				{
					WorldGen.LoadSettings_Internal(is_playing, true);
				});
				return;
			}
			if (WorldGen.loadSettingsTask != null)
			{
				WorldGen.loadSettingsTask.Wait();
				WorldGen.loadSettingsTask = null;
			}
			WorldGen.LoadSettings_Internal(is_playing, false);
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x00270218 File Offset: 0x0026E418
		private static void LoadSettings_Internal(bool is_playing, bool preloadTemplates = false)
		{
			ListPool<YamlIO.Error, WorldGen>.PooledList pooledList = ListPool<YamlIO.Error, WorldGen>.Allocate();
			if (SettingsCache.LoadFiles(pooledList))
			{
				TemplateCache.Init();
				if (preloadTemplates)
				{
					foreach (ProcGen.World world in SettingsCache.worlds.worldCache.Values)
					{
						if (world.worldTemplateRules != null)
						{
							foreach (ProcGen.World.TemplateSpawnRules templateSpawnRules in world.worldTemplateRules)
							{
								foreach (string templatePath in templateSpawnRules.names)
								{
									TemplateCache.GetTemplate(templatePath);
								}
							}
						}
					}
					foreach (SubWorld subWorld in SettingsCache.subworlds.Values)
					{
						if (subWorld.subworldTemplateRules != null)
						{
							foreach (ProcGen.World.TemplateSpawnRules templateSpawnRules2 in subWorld.subworldTemplateRules)
							{
								foreach (string templatePath2 in templateSpawnRules2.names)
								{
									TemplateCache.GetTemplate(templatePath2);
								}
							}
						}
					}
				}
			}
			CustomGameSettings.Instance != null;
			if (is_playing)
			{
				Global.Instance.modManager.HandleErrors(pooledList);
			}
			else
			{
				foreach (YamlIO.Error error in pooledList)
				{
					YamlIO.LogError(error, false);
				}
			}
			pooledList.Recycle();
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x00270434 File Offset: 0x0026E634
		public void InitRandom(int worldSeed, int layoutSeed, int terrainSeed, int noiseSeed)
		{
			this.data.globalWorldSeed = worldSeed;
			this.data.globalWorldLayoutSeed = layoutSeed;
			this.data.globalTerrainSeed = terrainSeed;
			this.data.globalNoiseSeed = noiseSeed;
			this.myRandom = new SeededRandom(worldSeed);
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x00270474 File Offset: 0x0026E674
		public void Initialise(WorldGen.OfflineCallbackFunction callbackFn, Action<OfflineWorldGen.ErrorInfo> error_cb, int worldSeed = -1, int layoutSeed = -1, int terrainSeed = -1, int noiseSeed = -1, bool debug = false)
		{
			if (this.wasLoaded)
			{
				global::Debug.LogError("Initialise called after load");
				return;
			}
			this.successCallbackFn = callbackFn;
			this.errorCallback = error_cb;
			global::Debug.Assert(this.successCallbackFn != null);
			this.isRunningDebugGen = debug;
			this.running = false;
			int num = UnityEngine.Random.Range(0, int.MaxValue);
			if (worldSeed == -1)
			{
				worldSeed = num;
			}
			if (layoutSeed == -1)
			{
				layoutSeed = num;
			}
			if (terrainSeed == -1)
			{
				terrainSeed = num;
			}
			if (noiseSeed == -1)
			{
				noiseSeed = num;
			}
			this.data.gameSpawnData = new GameSpawnData();
			this.InitRandom(worldSeed, layoutSeed, terrainSeed, noiseSeed);
			this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 0f, WorldGenProgressStages.Stages.Failure);
			WorldLayout.SetLayerGradient(SettingsCache.layers.LevelLayers);
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x00270532 File Offset: 0x0026E732
		public void DontGenerateNoiseData()
		{
			this.generateNoiseData = false;
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x0027053C File Offset: 0x0026E73C
		public void GenerateOffline()
		{
			int num = 1;
			int num2 = 0;
			while (num2 < num && !this.GenerateWorldData())
			{
				DebugUtil.DevLogError("Failed worldgen");
				this.successCallbackFn(UI.WORLDGEN.RETRYCOUNT.key, (float)num2, WorldGenProgressStages.Stages.Failure);
				num2++;
			}
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x00270582 File Offset: 0x0026E782
		private void PlaceTemplateSpawners(Vector2I position, TemplateContainer template, ref Dictionary<int, int> claimedCells)
		{
			this.data.gameSpawnData.AddTemplate(template, position, ref claimedCells);
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x00270598 File Offset: 0x0026E798
		public bool RenderOffline(bool doSettle, ref Sim.Cell[] cells, ref Sim.DiseaseCell[] dc, int baseId, ref List<WorldTrait> placedStoryTraits, bool isStartingWorld = false)
		{
			float[] bgTemp = null;
			dc = null;
			HashSet<int> hashSet = new HashSet<int>();
			this.POIBounds = new List<RectInt>();
			this.WriteOverWorldNoise(this.successCallbackFn);
			if (!this.RenderToMap(this.successCallbackFn, ref cells, ref bgTemp, ref dc, ref hashSet, ref this.POIBounds))
			{
				this.successCallbackFn(UI.WORLDGEN.FAILED.key, -100f, WorldGenProgressStages.Stages.Failure);
				if (!this.isRunningDebugGen)
				{
					return false;
				}
			}
			foreach (int num in hashSet)
			{
				cells[num].SetValues(WorldGen.unobtaniumElement, ElementLoader.elements);
				this.claimedPOICells[num] = 1;
			}
			try
			{
				this.POISpawners = TemplateSpawning.DetermineTemplatesForWorld(this.Settings, this.data.terrainCells, this.myRandom, ref this.POIBounds, this.isRunningDebugGen, ref placedStoryTraits, this.successCallbackFn);
			}
			catch (TemplateSpawningException ex)
			{
				if (!this.isRunningDebugGen)
				{
					this.ReportWorldGenError(ex, ex.userMessage);
					return false;
				}
			}
			catch (Exception e)
			{
				if (!this.isRunningDebugGen)
				{
					this.ReportWorldGenError(e, null);
					return false;
				}
			}
			if (isStartingWorld)
			{
				this.EnsureEnoughElementsInStartingBiome(cells);
			}
			List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
			foreach (TerrainCell terrainCell in this.OverworldCells)
			{
				foreach (TerrainCell terrainCell2 in terrainCellsForTag)
				{
					if (terrainCell.poly.PointInPolygon(terrainCell2.poly.Centroid()))
					{
						terrainCell.node.tags.Add(WorldGenTags.StartWorld);
						break;
					}
				}
			}
			if (doSettle)
			{
				this.running = WorldGenSimUtil.DoSettleSim(this.Settings, ref cells, ref bgTemp, ref dc, this.successCallbackFn, this.data, this.POISpawners, this.errorCallback, baseId);
			}
			foreach (TemplateSpawning.TemplateSpawner templateSpawner in this.POISpawners)
			{
				this.PlaceTemplateSpawners(templateSpawner.position, templateSpawner.container, ref this.claimedPOICells);
			}
			if (doSettle)
			{
				this.SpawnMobsAndTemplates(cells, bgTemp, dc, new HashSet<int>(this.claimedPOICells.Keys));
			}
			this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 1f, WorldGenProgressStages.Stages.Complete);
			this.running = false;
			return true;
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x00270888 File Offset: 0x0026EA88
		private void SpawnMobsAndTemplates(Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc, HashSet<int> claimedCells)
		{
			MobSpawning.DetectNaturalCavities(this.TerrainCells, this.successCallbackFn, cells);
			SeededRandom rnd = new SeededRandom(this.data.globalTerrainSeed);
			for (int i = 0; i < this.TerrainCells.Count; i++)
			{
				float completePercent = (float)i / (float)this.TerrainCells.Count;
				this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, completePercent, WorldGenProgressStages.Stages.PlacingCreatures);
				TerrainCell tc = this.TerrainCells[i];
				Dictionary<int, string> dictionary = MobSpawning.PlaceFeatureAmbientMobs(this.Settings, tc, rnd, cells, bgTemp, dc, claimedCells, this.isRunningDebugGen);
				if (dictionary != null)
				{
					this.data.gameSpawnData.AddRange(dictionary);
				}
				dictionary = MobSpawning.PlaceBiomeAmbientMobs(this.Settings, tc, rnd, cells, bgTemp, dc, claimedCells, this.isRunningDebugGen);
				if (dictionary != null)
				{
					this.data.gameSpawnData.AddRange(dictionary);
				}
			}
			this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, 1f, WorldGenProgressStages.Stages.PlacingCreatures);
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x00270988 File Offset: 0x0026EB88
		public void ReportWorldGenError(Exception e, string errorMessage = null)
		{
			if (errorMessage == null)
			{
				errorMessage = UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE;
			}
			bool flag = FileSystem.IsModdedFile(SettingsCache.RewriteWorldgenPathYaml(this.Settings.world.filePath));
			string arg = (CustomGameSettings.Instance != null) ? CustomGameSettings.Instance.GetSettingsCoordinate() : this.data.globalWorldLayoutSeed.ToString();
			global::Debug.LogWarning(string.Format("Worldgen Failure on seed {0}, modded={1}", arg, flag));
			if (this.errorCallback != null)
			{
				this.errorCallback(new OfflineWorldGen.ErrorInfo
				{
					errorDesc = string.Format(errorMessage, arg),
					exception = e
				});
			}
		}

		// Token: 0x060067C7 RID: 26567 RVA: 0x00270A34 File Offset: 0x0026EC34
		public void SetWorldSize(int width, int height)
		{
			if (this.data.world != null && this.data.world.offset != Vector2I.zero)
			{
				global::Debug.LogWarning("Resetting world chunk to defaults.");
			}
			this.data.world = new Chunk(0, 0, width, height);
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x00270A88 File Offset: 0x0026EC88
		public Vector2I GetSize()
		{
			return this.data.world.size;
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x00270A9A File Offset: 0x0026EC9A
		public void SetPosition(Vector2I position)
		{
			this.data.world.offset = position;
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x00270AAD File Offset: 0x0026ECAD
		public Vector2I GetPosition()
		{
			return this.data.world.offset;
		}

		// Token: 0x060067CB RID: 26571 RVA: 0x00270ABF File Offset: 0x0026ECBF
		public void SetClusterLocation(AxialI location)
		{
			this.data.clusterLocation = location;
		}

		// Token: 0x060067CC RID: 26572 RVA: 0x00270ACD File Offset: 0x0026ECCD
		public AxialI GetClusterLocation()
		{
			return this.data.clusterLocation;
		}

		// Token: 0x060067CD RID: 26573 RVA: 0x00270ADC File Offset: 0x0026ECDC
		public bool GenerateNoiseData(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			try
			{
				this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 0f, WorldGenProgressStages.Stages.SetupNoise);
				if (!this.running)
				{
					return false;
				}
				this.SetupNoise(updateProgressFn);
				this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 1f, WorldGenProgressStages.Stages.SetupNoise);
				if (!this.running)
				{
					return false;
				}
				this.GenerateUnChunkedNoise(updateProgressFn);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				this.ReportWorldGenError(ex, null);
				WorldGenLogger.LogException(message, stackTrace);
				this.running = this.successCallbackFn(new StringKey("Exception in GenerateNoiseData"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			return true;
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x00270BA0 File Offset: 0x0026EDA0
		public bool GenerateLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			try
			{
				this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 0f, WorldGenProgressStages.Stages.WorldLayout);
				if (!this.running)
				{
					return false;
				}
				global::Debug.Assert(this.data.world.size.x != 0 && this.data.world.size.y != 0, "Map size has not been set");
				this.data.worldLayout = new WorldLayout(this, this.data.world.size.x, this.data.world.size.y, this.data.globalWorldLayoutSeed);
				this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 1f, WorldGenProgressStages.Stages.WorldLayout);
				this.data.voronoiTree = null;
				try
				{
					this.data.voronoiTree = this.WorldLayout.GenerateOverworld(this.Settings.world.layoutMethod == ProcGen.World.LayoutMethod.PowerTree, this.isRunningDebugGen);
					this.WorldLayout.PopulateSubworlds();
					this.CompleteLayout(updateProgressFn);
				}
				catch (Exception ex)
				{
					string message = ex.Message;
					string stackTrace = ex.StackTrace;
					WorldGenLogger.LogException(message, stackTrace);
					this.ReportWorldGenError(ex, null);
					this.running = updateProgressFn(new StringKey("Exception in InitVoronoiTree"), -1f, WorldGenProgressStages.Stages.Failure);
					return false;
				}
				this.data.overworldCells = new List<TerrainCell>(40);
				for (int i = 0; i < this.data.voronoiTree.ChildCount(); i++)
				{
					VoronoiTree.Tree tree = this.data.voronoiTree.GetChild(i) as VoronoiTree.Tree;
					Cell node = this.data.worldLayout.overworldGraph.FindNodeByID(tree.site.id);
					this.data.overworldCells.Add(new TerrainCellLogged(node, tree.site, tree.minDistanceToTag));
				}
				this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 1f, WorldGenProgressStages.Stages.WorldLayout);
			}
			catch (Exception ex2)
			{
				string message2 = ex2.Message;
				string stackTrace2 = ex2.StackTrace;
				WorldGenLogger.LogException(message2, stackTrace2);
				this.ReportWorldGenError(ex2, null);
				this.successCallbackFn(new StringKey("Exception in GenerateLayout"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			return true;
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x00270E2C File Offset: 0x0026F02C
		public bool CompleteLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			try
			{
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0f, WorldGenProgressStages.Stages.CompleteLayout);
				if (!this.running)
				{
					return false;
				}
				this.data.terrainCells = null;
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.65f, WorldGenProgressStages.Stages.CompleteLayout);
				if (!this.running)
				{
					return false;
				}
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.75f, WorldGenProgressStages.Stages.CompleteLayout);
				if (!this.running)
				{
					return false;
				}
				this.data.terrainCells = new List<TerrainCell>(4000);
				List<VoronoiTree.Node> list = new List<VoronoiTree.Node>();
				this.data.voronoiTree.ForceLowestToLeaf();
				this.ApplyStartNode();
				this.ApplySwapTags();
				this.data.voronoiTree.GetLeafNodes(list, null);
				WorldLayout.ResetMapGraphFromVoronoiTree(list, this.WorldLayout.localGraph, true);
				for (int i = 0; i < list.Count; i++)
				{
					VoronoiTree.Node node = list[i];
					Cell tn = this.data.worldLayout.localGraph.FindNodeByID(node.site.id);
					if (tn != null)
					{
						TerrainCell terrainCell = this.data.terrainCells.Find((TerrainCell c) => c.node == tn);
						if (terrainCell == null)
						{
							TerrainCell item = new TerrainCellLogged(tn, node.site, node.parent.minDistanceToTag);
							this.data.terrainCells.Add(item);
						}
						else
						{
							global::Debug.LogWarning("Duplicate cell found" + terrainCell.node.NodeId.ToString());
						}
					}
				}
				for (int j = 0; j < this.data.terrainCells.Count; j++)
				{
					TerrainCell terrainCell2 = this.data.terrainCells[j];
					for (int k = j + 1; k < this.data.terrainCells.Count; k++)
					{
						int num = 0;
						TerrainCell terrainCell3 = this.data.terrainCells[k];
						LineSegment lineSegment;
						if (terrainCell3.poly.SharesEdge(terrainCell2.poly, ref num, out lineSegment) == Polygon.Commonality.Edge)
						{
							terrainCell2.neighbourTerrainCells.Add(k);
							terrainCell3.neighbourTerrainCells.Add(j);
						}
					}
				}
				this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 1f, WorldGenProgressStages.Stages.CompleteLayout);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				WorldGenLogger.LogException(message, stackTrace);
				this.successCallbackFn(new StringKey("Exception in CompleteLayout"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			return true;
		}

		// Token: 0x060067D0 RID: 26576 RVA: 0x00271110 File Offset: 0x0026F310
		public void UpdateVoronoiNodeTags(VoronoiTree.Node node)
		{
			ProcGen.Node node2;
			if (node.tags.Contains(WorldGenTags.Overworld))
			{
				node2 = this.WorldLayout.overworldGraph.FindNodeByID(node.site.id);
			}
			else
			{
				node2 = this.WorldLayout.localGraph.FindNodeByID(node.site.id);
			}
			if (node2 != null)
			{
				node2.tags.Union(node.tags);
			}
		}

		// Token: 0x060067D1 RID: 26577 RVA: 0x0027117F File Offset: 0x0026F37F
		public bool GenerateWorldData()
		{
			return (!this.generateNoiseData || this.GenerateNoiseData(this.successCallbackFn)) && this.GenerateLayout(this.successCallbackFn);
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x002711AC File Offset: 0x0026F3AC
		public void EnsureEnoughElementsInStartingBiome(Sim.Cell[] cells)
		{
			List<StartingWorldElementSetting> defaultStartingElements = this.Settings.GetDefaultStartingElements();
			List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
			foreach (StartingWorldElementSetting startingWorldElementSetting in defaultStartingElements)
			{
				float amount = startingWorldElementSetting.amount;
				Element element = ElementLoader.GetElement(new Tag(((SimHashes)Enum.Parse(typeof(SimHashes), startingWorldElementSetting.element, true)).ToString()));
				float num = 0f;
				int num2 = 0;
				foreach (TerrainCell terrainCell in terrainCellsForTag)
				{
					foreach (int num3 in terrainCell.GetAllCells())
					{
						if (element.idx == cells[num3].elementIdx)
						{
							num2++;
							num += cells[num3].mass;
						}
					}
				}
				DebugUtil.DevAssert(num2 > 0, string.Format("No {0} found in starting biome and trying to ensure at least {1}. Skipping.", element.id, amount), null);
				if (num < amount && num2 > 0)
				{
					float num4 = num / (float)num2;
					float num5 = (amount - num) / (float)num2;
					DebugUtil.DevAssert(num4 + num5 <= 2f * element.maxMass, string.Format("Number of cells ({0}) of {1} in the starting biome is insufficient, this will result in extremely dense cells. {2} but expecting less than {3}", new object[]
					{
						num2,
						element.id,
						num4 + num5,
						2f * element.maxMass
					}), null);
					foreach (TerrainCell terrainCell2 in terrainCellsForTag)
					{
						foreach (int num6 in terrainCell2.GetAllCells())
						{
							if (element.idx == cells[num6].elementIdx)
							{
								int num7 = num6;
								cells[num7].mass = cells[num7].mass + num5;
							}
						}
					}
				}
			}
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x00271480 File Offset: 0x0026F680
		public bool RenderToMap(WorldGen.OfflineCallbackFunction updateProgressFn, ref Sim.Cell[] cells, ref float[] bgTemp, ref Sim.DiseaseCell[] dcs, ref HashSet<int> borderCells, ref List<RectInt> poiBounds)
		{
			global::Debug.Assert(Grid.WidthInCells == this.Settings.world.worldsize.x);
			global::Debug.Assert(Grid.HeightInCells == this.Settings.world.worldsize.y);
			global::Debug.Assert(Grid.CellCount == Grid.WidthInCells * Grid.HeightInCells);
			global::Debug.Assert(Grid.CellSizeInMeters != 0f);
			borderCells = new HashSet<int>();
			cells = new Sim.Cell[Grid.CellCount];
			bgTemp = new float[Grid.CellCount];
			dcs = new Sim.DiseaseCell[Grid.CellCount];
			this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 0f, WorldGenProgressStages.Stages.ClearingLevel);
			if (!this.running)
			{
				return false;
			}
			for (int i = 0; i < cells.Length; i++)
			{
				cells[i].SetValues(WorldGen.katairiteElement, ElementLoader.elements);
				bgTemp[i] = -1f;
				dcs[i] = default(Sim.DiseaseCell);
				dcs[i].diseaseIdx = byte.MaxValue;
				this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, (float)i / (float)Grid.CellCount, WorldGenProgressStages.Stages.ClearingLevel);
				if (!this.running)
				{
					return false;
				}
			}
			updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 1f, WorldGenProgressStages.Stages.ClearingLevel);
			try
			{
				this.ProcessByTerrainCell(cells, bgTemp, dcs, updateProgressFn, this.highPriorityClaims);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				WorldGenLogger.LogException(message, stackTrace);
				this.running = updateProgressFn(new StringKey("Exception in ProcessByTerrainCell"), -1f, WorldGenProgressStages.Stages.Failure);
				return false;
			}
			if (this.Settings.GetBoolSetting("DrawWorldBorder"))
			{
				SeededRandom rnd = new SeededRandom(0);
				this.DrawWorldBorder(cells, this.data.world, rnd, ref borderCells, ref poiBounds, updateProgressFn);
				updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 1f, WorldGenProgressStages.Stages.DrawWorldBorder);
			}
			this.data.gameSpawnData.baseStartPos = this.data.worldLayout.GetStartLocation();
			return true;
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x002716A8 File Offset: 0x0026F8A8
		public SubWorld GetSubWorldForNode(VoronoiTree.Tree node)
		{
			ProcGen.Node node2 = this.WorldLayout.overworldGraph.FindNodeByID(node.site.id);
			if (node2 == null)
			{
				return null;
			}
			if (!this.Settings.HasSubworld(node2.type))
			{
				return null;
			}
			return this.Settings.GetSubWorld(node2.type);
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x002716FC File Offset: 0x0026F8FC
		public VoronoiTree.Tree GetOverworldForNode(Leaf leaf)
		{
			if (leaf == null)
			{
				return null;
			}
			return this.data.worldLayout.GetVoronoiTree().GetChildContainingLeaf(leaf);
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x00271719 File Offset: 0x0026F919
		public Leaf GetLeafForTerrainCell(TerrainCell cell)
		{
			if (cell == null)
			{
				return null;
			}
			return this.data.worldLayout.GetVoronoiTree().GetNodeForSite(cell.site) as Leaf;
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x00271740 File Offset: 0x0026F940
		public List<TerrainCell> GetTerrainCellsForTag(Tag tag)
		{
			List<TerrainCell> list = new List<TerrainCell>();
			List<VoronoiTree.Node> leafNodesWithTag = this.WorldLayout.GetLeafNodesWithTag(tag);
			for (int i = 0; i < leafNodesWithTag.Count; i++)
			{
				VoronoiTree.Node node = leafNodesWithTag[i];
				TerrainCell terrainCell = this.data.terrainCells.Find((TerrainCell cell) => cell.site.id == node.site.id);
				if (terrainCell != null)
				{
					list.Add(terrainCell);
				}
			}
			return list;
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x002717B0 File Offset: 0x0026F9B0
		private void GetStartCells(out int baseX, out int baseY)
		{
			Vector2I startLocation = new Vector2I(this.data.world.size.x / 2, (int)((float)this.data.world.size.y * 0.7f));
			if (this.data.worldLayout != null)
			{
				startLocation = this.data.worldLayout.GetStartLocation();
			}
			baseX = startLocation.x;
			baseY = startLocation.y;
		}

		// Token: 0x060067D9 RID: 26585 RVA: 0x00271828 File Offset: 0x0026FA28
		public void FinalizeStartLocation()
		{
			if (string.IsNullOrEmpty(this.Settings.world.startSubworldName))
			{
				return;
			}
			List<VoronoiTree.Node> startNodes = this.WorldLayout.GetStartNodes();
			global::Debug.Assert(startNodes.Count > 0, "Couldn't find a start node on a world that expects it!!");
			TagSet other = new TagSet
			{
				WorldGenTags.StartLocation
			};
			for (int i = 1; i < startNodes.Count; i++)
			{
				startNodes[i].tags.Remove(other);
			}
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x002718A0 File Offset: 0x0026FAA0
		private void SwitchNodes(VoronoiTree.Node n1, VoronoiTree.Node n2)
		{
			if (n1 is VoronoiTree.Tree || n2 is VoronoiTree.Tree)
			{
				global::Debug.Log("WorldGen::SwitchNodes() Skipping tree node");
				return;
			}
			Diagram.Site site = n1.site;
			n1.site = n2.site;
			n2.site = site;
			Cell cell = this.data.worldLayout.localGraph.FindNodeByID(n1.site.id);
			ProcGen.Node node = this.data.worldLayout.localGraph.FindNodeByID(n2.site.id);
			string type = cell.type;
			cell.SetType(node.type);
			node.SetType(type);
		}

		// Token: 0x060067DB RID: 26587 RVA: 0x0027193C File Offset: 0x0026FB3C
		private void ApplyStartNode()
		{
			List<VoronoiTree.Node> leafNodesWithTag = this.data.worldLayout.GetLeafNodesWithTag(WorldGenTags.StartLocation);
			if (leafNodesWithTag.Count == 0)
			{
				return;
			}
			VoronoiTree.Node node = leafNodesWithTag[0];
			VoronoiTree.Tree parent = node.parent;
			node.parent.AddTagToChildren(WorldGenTags.IgnoreCaveOverride);
			node.parent.tags.Remove(WorldGenTags.StartLocation);
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x0027199C File Offset: 0x0026FB9C
		private void ApplySwapTags()
		{
			List<VoronoiTree.Node> list = new List<VoronoiTree.Node>();
			for (int i = 0; i < this.data.voronoiTree.ChildCount(); i++)
			{
				if (this.data.voronoiTree.GetChild(i).tags.Contains(WorldGenTags.SwapLakesToBelow))
				{
					list.Add(this.data.voronoiTree.GetChild(i));
				}
			}
			foreach (VoronoiTree.Node node in list)
			{
				if (!node.tags.Contains(WorldGenTags.CenteralFeature))
				{
					List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
					((VoronoiTree.Tree)node).GetNodesWithoutTag(WorldGenTags.CenteralFeature, nodes);
					this.SwapNodesAround(WorldGenTags.Wet, true, nodes, node.site.poly.Centroid());
				}
			}
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x00271A88 File Offset: 0x0026FC88
		private void SwapNodesAround(Tag swapTag, bool sendTagToBottom, List<VoronoiTree.Node> nodes, Vector2 pivot)
		{
			nodes.ShuffleSeeded(this.myRandom.RandomSource());
			List<VoronoiTree.Node> list = new List<VoronoiTree.Node>();
			List<VoronoiTree.Node> list2 = new List<VoronoiTree.Node>();
			foreach (VoronoiTree.Node node in nodes)
			{
				bool flag = node.tags.Contains(swapTag);
				bool flag2 = node.site.poly.Centroid().y > pivot.y;
				bool flag3 = (flag2 && sendTagToBottom) || (!flag2 && !sendTagToBottom);
				if (flag && flag3)
				{
					if (list2.Count > 0)
					{
						this.SwitchNodes(node, list2[0]);
						list2.RemoveAt(0);
					}
					else
					{
						list.Add(node);
					}
				}
				else if (!flag && !flag3)
				{
					if (list.Count > 0)
					{
						this.SwitchNodes(node, list[0]);
						list.RemoveAt(0);
					}
					else
					{
						list2.Add(node);
					}
				}
			}
			if (list2.Count > 0)
			{
				int num = 0;
				while (num < list.Count && list2.Count > 0)
				{
					this.SwitchNodes(list[num], list2[0]);
					list2.RemoveAt(0);
					num++;
				}
			}
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x00271BD8 File Offset: 0x0026FDD8
		public void GetElementForBiomePoint(Chunk chunk, ElementBandConfiguration elementBands, Vector2I pos, out Element element, out Sim.PhysicsData pd, out Sim.DiseaseCell dc, float erode)
		{
			TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), null);
			elementOverride = this.GetElementFromBiomeElementTable(chunk, pos, elementBands, erode);
			element = elementOverride.element;
			pd = elementOverride.pdelement;
			dc = elementOverride.dc;
		}

		// Token: 0x060067DF RID: 26591 RVA: 0x00271C30 File Offset: 0x0026FE30
		public void ConvertIntersectingCellsToType(MathUtil.Pair<Vector2, Vector2> segment, string type)
		{
			List<Vector2I> line = ProcGen.Util.GetLine(segment.First, segment.Second);
			for (int i = 0; i < this.data.terrainCells.Count; i++)
			{
				if (this.data.terrainCells[i].node.type != type)
				{
					for (int j = 0; j < line.Count; j++)
					{
						if (this.data.terrainCells[i].poly.Contains(line[j]))
						{
							this.data.terrainCells[i].node.SetType(type);
						}
					}
				}
			}
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x00271CE8 File Offset: 0x0026FEE8
		public string GetSubWorldType(Vector2I pos)
		{
			for (int i = 0; i < this.data.overworldCells.Count; i++)
			{
				if (this.data.overworldCells[i].poly.Contains(pos))
				{
					return this.data.overworldCells[i].node.type;
				}
			}
			return null;
		}

		// Token: 0x060067E1 RID: 26593 RVA: 0x00271D50 File Offset: 0x0026FF50
		private void ProcessByTerrainCell(Sim.Cell[] map_cells, float[] bgTemp, Sim.DiseaseCell[] dcs, WorldGen.OfflineCallbackFunction updateProgressFn, HashSet<int> hightPriorityCells)
		{
			updateProgressFn(UI.WORLDGEN.PROCESSING.key, 0f, WorldGenProgressStages.Stages.Processing);
			SeededRandom seededRandom = new SeededRandom(this.data.globalTerrainSeed);
			try
			{
				for (int i = 0; i < this.data.terrainCells.Count; i++)
				{
					updateProgressFn(UI.WORLDGEN.PROCESSING.key, (float)i / (float)this.data.terrainCells.Count, WorldGenProgressStages.Stages.Processing);
					this.data.terrainCells[i].Process(this, map_cells, bgTemp, dcs, this.data.world, seededRandom);
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				updateProgressFn(new StringKey("Exception in TerrainCell.Process"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message + "\n" + stackTrace);
			}
			List<Border> list = new List<Border>();
			updateProgressFn(UI.WORLDGEN.BORDERS.key, 0f, WorldGenProgressStages.Stages.Borders);
			try
			{
				List<Edge> edgesWithTag = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeUnpassable);
				for (int j = 0; j < edgesWithTag.Count; j++)
				{
					Edge edge = edgesWithTag[j];
					List<Cell> cells = this.data.worldLayout.overworldGraph.GetNodes(edge);
					global::Debug.Assert(cells[0] != cells[1], "Both nodes on an arc were the same. Allegedly this means it was a world border but I don't think we do that anymore.");
					TerrainCell terrainCell = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[0]);
					TerrainCell terrainCell2 = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[1]);
					global::Debug.Assert(terrainCell != null && terrainCell2 != null, "NULL Terrainell nodes with EdgeUnpassable");
					terrainCell.LogInfo("BORDER WITH " + terrainCell2.site.id.ToString(), "UNPASSABLE", 0f);
					terrainCell2.LogInfo("BORDER WITH " + terrainCell.site.id.ToString(), "UNPASSABLE", 0f);
					list.Add(new Border(new Neighbors(terrainCell, terrainCell2), edge.corner0.position, edge.corner1.position)
					{
						element = SettingsCache.borders["impenetrable"],
						width = (float)seededRandom.RandomRange(2, 3)
					});
				}
				List<Edge> edgesWithTag2 = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);
				for (int k = 0; k < edgesWithTag2.Count; k++)
				{
					Edge edge2 = edgesWithTag2[k];
					if (!edgesWithTag.Contains(edge2))
					{
						List<Cell> cells = this.data.worldLayout.overworldGraph.GetNodes(edge2);
						global::Debug.Assert(cells[0] != cells[1], "Both nodes on an arc were the same. Allegedly this means it was a world border but I don't think we do that anymore.");
						TerrainCell terrainCell3 = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[0]);
						TerrainCell terrainCell4 = this.data.overworldCells.Find((TerrainCell c) => c.node == cells[1]);
						global::Debug.Assert(terrainCell3 != null && terrainCell4 != null, "NULL Terraincell nodes with EdgeClosed");
						string borderOverride = this.Settings.GetSubWorld(terrainCell3.node.type).borderOverride;
						string borderOverride2 = this.Settings.GetSubWorld(terrainCell4.node.type).borderOverride;
						string text;
						if (!string.IsNullOrEmpty(borderOverride2) && !string.IsNullOrEmpty(borderOverride))
						{
							int borderOverridePriority = this.Settings.GetSubWorld(terrainCell3.node.type).borderOverridePriority;
							int borderOverridePriority2 = this.Settings.GetSubWorld(terrainCell4.node.type).borderOverridePriority;
							if (borderOverridePriority == borderOverridePriority2)
							{
								text = ((seededRandom.RandomValue() > 0.5f) ? borderOverride2 : borderOverride);
								terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Picked Random:" + text, 0f);
								terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Picked Random:" + text, 0f);
							}
							else
							{
								text = ((borderOverridePriority > borderOverridePriority2) ? borderOverride : borderOverride2);
								terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Picked priority:" + text, 0f);
								terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Picked priority:" + text, 0f);
							}
						}
						else if (string.IsNullOrEmpty(borderOverride2) && string.IsNullOrEmpty(borderOverride))
						{
							text = "hardToDig";
							terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Both null", 0f);
							terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Both null", 0f);
						}
						else
						{
							text = ((!string.IsNullOrEmpty(borderOverride2)) ? borderOverride2 : borderOverride);
							terrainCell3.LogInfo("BORDER WITH " + terrainCell4.site.id.ToString(), "Picked specific " + text, 0f);
							terrainCell4.LogInfo("BORDER WITH " + terrainCell3.site.id.ToString(), "Picked specific " + text, 0f);
						}
						if (!(text == "NONE"))
						{
							Border border = new Border(new Neighbors(terrainCell3, terrainCell4), edge2.corner0.position, edge2.corner1.position);
							border.element = SettingsCache.borders[text];
							MinMax minMax = new MinMax(1.5f, 2f);
							MinMax borderSizeOverride = this.Settings.GetSubWorld(terrainCell3.node.type).borderSizeOverride;
							MinMax borderSizeOverride2 = this.Settings.GetSubWorld(terrainCell4.node.type).borderSizeOverride;
							bool flag = borderSizeOverride.min != 0f || borderSizeOverride.max != 0f;
							bool flag2 = borderSizeOverride2.min != 0f || borderSizeOverride2.max != 0f;
							if (flag && flag2)
							{
								minMax = ((borderSizeOverride.max > borderSizeOverride2.max) ? borderSizeOverride : borderSizeOverride2);
							}
							else if (flag)
							{
								minMax = borderSizeOverride;
							}
							else if (flag2)
							{
								minMax = borderSizeOverride2;
							}
							border.width = seededRandom.RandomRange(minMax.min, minMax.max);
							list.Add(border);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				string message2 = ex2.Message;
				string stackTrace2 = ex2.StackTrace;
				updateProgressFn(new StringKey("Exception in Border creation"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message2 + " " + stackTrace2);
			}
			try
			{
				if (this.data.world.defaultTemp == null)
				{
					this.data.world.defaultTemp = new float[this.data.world.density.Length];
				}
				for (int l = 0; l < this.data.world.defaultTemp.Length; l++)
				{
					this.data.world.defaultTemp[l] = bgTemp[l];
				}
			}
			catch (Exception ex3)
			{
				string message3 = ex3.Message;
				string stackTrace3 = ex3.StackTrace;
				updateProgressFn(new StringKey("Exception in border.defaultTemp"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message3 + " " + stackTrace3);
			}
			try
			{
				TerrainCell.SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
				{
					if (!Grid.IsValidCell(index))
					{
						global::Debug.LogError(string.Concat(new string[]
						{
							"Process::SetValuesFunction Index [",
							index.ToString(),
							"] is not valid. cells.Length [",
							map_cells.Length.ToString(),
							"]"
						}));
						return;
					}
					if (this.highPriorityClaims.Contains(index))
					{
						return;
					}
					if ((elem as Element).HasTag(GameTags.Special))
					{
						pd = (elem as Element).defaultValues;
					}
					map_cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
					dcs[index] = dc;
				};
				for (int m = 0; m < list.Count; m++)
				{
					Border border2 = list[m];
					SubWorld subWorld = this.Settings.GetSubWorld(border2.neighbors.n0.node.type);
					SubWorld subWorld2 = this.Settings.GetSubWorld(border2.neighbors.n1.node.type);
					float num = (SettingsCache.temperatures[subWorld.temperatureRange].min + SettingsCache.temperatures[subWorld.temperatureRange].max) / 2f;
					float num2 = (SettingsCache.temperatures[subWorld2.temperatureRange].min + SettingsCache.temperatures[subWorld2.temperatureRange].max) / 2f;
					float num3 = Mathf.Min(SettingsCache.temperatures[subWorld.temperatureRange].min, SettingsCache.temperatures[subWorld2.temperatureRange].min);
					float num4 = Mathf.Max(SettingsCache.temperatures[subWorld.temperatureRange].max, SettingsCache.temperatures[subWorld2.temperatureRange].max);
					float midTemp = (num + num2) / 2f;
					float num5 = num4 - num3;
					float rangeLow = 2f;
					float rangeHigh = 5f;
					int snapLastCells = 1;
					if (num5 >= 150f)
					{
						rangeLow = 0f;
						rangeHigh = border2.width * 0.2f;
						snapLastCells = 2;
						border2.width = Mathf.Max(border2.width, 2f);
						float f = num - 273.15f;
						float f2 = num2 - 273.15f;
						if (Mathf.Abs(f) < Mathf.Abs(f2))
						{
							midTemp = num;
						}
						else
						{
							midTemp = num2;
						}
					}
					border2.Stagger(seededRandom, (float)seededRandom.RandomRange(8, 13), seededRandom.RandomRange(rangeLow, rangeHigh));
					border2.ConvertToMap(this.data.world, setValues, num, num2, midTemp, seededRandom, snapLastCells);
				}
			}
			catch (Exception ex4)
			{
				string message4 = ex4.Message;
				string stackTrace4 = ex4.StackTrace;
				updateProgressFn(new StringKey("Exception in border.ConvertToMap"), -1f, WorldGenProgressStages.Stages.Failure);
				global::Debug.LogError("Error:" + message4 + " " + stackTrace4);
			}
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x00272854 File Offset: 0x00270A54
		private void DrawWorldBorder(Sim.Cell[] cells, Chunk world, SeededRandom rnd, ref HashSet<int> borderCells, ref List<RectInt> poiBounds, WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			WorldGen.<>c__DisplayClass137_0 CS$<>8__locals1 = new WorldGen.<>c__DisplayClass137_0();
			CS$<>8__locals1.world = world;
			bool boolSetting = this.Settings.GetBoolSetting("DrawWorldBorderForce");
			int intSetting = this.Settings.GetIntSetting("WorldBorderThickness");
			int intSetting2 = this.Settings.GetIntSetting("WorldBorderRange");
			ushort idx = WorldGen.vacuumElement.idx;
			ushort idx2 = WorldGen.voidElement.idx;
			ushort idx3 = WorldGen.unobtaniumElement.idx;
			float temperature = WorldGen.unobtaniumElement.defaultValues.temperature;
			float mass = WorldGen.unobtaniumElement.defaultValues.mass;
			int num = 0;
			int num2 = 0;
			updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 0f, WorldGenProgressStages.Stages.DrawWorldBorder);
			int num3 = CS$<>8__locals1.world.size.y - 1;
			int num4 = 0;
			int num5 = CS$<>8__locals1.world.size.x - 1;
			List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.RemoveWorldBorderOverVacuum);
			int y;
			int num9;
			for (y = num3; y >= 0; y = num9 - 1)
			{
				updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float)y / (float)num3 * 0.33f, WorldGenProgressStages.Stages.DrawWorldBorder);
				num = Mathf.Max(-intSetting2, Mathf.Min(num + rnd.RandomRange(-2, 2), intSetting2));
				bool flag = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2(0f, (float)y))) != null;
				for (int i = 0; i < intSetting + num; i++)
				{
					int num6 = Grid.XYToCell(i, y);
					if (boolSetting || (cells[num6].elementIdx != idx && cells[num6].elementIdx != idx2 && flag) || !flag)
					{
						borderCells.Add(num6);
						cells[num6].SetValues(idx3, temperature, mass);
						num4 = Mathf.Max(num4, i);
					}
				}
				num2 = Mathf.Max(-intSetting2, Mathf.Min(num2 + rnd.RandomRange(-2, 2), intSetting2));
				bool flag2 = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2((float)(CS$<>8__locals1.world.size.x - 1), (float)y))) != null;
				for (int j = 0; j < intSetting + num2; j++)
				{
					int num7 = CS$<>8__locals1.world.size.x - 1 - j;
					int num8 = Grid.XYToCell(num7, y);
					if (boolSetting || (cells[num8].elementIdx != idx && cells[num8].elementIdx != idx2 && flag2) || !flag2)
					{
						borderCells.Add(num8);
						cells[num8].SetValues(idx3, temperature, mass);
						num5 = Mathf.Min(num5, num7);
					}
				}
				num9 = y;
			}
			this.POIBounds.Add(new RectInt(0, 0, num4 + 1, this.World.size.y));
			this.POIBounds.Add(new RectInt(num5, 0, CS$<>8__locals1.world.size.x - num5, this.World.size.y));
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = this.World.size.y - 1;
			int x;
			for (x = 0; x < CS$<>8__locals1.world.size.x; x = num9 + 1)
			{
				updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float)x / (float)CS$<>8__locals1.world.size.x * 0.66f + 0.33f, WorldGenProgressStages.Stages.DrawWorldBorder);
				num10 = Mathf.Max(-intSetting2, Mathf.Min(num10 + rnd.RandomRange(-2, 2), intSetting2));
				bool flag3 = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2((float)x, 0f))) != null;
				for (int k = 0; k < intSetting + num10; k++)
				{
					int num14 = Grid.XYToCell(x, k);
					if (boolSetting || (cells[num14].elementIdx != idx && cells[num14].elementIdx != idx2 && flag3) || !flag3)
					{
						borderCells.Add(num14);
						cells[num14].SetValues(idx3, temperature, mass);
						num12 = Mathf.Max(num12, k);
					}
				}
				num11 = Mathf.Max(-intSetting2, Mathf.Min(num11 + rnd.RandomRange(-2, 2), intSetting2));
				bool flag4 = terrainCellsForTag.Find((TerrainCell n) => n.poly.Contains(new Vector2((float)x, (float)(CS$<>8__locals1.world.size.y - 1)))) != null;
				for (int l = 0; l < intSetting + num11; l++)
				{
					int num15 = CS$<>8__locals1.world.size.y - 1 - l;
					int num16 = Grid.XYToCell(x, num15);
					if (boolSetting || (cells[num16].elementIdx != idx && cells[num16].elementIdx != idx2 && flag4) || !flag4)
					{
						borderCells.Add(num16);
						cells[num16].SetValues(idx3, temperature, mass);
						num13 = Mathf.Min(num13, num15);
					}
				}
				num9 = x;
			}
			this.POIBounds.Add(new RectInt(0, 0, this.World.size.x, num12 + 1));
			this.POIBounds.Add(new RectInt(0, num13, this.World.size.x, this.World.size.y - num13));
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x00272E20 File Offset: 0x00271020
		private void SetupNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 0f, WorldGenProgressStages.Stages.SetupNoise);
			this.heatSource = this.BuildNoiseSource(this.data.world.size.x, this.data.world.size.y, "noise/Heat");
			updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 1f, WorldGenProgressStages.Stages.SetupNoise);
		}

		// Token: 0x060067E4 RID: 26596 RVA: 0x00272E98 File Offset: 0x00271098
		public NoiseMapBuilderPlane BuildNoiseSource(int width, int height, string name)
		{
			ProcGen.Noise.Tree tree = SettingsCache.noise.GetTree(name);
			global::Debug.Assert(tree != null, name);
			return this.BuildNoiseSource(width, height, tree);
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x00272EC4 File Offset: 0x002710C4
		public NoiseMapBuilderPlane BuildNoiseSource(int width, int height, ProcGen.Noise.Tree tree)
		{
			Vector2f lowerBound = tree.settings.lowerBound;
			Vector2f upperBound = tree.settings.upperBound;
			global::Debug.Assert(lowerBound.x < upperBound.x, string.Concat(new string[]
			{
				"BuildNoiseSource X range broken [l: ",
				lowerBound.x.ToString(),
				" h: ",
				upperBound.x.ToString(),
				"]"
			}));
			global::Debug.Assert(lowerBound.y < upperBound.y, string.Concat(new string[]
			{
				"BuildNoiseSource Y range broken [l: ",
				lowerBound.y.ToString(),
				" h: ",
				upperBound.y.ToString(),
				"]"
			}));
			global::Debug.Assert(width > 0, "BuildNoiseSource width <=0: [" + width.ToString() + "]");
			global::Debug.Assert(height > 0, "BuildNoiseSource height <=0: [" + height.ToString() + "]");
			NoiseMapBuilderPlane noiseMapBuilderPlane = new NoiseMapBuilderPlane(lowerBound.x, upperBound.x, lowerBound.y, upperBound.y, false);
			noiseMapBuilderPlane.SetSize(width, height);
			noiseMapBuilderPlane.SourceModule = tree.BuildFinalModule(this.data.globalNoiseSeed);
			return noiseMapBuilderPlane;
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x0027300C File Offset: 0x0027120C
		private void GetMinMaxDataValues(float[] data, int width, int height)
		{
		}

		// Token: 0x060067E7 RID: 26599 RVA: 0x00273010 File Offset: 0x00271210
		public static NoiseMap BuildNoiseMap(Vector2 offset, float zoom, NoiseMapBuilderPlane nmbp, int width, int height, NoiseMapBuilderCallback cb = null)
		{
			double num = (double)offset.x;
			double num2 = (double)offset.y;
			if (zoom == 0f)
			{
				zoom = 0.01f;
			}
			double num3 = num * (double)zoom;
			double num4 = (num + (double)width) * (double)zoom;
			double num5 = num2 * (double)zoom;
			double num6 = (num2 + (double)height) * (double)zoom;
			NoiseMap noiseMap = new NoiseMap(width, height);
			nmbp.NoiseMap = noiseMap;
			nmbp.SetBounds((float)num3, (float)num4, (float)num5, (float)num6);
			nmbp.CallBack = cb;
			nmbp.Build();
			return noiseMap;
		}

		// Token: 0x060067E8 RID: 26600 RVA: 0x00273088 File Offset: 0x00271288
		public static float[] GenerateNoise(Vector2 offset, float zoom, NoiseMapBuilderPlane nmbp, int width, int height, NoiseMapBuilderCallback cb = null)
		{
			NoiseMap noiseMap = WorldGen.BuildNoiseMap(offset, zoom, nmbp, width, height, cb);
			float[] result = new float[noiseMap.Width * noiseMap.Height];
			noiseMap.CopyTo(ref result);
			return result;
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x002730C0 File Offset: 0x002712C0
		public static void Normalise(float[] data)
		{
			global::Debug.Assert(data != null && data.Length != 0, "MISSING DATA FOR NORMALIZE");
			float num = float.MaxValue;
			float num2 = float.MinValue;
			for (int i = 0; i < data.Length; i++)
			{
				num = Mathf.Min(data[i], num);
				num2 = Mathf.Max(data[i], num2);
			}
			float num3 = num2 - num;
			for (int j = 0; j < data.Length; j++)
			{
				data[j] = (data[j] - num) / num3;
			}
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x00273134 File Offset: 0x00271334
		private void GenerateUnChunkedNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			Vector2 offset = new Vector2(0f, 0f);
			updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, 0f, WorldGenProgressStages.Stages.GenerateNoise);
			NoiseMapBuilderCallback noiseMapBuilderCallback = delegate(int line)
			{
				updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float)((int)(0f + 0.25f * ((float)line / (float)this.data.world.size.y))), WorldGenProgressStages.Stages.GenerateNoise);
			};
			noiseMapBuilderCallback = delegate(int line)
			{
				updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float)((int)(0.25f + 0.25f * ((float)line / (float)this.data.world.size.y))), WorldGenProgressStages.Stages.GenerateNoise);
			};
			if (noiseMapBuilderCallback == null)
			{
				global::Debug.LogError("nupd is null");
			}
			this.data.world.heatOffset = WorldGen.GenerateNoise(offset, SettingsCache.noise.GetZoomForTree("noise/Heat"), this.heatSource, this.data.world.size.x, this.data.world.size.y, noiseMapBuilderCallback);
			this.data.world.data = new float[this.data.world.heatOffset.Length];
			this.data.world.density = new float[this.data.world.heatOffset.Length];
			this.data.world.overrides = new float[this.data.world.heatOffset.Length];
			updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 0.5f, WorldGenProgressStages.Stages.GenerateNoise);
			if (SettingsCache.noise.ShouldNormaliseTree("noise/Heat"))
			{
				WorldGen.Normalise(this.data.world.heatOffset);
			}
			updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 1f, WorldGenProgressStages.Stages.GenerateNoise);
		}

		// Token: 0x060067EB RID: 26603 RVA: 0x002732D0 File Offset: 0x002714D0
		public void WriteOverWorldNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
		{
			Dictionary<HashedString, WorldGen.NoiseNormalizationStats> dictionary = new Dictionary<HashedString, WorldGen.NoiseNormalizationStats>();
			float num = (float)this.OverworldCells.Count;
			float perCell = 1f / num;
			float currentProgress = 0f;
			foreach (TerrainCell terrainCell in this.OverworldCells)
			{
				ProcGen.Noise.Tree tree = SettingsCache.noise.GetTree("noise/Default");
				ProcGen.Noise.Tree tree2 = SettingsCache.noise.GetTree("noise/DefaultCave");
				ProcGen.Noise.Tree tree3 = SettingsCache.noise.GetTree("noise/DefaultDensity");
				string s = "noise/Default";
				string s2 = "noise/DefaultCave";
				string s3 = "noise/DefaultDensity";
				SubWorld subWorld = this.Settings.GetSubWorld(terrainCell.node.type);
				if (subWorld == null)
				{
					global::Debug.Log("Couldnt find Subworld for overworld node [" + terrainCell.node.type + "] using defaults");
				}
				else
				{
					if (subWorld.biomeNoise != null)
					{
						ProcGen.Noise.Tree tree4 = SettingsCache.noise.GetTree(subWorld.biomeNoise);
						if (tree4 != null)
						{
							tree = tree4;
							s = subWorld.biomeNoise;
						}
					}
					if (subWorld.overrideNoise != null)
					{
						ProcGen.Noise.Tree tree5 = SettingsCache.noise.GetTree(subWorld.overrideNoise);
						if (tree5 != null)
						{
							tree2 = tree5;
							s2 = subWorld.overrideNoise;
						}
					}
					if (subWorld.densityNoise != null)
					{
						ProcGen.Noise.Tree tree6 = SettingsCache.noise.GetTree(subWorld.densityNoise);
						if (tree6 != null)
						{
							tree3 = tree6;
							s3 = subWorld.densityNoise;
						}
					}
				}
				WorldGen.NoiseNormalizationStats noiseNormalizationStats;
				if (!dictionary.TryGetValue(s, out noiseNormalizationStats))
				{
					noiseNormalizationStats = new WorldGen.NoiseNormalizationStats(this.BaseNoiseMap);
					dictionary.Add(s, noiseNormalizationStats);
				}
				WorldGen.NoiseNormalizationStats noiseNormalizationStats2;
				if (!dictionary.TryGetValue(s2, out noiseNormalizationStats2))
				{
					noiseNormalizationStats2 = new WorldGen.NoiseNormalizationStats(this.OverrideMap);
					dictionary.Add(s2, noiseNormalizationStats2);
				}
				WorldGen.NoiseNormalizationStats noiseNormalizationStats3;
				if (!dictionary.TryGetValue(s3, out noiseNormalizationStats3))
				{
					noiseNormalizationStats3 = new WorldGen.NoiseNormalizationStats(this.DensityMap);
					dictionary.Add(s3, noiseNormalizationStats3);
				}
				int width = (int)Mathf.Ceil(terrainCell.poly.bounds.width + 2f);
				int height = (int)Mathf.Ceil(terrainCell.poly.bounds.height + 2f);
				int num2 = (int)Mathf.Floor(terrainCell.poly.bounds.xMin - 1f);
				int num3 = (int)Mathf.Floor(terrainCell.poly.bounds.yMin - 1f);
				Vector2 vector;
				Vector2 offset = vector = new Vector2((float)num2, (float)num3);
				NoiseMapBuilderCallback cb = delegate(int line)
				{
					updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float)((int)(currentProgress + perCell * ((float)line / (float)height))), WorldGenProgressStages.Stages.NoiseMapBuilder);
				};
				NoiseMapBuilderPlane nmbp = this.BuildNoiseSource(width, height, tree);
				NoiseMap noiseMap = WorldGen.BuildNoiseMap(offset, tree.settings.zoom, nmbp, width, height, cb);
				NoiseMapBuilderPlane nmbp2 = this.BuildNoiseSource(width, height, tree2);
				NoiseMap noiseMap2 = WorldGen.BuildNoiseMap(offset, tree2.settings.zoom, nmbp2, width, height, cb);
				NoiseMapBuilderPlane nmbp3 = this.BuildNoiseSource(width, height, tree3);
				NoiseMap noiseMap3 = WorldGen.BuildNoiseMap(offset, tree3.settings.zoom, nmbp3, width, height, cb);
				vector.x = (float)((int)Mathf.Floor(terrainCell.poly.bounds.xMin));
				while (vector.x <= (float)((int)Mathf.Ceil(terrainCell.poly.bounds.xMax)))
				{
					vector.y = (float)((int)Mathf.Floor(terrainCell.poly.bounds.yMin));
					while (vector.y <= (float)((int)Mathf.Ceil(terrainCell.poly.bounds.yMax)))
					{
						if (terrainCell.poly.PointInPolygon(vector))
						{
							int num4 = Grid.XYToCell((int)vector.x, (int)vector.y);
							if (tree.settings.normalise)
							{
								noiseNormalizationStats.cells.Add(num4);
							}
							if (tree2.settings.normalise)
							{
								noiseNormalizationStats2.cells.Add(num4);
							}
							if (tree3.settings.normalise)
							{
								noiseNormalizationStats3.cells.Add(num4);
							}
							int x = (int)vector.x - num2;
							int y = (int)vector.y - num3;
							this.BaseNoiseMap[num4] = noiseMap.GetValue(x, y);
							this.OverrideMap[num4] = noiseMap2.GetValue(x, y);
							this.DensityMap[num4] = noiseMap3.GetValue(x, y);
							noiseNormalizationStats.min = Mathf.Min(this.BaseNoiseMap[num4], noiseNormalizationStats.min);
							noiseNormalizationStats.max = Mathf.Max(this.BaseNoiseMap[num4], noiseNormalizationStats.max);
							noiseNormalizationStats2.min = Mathf.Min(this.OverrideMap[num4], noiseNormalizationStats2.min);
							noiseNormalizationStats2.max = Mathf.Max(this.OverrideMap[num4], noiseNormalizationStats2.max);
							noiseNormalizationStats3.min = Mathf.Min(this.DensityMap[num4], noiseNormalizationStats3.min);
							noiseNormalizationStats3.max = Mathf.Max(this.DensityMap[num4], noiseNormalizationStats3.max);
						}
						vector.y += 1f;
					}
					vector.x += 1f;
				}
			}
			foreach (KeyValuePair<HashedString, WorldGen.NoiseNormalizationStats> keyValuePair in dictionary)
			{
				float num5 = keyValuePair.Value.max - keyValuePair.Value.min;
				foreach (int num6 in keyValuePair.Value.cells)
				{
					keyValuePair.Value.noise[num6] = (keyValuePair.Value.noise[num6] - keyValuePair.Value.min) / num5;
				}
			}
		}

		// Token: 0x060067EC RID: 26604 RVA: 0x00273978 File Offset: 0x00271B78
		private float GetValue(Chunk chunk, Vector2I pos)
		{
			int num = pos.x + this.data.world.size.x * pos.y;
			if (num < 0 || num >= chunk.data.Length)
			{
				throw new ArgumentOutOfRangeException("chunkDataIndex [" + num.ToString() + "]", "chunk data length [" + chunk.data.Length.ToString() + "]");
			}
			return chunk.data[num];
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x002739FC File Offset: 0x00271BFC
		public bool InChunkRange(Chunk chunk, Vector2I pos)
		{
			int num = pos.x + this.data.world.size.x * pos.y;
			return num >= 0 && num < chunk.data.Length;
		}

		// Token: 0x060067EE RID: 26606 RVA: 0x00273A40 File Offset: 0x00271C40
		private TerrainCell.ElementOverride GetElementFromBiomeElementTable(Chunk chunk, Vector2I pos, List<ElementGradient> table, float erode)
		{
			float num = this.GetValue(chunk, pos) * erode;
			TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), null);
			if (table.Count == 0)
			{
				return elementOverride;
			}
			for (int i = 0; i < table.Count; i++)
			{
				global::Debug.Assert(table[i].content != null, i.ToString());
				if (num < table[i].maxValue)
				{
					return TerrainCell.GetElementOverride(table[i].content, table[i].overrides);
				}
			}
			return TerrainCell.GetElementOverride(table[table.Count - 1].content, table[table.Count - 1].overrides);
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x00273B08 File Offset: 0x00271D08
		public static bool CanLoad(string fileName)
		{
			if (fileName == null || fileName == "")
			{
				return false;
			}
			bool result;
			try
			{
				if (File.Exists(fileName))
				{
					using (BinaryReader binaryReader = new BinaryReader(File.Open(fileName, FileMode.Open)))
					{
						return binaryReader.BaseStream.CanRead;
					}
				}
				result = false;
			}
			catch (FileNotFoundException)
			{
				result = false;
			}
			catch (Exception ex)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Failed to read " + fileName + "\n" + ex.ToString()
				});
				result = false;
			}
			return result;
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x00273BB0 File Offset: 0x00271DB0
		public void SaveWorldGen()
		{
			try
			{
				Manager.Clear();
				WorldGenSave worldGenSave = new WorldGenSave();
				worldGenSave.version = new Vector2I(1, 1);
				worldGenSave.data = this.data;
				worldGenSave.worldID = this.Settings.world.filePath;
				worldGenSave.traitIDs = new List<string>(this.Settings.GetWorldTraitIDs());
				worldGenSave.storyTraitIDs = new List<string>(this.Settings.GetStoryTraitIDs());
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						try
						{
							Serializer.Serialize(worldGenSave, binaryWriter);
						}
						catch (Exception ex)
						{
							DebugUtil.LogErrorArgs(new object[]
							{
								"Couldn't serialize",
								ex.Message,
								ex.StackTrace
							});
						}
					}
					using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Create)))
					{
						Manager.SerializeDirectory(binaryWriter2);
						binaryWriter2.Write(memoryStream.ToArray());
					}
				}
			}
			catch (Exception ex2)
			{
				DebugUtil.LogErrorArgs(new object[]
				{
					"Couldn't write",
					ex2.Message,
					ex2.StackTrace
				});
			}
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x00273D1C File Offset: 0x00271F1C
		public static WorldGen Load(IReader reader, bool defaultDiscovered)
		{
			WorldGen result;
			try
			{
				WorldGenSave worldGenSave = new WorldGenSave();
				Deserializer.Deserialize(worldGenSave, reader);
				WorldGen worldGen = new WorldGen(worldGenSave.worldID, worldGenSave.data, worldGenSave.traitIDs, worldGenSave.storyTraitIDs, false);
				worldGen.isStartingWorld = true;
				if (worldGenSave.version.x != 1 || worldGenSave.version.y > 1)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						string.Concat(new string[]
						{
							"LoadWorldGenSim Error! Wrong save version Current: [",
							1.ToString(),
							".",
							1.ToString(),
							"] File: [",
							worldGenSave.version.x.ToString(),
							".",
							worldGenSave.version.y.ToString(),
							"]"
						})
					});
					worldGen.wasLoaded = false;
				}
				else
				{
					worldGen.wasLoaded = true;
				}
				result = worldGen;
			}
			catch (Exception ex)
			{
				DebugUtil.LogErrorArgs(new object[]
				{
					"WorldGen.Load Error!\n",
					ex.Message,
					ex.StackTrace
				});
				result = null;
			}
			return result;
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x00273E50 File Offset: 0x00272050
		public void DrawDebug()
		{
		}

		// Token: 0x0400478A RID: 18314
		private const string _SIM_SAVE_FILENAME = "WorldGenSimSave";

		// Token: 0x0400478B RID: 18315
		private const string _SIM_SAVE_EXTENSION = ".dat";

		// Token: 0x0400478C RID: 18316
		private const string _WORLDGEN_SAVE_FILENAME = "WorldGenDataSave.dat";

		// Token: 0x0400478D RID: 18317
		private const int heatScale = 2;

		// Token: 0x0400478E RID: 18318
		private const int UNPASSABLE_EDGE_COUNT = 4;

		// Token: 0x0400478F RID: 18319
		private const string heat_noise_name = "noise/Heat";

		// Token: 0x04004790 RID: 18320
		private const string default_base_noise_name = "noise/Default";

		// Token: 0x04004791 RID: 18321
		private const string default_cave_noise_name = "noise/DefaultCave";

		// Token: 0x04004792 RID: 18322
		private const string default_density_noise_name = "noise/DefaultDensity";

		// Token: 0x04004793 RID: 18323
		public const int WORLDGEN_SAVE_MAJOR_VERSION = 1;

		// Token: 0x04004794 RID: 18324
		public const int WORLDGEN_SAVE_MINOR_VERSION = 1;

		// Token: 0x04004795 RID: 18325
		private const float EXTREME_TEMPERATURE_BORDER_RANGE = 150f;

		// Token: 0x04004796 RID: 18326
		private const float EXTREME_TEMPERATURE_BORDER_MIN_WIDTH = 2f;

		// Token: 0x04004797 RID: 18327
		public static Element voidElement;

		// Token: 0x04004798 RID: 18328
		public static Element vacuumElement;

		// Token: 0x04004799 RID: 18329
		public static Element katairiteElement;

		// Token: 0x0400479A RID: 18330
		public static Element unobtaniumElement;

		// Token: 0x0400479B RID: 18331
		private static Diseases m_diseasesDb;

		// Token: 0x0400479C RID: 18332
		public bool isRunningDebugGen;

		// Token: 0x0400479D RID: 18333
		private bool generateNoiseData = true;

		// Token: 0x0400479F RID: 18335
		private HashSet<int> claimedCells = new HashSet<int>();

		// Token: 0x040047A0 RID: 18336
		public Dictionary<int, int> claimedPOICells = new Dictionary<int, int>();

		// Token: 0x040047A1 RID: 18337
		private HashSet<int> highPriorityClaims = new HashSet<int>();

		// Token: 0x040047A2 RID: 18338
		public List<RectInt> POIBounds = new List<RectInt>();

		// Token: 0x040047A3 RID: 18339
		public List<TemplateSpawning.TemplateSpawner> POISpawners;

		// Token: 0x040047A4 RID: 18340
		private WorldGen.OfflineCallbackFunction successCallbackFn;

		// Token: 0x040047A5 RID: 18341
		private bool running = true;

		// Token: 0x040047A6 RID: 18342
		private Action<OfflineWorldGen.ErrorInfo> errorCallback;

		// Token: 0x040047A7 RID: 18343
		private SeededRandom myRandom;

		// Token: 0x040047A8 RID: 18344
		private NoiseMapBuilderPlane heatSource;

		// Token: 0x040047AA RID: 18346
		private bool wasLoaded;

		// Token: 0x040047AB RID: 18347
		public int polyIndex = -1;

		// Token: 0x040047AC RID: 18348
		public bool isStartingWorld;

		// Token: 0x040047AD RID: 18349
		public bool isModuleInterior;

		// Token: 0x040047AE RID: 18350
		private static Task loadSettingsTask;

		// Token: 0x02001BF1 RID: 7153
		// (Invoke) Token: 0x06009B4D RID: 39757
		public delegate bool OfflineCallbackFunction(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage);

		// Token: 0x02001BF2 RID: 7154
		public enum GenerateSection
		{
			// Token: 0x04007E5D RID: 32349
			SolarSystem,
			// Token: 0x04007E5E RID: 32350
			WorldNoise,
			// Token: 0x04007E5F RID: 32351
			WorldLayout,
			// Token: 0x04007E60 RID: 32352
			RenderToMap,
			// Token: 0x04007E61 RID: 32353
			CollectSpawners
		}

		// Token: 0x02001BF3 RID: 7155
		private class NoiseNormalizationStats
		{
			// Token: 0x06009B50 RID: 39760 RVA: 0x003488B3 File Offset: 0x00346AB3
			public NoiseNormalizationStats(float[] noise)
			{
				this.noise = noise;
			}

			// Token: 0x04007E62 RID: 32354
			public float[] noise;

			// Token: 0x04007E63 RID: 32355
			public float min = float.MaxValue;

			// Token: 0x04007E64 RID: 32356
			public float max = float.MinValue;

			// Token: 0x04007E65 RID: 32357
			public HashSet<int> cells = new HashSet<int>();
		}
	}
}
