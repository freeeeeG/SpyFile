using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Klei;
using KSerialization;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace ProcGenGame
{
	// Token: 0x02000CB2 RID: 3250
	[Serializable]
	public class Cluster
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060067F3 RID: 26611 RVA: 0x00273E52 File Offset: 0x00272052
		// (set) Token: 0x060067F4 RID: 26612 RVA: 0x00273E5A File Offset: 0x0027205A
		public ClusterLayout clusterLayout { get; private set; }

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060067F5 RID: 26613 RVA: 0x00273E63 File Offset: 0x00272063
		// (set) Token: 0x060067F6 RID: 26614 RVA: 0x00273E6B File Offset: 0x0027206B
		public bool IsGenerationComplete { get; private set; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060067F7 RID: 26615 RVA: 0x00273E74 File Offset: 0x00272074
		public bool IsGenerating
		{
			get
			{
				return this.thread != null && this.thread.IsAlive;
			}
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x00273E8B File Offset: 0x0027208B
		private Cluster()
		{
		}

		// Token: 0x060067F9 RID: 26617 RVA: 0x00273EC4 File Offset: 0x002720C4
		public Cluster(string name, int seed, List<string> chosenStoryTraitIds, bool assertMissingTraits, bool skipWorldTraits)
		{
			DebugUtil.Assert(!string.IsNullOrEmpty(name), "Cluster file is missing");
			this.seed = seed;
			WorldGen.LoadSettings(false);
			this.clusterLayout = SettingsCache.clusterLayouts.clusterCache[name];
			this.unplacedStoryTraits = new List<WorldTrait>();
			if (!this.clusterLayout.disableStoryTraits)
			{
				this.chosenStoryTraitIds = chosenStoryTraitIds;
				using (List<string>.Enumerator enumerator = chosenStoryTraitIds.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string name2 = enumerator.Current;
						WorldTrait cachedStoryTrait = SettingsCache.GetCachedStoryTrait(name2, assertMissingTraits);
						if (cachedStoryTrait != null)
						{
							this.unplacedStoryTraits.Add(cachedStoryTrait);
						}
					}
					goto IL_D5;
				}
			}
			this.chosenStoryTraitIds = new List<string>();
			IL_D5:
			this.Id = name;
			bool flag = seed > 0 && !skipWorldTraits;
			for (int i = 0; i < this.clusterLayout.worldPlacements.Count; i++)
			{
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(this.clusterLayout.worldPlacements[i].world);
				if (worldData != null)
				{
					this.clusterLayout.worldPlacements[i].SetSize(worldData.worldsize);
					if (i == this.clusterLayout.startWorldIndex)
					{
						this.clusterLayout.worldPlacements[i].startWorld = true;
					}
				}
			}
			this.size = BestFit.BestFitWorlds(this.clusterLayout.worldPlacements, false);
			foreach (WorldPlacement worldPlacement in this.clusterLayout.worldPlacements)
			{
				List<string> chosenWorldTraits = new List<string>();
				if (flag)
				{
					ProcGen.World worldData2 = SettingsCache.worlds.GetWorldData(worldPlacement.world);
					chosenWorldTraits = SettingsCache.GetRandomTraits(seed, worldData2);
					seed++;
				}
				WorldGen worldGen = new WorldGen(worldPlacement.world, chosenWorldTraits, null, assertMissingTraits);
				Vector2I worldsize = worldGen.Settings.world.worldsize;
				worldGen.SetWorldSize(worldsize.x, worldsize.y);
				worldGen.SetPosition(new Vector2I(worldPlacement.x, worldPlacement.y));
				this.worlds.Add(worldGen);
				if (worldPlacement.startWorld)
				{
					this.currentWorld = worldGen;
					worldGen.isStartingWorld = true;
				}
			}
			if (this.currentWorld == null)
			{
				global::Debug.LogWarning(string.Format("Start world not set. Defaulting to first world {0}", this.worlds[0].Settings.world.name));
				this.currentWorld = this.worlds[0];
			}
			if (this.clusterLayout.numRings > 0)
			{
				this.numRings = this.clusterLayout.numRings;
			}
		}

		// Token: 0x060067FA RID: 26618 RVA: 0x002741B8 File Offset: 0x002723B8
		public void Reset()
		{
			this.worlds.Clear();
		}

		// Token: 0x060067FB RID: 26619 RVA: 0x002741C8 File Offset: 0x002723C8
		private void LogBeginGeneration()
		{
			string str = (CustomGameSettings.Instance != null) ? CustomGameSettings.Instance.GetSettingsCoordinate() : this.seed.ToString();
			Console.WriteLine("\n\n");
			DebugUtil.LogArgs(new object[]
			{
				"WORLDGEN START"
			});
			DebugUtil.LogArgs(new object[]
			{
				" - seed:     " + str
			});
			DebugUtil.LogArgs(new object[]
			{
				" - cluster:  " + this.clusterLayout.filePath
			});
			if (this.chosenStoryTraitIds.Count == 0)
			{
				DebugUtil.LogArgs(new object[]
				{
					" - storytraits: none"
				});
				return;
			}
			DebugUtil.LogArgs(new object[]
			{
				" - storytraits:"
			});
			foreach (string str2 in this.chosenStoryTraitIds)
			{
				DebugUtil.LogArgs(new object[]
				{
					"    - " + str2
				});
			}
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x002742E0 File Offset: 0x002724E0
		public void Generate(WorldGen.OfflineCallbackFunction callbackFn, Action<OfflineWorldGen.ErrorInfo> error_cb, int worldSeed = -1, int layoutSeed = -1, int terrainSeed = -1, int noiseSeed = -1, bool doSimSettle = true, bool debug = false)
		{
			this.doSimSettle = doSimSettle;
			for (int num = 0; num != this.worlds.Count; num++)
			{
				this.worlds[num].Initialise(callbackFn, error_cb, worldSeed + num, layoutSeed + num, terrainSeed + num, noiseSeed + num, debug);
			}
			this.IsGenerationComplete = false;
			this.thread = new Thread(new ThreadStart(this.ThreadMain));
			global::Util.ApplyInvariantCultureToThread(this.thread);
			this.thread.Start();
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x00274363 File Offset: 0x00272563
		private void StopThread()
		{
			this.thread = null;
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x0027436C File Offset: 0x0027256C
		private void BeginGeneration()
		{
			this.LogBeginGeneration();
			Sim.Cell[] arg = null;
			Sim.DiseaseCell[] arg2 = null;
			int num = 0;
			List<WorldGen> list = new List<WorldGen>(this.worlds);
			list.Sort(delegate(WorldGen a, WorldGen b)
			{
				WorldPlacement a2 = this.clusterLayout.worldPlacements.Find((WorldPlacement x) => x.world == a.Settings.world.filePath);
				WorldPlacement b2 = this.clusterLayout.worldPlacements.Find((WorldPlacement x) => x.world == b.Settings.world.filePath);
				return WorldPlacement.CompareLocationType(a2, b2);
			});
			for (int i = 0; i < list.Count; i++)
			{
				WorldGen worldGen = list[i];
				if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(i, worldGen))
				{
					DebugUtil.Separator();
					DebugUtil.LogArgs(new object[]
					{
						"Generating world: " + worldGen.Settings.world.filePath
					});
					if (worldGen.Settings.GetWorldTraitIDs().Length != 0)
					{
						DebugUtil.LogArgs(new object[]
						{
							" - worldtraits: " + string.Join(", ", worldGen.Settings.GetWorldTraitIDs().ToArray<string>())
						});
					}
					if (this.PerWorldGenBeginCallback != null)
					{
						this.PerWorldGenBeginCallback(i, worldGen);
					}
					List<WorldTrait> list2 = new List<WorldTrait>();
					list2.AddRange(this.unplacedStoryTraits);
					worldGen.Settings.SetStoryTraitCandidates(list2);
					GridSettings.Reset(worldGen.GetSize().x, worldGen.GetSize().y);
					worldGen.GenerateOffline();
					worldGen.FinalizeStartLocation();
					arg = null;
					arg2 = null;
					List<WorldTrait> list3 = new List<WorldTrait>();
					if (!worldGen.RenderOffline(this.doSimSettle, ref arg, ref arg2, num, ref list3, worldGen.isStartingWorld))
					{
						this.StopThread();
						return;
					}
					if (this.PerWorldGenCompleteCallback != null)
					{
						this.PerWorldGenCompleteCallback(i, worldGen, arg, arg2);
					}
					foreach (WorldTrait item in list3)
					{
						this.unplacedStoryTraits.Remove(item);
					}
					num++;
				}
			}
			if (this.unplacedStoryTraits.Count > 0)
			{
				List<string> list4 = new List<string>();
				foreach (WorldTrait worldTrait in this.unplacedStoryTraits)
				{
					list4.Add(worldTrait.filePath);
				}
				string message = "Story trait failure, unable to place on any world: " + string.Join(", ", list4.ToArray());
				if (!this.worlds[0].isRunningDebugGen)
				{
					this.worlds[0].ReportWorldGenError(new Exception(message), UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_STORY);
				}
				this.StopThread();
				return;
			}
			DebugUtil.Separator();
			DebugUtil.LogArgs(new object[]
			{
				"Placing worlds on cluster map"
			});
			if (!this.AssignClusterLocations())
			{
				this.StopThread();
				return;
			}
			this.Save();
			this.StopThread();
			DebugUtil.Separator();
			DebugUtil.LogArgs(new object[]
			{
				"WORLDGEN COMPLETE\n\n\n"
			});
			this.IsGenerationComplete = true;
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x00274660 File Offset: 0x00272860
		private bool IsValidHex(AxialI location)
		{
			return location.IsWithinRadius(AxialI.ZERO, this.numRings - 1);
		}

		// Token: 0x06006800 RID: 26624 RVA: 0x00274678 File Offset: 0x00272878
		public bool AssignClusterLocations()
		{
			this.myRandom = new SeededRandom(this.seed);
			ClusterLayout clusterLayout = SettingsCache.clusterLayouts.clusterCache[this.Id];
			List<WorldPlacement> list = new List<WorldPlacement>(clusterLayout.worldPlacements);
			List<SpaceMapPOIPlacement> list2 = (clusterLayout.poiPlacements == null) ? new List<SpaceMapPOIPlacement>() : new List<SpaceMapPOIPlacement>(clusterLayout.poiPlacements);
			this.currentWorld.SetClusterLocation(AxialI.ZERO);
			HashSet<AxialI> assignedLocations = new HashSet<AxialI>();
			HashSet<AxialI> worldForbiddenLocations = new HashSet<AxialI>();
			new HashSet<AxialI>();
			HashSet<AxialI> poiWorldAvoidance = new HashSet<AxialI>();
			int maxRadius = 2;
			for (int i = 0; i < this.worlds.Count; i++)
			{
				WorldGen worldGen = this.worlds[i];
				WorldPlacement worldPlacement = list[i];
				DebugUtil.Assert(worldPlacement != null, "Somehow we're trying to generate a cluster with a world that isn't the cluster .yaml's world list!", worldGen.Settings.world.filePath);
				HashSet<AxialI> antiBuffer = new HashSet<AxialI>();
				foreach (AxialI center in assignedLocations)
				{
					antiBuffer.UnionWith(AxialUtil.GetRings(center, 1, worldPlacement.buffer));
				}
				List<AxialI> list3 = (from location in AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, this.numRings - 1))
				where !assignedLocations.Contains(location) && !worldForbiddenLocations.Contains(location) && !antiBuffer.Contains(location)
				select location).ToList<AxialI>();
				if (list3.Count > 0)
				{
					AxialI axialI = list3[this.myRandom.RandomRange(0, list3.Count)];
					worldGen.SetClusterLocation(axialI);
					assignedLocations.Add(axialI);
					worldForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI, 1, worldPlacement.buffer));
					poiWorldAvoidance.UnionWith(AxialUtil.GetRings(axialI, 1, maxRadius));
				}
				else
				{
					DebugUtil.DevLogError(string.Concat(new string[]
					{
						"Could not find a spot in the cluster for ",
						worldGen.Settings.world.filePath,
						". Check the placement settings in ",
						this.Id,
						".yaml to ensure there are no conflicts."
					}));
					HashSet<AxialI> minBuffers = new HashSet<AxialI>();
					foreach (AxialI center2 in assignedLocations)
					{
						minBuffers.UnionWith(AxialUtil.GetRings(center2, 1, 2));
					}
					list3 = (from location in AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, this.numRings - 1))
					where !assignedLocations.Contains(location) && !minBuffers.Contains(location)
					select location).ToList<AxialI>();
					if (list3.Count <= 0)
					{
						string text = string.Concat(new string[]
						{
							"Could not find a spot in the cluster for ",
							worldGen.Settings.world.filePath,
							" EVEN AFTER REDUCING BUFFERS. Check the placement settings in ",
							this.Id,
							".yaml to ensure there are no conflicts."
						});
						DebugUtil.LogErrorArgs(new object[]
						{
							text
						});
						if (!worldGen.isRunningDebugGen)
						{
							this.currentWorld.ReportWorldGenError(new Exception(text), null);
						}
						return false;
					}
					AxialI axialI2 = list3[this.myRandom.RandomRange(0, list3.Count)];
					worldGen.SetClusterLocation(axialI2);
					assignedLocations.Add(axialI2);
					worldForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI2, 1, worldPlacement.buffer));
					poiWorldAvoidance.UnionWith(AxialUtil.GetRings(axialI2, 1, maxRadius));
				}
			}
			if (DlcManager.FeatureClusterSpaceEnabled() && list2 != null)
			{
				HashSet<AxialI> poiClumpLocations = new HashSet<AxialI>();
				HashSet<AxialI> poiForbiddenLocations = new HashSet<AxialI>();
				float num = 0.5f;
				int num2 = 3;
				int num3 = 0;
				Func<AxialI, bool> <>9__2;
				Func<AxialI, bool> <>9__3;
				foreach (SpaceMapPOIPlacement spaceMapPOIPlacement in list2)
				{
					List<string> list4 = new List<string>(spaceMapPOIPlacement.pois);
					for (int j = 0; j < spaceMapPOIPlacement.numToSpawn; j++)
					{
						bool flag = this.myRandom.RandomRange(0f, 1f) <= num;
						List<AxialI> list5 = null;
						if (flag && num3 < num2 && !spaceMapPOIPlacement.avoidClumping)
						{
							num3++;
							IEnumerable<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, spaceMapPOIPlacement.allowedRings.min, Mathf.Min(spaceMapPOIPlacement.allowedRings.max, this.numRings - 1));
							Func<AxialI, bool> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((AxialI location) => !assignedLocations.Contains(location) && poiClumpLocations.Contains(location) && !poiWorldAvoidance.Contains(location)));
							}
							list5 = rings.Where(predicate).ToList<AxialI>();
						}
						if (list5 == null || list5.Count <= 0)
						{
							num3 = 0;
							poiClumpLocations.Clear();
							IEnumerable<AxialI> rings2 = AxialUtil.GetRings(AxialI.ZERO, spaceMapPOIPlacement.allowedRings.min, Mathf.Min(spaceMapPOIPlacement.allowedRings.max, this.numRings - 1));
							Func<AxialI, bool> predicate2;
							if ((predicate2 = <>9__3) == null)
							{
								predicate2 = (<>9__3 = ((AxialI location) => !assignedLocations.Contains(location) && !poiWorldAvoidance.Contains(location) && !poiForbiddenLocations.Contains(location)));
							}
							list5 = rings2.Where(predicate2).ToList<AxialI>();
						}
						if (list5 != null && list5.Count > 0)
						{
							AxialI axialI3 = list5[this.myRandom.RandomRange(0, list5.Count)];
							string text2 = list4[this.myRandom.RandomRange(0, list4.Count)];
							if (!spaceMapPOIPlacement.canSpawnDuplicates)
							{
								list4.Remove(text2);
							}
							this.poiPlacements[axialI3] = text2;
							poiForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI3, 1, 3));
							poiClumpLocations.UnionWith(AxialUtil.GetRings(axialI3, 1, 1));
							assignedLocations.Add(axialI3);
						}
						else
						{
							global::Debug.LogWarning(string.Format("There is no room for a Space POI in ring range [{0}, {1}]", spaceMapPOIPlacement.allowedRings.min, spaceMapPOIPlacement.allowedRings.max));
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x00274DA8 File Offset: 0x00272FA8
		public void AbortGeneration()
		{
			if (this.thread != null && this.thread.IsAlive)
			{
				this.thread.Abort();
				this.thread = null;
			}
		}

		// Token: 0x06006802 RID: 26626 RVA: 0x00274DD1 File Offset: 0x00272FD1
		private void ThreadMain()
		{
			this.BeginGeneration();
		}

		// Token: 0x06006803 RID: 26627 RVA: 0x00274DDC File Offset: 0x00272FDC
		private void Save()
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						try
						{
							Manager.Clear();
							ClusterLayoutSave clusterLayoutSave = new ClusterLayoutSave();
							clusterLayoutSave.version = new Vector2I(1, 1);
							clusterLayoutSave.size = this.size;
							clusterLayoutSave.ID = this.Id;
							clusterLayoutSave.numRings = this.numRings;
							clusterLayoutSave.poiLocations = this.poiLocations;
							clusterLayoutSave.poiPlacements = this.poiPlacements;
							for (int num = 0; num != this.worlds.Count; num++)
							{
								WorldGen worldGen = this.worlds[num];
								if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(num, worldGen))
								{
									clusterLayoutSave.worlds.Add(new ClusterLayoutSave.World
									{
										data = worldGen.data,
										name = worldGen.Settings.world.filePath,
										isDiscovered = worldGen.isStartingWorld,
										traits = worldGen.Settings.GetWorldTraitIDs().ToList<string>(),
										storyTraits = worldGen.Settings.GetStoryTraitIDs().ToList<string>()
									});
									if (worldGen == this.currentWorld)
									{
										clusterLayoutSave.currentWorldIdx = num;
									}
								}
							}
							Serializer.Serialize(clusterLayoutSave, binaryWriter);
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

		// Token: 0x06006804 RID: 26628 RVA: 0x00275038 File Offset: 0x00273238
		public static Cluster Load()
		{
			Cluster cluster = new Cluster();
			try
			{
				FastReader fastReader = new FastReader(File.ReadAllBytes(WorldGen.WORLDGEN_SAVE_FILENAME));
				Manager.DeserializeDirectory(fastReader);
				int position = fastReader.Position;
				ClusterLayoutSave clusterLayoutSave = new ClusterLayoutSave();
				if (!Deserializer.Deserialize(clusterLayoutSave, fastReader))
				{
					fastReader.Position = position;
					WorldGen worldGen = WorldGen.Load(fastReader, true);
					cluster.worlds.Add(worldGen);
					cluster.size = worldGen.GetSize();
					cluster.currentWorld = (cluster.worlds[0] ?? null);
				}
				else
				{
					for (int num = 0; num != clusterLayoutSave.worlds.Count; num++)
					{
						ClusterLayoutSave.World world = clusterLayoutSave.worlds[num];
						WorldGen item = new WorldGen(world.name, world.data, world.traits, world.storyTraits, false);
						cluster.worlds.Add(item);
						if (num == clusterLayoutSave.currentWorldIdx)
						{
							cluster.currentWorld = item;
							cluster.worlds[num].isStartingWorld = true;
						}
					}
					cluster.size = clusterLayoutSave.size;
					cluster.Id = clusterLayoutSave.ID;
					cluster.numRings = clusterLayoutSave.numRings;
					cluster.poiLocations = clusterLayoutSave.poiLocations;
					cluster.poiPlacements = clusterLayoutSave.poiPlacements;
				}
				DebugUtil.Assert(cluster.currentWorld != null);
				if (cluster.currentWorld == null)
				{
					DebugUtil.Assert(0 < cluster.worlds.Count);
					cluster.currentWorld = cluster.worlds[0];
				}
			}
			catch (Exception ex)
			{
				DebugUtil.LogErrorArgs(new object[]
				{
					"SolarSystem.Load Error!\n",
					ex.Message,
					ex.StackTrace
				});
				cluster = null;
			}
			return cluster;
		}

		// Token: 0x06006805 RID: 26629 RVA: 0x00275200 File Offset: 0x00273400
		public void LoadClusterLayoutSim(List<SimSaveFileStructure> loadedWorlds)
		{
			for (int num = 0; num != this.worlds.Count; num++)
			{
				SimSaveFileStructure simSaveFileStructure = new SimSaveFileStructure();
				try
				{
					FastReader reader = new FastReader(File.ReadAllBytes(WorldGen.GetSIMSaveFilename(num)));
					Manager.DeserializeDirectory(reader);
					Deserializer.Deserialize(simSaveFileStructure, reader);
				}
				catch (Exception ex)
				{
					if (!GenericGameSettings.instance.devAutoWorldGenActive)
					{
						DebugUtil.LogErrorArgs(new object[]
						{
							"LoadSim Error!\n",
							ex.Message,
							ex.StackTrace
						});
						break;
					}
				}
				if (simSaveFileStructure.worldDetail == null)
				{
					if (!GenericGameSettings.instance.devAutoWorldGenActive)
					{
						global::Debug.LogError("Detail is null for world " + num.ToString());
					}
				}
				else
				{
					loadedWorlds.Add(simSaveFileStructure);
				}
			}
		}

		// Token: 0x06006806 RID: 26630 RVA: 0x002752CC File Offset: 0x002734CC
		public void SetIsRunningDebug(bool isDebug)
		{
			foreach (WorldGen worldGen in this.worlds)
			{
				worldGen.isRunningDebugGen = isDebug;
			}
		}

		// Token: 0x06006807 RID: 26631 RVA: 0x00275320 File Offset: 0x00273520
		public void DEBUG_UpdateSeed(int seed)
		{
			this.seed = seed;
		}

		// Token: 0x040047AF RID: 18351
		public List<WorldGen> worlds = new List<WorldGen>();

		// Token: 0x040047B0 RID: 18352
		public WorldGen currentWorld;

		// Token: 0x040047B1 RID: 18353
		public Vector2I size;

		// Token: 0x040047B2 RID: 18354
		public string Id;

		// Token: 0x040047B3 RID: 18355
		public int numRings = 5;

		// Token: 0x040047B4 RID: 18356
		private int seed;

		// Token: 0x040047B5 RID: 18357
		private SeededRandom myRandom;

		// Token: 0x040047B6 RID: 18358
		private bool doSimSettle = true;

		// Token: 0x040047B7 RID: 18359
		public Action<int, WorldGen> PerWorldGenBeginCallback;

		// Token: 0x040047B8 RID: 18360
		public Action<int, WorldGen, Sim.Cell[], Sim.DiseaseCell[]> PerWorldGenCompleteCallback;

		// Token: 0x040047B9 RID: 18361
		public Func<int, WorldGen, bool> ShouldSkipWorldCallback;

		// Token: 0x040047BA RID: 18362
		public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

		// Token: 0x040047BB RID: 18363
		public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

		// Token: 0x040047BC RID: 18364
		public List<WorldTrait> unplacedStoryTraits;

		// Token: 0x040047BD RID: 18365
		public List<string> chosenStoryTraitIds;

		// Token: 0x040047BF RID: 18367
		private Thread thread;
	}
}
