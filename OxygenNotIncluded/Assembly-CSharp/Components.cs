using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020006ED RID: 1773
public class Components
{
	// Token: 0x04001CD8 RID: 7384
	public static Components.Cmps<MinionIdentity> LiveMinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04001CD9 RID: 7385
	public static Components.Cmps<MinionIdentity> MinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04001CDA RID: 7386
	public static Components.Cmps<StoredMinionIdentity> StoredMinionIdentities = new Components.Cmps<StoredMinionIdentity>();

	// Token: 0x04001CDB RID: 7387
	public static Components.Cmps<MinionStorage> MinionStorages = new Components.Cmps<MinionStorage>();

	// Token: 0x04001CDC RID: 7388
	public static Components.Cmps<MinionResume> MinionResumes = new Components.Cmps<MinionResume>();

	// Token: 0x04001CDD RID: 7389
	public static Components.Cmps<Sleepable> Sleepables = new Components.Cmps<Sleepable>();

	// Token: 0x04001CDE RID: 7390
	public static Components.Cmps<IUsable> Toilets = new Components.Cmps<IUsable>();

	// Token: 0x04001CDF RID: 7391
	public static Components.Cmps<Pickupable> Pickupables = new Components.Cmps<Pickupable>();

	// Token: 0x04001CE0 RID: 7392
	public static Components.Cmps<Brain> Brains = new Components.Cmps<Brain>();

	// Token: 0x04001CE1 RID: 7393
	public static Components.Cmps<BuildingComplete> BuildingCompletes = new Components.Cmps<BuildingComplete>();

	// Token: 0x04001CE2 RID: 7394
	public static Components.Cmps<Notifier> Notifiers = new Components.Cmps<Notifier>();

	// Token: 0x04001CE3 RID: 7395
	public static Components.Cmps<Fabricator> Fabricators = new Components.Cmps<Fabricator>();

	// Token: 0x04001CE4 RID: 7396
	public static Components.Cmps<Refinery> Refineries = new Components.Cmps<Refinery>();

	// Token: 0x04001CE5 RID: 7397
	public static Components.CmpsByWorld<PlantablePlot> PlantablePlots = new Components.CmpsByWorld<PlantablePlot>();

	// Token: 0x04001CE6 RID: 7398
	public static Components.Cmps<Ladder> Ladders = new Components.Cmps<Ladder>();

	// Token: 0x04001CE7 RID: 7399
	public static Components.Cmps<NavTeleporter> NavTeleporters = new Components.Cmps<NavTeleporter>();

	// Token: 0x04001CE8 RID: 7400
	public static Components.Cmps<ITravelTubePiece> ITravelTubePieces = new Components.Cmps<ITravelTubePiece>();

	// Token: 0x04001CE9 RID: 7401
	public static Components.CmpsByWorld<CreatureFeeder> CreatureFeeders = new Components.CmpsByWorld<CreatureFeeder>();

	// Token: 0x04001CEA RID: 7402
	public static Components.Cmps<Light2D> Light2Ds = new Components.Cmps<Light2D>();

	// Token: 0x04001CEB RID: 7403
	public static Components.Cmps<Radiator> Radiators = new Components.Cmps<Radiator>();

	// Token: 0x04001CEC RID: 7404
	public static Components.Cmps<Edible> Edibles = new Components.Cmps<Edible>();

	// Token: 0x04001CED RID: 7405
	public static Components.Cmps<Diggable> Diggables = new Components.Cmps<Diggable>();

	// Token: 0x04001CEE RID: 7406
	public static Components.Cmps<IResearchCenter> ResearchCenters = new Components.Cmps<IResearchCenter>();

	// Token: 0x04001CEF RID: 7407
	public static Components.Cmps<Harvestable> Harvestables = new Components.Cmps<Harvestable>();

	// Token: 0x04001CF0 RID: 7408
	public static Components.Cmps<HarvestDesignatable> HarvestDesignatables = new Components.Cmps<HarvestDesignatable>();

	// Token: 0x04001CF1 RID: 7409
	public static Components.Cmps<Uprootable> Uprootables = new Components.Cmps<Uprootable>();

	// Token: 0x04001CF2 RID: 7410
	public static Components.Cmps<Health> Health = new Components.Cmps<Health>();

	// Token: 0x04001CF3 RID: 7411
	public static Components.Cmps<Equipment> Equipment = new Components.Cmps<Equipment>();

	// Token: 0x04001CF4 RID: 7412
	public static Components.Cmps<FactionAlignment> FactionAlignments = new Components.Cmps<FactionAlignment>();

	// Token: 0x04001CF5 RID: 7413
	public static Components.Cmps<Telepad> Telepads = new Components.Cmps<Telepad>();

	// Token: 0x04001CF6 RID: 7414
	public static Components.Cmps<Generator> Generators = new Components.Cmps<Generator>();

	// Token: 0x04001CF7 RID: 7415
	public static Components.Cmps<EnergyConsumer> EnergyConsumers = new Components.Cmps<EnergyConsumer>();

	// Token: 0x04001CF8 RID: 7416
	public static Components.Cmps<Battery> Batteries = new Components.Cmps<Battery>();

	// Token: 0x04001CF9 RID: 7417
	public static Components.Cmps<Breakable> Breakables = new Components.Cmps<Breakable>();

	// Token: 0x04001CFA RID: 7418
	public static Components.Cmps<Crop> Crops = new Components.Cmps<Crop>();

	// Token: 0x04001CFB RID: 7419
	public static Components.Cmps<Prioritizable> Prioritizables = new Components.Cmps<Prioritizable>();

	// Token: 0x04001CFC RID: 7420
	public static Components.Cmps<Clinic> Clinics = new Components.Cmps<Clinic>();

	// Token: 0x04001CFD RID: 7421
	public static Components.Cmps<HandSanitizer> HandSanitizers = new Components.Cmps<HandSanitizer>();

	// Token: 0x04001CFE RID: 7422
	public static Components.Cmps<BuildingCellVisualizer> BuildingCellVisualizers = new Components.Cmps<BuildingCellVisualizer>();

	// Token: 0x04001CFF RID: 7423
	public static Components.Cmps<RoleStation> RoleStations = new Components.Cmps<RoleStation>();

	// Token: 0x04001D00 RID: 7424
	public static Components.Cmps<Telescope> Telescopes = new Components.Cmps<Telescope>();

	// Token: 0x04001D01 RID: 7425
	public static Components.Cmps<Capturable> Capturables = new Components.Cmps<Capturable>();

	// Token: 0x04001D02 RID: 7426
	public static Components.Cmps<NotCapturable> NotCapturables = new Components.Cmps<NotCapturable>();

	// Token: 0x04001D03 RID: 7427
	public static Components.Cmps<DiseaseSourceVisualizer> DiseaseSourceVisualizers = new Components.Cmps<DiseaseSourceVisualizer>();

	// Token: 0x04001D04 RID: 7428
	public static Components.Cmps<Grave> Graves = new Components.Cmps<Grave>();

	// Token: 0x04001D05 RID: 7429
	public static Components.Cmps<AttachableBuilding> AttachableBuildings = new Components.Cmps<AttachableBuilding>();

	// Token: 0x04001D06 RID: 7430
	public static Components.Cmps<BuildingAttachPoint> BuildingAttachPoints = new Components.Cmps<BuildingAttachPoint>();

	// Token: 0x04001D07 RID: 7431
	public static Components.Cmps<MinionAssignablesProxy> MinionAssignablesProxy = new Components.Cmps<MinionAssignablesProxy>();

	// Token: 0x04001D08 RID: 7432
	public static Components.Cmps<ComplexFabricator> ComplexFabricators = new Components.Cmps<ComplexFabricator>();

	// Token: 0x04001D09 RID: 7433
	public static Components.Cmps<MonumentPart> MonumentParts = new Components.Cmps<MonumentPart>();

	// Token: 0x04001D0A RID: 7434
	public static Components.Cmps<PlantableSeed> PlantableSeeds = new Components.Cmps<PlantableSeed>();

	// Token: 0x04001D0B RID: 7435
	public static Components.Cmps<IBasicBuilding> BasicBuildings = new Components.Cmps<IBasicBuilding>();

	// Token: 0x04001D0C RID: 7436
	public static Components.Cmps<Painting> Paintings = new Components.Cmps<Painting>();

	// Token: 0x04001D0D RID: 7437
	public static Components.Cmps<BuildingComplete> TemplateBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04001D0E RID: 7438
	public static Components.Cmps<Teleporter> Teleporters = new Components.Cmps<Teleporter>();

	// Token: 0x04001D0F RID: 7439
	public static Components.Cmps<MutantPlant> MutantPlants = new Components.Cmps<MutantPlant>();

	// Token: 0x04001D10 RID: 7440
	public static Components.Cmps<LandingBeacon.Instance> LandingBeacons = new Components.Cmps<LandingBeacon.Instance>();

	// Token: 0x04001D11 RID: 7441
	public static Components.Cmps<HighEnergyParticle> HighEnergyParticles = new Components.Cmps<HighEnergyParticle>();

	// Token: 0x04001D12 RID: 7442
	public static Components.Cmps<HighEnergyParticlePort> HighEnergyParticlePorts = new Components.Cmps<HighEnergyParticlePort>();

	// Token: 0x04001D13 RID: 7443
	public static Components.Cmps<Clustercraft> Clustercrafts = new Components.Cmps<Clustercraft>();

	// Token: 0x04001D14 RID: 7444
	public static Components.Cmps<ClustercraftInteriorDoor> ClusterCraftInteriorDoors = new Components.Cmps<ClustercraftInteriorDoor>();

	// Token: 0x04001D15 RID: 7445
	public static Components.Cmps<PassengerRocketModule> PassengerRocketModules = new Components.Cmps<PassengerRocketModule>();

	// Token: 0x04001D16 RID: 7446
	public static Components.Cmps<ClusterTraveler> ClusterTravelers = new Components.Cmps<ClusterTraveler>();

	// Token: 0x04001D17 RID: 7447
	public static Components.Cmps<LaunchPad> LaunchPads = new Components.Cmps<LaunchPad>();

	// Token: 0x04001D18 RID: 7448
	public static Components.Cmps<WarpReceiver> WarpReceivers = new Components.Cmps<WarpReceiver>();

	// Token: 0x04001D19 RID: 7449
	public static Components.Cmps<RocketControlStation> RocketControlStations = new Components.Cmps<RocketControlStation>();

	// Token: 0x04001D1A RID: 7450
	public static Components.Cmps<Reactor> NuclearReactors = new Components.Cmps<Reactor>();

	// Token: 0x04001D1B RID: 7451
	public static Components.Cmps<BuildingComplete> EntombedBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04001D1C RID: 7452
	public static Components.Cmps<SpaceArtifact> SpaceArtifacts = new Components.Cmps<SpaceArtifact>();

	// Token: 0x04001D1D RID: 7453
	public static Components.Cmps<ArtifactAnalysisStationWorkable> ArtifactAnalysisStations = new Components.Cmps<ArtifactAnalysisStationWorkable>();

	// Token: 0x04001D1E RID: 7454
	public static Components.Cmps<RocketConduitReceiver> RocketConduitReceivers = new Components.Cmps<RocketConduitReceiver>();

	// Token: 0x04001D1F RID: 7455
	public static Components.Cmps<RocketConduitSender> RocketConduitSenders = new Components.Cmps<RocketConduitSender>();

	// Token: 0x04001D20 RID: 7456
	public static Components.Cmps<LogicBroadcaster> LogicBroadcasters = new Components.Cmps<LogicBroadcaster>();

	// Token: 0x04001D21 RID: 7457
	public static Components.Cmps<Telephone> Telephones = new Components.Cmps<Telephone>();

	// Token: 0x04001D22 RID: 7458
	public static Components.Cmps<MissionControlWorkable> MissionControlWorkables = new Components.Cmps<MissionControlWorkable>();

	// Token: 0x04001D23 RID: 7459
	public static Components.Cmps<MissionControlClusterWorkable> MissionControlClusterWorkables = new Components.Cmps<MissionControlClusterWorkable>();

	// Token: 0x04001D24 RID: 7460
	public static Components.Cmps<MinorFossilDigSite.Instance> MinorFossilDigSites = new Components.Cmps<MinorFossilDigSite.Instance>();

	// Token: 0x04001D25 RID: 7461
	public static Components.Cmps<MajorFossilDigSite.Instance> MajorFossilDigSites = new Components.Cmps<MajorFossilDigSite.Instance>();

	// Token: 0x04001D26 RID: 7462
	public static Components.CmpsByWorld<Geyser> Geysers = new Components.CmpsByWorld<Geyser>();

	// Token: 0x04001D27 RID: 7463
	public static Components.CmpsByWorld<GeoTuner.Instance> GeoTuners = new Components.CmpsByWorld<GeoTuner.Instance>();

	// Token: 0x04001D28 RID: 7464
	public static Components.CmpsByWorld<Comet> Meteors = new Components.CmpsByWorld<Comet>();

	// Token: 0x04001D29 RID: 7465
	public static Components.CmpsByWorld<DetectorNetwork.Instance> DetectorNetworks = new Components.CmpsByWorld<DetectorNetwork.Instance>();

	// Token: 0x04001D2A RID: 7466
	public static Components.CmpsByWorld<ScannerNetworkVisualizer> ScannerVisualizers = new Components.CmpsByWorld<ScannerNetworkVisualizer>();

	// Token: 0x04001D2B RID: 7467
	public static Components.Cmps<IncubationMonitor.Instance> IncubationMonitors = new Components.Cmps<IncubationMonitor.Instance>();

	// Token: 0x04001D2C RID: 7468
	public static Components.Cmps<FixedCapturableMonitor.Instance> FixedCapturableMonitors = new Components.Cmps<FixedCapturableMonitor.Instance>();

	// Token: 0x04001D2D RID: 7469
	public static Components.Cmps<BeeHive.StatesInstance> BeeHives = new Components.Cmps<BeeHive.StatesInstance>();

	// Token: 0x02001435 RID: 5173
	public class Cmps<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060083DB RID: 33755 RVA: 0x00300467 File Offset: 0x002FE667
		public List<T> Items
		{
			get
			{
				return this.items.GetDataList();
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060083DC RID: 33756 RVA: 0x00300474 File Offset: 0x002FE674
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x060083DD RID: 33757 RVA: 0x00300481 File Offset: 0x002FE681
		public Cmps()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.items = new KCompactedVector<T>(0);
			this.table = new Dictionary<T, HandleVector<int>.Handle>();
		}

		// Token: 0x170008B5 RID: 2229
		public T this[int idx]
		{
			get
			{
				return this.Items[idx];
			}
		}

		// Token: 0x060083DF RID: 33759 RVA: 0x003004CE File Offset: 0x002FE6CE
		private void Clear()
		{
			this.items.Clear();
			this.table.Clear();
			this.OnAdd = null;
			this.OnRemove = null;
		}

		// Token: 0x060083E0 RID: 33760 RVA: 0x003004F4 File Offset: 0x002FE6F4
		public void Add(T cmp)
		{
			HandleVector<int>.Handle value = this.items.Allocate(cmp);
			this.table[cmp] = value;
			if (this.OnAdd != null)
			{
				this.OnAdd(cmp);
			}
		}

		// Token: 0x060083E1 RID: 33761 RVA: 0x00300530 File Offset: 0x002FE730
		public void Remove(T cmp)
		{
			HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
			if (this.table.TryGetValue(cmp, out invalidHandle))
			{
				this.table.Remove(cmp);
				this.items.Free(invalidHandle);
				if (this.OnRemove != null)
				{
					this.OnRemove(cmp);
				}
			}
		}

		// Token: 0x060083E2 RID: 33762 RVA: 0x00300584 File Offset: 0x002FE784
		public void Register(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd += on_add;
			this.OnRemove += on_remove;
			foreach (T obj in this.Items)
			{
				this.OnAdd(obj);
			}
		}

		// Token: 0x060083E3 RID: 33763 RVA: 0x003005EC File Offset: 0x002FE7EC
		public void Unregister(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd -= on_add;
			this.OnRemove -= on_remove;
		}

		// Token: 0x060083E4 RID: 33764 RVA: 0x003005FC File Offset: 0x002FE7FC
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds = false)
		{
			List<T> list = new List<T>();
			foreach (T t in this.Items)
			{
				KMonoBehaviour component = t as KMonoBehaviour;
				bool flag = component.GetMyWorldId() == worldId;
				if (!flag && checkChildWorlds)
				{
					WorldContainer myWorld = component.GetMyWorld();
					if (myWorld != null && myWorld.ParentWorldId == worldId)
					{
						flag = true;
					}
				}
				if (flag)
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060083E5 RID: 33765 RVA: 0x0030069C File Offset: 0x002FE89C
		// (remove) Token: 0x060083E6 RID: 33766 RVA: 0x003006D4 File Offset: 0x002FE8D4
		public event Action<T> OnAdd;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060083E7 RID: 33767 RVA: 0x0030070C File Offset: 0x002FE90C
		// (remove) Token: 0x060083E8 RID: 33768 RVA: 0x00300744 File Offset: 0x002FE944
		public event Action<T> OnRemove;

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060083E9 RID: 33769 RVA: 0x00300779 File Offset: 0x002FE979
		public bool IsSynchronized
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060083EA RID: 33770 RVA: 0x00300780 File Offset: 0x002FE980
		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060083EB RID: 33771 RVA: 0x00300787 File Offset: 0x002FE987
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060083EC RID: 33772 RVA: 0x0030078E File Offset: 0x002FE98E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x060083ED RID: 33773 RVA: 0x003007A0 File Offset: 0x002FE9A0
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x060083EE RID: 33774 RVA: 0x003007B2 File Offset: 0x002FE9B2
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x040064A8 RID: 25768
		private Dictionary<T, HandleVector<int>.Handle> table;

		// Token: 0x040064A9 RID: 25769
		private KCompactedVector<T> items;
	}

	// Token: 0x02001436 RID: 5174
	public class CmpsByWorld<T>
	{
		// Token: 0x060083EF RID: 33775 RVA: 0x003007C4 File Offset: 0x002FE9C4
		public CmpsByWorld()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.m_CmpsByWorld = new Dictionary<int, Components.Cmps<T>>();
		}

		// Token: 0x060083F0 RID: 33776 RVA: 0x003007F7 File Offset: 0x002FE9F7
		public void Clear()
		{
			this.m_CmpsByWorld.Clear();
		}

		// Token: 0x060083F1 RID: 33777 RVA: 0x00300804 File Offset: 0x002FEA04
		public Components.Cmps<T> CreateOrGetCmps(int worldId)
		{
			Components.Cmps<T> cmps;
			if (!this.m_CmpsByWorld.TryGetValue(worldId, out cmps))
			{
				cmps = new Components.Cmps<T>();
				this.m_CmpsByWorld[worldId] = cmps;
			}
			return cmps;
		}

		// Token: 0x060083F2 RID: 33778 RVA: 0x00300835 File Offset: 0x002FEA35
		public void Add(int worldId, T cmp)
		{
			DebugUtil.DevAssertArgs(worldId != -1, new object[]
			{
				"CmpsByWorld tried to add a component to an invalid world. Did you call this during a state machine's constructor instead of StartSM? ",
				cmp
			});
			this.CreateOrGetCmps(worldId).Add(cmp);
		}

		// Token: 0x060083F3 RID: 33779 RVA: 0x00300867 File Offset: 0x002FEA67
		public void Remove(int worldId, T cmp)
		{
			this.CreateOrGetCmps(worldId).Remove(cmp);
		}

		// Token: 0x060083F4 RID: 33780 RVA: 0x00300876 File Offset: 0x002FEA76
		public void Register(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Register(on_add, on_remove);
		}

		// Token: 0x060083F5 RID: 33781 RVA: 0x00300886 File Offset: 0x002FEA86
		public void Unregister(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Unregister(on_add, on_remove);
		}

		// Token: 0x060083F6 RID: 33782 RVA: 0x00300896 File Offset: 0x002FEA96
		public List<T> GetItems(int worldId)
		{
			return this.CreateOrGetCmps(worldId).Items;
		}

		// Token: 0x060083F7 RID: 33783 RVA: 0x003008A4 File Offset: 0x002FEAA4
		public Dictionary<int, Components.Cmps<T>>.KeyCollection GetWorldsIds()
		{
			return this.m_CmpsByWorld.Keys;
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060083F8 RID: 33784 RVA: 0x003008B4 File Offset: 0x002FEAB4
		public int GlobalCount
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<int, Components.Cmps<T>> keyValuePair in this.m_CmpsByWorld)
				{
					num += this.m_CmpsByWorld.Count;
				}
				return num;
			}
		}

		// Token: 0x040064AC RID: 25772
		private Dictionary<int, Components.Cmps<T>> m_CmpsByWorld;
	}
}
